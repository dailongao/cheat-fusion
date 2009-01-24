//适用于PE2,windows填充不同于一般lz,到头后又返回原点填充
//PE2的压缩格式,以bit顺序线型排列,所以压缩时候先将各个字节bit释放成bit流,最后一起组合,方便操作
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
unsigned char bitpool[0x1000];

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
		
		if( ptr > start ) break;
		if(ptr==0) continue;
			
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

int read_count=0;
int ResolveByte()
{
	int i,bitcount=0;
	unsigned int test;
	while(1)
	{
		test=write_buf[read_count++];
		if(test==1){
			bitpool[bitcount++]=test;
			test=write_buf[read_count++];
			for(i=0;i<8;i++)
				bitpool[bitcount++]=(test>>(7-i))&0x1;
			if(bitcount%8==0) return bitcount;
		}
		else if(test==0){
			bitpool[bitcount++]=test;
			test=write_buf[read_count++];
			for(i=0;i<8;i++)
				bitpool[bitcount++]=(test>>(7-i))&0x1;
			test=write_buf[read_count++];
			for(i=0;i<4;i++)
				bitpool[bitcount++]=(test>>(3-i))&0x1;
			if(bitcount%8==0) return bitcount;
		}
		else{
			bitpool[bitcount++]=0;
			return bitcount;
		}
	}
}

void Encode( FILE *infile, FILE *outfile )
{
	int bc;
	int i,j,k,ch;
	while(1){
		bc=ResolveByte();
		if(bc%8==0){
			for(i=0;i<bc;i+=8){
				ch=0;
				for(j=0;j<8;j++)
					ch|=(bitpool[i+j]<<(7-j));
				fputc(ch,outfile);
			}
		}
		else{
			ch=0;
			i=bc&7;
			for(j=0;j<8;j++){
				if((i+j)<bc)
					ch|=(bitpool[i+j]<<(7-j));
			}
			fputc(ch,outfile);
			break;
		}
	}
	fputc(0,outfile);fputc(0,outfile);
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
	Encode(in,out);
	
	fclose(in);fclose(out);
	return 0;
}

