#include <stdio.h>
#include <stdlib.h>
#include <conio.h>
#include <time.h>
#include <memory.h>
#include <vector>
#include <deque>
using namespace std;

#define LOOKAHEAD 1

typedef struct
{
	int pos;
	int ptr;
	int len;
	int method;
} lz_find;

const int encoded_size[]={1,2,2,3,4,2,3,4,2,2,3,4,2,2,1};
const int methodlist[]={0xfe,0xfd,0xfc,0xf0,0xf1,0xf3,0xf4,0,0,0,0,0,0,0,0,0,0,0};

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
		int base = ptr;
		// invalid string; stop scanning
		if( ptr >= start )
			break;

		// LZ window restriction
		if( start - ptr > window_size )
			continue;
		if(method==0xfe && ((start-ptr)&0x07)!=0) continue;
		// search for longest identical substring
		for( int lcv2 = start; ( lcv2 < start + max_match ) && ( lcv2 < size ); lcv2++ ) {
			// look for a mismatch
			if( buffer[ lcv2 ] != buffer[ ptr ] )
				break;

			// keep looking
			ptr++;
			match_length++;
		}
		//if(match_length>=3 && method==0xfe){printf("start:%x,ptr:%x,len:%x,long:%x,",start,table[ byte ][ lcv ],match_length,longest_length);getch();}
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

static int Check( int start, unsigned char byte, int &length, int &pos )
{
	int long_method=0,long_pos=0,long_length=0;
	int match_pos,match_length;
	int run,gain=0;
	unsigned char ch,ch1,ch2,ch3;
	
	for(int k=0;k<15;k++){
		match_length=0;
		int i=methodlist[k];
		switch(i)
		{
			case 0xf0:
				min_match=0x3;
				ch=buffer[start];
				if(ch<=0xf){
					run=1;
					while(buffer[start+run]==ch){
						if(start+run>=size) break;
						run++;
						if(run>=0xf+min_match) break;
					}
					match_length=run;
				}
				
				if(match_length-encoded_size[i-0xf0]>=gain){
					gain=match_length-encoded_size[i-0xf0];
					long_pos=match_pos;
					long_length=match_length;
					long_method=i;
				}
				break;
			case 0xf1:
				min_match=0x4;
				ch=buffer[start];
				run=1;
				while(buffer[start+run]==ch){
					if(start+run>=size) break;
					run++;
					if(run>=0xff+min_match) break;
				}
				match_length=run;
				
				if(match_length-encoded_size[i-0xf0]>=gain){
					gain=match_length-encoded_size[i-0xf0];
					long_pos=match_pos;
					long_length=match_length;
					long_method=i;
				}
				break;
			case 0xf3:
				min_match=0x2;
				if(start+1<size && start+2<size){
					ch1=buffer[start];
					ch2=buffer[start+1];
					run=1;
					while(ch1==buffer[start+2*run] && ch2==buffer[start+1+2*run]){
						if(start+run*2>=size) break;
						run++;
						if(run>=0xff+min_match) break;
					}
					match_length=run*2;
					if(match_length>=min_match*2 && match_length-encoded_size[i-0xf0]>=gain){
						gain=match_length-encoded_size[i-0xf0];
						long_pos=match_pos;
						long_length=match_length;
						long_method=i;
					}					
				}
				break;
			case 0xf4:
				min_match=0x2;
				if(start+1<size && start+2<size){
					ch1=buffer[start];
					ch2=buffer[start+1];
					ch3=buffer[start+2];
					run=1;
					while(ch1==buffer[start+3*run] && ch2==buffer[start+1+3*run] && ch3==buffer[start+2+3*run]){
						if(start+run*3>=size) break;
						run++;
						if(run>=0xff+min_match) break;
					}
					match_length=run*3;
					
					if(match_length>=min_match*3 && match_length-run-encoded_size[i-0xf0]>=gain){
						gain=match_length-run-encoded_size[i-0xf0];
						long_pos=match_pos;
						long_length=match_length;
						long_method=i;
					}
				}
				break;
				
			case 0xf5:
				min_match=0x4;
				if(start+1<size){
					ch1=buffer[start];
					run=1;
					while(ch1==buffer[start+2*run]){
						if(start+run*2>=size) break;
						run++;
						if(run>=0xff+min_match) break;				
					}
					match_length=run*2;
					
					if(match_length>=min_match*2 && match_length-run*2-encoded_size[i-0xf0]>=gain){
						gain=match_length-run*2-encoded_size[i-0xf0];
						long_pos=match_pos;
						long_length=match_length;
						long_method=i;
					}
				}
				break;

			case 0xf7:
				min_match=0x2;
				if(start+1<size && start+2<size){
					ch1=buffer[start];
					ch2=buffer[start+1];
					ch3=buffer[start+2];
					run=1;
					while(ch1==buffer[start+4*run] && ch2==buffer[start+1+4*run] && ch3==buffer[start+2+4*run]){
						if(start+run*4>=size) break;
						run++;
						if(run>=0xff+min_match) break;				
					}
					match_length=run*4;
					
					if(match_length>=min_match*4 && match_length-run*2-encoded_size[i-0xf0]>=gain){
						gain=match_length-run*2-encoded_size[i-0xf0];
						long_pos=match_pos;
						long_length=match_length;
						long_method=i;
					}
				}
				break;
			case 0xfc:			//fc
				min_match = 0x4;
				max_match = 0x0f + min_match;
				window_size = 0xfff;
				Find_LZ( start, byte, match_length, match_pos, i );
				if(match_length-encoded_size[i-0xf0]>=gain){
					gain=match_length-encoded_size[i-0xf0];
					long_pos=match_pos;
					long_length=match_length;
					long_method=i;
				}
				break;
			case 0xfd:			//fd
				min_match = 0x14;
				max_match = 0xff + min_match;
				window_size = 0xff;
				Find_LZ( start, byte, match_length, match_pos, i );
				if(match_length-encoded_size[i-0xf0]>=gain){
					gain=match_length-encoded_size[i-0xf0];
					long_pos=match_pos;
					long_length=match_length;
					long_method=i;
				}
				break;
			case 0xfe:		//fe			//pos must %8==0 (0~120)
				min_match = 0x3;
				max_match = 0x0f + min_match;
				window_size = 0x7f;
				Find_LZ( start, byte, match_length, match_pos, i );
				if(match_length-encoded_size[i-0xf0]>=gain){
					gain=match_length-encoded_size[i-0xf0];
					long_pos=match_pos;
					long_length=match_length;
					long_method=i;
				}
				break;
			default:
				break;
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
		method=Check( start, buffer[ start ], length, pos );

		// Slightly increase ratio performance
		for( lcv = 0; lcv < LOOKAHEAD; lcv++ ) {
			start++; table[ buffer[ start ] ].push_back( start );
			Check( start, buffer[ start ], future_length[ lcv ], future_pos[ lcv ]);
		}

		// Un-do lookahead
		for( lcv = LOOKAHEAD; lcv > 0; lcv-- ) {
			table[ buffer[ start ] ].pop_back(); start--;
			if( future_length[ lcv-1 ] - length >= lcv )
				length = 0;
		}
		
		switch(method){
			case 0xf0:
				min_match=0x3;
				break;
			case 0xf1:
				min_match=0x4;
				break;
			case 0xf3:
				min_match=0x2*2;
				break;
			case 0xf4:
				min_match=0x2*3;
				break;
			case 0xf5:
				min_match=0x4*2;
				break;
			case 0xf7:
				min_match=0x2*4;
				break;
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
	//if(method==0xfe && (start-pos)%8!=0){printf("start:%x,len:%x,pos:%x,",start,length,pos);getch();}
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
			
			//encode_bytes += encoded_size[method-0xfc];
		}
		else {
			// No substrings found; try again
			start++;

			//encode_bytes++;
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
				case 0xf0:
					temp=((lz.len-3)|(buffer[start]<<4));
					fputc(temp,out);
					break;
				case 0xf1:
					fputc(lz.len-4,out);fputc(buffer[start],out);
					break;
				case 0xf3:
					fputc((lz.len/2)-2,out);fputc(buffer[start],out);fputc(buffer[start+1],out);
					break;
				case 0xf4:
					fputc((lz.len/3)-2,out);fputc(buffer[start],out);fputc(buffer[start+1],out);fputc(buffer[start+2],out);
					break;
				case 0xf5:
					fputc((lz.len/2)-4,out);fputc(buffer[start],out);
					for(temp=0;temp<lz.len/2;temp++) fputc(buffer[start+1+2*temp],out);
					break;
				case 0xf7:
					fputc((lz.len/4)-2,out);fputc(buffer[start],out);fputc(buffer[start+1],out);fputc(buffer[start+2],out);
					for(temp=0;temp<lz.len/4;temp++) fputc(buffer[start+3+4*temp],out);
					break;
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
