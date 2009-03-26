//{skip}控制符要注意
#include <stdio.h>
#include <fcntl.h>
#include <string.h>
#include <stdlib.h>
#include <conio.h>
#include <dirent.h>

#define WORKDIR "txt\\"

typedef struct{
	char cmd[64];
	char *p;
	int repeat_no;
} atlas_block;

atlas_block cmdblock[2048];
unsigned char txtpool[0x400000];
unsigned char *txtpool_ptr=txtpool;
int block_count=0;

int add_block(char *txtbuf, char *ptrbuf)
{
	strcpy(cmdblock[block_count].cmd,ptrbuf);
	cmdblock[block_count].repeat_no=block_count;
	cmdblock[block_count].p=txtpool_ptr;
	
	int i;
	for(i=0;i<block_count;i++){
		if(cmdblock[i].repeat_no!=i) continue;
		if(strcmp(cmdblock[i].p,txtbuf)==0){
			cmdblock[block_count].repeat_no=i;
			cmdblock[block_count].p=NULL;
			break;
		}
	}
	if(cmdblock[block_count].p!=NULL){
		int len=strlen(txtbuf);
		memcpy(txtpool_ptr,txtbuf,len+1);
		txtpool_ptr+=len+1;
	}	
	block_count++;
	return 0;
}

int write_block(FILE *fp)
{
	int i,j;
	for(i=0;i<block_count;i++){
		if(cmdblock[i].p!=NULL){
			for(j=0;j<block_count;j++){
				if(cmdblock[j].repeat_no==i){
					fputs(cmdblock[j].cmd,fp);
				}
			}
			fputs(cmdblock[i].p,fp);
		}
	}
	return 0;
}

char * convert_linkstr(char * buffer,char * string)
{
	unsigned int i,j=0;
	for(i=0;string[i]!='\0';i+=2){
		buffer[j++]='<';
		buffer[j++]='$';
		buffer[j++]=string[i];
		buffer[j++]=string[i+1];
		buffer[j++]='>';
	}
	buffer[j]='\0';
	return buffer;
}

int convert_read(char *write_buffer,char * readtxt_buffer, FILE * txt_fp)
{
	unsigned int len,lenth,i,j,m;
	unsigned char linkbuffer[200],*s,buffer[4000],link[50],temp[50];
	
	fgets(readtxt_buffer,990,txt_fp);
	strcpy(write_buffer,"\0");
	
//读取文本直到遇上下一个"#### "符号或者文件结束,说明该段文本结束 
		while( !strstr(readtxt_buffer,"#### ") && !feof(txt_fp) ){
 //处理含有link符号的文本                                          
		   if((strchr(readtxt_buffer,':'))){			
					strcpy(buffer,strtok(readtxt_buffer,":"));
					strcat(buffer,"}");

//判断link控制符
					s=strtok(NULL,"}");
					convert_linkstr(linkbuffer,s);
					strcat( buffer, linkbuffer );
					s=strtok(NULL,"\a");
					while((strchr(s,':'))){
						strcat(buffer,strtok(s,":"));
						strcat(buffer,"}");
										
//判断link控制符	
						s=strtok(NULL,"}");					
						convert_linkstr(linkbuffer,s);
						strcat( buffer, linkbuffer );
						s=strtok(NULL,"\a");
					
					}
					strcat(buffer,s);
//去掉最后的回车 
					strtok(buffer,"\n");
					
					strcat(buffer,"{N}");
					strcat(write_buffer,buffer);					
		   }
//一般文本,文本空行变{N},遇换行符号变{N}
		   else {
                 if(strcmp(readtxt_buffer,"\n")){
                 	strtok(readtxt_buffer,"\n");
					strcat(readtxt_buffer,"{N}");
					strcat(write_buffer,readtxt_buffer);					
                 }
                 else strcat(write_buffer,"{N}");	//处理空行
		   }
		   fgets(readtxt_buffer,990,txt_fp);	   
	    }
		
		if(strstr(write_buffer,"{end}")==0){
			strcpy(write_buffer,"{end}{N}{N}");
		}
/*//该段文本结束,先看看有没有{skip}		
		if((strstr(write_buffer,"{skip}"))){
			s=strstr(write_buffer,"{skip}");
			s[6]=0;
			strcat(write_buffer,"\n");
		}
		else*/
		{
//该段文本结束,去掉最后的两个"{N}",并写入 
			len=strlen(write_buffer);
			write_buffer[len-6]='\0';
			strcat(write_buffer,"\n");
		}
	    return 	1;
}

unsigned int convert_readptr(char *write_buffer,char * readptr_buffer, FILE * ptr_fp)
{
	fgets(readptr_buffer,99,ptr_fp);
	strcpy(write_buffer,"\0");
	unsigned int jmp_end;
	
	strtok(readptr_buffer,">");
	strcpy(write_buffer,strtok(NULL,":"));
	strcat(write_buffer,"\n");
	strtok(NULL,",");
	char *s=strtok(NULL,")");
	jmp_end=strtoul(s+1,NULL,16);
	return jmp_end;
}

int main(){
	const char *atlas="#VAR(table,TABLE)\n#ADDTBL(\"lom.tbl\",table)\n#ACTIVETBL(table)\n#VAR(MyPtr,CUSTOMPOINTER)\n#CREATEPTR(MyPtr,\"LINEAR\",";
	unsigned char *s,readtxt_buffer[1000],readptr_buffer[100],buffer[9500],
		write_buffer[12000],filename[100],char_buffer[60],readlist[100],orgname[100];
	unsigned int i,j,k,script_end,offset,ptr;	
	FILE *txt_fp,*ptr_fp,*convert_fp,*batch_fp,*org_fp;
	DIR *dir;
	struct dirent *p;
	
	strcpy(orgname,"F0175.BIN");

	if((batch_fp=fopen("atlas_batch.bat","wt"))==NULL){
		printf("error on create batch_file!\n");
		return 1;
	}
	fprintf(batch_fp,"del log.txt\n");
	mkdir("conv");

dir=opendir(WORKDIR);
while( (p=readdir(dir))!=NULL )
{
	if(strstr(p->d_name,".txt")==0) continue;

	strcpy(filename,WORKDIR);
	strcat(filename,p->d_name);
	if((txt_fp=fopen(filename,"rt"))==NULL){
		printf("error on open %s!\n",filename);
		continue;
	}
	
	strcpy(char_buffer,p->d_name);
	*strstr(char_buffer,".txt")=0;
	sprintf(filename,"ptr\\%s.ptr",char_buffer);
	if((ptr_fp=fopen(filename,"rt"))==NULL){
		printf("error on open %s!\n",filename);
		getch();
		return 1;
	}
	
	strcpy(filename,"conv\\converted_");
	strcat(filename,p->d_name);
	if((convert_fp=fopen(filename,"wt"))==NULL){
		printf("error on open %s!\n",filename);
		getch();
		return 1;
	}
	
	fputs(atlas,convert_fp);
	fgets(readptr_buffer,99,ptr_fp);
	s=strtok(readptr_buffer,",");
	s=strtok(NULL,")");
	fputs(s,convert_fp);
	fputs(",16)\n",convert_fp);
	fseek(ptr_fp,0,0);

	fgets(readptr_buffer,99,ptr_fp);
	s=strtok(readptr_buffer,":");
	s=strtok(NULL,",");
	fputs(s,convert_fp);
	fputs(")\n",convert_fp);
	fseek(ptr_fp,0,0);	

	txtpool_ptr=txtpool;
	block_count=0;

	fgets(readtxt_buffer,990,txt_fp);  
	while(!feof(txt_fp)){
//遇到一个"#### "符号
		if(strstr(readtxt_buffer,"#### ")){
			script_end=convert_readptr(write_buffer,readptr_buffer,ptr_fp);
		}		
		convert_read(buffer,readtxt_buffer,txt_fp);

		add_block(buffer,write_buffer);
    }	
	write_block(convert_fp);

	fprintf(convert_fp,"#FILLTO($%X)\n<$00>\n",script_end);
	fprintf(batch_fp,"echo #######import %s####### >>log.txt\n",filename);
	fprintf(batch_fp,"atlas %s %s >>log.txt\n",orgname,filename);

	fclose(txt_fp);
	fclose(ptr_fp);
	fclose(convert_fp);
}

	fclose(batch_fp);
	printf("finish converting");
	getch();
	return 1;
}

