#include <stdio.h>

#define PART_SIZE 127
#define LBA_SIZE  2352
#define LBA_HEADER_SIZE 24
#define LBA_DATA_SIZE 2048
#define HEADER_SIZE 1024

#define INT_REVERSE(x) ((x<<24) | ((x<<8)&0x00ff0000) | ((x>>8)&0x0000ff00) | ((x>>24)&0x000000ff))

void _WritePPFData(FILE* ppfout,FILE* backupout,FILE* isoin,char* fsrc,int offset);
void MakePPF(int listlength, char** flist,int* offsetlist,char* fiso,char* fppf);
