#include <stdio.h>
#include <fcntl.h>
#include <string.h>
#include <stdlib.h>
#include <conio.h>
#include <memory.h>

#include <vector>
#include <deque>

using namespace std;

typedef struct
{
	int pos;
	int ptr;
	int len;
} lz_find;


//#define DEBUG_LZ

unsigned char buffer[0x100000];
int buf_ptr;


void Decode(FILE *fp, int offset, FILE *out)
{
	unsigned int data,bits,flag,ch,bitmask,nextch,testch;
	int win_pos,win_len;
	fseek( fp, offset, SEEK_SET );
	buf_ptr=1;
	flag=0;
	while(1)
	{
		if(flag==0){
			data = fgetc( fp );
			bits = 8;
			flag = 0xff;
		}
		ch=flag&data;
		flag>>=1;
		testch=ch&(flag^0xff);
		bits--;		
		bitmask=8;
		if(flag==0){
			flag=0xff;
			bits=8;
			data = fgetc( fp );
		}
		ch=flag&data;
		bitmask-=bits;
		flag=0xff;
		bits=8;
		data = fgetc( fp );
		if(bitmask!=0){
			flag>>=bitmask;
			bits-=bitmask;
			ch=(ch<<bitmask)|(((flag^0xff)&data)>>bits);
		}
		printf("%02X,",ch);getch();
		if((int)testch>0){		//raw byte
			buffer[buf_ptr++]=ch;
			buf_ptr&=0xff;
			fputc(ch,out);
		}
		else{			
			if(ch==0) return;
			bitmask=4;
			win_len=0;
			if(flag==0){
				flag=0xff;
				bits=8;
				data = fgetc( fp );
			}
			if((int)(bitmask-bits)>0){
				bitmask=bitmask-bits;
				win_len=flag&data;
				flag=0xff;
				bits=8;
				data = fgetc( fp );
			}
			nextch=flag&data;
			flag>>=bitmask;
			bits-=bitmask;
			nextch=nextch&(flag^0xff);
			win_len=(win_len<<bitmask)|(nextch>>bits);
			printf("winlen:%02x,",win_len);getch();
			win_len++;
			win_pos=ch&0xff;
			while(win_len>=0){
				buffer[buf_ptr++]=buffer[win_pos];
				fputc(buffer[win_pos],out);
				win_pos++;
				win_pos&=0xff;
				buf_ptr&=0xff;
				win_len--;
			}
		}
	}
}



///////////////////////////////////////////////////////

void LZ_Decode(FILE *fp, int offset, FILE *out)
{
	buf_ptr = 1;
	fseek( fp, offset, SEEK_SET );

	unsigned int testch,ch;
	int pos,len;
	while(1){
		testch=fgetc(fp);
		//printf("%02x",testch);getch();
		if(testch==1){
			ch=fgetc(fp);
			buffer[buf_ptr++]=ch;
			buf_ptr&=0xff;
			fputc(ch,out);
		}
		else if(testch==0){
			pos=fgetc(fp);
			len=fgetc(fp)+1;
			while(len>=0){
				buffer[buf_ptr++]=buffer[pos];
				fputc(buffer[pos],out);
				buf_ptr&=0xff;
				pos++;
				pos&=0xff;
				len--;
			}
		}
		else return;
	}
}

//////////////////////////////////////////////////////////

vector<int> table[256];
deque<lz_find> lz_table;

int ptr = 0;
int size = 0;

int max_runs = 0;
int max_delta = 0;

int min_match = 2;
int window_size = 0x100-1;
int max_match = 15+2;


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


void LZ_Encode( FILE *in, FILE *out )
{
	lz_find lz;
	int start;
	int lcv;
	int counter = 0;

	// Step 0: Check file size
	fread( buffer, 1, 0x100000, in );
	size = ftell( in );
	fseek( in, 0, SEEK_SET );

////////////////////////////////////////////////

	// Step 1: Find all LZ matches
	start = 0;

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
		for( lcv = 0; lcv < 1; lcv++) {
			start++; table[ buffer[ start ] ].push_back( start );
			Find_LZ( start, buffer[ start ], future_length[ lcv ], future_pos[ lcv ]);
		}

		// Un-do lookahead
		for( lcv = 1; lcv > 0; lcv-- ) {
			table[ buffer[ start ] ].pop_back(); start--;
			if( future_length[ lcv-1 ] - length >= lcv )
				length = 0;
		}

		if( length >= min_match ) {
			// Found substring match; record and re-do
			lz.pos = start;
			lz.ptr = start>window_size? window_size+1-(start-pos):pos+1;
			lz.len = length;
			
			lz_table.push_back( lz );

			// Need to add to LZ table
			for( int lcv = 1; lcv < length; lcv++ )
				table[ buffer[ start + lcv ] ].push_back( start + lcv );

			// Fast update
			start += length;
		}
		else {
			// No substrings found; try again
			start++;
		}
	}

	// insert dummy entry
	lz.pos = -1;
	lz_table.push_back( lz );

///////////////////////////////////////////////////////////////

	int lz_p;
	int out_byte;
	int out_bits;
	int out_ptr;
	unsigned char out_buffer[0x100];

	// init
	lz_p = 0;
	start = 0;
	out_ptr = 0;
	out_byte = 0;
	out_bits = 8;

	// Step 2: Prepare encoding methods

	while( start < size ) {
		lz = lz_table[ lz_p ];
		if( lz.pos == start ) {
			// LZ
			fputc(0x00,out);
			fputc(lz.ptr,out);
			fputc(lz.len-min_match,out);
			start += lz.len;
			lz_p++;
		}
		else {
			// Free byte
			fputc(0x01,out);
			fputc(buffer[start],out);
			start++;
		}
	} // end method check
	
	fputc(0xFF,out);
}

int main()
{
	FILE *in,*out;
	in=fopen("zk_dec","rb");
	out=fopen("zk_dec.enc","wb");
	
	//LZ_Decode(in,0,out);
	LZ_Encode(in,out);
	fclose(in);fclose(out);
	return 0;
}

