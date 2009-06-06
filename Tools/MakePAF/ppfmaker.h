#include <stdio.h>

#define PART_SIZE 255 //1~255
#define LBA_SIZE  2048
#define LBA_HEADER_SIZE 0
#define LBA_DATA_SIZE 2048


#define INT_REVERSE(x) ((x<<24) | ((x<<8)&0x00ff0000) | ((x>>8)&0x0000ff00) | ((x>>24)&0x000000ff))

void _WritePPFData(FILE* ppfout,FILE* backupout,FILE* isoin,char* fsrc,unsigned long long offset);
void MakePPF(int listlength, char** flist,unsigned long long* offsetlist,char* fiso,char* fppf);

