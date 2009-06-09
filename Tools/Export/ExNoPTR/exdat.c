//没有指针表的文本导出
//list.txt要求:第一行rom名称,第二行tbl名称,接下来行数是导出地址起始-结束
#include <stdio.h>
#include <fcntl.h>
#include <string.h>
#include <stdlib.h>
#include <conio.h>
#include "tbl.h"

#define ALIGN4(adr) ((adr+3)&(~3))
#define MINEXCOUNT 2

int IsScriptStart(unsigned int adr)
{
	if((adr&3) == 0) return 1;
	else return 0;
}

int main()
{
	TBLINFO tbl;
	int found,jump,i,j;
	const char *str;
	char loadtbl[100],outfile[100];

	FILE *list_fp,*rom_fp,*out_fp;
	unsigned int cur,ex_start,ex_end,previous,next,select=0,datcount,filesize,end_count;
	char *temp;	
	unsigned int txt_number=1,found_count=0,hex_count=0,*ptr,check,sect_flag,sect_start=1,sect_end,offset;
	unsigned char char_buffer[40],in_value[16],out_value[6],readlist[1000],
		buffer[30000],unknown_char[10],ctrl_char[80],filename[100];

	if((list_fp=fopen("list.txt","rt"))==NULL){
		printf("error on open list!\n");
		return 1;
	}
	fgets(filename,99,list_fp);
	fgets(loadtbl,99,list_fp);
	strtok(filename,"\n");
	strtok(loadtbl,"\n");
	if((rom_fp=fopen(filename,"rb"))==NULL)	{
		printf("error on open %s\n",filename);
		return 1;
	}
	tbl = TBL_LoadTable(loadtbl);
	if(!tbl) {
		printf("%s\n",TBL_GetErrorString());
		return 1;
	}
	
	fseek(rom_fp,0,SEEK_END);
	filesize=ftell(rom_fp);

j=1;
fgets(readlist,999,list_fp);
while(!feof(list_fp) && strcmp(readlist,"\n")!=0)
{
	char *nextp;
	ex_start=strtoul(readlist,&nextp,16);
	ex_end=strtoul(nextp+1,NULL,16);
	
	strtok(filename,".");
	sprintf(outfile,"%s_%08X.txt",filename,ex_start);
	out_fp=fopen(outfile,"wt");
	txt_number=1;	
		cur=ex_start;
		sect_flag=0;found_count=0;end_count=1;
		for(fseek(rom_fp,cur,0);cur<ex_end && cur<filesize-4;cur+=(4-check),fseek(rom_fp,cur,0) ) {		
			fread(in_value,1,4,rom_fp);		
			out_value[0] = in_value[3];
			out_value[1] = in_value[2];
			out_value[2] = in_value[1];
			out_value[3] = in_value[0];
			out_value[4] = 0;
			out_value[5] = 0;
			out_value[6] = 0;

			for ( check = 0 ; check < 4 ; check++ ) {
	            ptr = (unsigned int *) &out_value[check];
	            found = TBL_GetString(tbl,ptr[0],(4-check),&str);                       
				if(found){
					if(sect_flag==0 && IsScriptStart(cur) && strcmp(str,"{end}")!=0) {
						strcpy(buffer,"\0");
						sect_start=cur;sect_flag=1;
						found_count=0;end_count=0;
					}
					if(sect_flag==1){
						if(strcmp(str,"{end}")==0) {
							sect_end=ALIGN4(cur+(4-check));
							if(found_count>=MINEXCOUNT){
								strcat(buffer,"{end}\n");
								fprintf(out_fp,"#### %d <#JMP($%X,$%X)>####\n%s\n",txt_number++,sect_start,sect_end,buffer);
							}
							else{//返回句子开头,往后检测
								cur=sect_start+4-(4-check);
							}
							sect_flag=0;found_count=0;end_count=1;
						}
						else {
							if(strchr(str,':') && strchr(str,'{') && strchr(str,'}')){
		//如果有link符号
								strcpy(char_buffer,str);					
								strcpy(ctrl_char,strtok(char_buffer,":"));
								strcat(ctrl_char,":");
								temp=strtok(NULL,":");
								jump=strtol(temp,NULL,10);
								fseek(rom_fp,cur+(4-check),0);
								fread(in_value,1,jump,rom_fp);					
								for(i=0;i<jump;i++){
									sprintf(char_buffer,"%02X",in_value[i]);
									strcat(ctrl_char,char_buffer);					
								}
								strcat(ctrl_char,"}");
								strcat(buffer,ctrl_char);								
								cur=cur+jump;
								break;
							}
							else{
								if(strcmp(str,"{N}")==0) strcat(buffer,"\n");
								else strcat(buffer,str);
								found_count++;
							}
						}
					}
					else{
						cur=cur+1-(4-check);
						sect_flag=0;found_count=0;end_count=1;
					}					
					break;
				}
				else{
					if(sect_flag==1 && check==3){
						sprintf(unknown_char,"<$%02X>",ptr[0]);
						strcat(buffer,unknown_char);
						break;
					}
				}
				
				if(check==3) {sect_flag=0;break;}
			}			
		}
		fclose(out_fp);
		
printf("%d exported\n",j++);//getch();
fgets(readlist,999,list_fp);
}

	fclose(list_fp);fclose(rom_fp);
	TBL_CloseTable(tbl);
	printf("finish all exporting");
	getch();
	return 0;
}


