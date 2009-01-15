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
//��ȡ�ı�ֱ��������һ��"#### "���Ż����ļ�����,˵���ö��ı����� 
		while( !strstr(readtxt_buffer,"#### ") && !feof(txt_fp) ){
 //������link���ŵ��ı�                                          
		   if((strchr(readtxt_buffer,':'))){			
					strcpy(buffer,strtok(readtxt_buffer,":"));
					strcat(buffer,"}");

//�ж�link���Ʒ�
					s=strtok(NULL,"}");
					convert_linkstr(linkbuffer,s);
					strcat( buffer, linkbuffer );
					s=strtok(NULL,"\a");
					while((strchr(s,':'))){
						strcat(buffer,strtok(s,":"));
						strcat(buffer,"}");
										
//�ж�link���Ʒ�	
						s=strtok(NULL,"}");					
						convert_linkstr(linkbuffer,s);
						strcat( buffer, linkbuffer );
						s=strtok(NULL,"\a");
					
					}
					strcat(buffer,s);
//ȥ�����Ļس� 
					strtok(buffer,"\n");
					
					strcat(buffer,"{N}");
					strcat(write_buffer,buffer);					
		   }
//һ���ı�,�ı����б�{N},�����з��ű�{N}
		   else {
                 if(strcmp(readtxt_buffer,"\n")){
                 	strtok(readtxt_buffer,"\n");
					strcat(readtxt_buffer,"{N}");
					strcat(write_buffer,readtxt_buffer);					
                 }
                 else strcat(write_buffer,"{N}");	//�������
		   }
		   fgets(readtxt_buffer,990,txt_fp);	   
	    }	    
//�ö��ı�����,ȥ����������"{N}",��д�� 
	    len=strlen(write_buffer);
		write_buffer[len-6]='\0';
	    strcat(write_buffer,"\n");
	    return 	1;
}

int main(){
	const char *atlas="#VAR(table,TABLE)\n#ADDTBL(\"gensui3.tbl\",table)\n#ACTIVETBL(table)\n#VAR(MyPtr,CUSTOMPOINTER)\n#CREATEPTR(MyPtr,\"LINEAR\",";
	unsigned char *s,*jmpaddr,readtxt_buffer[1000],readptr_buffer[100],readjp_buffer[1000],buffer[8000],jp_buffer[8000],
		write_buffer[20000],filename[100],char_buffer[60],readlist[100],binname[16],ptradr[16];
	unsigned int i,j,k,START,END;	
	FILE *txt_fp,*ptr_fp,*convert_fp,*batch_fp;
	
	printf("input BIN name(VDZK,AKMT,...)\n");
	scanf("%s",binname);
	printf("input file number_start number_end to convert\n");
	scanf("%d %d",&START,&END);

	if((batch_fp=fopen("atlas_batch.bat","wt"))==NULL){
		printf("error on create batch_file!\n");
		return 1;
	}
	fprintf(batch_fp,"del log.txt\n");

for(j=START;j<(END+1);j++)
{
	sprintf(filename,"txt\\%s_%04d.txt", binname, j);
	if((txt_fp=fopen(filename,"rt"))==NULL){
		printf("error on open %s!\n",filename);
		continue;
	}
	sprintf(filename,"ptr\\%s_%04d.ptr", binname, j);
	if((ptr_fp=fopen(filename,"rt"))==NULL){
		printf("error on open %s!\n",filename);
		getch();
		return 1;
	}
	
	sprintf(filename,"conv\\cvt_%s_%04d.txt", binname, j);
	if((convert_fp=fopen(filename,"wt"))==NULL){
		printf("error on open %s!\n",filename);
		getch();
		return 1;
	}
	
	fputs(atlas,convert_fp);
	fgets(readptr_buffer,99,ptr_fp);
	s=strtok(readptr_buffer,":");
	s=strtok(NULL,";");
	fputs(s,convert_fp);
	fputs(",32)\n",convert_fp);

	fgets(readptr_buffer,99,ptr_fp);
	strcpy(buffer,readptr_buffer);
	s=strtok(readptr_buffer,":");
	s=strtok(NULL,",");
	fputs(s,convert_fp);
	fputs(")\n",convert_fp);
	
	s=strtok(buffer,",");
	s=strtok(NULL,")");
	strcpy(ptradr,s);
	
	fseek(ptr_fp,0,0);	
	fgets(readptr_buffer,99,ptr_fp);

	fgets(readtxt_buffer,990,txt_fp);
	while(!feof(txt_fp)){
//����һ��"#### "����
		if(strstr(readtxt_buffer,"#### ")){
			fgets(readptr_buffer,99,ptr_fp);
			strcpy(write_buffer,"#TEST($0,$8)\n");
			s=strtok(readptr_buffer,">");
			s=strtok(NULL,":");
			strcat(write_buffer,s);
			strcat(write_buffer,"\n");
		}
				
		fgets(readtxt_buffer,990,txt_fp);
		strcpy(buffer,"\0");
		
		convert_read(buffer,readtxt_buffer,txt_fp);
		
		strcat(write_buffer,buffer);
	    fputs(write_buffer,convert_fp);
    }

	fprintf(convert_fp,"#FILLTO(%s)\n<$00>\n",ptradr);
	fprintf(batch_fp,"echo #######import %s####### >>log.txt\n",filename);
	fprintf(batch_fp,"atlas %s.BIN %s >>log.txt\n",binname,filename);

	fclose(txt_fp);
	fclose(ptr_fp);
	fclose(convert_fp);	
}

	fclose(batch_fp);
	printf("finish converting");
	getch();
	return 1;
}

