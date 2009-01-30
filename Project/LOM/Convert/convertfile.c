
#include <stdio.h>
#include <fcntl.h>
#include <string.h>
#include <stdlib.h>
#include <conio.h>

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
			strcpy(write_buffer,"没翻{end}{N}{N}");
		}
//该段文本结束,去掉最后的两个"{N}",并写入 
	    len=strlen(write_buffer);
		write_buffer[len-6]='\0';
	    strcat(write_buffer,"\n");
	    return 	1;
}

int main(){
	const char *atlas="#VAR(table,TABLE)\n#ADDTBL(\"lom.tbl\",table)\n#ACTIVETBL(table)\n#VAR(MyPtr,CUSTOMPOINTER)\n#CREATEPTR(MyPtr,\"LINEAR\",";
	unsigned char *s,*jmpaddr,readtxt_buffer[1000],readptr_buffer[100],readjp_buffer[1000],buffer[8000],jp_buffer[8000],
		write_buffer[9900],filename[100],char_buffer[60],filloffset[100],orgname[100];
	unsigned int i,j,k,START,END,script_end,offset,repeat_ptr,ptr,repeat_ptr_flag;	
	FILE *txt_fp,*ptr_fp,*convert_fp,*batch_fp;
	
	printf("input file base name to convert:");
	scanf("%s",orgname);
	
	printf("input txtfile number_start number_end to convert:");
	scanf("%d %d",&START,&END);
	
	batch_fp=fopen("atlas_batch.bat","wt");
	fprintf(batch_fp,"del log.txt\n");

while(START!=0)
{
		
for(j=START;j<(END+1);j++) {

	sprintf(filename,"txt\\%s_%02d.txt",orgname,j);
	if((txt_fp=fopen(filename,"rt"))==NULL){
		printf("error on open %s!\n",filename);
		continue;
	}
	
	sprintf(filename,"ptr\\%s.BIN.de_ptr%02d.txt",orgname,j);
	if((ptr_fp=fopen(filename,"rt"))==NULL){
		printf("error on open %s!\n",filename);
		getch();
		return 1;
	}
	
	sprintf(filename,"conv\\converted_%s_%d.txt",orgname,j);
	if((convert_fp=fopen(filename,"wt"))==NULL){
		printf("error on open %s!\n",filename);
		getch();
		return 1;
	}	

	fputs(atlas,convert_fp);
	fgets(readptr_buffer,99,ptr_fp);
	s=strtok(readptr_buffer,",");
	s=strtok(NULL,",");
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

	

  fgets(readtxt_buffer,990,txt_fp);

  
	while(!feof(txt_fp)){
//遇到一个"#### "符号
		repeat_ptr_flag=0;
		if(strstr(readtxt_buffer,"#### ")){
			fgets(readptr_buffer,99,ptr_fp);
			strcpy(write_buffer,"\0");
			if(strchr(readptr_buffer,'>')!=0) repeat_ptr_flag=1;
			s=strtok(readptr_buffer,",");
			if(repeat_ptr_flag)	repeat_ptr=strtoul(strtok(NULL,">"),NULL,10);
			else repeat_ptr=1;
			s=strtok(NULL,"$");
			ptr=strtoul(strtok(NULL,":"),NULL,16);
			while(repeat_ptr>0){
				sprintf(buffer,"#WRITE(MyPtr,$%X)\n",ptr);
				strcat(write_buffer,buffer);
				ptr-=2;
				repeat_ptr--;
			}
			s=strtok(NULL,",");
			s=strtok(NULL,")");
			strcpy(filloffset,s);
		}
				
		fgets(readtxt_buffer,990,txt_fp);
		strcpy(buffer,"\0");
		
		convert_read(buffer,readtxt_buffer,txt_fp);
		
		strcat(write_buffer,buffer);
	    fputs(write_buffer,convert_fp);
    }


	fprintf(convert_fp,"#FILLTO(%s)\n<$00>\n",filloffset);

	fclose(txt_fp);
	fclose(ptr_fp);
	fclose(convert_fp);
	
	fprintf(batch_fp,"echo ########### import %s >>log.txt\n",filename);
	fprintf(batch_fp,"atlas %s.BIN.de %s >>log.txt\n",orgname,filename);	
	
}


	orgname[0]=0;
	START=0;
	printf("input file base name to convert:");
	scanf("%s",orgname);
	
	printf("input txtfile number_start number_end to convert:");
	scanf("%d %d",&START,&END);
}

	fclose(batch_fp);
	printf("finish converting");
	getch();
	return 1;
}

