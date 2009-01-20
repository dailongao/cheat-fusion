//适用于PE2,windows填充不同于一般lz,到头后又返回原点填充
#include <stdio.h>
#include <fcntl.h>
#include <string.h>
#include <stdlib.h>
#include <conio.h>
#include <memory.h>

#include <vector>
#include <deque>

using namespace std;


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

deque<int> table[256];

int ptr = 0;
int size = 0;

int min_match = 2;
int window_size = 0x100-1;
int max_match = 15+2;

unsigned char cache[0x100];
int cache_count = 1;

void CachePush(unsigned int number)
{
	if(number>window_size){
		table[ cache[cache_count] ].pop_front();
	}
	table[ buffer[number] ].push_back(cache_count);
	cache[cache_count++]=buffer[number];
	cache_count&=window_size;
}

void CacheLZFind( int start, unsigned char byte, int &length, int &pos )
{
	int longest_length = 0;
	int longest_ptr = 0;
	deque<int>::iterator it;

	// use cached positions to quickly look through the file
	for(it=table[ byte ].begin(); it!=table[ byte ].end(); it++) {
		int match_length = 0;
		int ptr = *it;
		int base = ptr;
		
		if( ptr >= start ) break;	
			
		// search for longest identical substring
		for( int lcv2 = start; ( lcv2 < start + max_match ) && ( lcv2 < size ); lcv2++ ) {
			// look for a mismatch
			if( buffer[ lcv2 ] != cache[ ptr ] )
				break;
				
			if((start>=window_size) && (ptr>window_size)) break;
			if((start<window_size) && (ptr>=cache_count)) break;
			if((cache_count>=base) && (ptr>=cache_count)) break;//prevent next char overwrited before copy
			
			// keep looking
			ptr++;
			match_length++;
		}

		// record new long find
		if( longest_length <= match_length ) {
			longest_ptr = *it;
			longest_length = match_length;
		}
	}

	// output findings
	length = longest_length;
	pos = longest_ptr;
}

void CacheLZEncode( FILE *in, FILE *out )
{
	int start;
	int lcv;
	int counter = 0;

	fread( buffer, 1, 0x100000, in );
	size = ftell( in );
	fseek( in, 0, SEEK_SET );
	start = 0;

	while( start < size ) {
		int future_length[10];
		int future_pos[10];

		int length;
		int pos;

		// Prepare for LZ
		CachePush(start);
		// Go find the longest substring match (and future ones)
		CacheLZFind( start, buffer[ start ], length, pos );
		
		if( length >= min_match ) {
/*			if(start>=0xc5a && start+length<=0xc60){
				printf("start:%x;pos:%02x,len:%02x,cachecount:%02x\n",start,pos,length,cache_count);
				FILE *debug;
				debug=fopen("debug.bin","wb");
				fwrite(cache,1,0x100,debug);
				fclose(debug);
				getch();
			}*/
			
			fputc(0x00,out);
			fputc(pos,out);
			fputc(length-min_match,out);
			
			// Need to add to LZ table
			for( int lcv = 1; lcv < length; lcv++ )
				CachePush(start + lcv);
			
			// Fast update
			start += length;
		}
		else {
			fputc(0x01,out);
			fputc(buffer[start],out);
			start++;
		}
	}
	fputc(0xff,out);
}


int main()
{
	FILE *in,*out;
	in=fopen("zk_dec.enc","rb");
	out=fopen("zk_dec.enc.dec","wb");
	
	LZ_Decode(in,0,out);
	//CacheLZEncode(in,out);
	fclose(in);fclose(out);
	return 0;
}

