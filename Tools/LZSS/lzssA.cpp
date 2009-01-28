//压缩率比标准高一点
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <ctype.h>
#include <time.h>
#include <memory.h>
#include <conio.h>

#include <vector>
#include <deque>

#define LOOKAHEAD 1
#define LOOKAHEAD_COMPARE_LEN 5

using namespace std;

#define N		 4096	/* size of ring buffer */
#define F		   18	/* upper limit for match_length */
#define THRESHOLD	2   /* encode string into position and length
						   if match_length is greater than this */
#define NIL			N	/* index for root of binary search trees */
unsigned long int
		textsize = 0,	/* text size counter */
		codesize = 0,	/* code size counter */
		printcount = 0;	/* counter for reporting progress every 1K bytes */
unsigned char
		text_buf[N + F - 1];	/* ring buffer of size N,
			with extra F-1 bytes to facilitate string comparison */

typedef struct
{
	int pos;
	int ptr;
	int len;
} lz_find;

unsigned char buffer[0x100000];
unsigned int buf_ptr;
unsigned char file_buffer[0x100000];

FILE	*infile, *outfile;  /* input & output files */

vector<int> table[256];
vector<lz_find> lz_table;
int encode_bytes;

int ptr = 0;
int size = 0;

int min_match = THRESHOLD + 1;
//int window_size = N;
//int max_match = F;

int max_match = 0x0f + 3;
int window_size = 0xfff;
int	run_mask = 0x0f;
int win_mask = 0xf0;
int win_bits = 4;
int cache_ptr = 0xfee;
int cache_mask = 0xfff;

void LZInit(unsigned int filesize)
{
	int i;
	for(i=0;i<256;i++)
		table[i].reserve(filesize/128);
	lz_table.reserve(filesize/8);
}

void Find_LZ( int start, unsigned char byte, int &length, int &pos )
{
	int longest_length = 0;
	int longest_ptr = 0;

	// use cached positions to quickly look through the file
	for( int lcv = 0; lcv < table[ byte ].size(); lcv++ ) {
		int match_length = 0;
		int ptr = table[ byte ][ lcv ];

		// invalid string; stop scanning
		if( ptr >= start )
			break;

		// LZ window restriction
		if( start - ptr > window_size )
			continue;

		// search for longest identical substring
		for( int lcv2 = start; ( lcv2 < start + max_match ) && ( lcv2 < size ); lcv2++ ) {
			// look for a mismatch
			if( buffer[ lcv2 ] != buffer[ ptr ] )
				break;
				
			// keep looking
			ptr++;
			match_length++;
		}

		// record new long find
		if( longest_length <= match_length ) {
			longest_ptr = table[ byte ][ lcv ];
			longest_length = match_length;
		}
	}

	// output findings
	length = longest_length;
	pos = longest_ptr;
}

void LZEncode(FILE *in, FILE *out)
{
	lz_find lz;
	int start;
	int lcv;
	int count=N-F;
	
	fseek(in,0,SEEK_END);
	buf_ptr = ftell(in);
	fseek(in,0,SEEK_SET);
	fread(file_buffer,1,buf_ptr,in);
	
	//LZInit(buf_ptr);
	
	// Step 1: Find all LZ matches (optimal)
	start = 0;
	size = buf_ptr;
	encode_bytes = 0;
	
	memset( buffer, 0, cache_ptr );
	memcpy( buffer + cache_ptr, file_buffer, size );
	
	start += cache_ptr;
	size += cache_ptr;
	for( lcv = 0; lcv < cache_ptr; lcv++ ) {
		table[ 0 ].push_back( lcv );
	}
	
	while( start < size ) {
		int future_length[10];
		int future_pos[10];

		int length;
		int pos;

		// Prepare for LZ
		table[ buffer[ start ] ].push_back( start );

		// Go find the longest substring match (and future ones)
		Find_LZ( start, buffer[ start ], length, pos );

		// Slightly increase ratio performance
		for( lcv = 0; lcv < LOOKAHEAD; lcv++ ) {
			start++; table[ buffer[ start ] ].push_back( start );
			Find_LZ( start, buffer[ start ], future_length[ lcv ], future_pos[ lcv ]);
		}

		// Un-do lookahead
		for( lcv = LOOKAHEAD; lcv > 0; lcv-- ) {
			table[ buffer[ start ] ].pop_back(); start--;
			if(length<LOOKAHEAD_COMPARE_LEN){                                //5这个数字可以提高压缩率 
				if( future_length[ lcv-1 ] - length >=  lcv ) length = 0;
			}
			else{
				if( future_length[ lcv-1 ] - length >   lcv ) length = 0;
			}
		}

		if( length >= min_match ) {
			// Found substring match; record and re-do
			lz.pos = start;
			lz.ptr = count-(start-pos);
			lz.len = length;
//printf("ptr:%x,len:%x;start:%x\n",pos,length,start-cache_ptr);getch();			
			lz_table.push_back( lz );

			// Need to add to LZ table
			for( int lcv = 1; lcv < length; lcv++ )
				table[ buffer[ start + lcv ] ].push_back( start + lcv );

			// Fast update
			start += length;
			count += length;
			encode_bytes += 2;
		}
		else {
			// No substrings found; try again
			start++;
			count++;
			encode_bytes++;
		}
		count&=N-1;
	}

	// insert dummy entry
	lz.pos = -1;
	lz_table.push_back( lz );
	
	start -= cache_ptr;
	size -= cache_ptr;

///////////////////////////////////////////////////////////////

	int lz_ptr;
	int out_byte;
	int out_bits;
	int out_ptr;
	unsigned char out_buffer[0x100];

	// init
	lz_ptr = 0;
	start = 0;
	out_ptr = 0;
	out_byte = 0;
	out_bits = 8;
	
	start += cache_ptr;
	size += cache_ptr;

	// Step 2: Prepare encoding methods

	while( start < size ) {
		lz = lz_table[ lz_ptr ];
		out_byte >>= 1;

		if( lz.pos == start ) {

			// LZ
			out_byte |= ( 0 << 7 );
			int temp = (lz.ptr);
			out_buffer[ out_ptr++ ] = temp & 0xff;
			out_buffer[ out_ptr ] = ( lz.len - 1 - 2 ) & 0x0f;
			out_buffer[ out_ptr++ ] |= ( ( temp & 0xf00 ) >> 8 ) << 4;

			start += lz.len;
			lz_ptr++;
		}
		else {
			// Free byte
			out_byte |= ( 1 << 7 );
			out_buffer[ out_ptr++ ] = buffer[ start ];

			start++;
		}

		// flush out data
		out_bits--;
		if( !out_bits ) {
			fputc( out_byte, out );
			fwrite( out_buffer, 1, out_ptr, out );
			codesize+=out_ptr+1;
			out_ptr = 0;
			out_byte = 0;
			out_bits = 8;
		}
	} // end method check

	// add final LZ bit
	out_byte >>= 1;
	out_byte |= ( 0 << 7 );
	//out_buffer[ out_ptr++ ] = 0;
	//out_buffer[ out_ptr++ ] = 0;
	out_bits--;

	// shove in dummy bits
	while( out_bits-- ) out_byte >>= 1;

	// flush out data
	fputc( out_byte, out );
	fwrite( out_buffer, 1, out_ptr, out );
	codesize+=out_ptr+1;
}


void Decode(void)	/* Just the reverse of Encode(). */
{
			
	int  i, j, k, r, c;
	unsigned int  flags;
	
	for (i = 0; i < N - F; i++) text_buf[i] = 0;
	r = N - F;  flags = 0;
	for ( ; ; ) {
		if (((flags >>= 1) & 256) == 0) {
			if ((c = getc(infile)) == EOF) break;
			flags = c | 0xff00;		/* uses higher byte cleverly */
		}							/* to count eight */
		if (flags & 1) {
			if ((c = getc(infile)) == EOF) break;
			putc(c, outfile);  text_buf[r++] = c;  r &= (N - 1);
		} else {
			if ((i = getc(infile)) == EOF) break;
			if ((j = getc(infile)) == EOF) break;
			i |= ((j & 0xf0) << 4);  j = (j & 0x0f) + THRESHOLD;
			for (k = 0; k <= j; k++) {
				c = text_buf[(i + k) & (N - 1)];
				putc(c, outfile);  text_buf[r++] = c;  r &= (N - 1);
			}
		}
	}
}

int main(int argc, char *argv[])
{
	char  *s;
	
	time_t t_start,t_finish;
	
	if (argc != 4) {
		printf("'lzss e file1 file2' encodes file1 into file2.\n"
			   "'lzss d file2 file1' decodes file2 into file1.\n");
		return EXIT_FAILURE;
	}
	if ((s = argv[1], s[1] || strpbrk(s, "DEde") == NULL)
	 || (s = argv[2], (infile  = fopen(s, "rb")) == NULL)
	 || (s = argv[3], (outfile = fopen(s, "wb")) == NULL)) {
		printf("??? %s\n", s);  return EXIT_FAILURE;
	}
	
	t_start=time(NULL);
	if (toupper(*argv[1]) == 'E') {
		fwrite(&codesize,1,4,outfile);
		LZEncode(infile, outfile);
		fseek(outfile,0,SEEK_SET);
		codesize+=4;
		fwrite(&codesize,1,4,outfile);
	}
	else {
		fseek(infile,4,SEEK_SET);
		Decode();
	}	
	t_finish=time(NULL);

	fclose(infile);  fclose(outfile);
	printf("%f Secs\n",difftime(t_finish,t_start)) ;
	return EXIT_SUCCESS;
}
