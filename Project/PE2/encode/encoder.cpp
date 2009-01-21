//������PE2,windows��䲻ͬ��һ��lz,��ͷ���ַ���ԭ�����
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
	char name[128];
	char outname[128];
	printf("input infile\n");
	scanf("%s",name);
	sprintf(outname,"%s.enc",name);
	in=fopen(name,"rb");
	out=fopen(outname,"wb");
	
	CacheLZEncode(in,out);
	fclose(in);fclose(out);
	return 0;
}

