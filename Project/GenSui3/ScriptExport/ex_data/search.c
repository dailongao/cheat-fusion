#include <stdio.h>
#include <fcntl.h>
#include <string.h>
#include <stdlib.h>
#include <conio.h>
#include "tbl.h"

typedef struct _script_block{
	unsigned int text;
	unsigned int text_adr[256];
	unsigned int ptr;
	unsigned int ptr_base_val;
	int ptr_step;
	int count;
} script_block;

typedef struct _export_info{
	FILE *log;
	FILE *batch;
	char name[128];
	int  num;
} export_info;

#define BUFSIZE (1024*32)
int CheckScriptBlock(FILE *fp, unsigned int base, script_block *p, export_info *ex_info)
{
	base = (base + 7) & ~7;
	unsigned char buf[BUFSIZE];
	fseek(fp, base, SEEK_SET);
	fread(buf,1,4,fp);
	if(((buf[0]==0x81 && buf[1]==0x75)||(buf[0]==0x11 && buf[1]==0x40)) && buf[2]!=0 && buf[3]!=0){
		p->text=base;
		p->count=0;
		fread(buf+4,1,BUFSIZE-4,fp);
		int i=0;
		while(((*(unsigned int*)(buf+i))>0x00400000)||((*(unsigned int*)(buf+i))<0x00100000)){
			int zero_count = 0;
			p->text_adr[p->count] = i+base;
			while(zero_count==0){
				while(buf[i]!=0x40){
					if(buf[i]==0 && buf[i+1]==0) return -1;
					i++;
				}
				if(buf[i-1]==0 && buf[i+1]==0) zero_count++;
				i++;
			}
			zero_count=0;
			p->count++;
			if(p->count>=256){
				printf("err!%lx ScriptBlock have 256+ sentences??\n",p->text);getch();
				return -1;
			}
			i+=1;
			i=(i+7)&~7;
		}
		p->ptr=i+base;
		p->ptr_base_val=*(unsigned int*)(buf+i);
		p->ptr_step=4;
		
		if(p->count>1){
			if(*(unsigned int*)(buf+i+4)>0x00400000 || *(unsigned int*)(buf+i+4)<0x00100000)
				p->ptr_step=8;
			for(i=0;i<p->count;i++){
				unsigned int vt2,vt1;
				unsigned int vp2,vp1;
				vt2 = p->text_adr[i];
				vt1 = p->text;
				vp2 = *(unsigned int*)(buf+(p->ptr-base)+i*p->ptr_step);
				vp1 = p->ptr_base_val;
				if((vp2-vp1)!=(vt2-vt1)){
					//fprintf("%lx,%lx;%lx,%lx;",vt2,vt1,vp2,vp1);
					fprintf(ex_info->log,"file:%s,%d err!",ex_info->name,ex_info->num);
					fprintf(ex_info->log,"Block text-ptr(%X-%X) mismatch?\n",p->text_adr[i],p->ptr+i*p->ptr_step);
					break;
				}
			}
		}
		return 0;
	}
	else return -1;
}

int ExportScriptBlock(FILE *rom_fp, script_block *blk, TBLINFO *tableinfo, export_info *ex_info)
{
	TBLINFO tbl=*tableinfo;
	FILE *out_fp,*ptr_fp;
	char filename[128];
	int i;

    sprintf(filename,"ex\\%s_%04d.txt",ex_info->name,ex_info->num);
	if((out_fp=fopen(filename,"wt"))==NULL)	{
		printf("error on create %s!\n",filename);
		return 1;
	}
	
	sprintf(filename,"ex\\ptr\\%s_%04d.ptr",ex_info->name,ex_info->num);
	if((ptr_fp=fopen(filename,"wt"))==NULL)	{
		printf("error on create %s!\n",filename);
		return 1;
	}
	fprintf(ptr_fp,"PointBASE:$%08X;Count:%d\n",blk->ptr_base_val,blk->count);
	
	unsigned int txt_number=1;
	for(i=0;i<blk->count;i++){
		const char *str;
		char *temp;
		int found,jump,j;
		unsigned int cur,end_line,ptr_cur;
		unsigned int found_count=0,hex_count=0,check,*ptr,sect_start,sect_end;
		unsigned char char_buffer[40],in_value[16],out_value[6],
			buffer[20000],unknown_char[10],ctrl_char[80];		
		
		cur=blk->text_adr[i];
		sect_start=cur;
		if(i<blk->count-1) sect_end=blk->text_adr[i+1];
		else sect_end= blk->ptr;
		ptr_cur=blk->ptr+i*blk->ptr_step;
		strcpy(buffer,"\0");	end_line=0;
		for(fseek(rom_fp,cur,0);end_line==0 && cur<sect_end;cur+=(4-check),fseek(rom_fp,cur,0) ) {		
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
					temp=strchr(str,':');
					if( temp){
//Èç¹ûÓÐlink·ûºÅ
						strcpy(char_buffer,str);					
						strcpy(ctrl_char,strtok(char_buffer,":"));
						strcat(ctrl_char,":");
						temp=strtok(NULL,":");
						jump=strtol(temp,NULL,10);
						fseek(rom_fp,cur+(4-check),0);
						fread(in_value,1,jump,rom_fp);					
						for(j=0;j<jump;j++){
							sprintf(char_buffer,"%02X",in_value[j]);
							strcat(ctrl_char,char_buffer);					
						}
						strcat(ctrl_char,"}");					
						strcat(buffer,ctrl_char);
						cur=cur+jump;
						break;
					}
						else {
							if(strcmp(str,"{N}")==0)	strcat(buffer,"\n");
							else{
								if(strcmp(str,"{end}")==0) {
									end_line=1;
									cur+=(4-check);
									//sect_end=cur;
								}
								strcat(buffer,str);
							}
							break;
						}

				}				
				if(check==3) {
						hex_count++;
						sprintf(unknown_char,"<$%02X>",ptr[0]);
						strcat(buffer,unknown_char);
					break;
				}			
			}			
		}
			fprintf(ptr_fp,"%03d>#WRITE(MyPtr,$%lX):#JMP($%lX,$%lX)\n",
				txt_number,ptr_cur,sect_start,sect_end);
			fprintf(out_fp,"#### %d ####\n%s\n\n",txt_number++,buffer);		
	}
	
	fclose(ptr_fp);
	fclose(out_fp);	
}

int main()
{
	TBLINFO tbl;
	FILE *list_fp,*rom_fp;
	FILE *log_fp,*batch_fp;
	unsigned int i,filesize,fileoffset;
	export_info exinfo;
	script_block blk;
	char romfile[256];
	char buffer[256];
	char *loadtbl="gensui3.tbl";
	
	tbl = TBL_LoadTable(loadtbl);
	if(!tbl) {
		printf("%s\n",TBL_GetErrorString());
		return 1;
	}
	
	if((list_fp=fopen("list.txt","rt"))==NULL)	{
		printf("error on open list!\n");
		return 1;
	}
	
	log_fp=fopen("log.txt","wt");	
	//batch_fp=fopen("import.bat","wt");
	exinfo.log = log_fp;
	//exinfo.batch = batch_fp;
	
	fgets(buffer,255,list_fp);
	while(strcmp(buffer,"\n")!=0)
	{
		sscanf(buffer,"%08X,%s",&fileoffset,romfile);
		rom_fp=fopen(romfile,"rb");
		if(rom_fp==NULL){
			printf("error on open rom:%s\n",romfile);
			return 1;
		}
		
		printf("file:%s\n",romfile);
		exinfo.num=1;
		strncpy(exinfo.name, strrchr(romfile, '\\')+1, 4);
		exinfo.name[4]=0;
		
		fseek(rom_fp,0,SEEK_END);
		filesize = ftell(rom_fp);
		filesize /=10;					//enough scan range
		for(i=0;i<filesize;i+=8){
			if(CheckScriptBlock(rom_fp, i, &blk, &exinfo)==0){
				i=blk.ptr+blk.count*4-8;
				//printf("text:%lx;ptr:%lx;count:%d;\n",blk.text,blk.ptr,blk.count);getch();
				//fprintf(batch_fp,"atlas %s.BIN conv_%s_%04d.txt\n",exinfo.name,exinfo.name,exinfo.num);
				ExportScriptBlock(rom_fp, &blk, &tbl, &exinfo);
				exinfo.num++;
			}
		}
		fclose(rom_fp);
		
		//printf("finish\n");getch();
		fgets(buffer,255,list_fp);
	}
	
	fclose(log_fp);
	//fclose(batch_fp);
	fclose(list_fp);
	TBL_CloseTable(tbl);
	printf("finish");getch();
	return 0;
}

