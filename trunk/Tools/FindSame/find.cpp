//将重复文本打上〓标记
//2s
#include <stdio.h>
#include <fcntl.h>
#include <string.h>
#include <stdlib.h>
#include <conio.h>
#include <time.h>
#include <dirent.h>

#include <vector>
using namespace std;

#define ORGDIR "txt\\"
#define CONVDIR "checked\\%s"

typedef struct _block_attr{
	char filename[32];
	int num;
	int poolpos;
}block_attr;
vector<block_attr> table[65536];

char *pool;
char buffer[1*1024*1024];
char write_buffer[1*1024*1024];

int findrepeat(char *s, char *repeatfilename, int *repeatnum)
{
	block_attr block;	
	unsigned short byte=((unsigned short)s[0])*256+(unsigned short)s[1];
	for( int lcv = 0; lcv < table[byte].size(); lcv++ ){
		block=table[byte][lcv];			
		if(strcmp(pool+block.poolpos,s)==0){
			strcpy(repeatfilename,block.filename);
			*repeatnum=block.num;
			return 0;
		}
	}
	
	return -1;
}

int main(){

	char *s,readtxt_buffer[1000],filename[100];
	unsigned int i,j,number,flag;
	int blknum,pos=0;
	block_attr block;
	FILE *txt_fp,*convert_fp;
	DIR *dir;
	struct dirent *p;

	pool=(char*)malloc(20*1024*1024);
		mkdir("checked\\");
	time_t start,finish;
	start=time(NULL);

	dir=opendir(ORGDIR);
while( (p=readdir(dir))!=NULL )
{	
	if(p->d_name[0]=='.') continue;
	
	strcpy(filename,ORGDIR);
	strcat(filename,p->d_name);
	if((txt_fp=fopen(filename,"rt"))==NULL){
		printf("error on open %s!\n",p->d_name);
		continue;
	}
	
	strcpy(block.filename,p->d_name);
	*strrchr(block.filename,'.')=0;
	block.poolpos=pos;
	block.num=1;
	blknum=1;
	
	sprintf(filename,CONVDIR,p->d_name);
	convert_fp=fopen(filename,"wt");
	
    fgets(readtxt_buffer,990,txt_fp);
	while(!feof(txt_fp)){
//遇到一个"#### "符号                         				
		if(strstr(readtxt_buffer,"#### ")){
			strcpy(buffer,readtxt_buffer);
			write_buffer[0]=0;
			block.poolpos=pos;
			block.num=blknum;
		}		
		fgets(readtxt_buffer,990,txt_fp);
//读取文本直到遇上下一个"#### "符号或者文件结束,说明该段文本结束 
		while( !strstr(readtxt_buffer,"#### ") && !feof(txt_fp) ){
			strcat(write_buffer,readtxt_buffer);
			fgets(readtxt_buffer,990,txt_fp);
	    }

		int testnum;
		char testfile[32];
		unsigned short byte=(unsigned short)write_buffer[0]*256+(unsigned short)write_buffer[1];
		if(findrepeat(write_buffer,testfile,&testnum)==0){
			sprintf(filename,"〓%s-%d\n",testfile,testnum);
			strcat(buffer,filename);
		}
		else{
			memcpy(pool+pos,write_buffer,strlen(write_buffer)+1);
			pos+=strlen(write_buffer)+1;
			table[byte].push_back(block);
		}
		
		strcat(buffer,write_buffer);
	    fputs(buffer,convert_fp);
		blknum++;
	}

	fclose(txt_fp);
	fclose(convert_fp);
	printf("%s converted!\n",p->d_name);
}
	closedir(dir);
	free(pool);
	finish=time(NULL);
	printf("finish converting,%d bytes\n",pos);
	printf(" %f Secs\n",difftime(finish,start));getch();
	return 1;
}	
