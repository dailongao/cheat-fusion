//�����ı�ָ���ת��,��Ҫ������ԭ�ı��ͷ����ı�,ÿ�����ָ��һһ��Ӧ
//����link���Ʒ�����,�ڿ��Ʒ�ǰ�����#test����,���atlas����

#include <stdio.h>
#include <fcntl.h>
#include <string.h>
#include <stdlib.h>
#include "Conio.h"

unsigned int cmd_flag;

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
	unsigned char linkbuffer[200],*s,buffer[2000],link[50],temp[50];
//��ȡ�ı�ֱ��������һ��"#### "���Ż����ļ�����,˵���ö��ı����� 
		while( !strstr(readtxt_buffer,"#### ") && !feof(txt_fp) ){
 //������link���ŵ��ı�                                          
		   if((strchr(readtxt_buffer,':'))){			
					strcpy(buffer,strtok(readtxt_buffer,":"));

//�ж�link���Ʒ�,���TEST����										
					lenth=strlen(buffer);i=lenth;m=0;
					while(buffer[--i]!='{')	link[m++]=buffer[i];
					buffer[i]='\0';				
					j=1;temp[0]='{';
					while(i++<lenth-1)	temp[j++]=link[--m];
					temp[j++]='}';temp[j]='\0';
					if(temp[1]!='u' && temp[2]!='n'){	//"un1""un2"���Ʒ�ʱ���ÿ��Ƕ��� 
						if(strlen(buffer)==0 && cmd_flag==1) 
							strcat(buffer,"#TEST($1,$2)\n");
						else 
							strcat(buffer,"\n#TEST($1,$2)\n");
					}
					cmd_flag=0;											
					strcat(buffer,temp);				


					s=strtok(NULL,"}");
					convert_linkstr(linkbuffer,s);
					strcat( buffer, linkbuffer );
					s=strtok(NULL,"\a");
					while((strchr(s,':'))){
						strcat(buffer,strtok(s,":"));
						
//�ж�link���Ʒ�,���TEST����
						lenth=strlen(buffer);i=lenth;m=0;
						while(buffer[--i]!='{')	link[m++]=buffer[i];
						buffer[i]='\0';				
						j=1;temp[0]='{';
						while(i++<lenth-1)	temp[j++]=link[--m];
						temp[j++]='}';temp[j]='\0';
						if(temp[1]!='u' && temp[2]!='n'){		//"un1""un2"���Ʒ�ʱ���ÿ��Ƕ���	
							strcat(buffer,"\n#TEST($1,$2)\n");
						}
						strcat(buffer,temp);
					
						s=strtok(NULL,"}");					
						convert_linkstr(linkbuffer,s);
						strcat( buffer, linkbuffer );
						s=strtok(NULL,"\a");
					
					}
					strcat(buffer,s);
//ȥ�����Ļس� 
					lenth=strlen(buffer);				
					while(buffer[--lenth]!='\n');
					buffer[lenth]='\0';
					
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
			cmd_flag=0;	   
	    }	    
//�ö��ı�����,ȥ����������"{N}",��д�� 
	    len=strlen(write_buffer);
		write_buffer[len-6]='\0';
	    strcat(write_buffer,"\n");
	    return 	1;
}	
	
	
int main(){
	const char *atlas="#VAR(table,TABLE)\n#ADDTBL(\"toe.tbl\",table)\n#ACTIVETBL(table)\n#VAR(MyPtr,CUSTOMPOINTER)\n#CREATEPTR(MyPtr,\"LINEAR\",";
	unsigned char *s,*jmpaddr,readtxt_buffer[1000],readptr_buffer[100],readjp_buffer[1000],buffer[8000],jp_buffer[8000],
		write_buffer[9000],filename[80],char_buffer[40],bakptr[100];
	unsigned int i,j,number,previous,START,END,scriptend,no;	
	FILE *txt_fp,*ptr_fp,*convert_fp,*batch_fp,*jptxt_fp,*unpack_fp;
	
	printf("input file number_start number_end to convert\n");
	scanf("%d %d",&START,&END);

	if((batch_fp=fopen("atlas_batch.bat","wt"))==NULL){
		printf("error on create batch_file!\n");
		return 1;
	}
	fprintf(batch_fp,"del log.txt\n");
		
for(j=START;j<(END+1);j++) {
	sprintf(char_buffer,"%04d",j);

	strcpy(filename,"O:\\bin\\hack\\convert\\txt\\");
	strcat(filename,char_buffer);
	strcat(filename,".txt");
	if((txt_fp=fopen(filename,"rt"))==NULL){
		printf("error on open %s!\n",filename);
		continue;
	}

	strcpy(filename,"O:\\bin\\hack\\producelist\\unpack\\");
	strcat(filename,char_buffer);
	strcat(filename,"_unpack.bin");	
	if((unpack_fp=fopen(filename,"rb"))==NULL){
		printf("error on open %s!\n",filename);
		getch();
		return 1;
	}
	
	
	strcpy(filename,"O:\\bin\\hack\\ex\\extext\\ptr_");
	strcat(filename,char_buffer);
	strcat(filename,".txt");
	if((ptr_fp=fopen(filename,"rt"))==NULL){
		printf("error on open %s!\n",filename);
		getch();
		return 1;
	}

	strcpy(filename,"O:\\bin\\hack\\ex\\txt\\");
	strcat(filename,char_buffer);
	strcat(filename,".txt");
	if((jptxt_fp=fopen(filename,"rt"))==NULL){
		printf("error on open %s!\n",filename);
		getch();
		return 1;
	}	
	
	strcpy(filename,"converted_");
	strcat(filename,char_buffer);
	strcat(filename,".txt");
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

	
	previous=0;
  fgets(readtxt_buffer,990,txt_fp);
  fgets(readjp_buffer,990,jptxt_fp);
  
	while(!feof(txt_fp)){
//����һ��"#### "����                         				
		if(strstr(readtxt_buffer,"#### ")){

			s=strtok(readtxt_buffer," ");
			s=strtok(NULL," ");
			number=strtol(s,NULL,10);
//������ı����ܱ��������,��Ӧ��,ָ���ı���ԭ�����ı�ҲҪ����			
			for(i=previous;i<number;i++){
				if(i>previous){
					fgets(readjp_buffer,990,jptxt_fp);
					strcpy(jp_buffer,"\0");
					cmd_flag=1;
					convert_read(jp_buffer,readjp_buffer,jptxt_fp);
		
					s=strtok(readptr_buffer,",");
					s=strtok(NULL,":");			
					strcpy(write_buffer,s);
					strcat(write_buffer,"\n");				
					strcat(write_buffer,jp_buffer);
					fputs(write_buffer,convert_fp);
				}		
				fgets(readptr_buffer,99,ptr_fp);
			}				
			previous=number;								
					
			s=strtok(readptr_buffer,",");
			s=strtok(NULL,":");			
			strcpy(write_buffer,s);
			strcat(write_buffer,"\n");
		}
				
		fgets(readtxt_buffer,990,txt_fp);
		fgets(readjp_buffer,990,jptxt_fp);
		strcpy(jp_buffer,"\0");
		strcpy(buffer,"\0");
		cmd_flag=1;				
		convert_read(jp_buffer,readjp_buffer,jptxt_fp);
		cmd_flag=1;		
		convert_read(buffer,readtxt_buffer,txt_fp);
		
		if(strstr(buffer,"{end}"))	strcat(write_buffer,buffer);
	    else strcat(write_buffer,jp_buffer);
	    fputs(write_buffer,convert_fp);
     }
     
//���jp�ı���û���ļ�β,˵�������ı������ű�jp�ı�С,������jp�ı���ȡ     
    while(!feof(jptxt_fp)) {
    	if(strstr(readjp_buffer,"#### ")) {
    		fgets(readptr_buffer,99,ptr_fp);
    		
			s=strtok(readptr_buffer,",");
			s=strtok(NULL,":");			
			strcpy(write_buffer,s);
			strcat(write_buffer,"\n");
		}
		fgets(readjp_buffer,990,jptxt_fp);
		strcpy(jp_buffer,"\0");
		cmd_flag=1;		
		convert_read(jp_buffer,readjp_buffer,jptxt_fp);
		strcat(write_buffer,jp_buffer);
	    fputs(write_buffer,convert_fp);
	}				
	
	fseek(unpack_fp,8L,0);
	fread(&scriptend,1,4,unpack_fp);
	fprintf(convert_fp,"#FILLTO($%X)\n<$00>\n",scriptend);

	strcat(char_buffer,"_unpack.bin");
	fprintf(batch_fp,"echo #######import %s####### >>log.txt\n",filename);
	fprintf(batch_fp,"atlas O:\\bin\\hack\\Atlas\\unpack\\%s %s >>log.txt\n",char_buffer,filename);

	fclose(unpack_fp);	
	fclose(jptxt_fp);
	fclose(txt_fp);
	fclose(ptr_fp);
	fclose(convert_fp);	
}
	
	fclose(batch_fp);
	printf("finish converting");
	getch();
	return 1;
}	
