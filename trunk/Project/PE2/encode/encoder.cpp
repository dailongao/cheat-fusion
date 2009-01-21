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
unsigned char write_buf[0x200000];


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
			if(ptr==cache_count) break;
			
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
	int write_ptr=0;
	
	fread( buffer, 1, 0x100000, in );
	size = ftell( in );
	fseek( in, 0, SEEK_SET );
	start = 0;

	while( start < size ) {
		int future_length;
		int future_pos;

		int length;
		int pos;

		// Prepare for LZ
		CachePush(start);
		// Go find the longest substring match (and future ones)
		CacheLZFind( start, buffer[ start ], length, pos );
		
		if( length >= min_match ) {
/* 			if(start>=0x119 && start<=0x120)
			{
				printf("start:%x;pos:%02x,len:%02x,cachecount:%02x\n",start,pos,length,cache_count);
				FILE *debug;
				debug=fopen("debug.bin","wb");
				fwrite(cache,1,0x100,debug);
				fclose(debug);
				getch();
			} */
			write_buf[write_ptr++]=00;
			write_buf[write_ptr++]=pos;
			write_buf[write_ptr++]=length-min_match;
			
			// Need to add to LZ table
			for( int lcv = 1; lcv < length; lcv++ )
				CachePush(start + lcv);
			
			// Fast update
			start += length;
		}
		else {
			write_buf[write_ptr++]=01;
			write_buf[write_ptr++]=buffer[start];
			start++;
		}
	}
	write_buf[write_ptr++]=0xff;
	//fwrite(write_buf,1,write_ptr,out);
}

void ConvertEncode( FILE *infile, FILE *outfile )
{
	int outc=0,count=0;
	unsigned int flag,bits,lastch,ch,nextch,testch;
	
	lastch=0;
	bits=8;
	while(write_buf[count]!=0xff){
		bits--;
		if(write_buf[count]==1){
			ch=write_buf[count+1];
			testch=(1<<bits)|(ch>>(8-bits))|(lastch<<bits)&0xff;
			buffer[outc++]=testch;
			lastch=ch;
			count+=2;
		}
		else if(write_buf[count]==0){
			ch=write_buf[count+1];
			testch=(ch>>(8-bits))|(lastch<<bits)&((1<<bits)^0xff);
			buffer[outc++]=testch;
			lastch=ch;
			ch=write_buf[count+2];
			testch=(ch>>(8-bits))|(lastch<<bits)&((1<<bits)^0xff);
			buffer[outc++]=testch;
			lastch=ch;
			count+=3;
		}
		else{
			printf("error!\n");
		}
		//printf("last:%02x,%02x;",lastch,testch);getch();
		if(bits==0) bits=8;
	}	
	fwrite(buffer,1,outc,outfile);
}

int main()
{
	FILE *in,*out;
	char name[128];
	char outname[128];
	printf("input infile\n");
	scanf("%s",name);
	sprintf(outname,"%s.enc",name);
	in=fopen(name,"rb");
	out=fopen(outname,"wb");
	
	CacheLZEncode(in,out);
	ConvertEncode(in,out);
	
	fclose(in);fclose(out);
	return 0;
}

