#include <stdio.h>
#include <fcntl.h>
#include <string.h>
#include <stdlib.h>
#include <conio.h>
#include <dirent.h>

#define WORKDIR "dattxt\\"

const char *atlas="#VAR(table,TABLE)\n#ADDTBL(\"sh.tbl\",table)\n#ACTIVETBL(table)\n";

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
	
	int nline=0;
	
	fgets(readtxt_buffer,990,txt_fp);
	write_buffer[0]=0;
	
//读取文本直到遇上下一个"#### "符号或者文件结束,说明该段文本结束 
		while( !strstr(readtxt_buffer,"#### ") && !feof(txt_fp) )
		{
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
					strcat(readtxt_buffer,"\n{N}");
					strcat(write_buffer,readtxt_buffer);
                 }
                 else strcat(write_buffer,"{N}");	//处理空行
		   }

		   fgets(readtxt_buffer,990,txt_fp);	   
	    }
		
/*		if(strstr(write_buffer,"{end}")==0){
			strcpy(write_buffer,"{N}{N}");
		}*/
		{
//该段文本结束,去掉最后的两个"{N}",并写入 
			len=strlen(write_buffer);
			write_buffer[len-6]='\0';
			strcat(write_buffer,"\n");
		}
	    return 	1;
}

unsigned int convert_readptr(char *write_buffer,char * readptr_buffer)
{	
	strcpy(write_buffer,strtok(readptr_buffer,">"));
	strcat(write_buffer,"\n");
	return 0;
}

int main(){
	unsigned char *s,readtxt_buffer[1500],readptr_buffer[200],buffer[20000],
		write_buffer[20000],filename[100],char_buffer[60],readlist[100],orgname[100];
	unsigned int i,j,k,script_end,offset,ptr;	
	FILE *txt_fp,*convert_fp,*batch_fp,*org_fp;
	DIR *dir;
	struct dirent *p;
	
	strcpy(orgname,"PSXCD.IMG");

	if((batch_fp=fopen("fix_batch.bat","wt"))==NULL){
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
	
	strcpy(filename,"conv\\conv_");
	strcat(filename,p->d_name);
	if((convert_fp=fopen(filename,"wt"))==NULL){
		printf("error on open %s!\n",filename);
		getch();
		return 1;
	}
	
	fputs(atlas,convert_fp);
	fgets(readtxt_buffer,990,txt_fp);  
	while(!feof(txt_fp)){
//遇到一个"#### "符号
		if(strstr(readtxt_buffer,"#### ")){
			strcpy(readptr_buffer,strstr(readtxt_buffer,"<")+1);
			script_end=convert_readptr(write_buffer,readptr_buffer);
		}
		convert_read(buffer,readtxt_buffer,txt_fp);

		fputs(write_buffer,convert_fp);
		fputs(buffer,convert_fp);
    }

	fprintf(batch_fp,"echo #######import %s####### >>log.txt\n",filename);
	fprintf(batch_fp,"atlas %s ..\\convert\\%s >>log.txt\n",orgname,filename);

	fclose(txt_fp);
	fclose(convert_fp);
}

	fclose(batch_fp);
	printf("finish converting");
	getch();
	return 1;
}



