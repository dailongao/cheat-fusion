#include <stdio.h>
#include <stdlib.h>
#include <conio.h>
#include <time.h>
#include <memory.h>
#include <vector>
#include <deque>
using namespace std;

#define LOOKAHEAD 0

typedef struct
{
	int pos;
	int ptr;
	int len;
	int method;
} lz_find;

const int lz_encoded_size[]={2,2,1};

unsigned char buffer[0x200000];
int buf_ptr;
unsigned char out_buffer[0x200000];
int out_ptr;

vector<int> table[256];
vector<lz_find> lz_table;
int encode_bytes;

int size = 0;

int max_runs = 0;
int max_delta = 0;

int min_match;
int window_size;
int max_match;

int cache_ptr;
int cache_mask;
int run_mask;
int win_mask;
int win_bits;

static void Find_LZ( int start, unsigned char byte, int &length, int &pos , int method)
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
		if(method==0xfe && ((start-ptr)%8)!=0) continue;
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

static int CheckLZ( int start, unsigned char byte, int &length, int &pos )
{
	int long_method=0,long_pos=0,long_length=0;
	int match_pos = 0,match_length = 0;
	
	for(int i=0xfc;i<0xff;i++){
		switch(i){
			case 0xfc:			//fc
				min_match = 0x4;
				max_match = 0x0f + min_match;
				window_size = 0xfff;
				break;
			case 0xfd:			//fd
				min_match = 0x14;
				max_match = 0xff + min_match;
				window_size = 0xff;
				break;
			case 0xfe:		//fe			//pos must %8==0 (0~120)
				min_match = 0x3;
				max_match = 0x0f + min_match;
				window_size = 0x7f;
				break;
		}
		Find_LZ( start, byte, match_length, match_pos, i );
		if(match_length>=long_length){
			long_pos=match_pos;
			long_length=match_length;
			long_method=i;
		}
	}
	length = long_length;
	pos = long_pos;
	return long_method;
}

void LZ_Encode(FILE *in, FILE *out)
{
	lz_find lz;
	int start;
	int lcv;
	
	fread( buffer, 1, 0x200000, in );
	size = ftell( in );
	fseek( in, 0, SEEK_SET );
	start = 0;
	
////////////////////////////////////////////////

	while( start < size ){
		int future_length[10];
		int future_pos[10];
		
		int length;
		int pos;
		int method;

		// Prepare for LZ
		table[ buffer[ start ] ].push_back( start );

		// Go find the longest substring match (and future ones)
		method=CheckLZ( start, buffer[ start ], length, pos );

		// Slightly increase ratio performance
		for( lcv = 0; lcv < LOOKAHEAD; lcv++ ) {
			start++; table[ buffer[ start ] ].push_back( start );
			CheckLZ( start, buffer[ start ], future_length[ lcv ], future_pos[ lcv ]);
		}

		// Un-do lookahead
		for( lcv = LOOKAHEAD; lcv > 0; lcv-- ) {
			table[ buffer[ start ] ].pop_back(); start--;
			if( future_length[ lcv-1 ] - length >= lcv )
				length = 0;
		}
		
		switch(method){
			case 0xfc:			//fc
				min_match = 0x4;
				break;
			case 0xfd:			//fd
				min_match = 0x14;
				break;
			case 0xfe:		//fe			//pos must %8==0 (0~120)
				min_match = 0x3;
				break;
		}
		if( length >= min_match ) {
	//printf("start:%X,method:%X,",start,method);getch();
			// Found substring match; record and re-do
			lz.pos = start;
			lz.ptr = start-pos;
			lz.len = length;
			lz.method = method;
			lz_table.push_back( lz );

			// Need to add to LZ table
			for( int lcv = 1; lcv < length; lcv++ )
				table[ buffer[ start + lcv ] ].push_back( start + lcv );

			// Fast update
			start += length;
			
			encode_bytes += lz_encoded_size[method-0xfc];
		}
		else {
			// No substrings found; try again
			start++;

			encode_bytes++;
		}
	}

	// insert dummy entry
	lz.pos = -1;
	lz_table.push_back( lz );
}

void LZ_Commit(FILE *in, FILE *out)
{
	lz_find lz;
	int lz_ptr=0;
	int start=0;
	int rawbytes=0;
	
	while(start<size){
		lz = lz_table[ lz_ptr ];
		if( lz.pos == start ) {
			if(rawbytes>0){
				fputc(rawbytes-1,out);
				fwrite(out_buffer,1,rawbytes,out);
				rawbytes=0;
			}
			
			// LZ
			fputc(lz.method,out);
			unsigned int temp;
			switch(lz.method){
				case 0xfc:			//fc
					temp=((lz.len-4)<<4)|(((lz.ptr-1)&0x0f00)>>8);
					fputc(lz.ptr-1,out);fputc(temp,out);					
					break;
				case 0xfd:			//fd
					fputc(lz.ptr-1,out);fputc(lz.len-0x14,out);
					break;
				case 0xfe:		//fe			//pos must %8==0 (0~120)
					temp=(lz.len-3)|((lz.ptr-8)<<1);
					fputc(temp,out);
					break;
			}
			
			start += lz.len;
			lz_ptr++;
		}
		else {
			// Free byte
			out_buffer[rawbytes++]=buffer[start++];
			if(rawbytes>=0xf0){
				fputc(rawbytes-1,out);
				fwrite(out_buffer,1,rawbytes,out);
				rawbytes=0;
			}
		}
	}
	
	if(rawbytes>0){
		fputc(rawbytes-1,out);
		fwrite(out_buffer,1,rawbytes,out);
		rawbytes=0;
	}
}

int main()
{
	FILE *in,*out;
	char name[128];
	char outname[128];
	printf("input infile\n");
	scanf("%s",name);
	sprintf(outname,"%s.cpc",name);
	in=fopen(name,"rb");
	out=fopen(outname,"wb");
	fputc(0x01,out);	
	LZ_Encode(in,out);
	LZ_Commit(in,out);
	fputc(0xff,out);	
	fclose(in);fclose(out);
	return 0;
}
