//统计文本字符
#include <stdio.h>
#include <fcntl.h>
#include <string.h>
#include <stdlib.h>
#include <conio.h>
#include <dirent.h>

typedef struct hz {
	struct hz *next;
	unsigned char str[20];
	unsigned int count;
} hz;

hz *hz_head=NULL,*hz_tail=NULL;


hz * sort(hz *head) //排序函数
{
	
int i;
hz *p1,*stu,*p2,*p0=NULL;
if(!hz_head || !hz_head->next)      //没有结点或只有一个结点，直接退出
return head;
if(!hz_head->next->next) //有两个结点，交换两个结点后退出
{
if(hz_head->count<hz_head->next->count)
{
stu=hz_head;
hz_head=hz_head->next;
hz_head->next=stu;
stu->next=NULL;
}
return head;
}

while(p0!=hz_head->next->next) //三个以上结点排序,使用冒泡排序法
{
p1=hz_head;stu=p1->next;p2=stu->next;
while(p2!=p0)
{
if(stu->count<p2->count)
{
hz * p;
p1->next=p2;
stu->next=p2->next;
p2->next=stu;
p=stu;stu=p2;p2=p;
}
p1=p1->next;stu=stu->next;p2=p2->next;
}
p0=stu;
}


p0=hz_head;
if(p0->count<p0->next->count)//特殊考虑前两个结点
{
stu=p0;
p0=p0->next;
stu->next=p0->next;
p0->next=stu;
hz_head=p0;
}

p1=stu;
while(p1->count<p1->next->count)
{
	p2=p1->next;
	p0->next=p2;
	p1->next=p2->next;
	p2->next=p1;
	p0=p2;
	
}

return head; 
}




int insert(char *s) {
	hz *ptr,*temp,*p;
	
	if(hz_head==NULL) {
		ptr=(hz *)malloc(sizeof(hz));
		strcpy(ptr->str,s);
		ptr->count=1;
		ptr->next=NULL;
		hz_head=ptr;
		hz_tail=ptr;
	}
	else {
		temp=hz_head;
		while(temp) {
			p=temp->next;
			if(strcmp(temp->str,s)==0) {
				temp->count++;
				return temp->count;
			}
			temp=p;			
		}
		ptr=(hz *)malloc(sizeof(hz));
		strcpy(ptr->str,s);
		ptr->count=1;
		ptr->next=NULL;				
		hz_tail->next=ptr;
		hz_tail=ptr;
	}
	return 0;
}

int output(FILE *fp) {
	unsigned int number=0x5E;
	hz *temp,*ptr;
	ptr=hz_head;
	while(ptr) {
		temp=ptr->next;
		if(number<0x2000) fprintf(fp,"%X=%s\n",number,ptr->str);
		else fprintf(fp,"20=%s\n",number,ptr->str);
		if(strlen(ptr->str)<15) {number++;}
		free(ptr);
		ptr=temp;
		if(number==0x100) number=0x1600;		
		if(number==0x1700){
			number=0x1701;
			fprintf(fp,"1700=＋\n");
		}
		if(number==0x1d00){
			number=0x1d0b;
			fprintf(fp,"1D00=０\n");
			fprintf(fp,"1D01=１\n");
			fprintf(fp,"1D02=２\n");
			fprintf(fp,"1D03=３\n");
			fprintf(fp,"1D04=４\n");
			fprintf(fp,"1D05=５\n");
			fprintf(fp,"1D06=６\n");
			fprintf(fp,"1D07=７\n");
			fprintf(fp,"1D08=８\n");
			fprintf(fp,"1D09=９\n");
			fprintf(fp,"1D0A=％\n");
		}
	}
	//字库编码未到1F59,补
	while(number<0x1f59){
		fprintf(fp,"%X= \n",number++);
	}
	
	return 0;
}


int main(int argc, char **argv)
{	
	unsigned char buf[1500],filename[150],char_buffer[40],temp[20];
	unsigned int i,j,m,n,START,END;
	FILE *txt_fp,*count_fp;
	DIR * dir;
	struct dirent * dirptr;
	
	if((count_fp=fopen("hz.tbl","wt"))==NULL){
		printf("error on create tbl_file!\n");
		return 1;
	}
	
	dir =opendir("..\\convertfile\\conv\\");
if(dir!=NULL)
{
//以下统计菜单字库文本汉字	
	while((dirptr=readdir(dir))!=NULL)
	{
		if(dirptr->d_name[0]=='.') continue;
		strcpy(filename,"..\\convertfile\\conv\\");
		strcat(filename,dirptr->d_name);
		if((txt_fp=fopen(filename,"rt"))==NULL){
			printf("error on open %s!\n",filename);
			continue;
		}		
		
		fgets(buf,1499,txt_fp);
		while(!feof(txt_fp)) {
			for(i=0;buf[i]!='\0';i++) {
				if(buf[i]>0x7f) {
					if(buf[i]>=0xb0 && buf[i]<=0xf7 && buf[i+1]>=0xa1 && buf[i+1]<=0xfe) {
						temp[0]=buf[i++];
						temp[1]=buf[i];
						temp[2]='\0';
						insert(temp);
					}
					else if(buf[i]>=0x81 && buf[i]<=0xA0 && buf[i+1]>=0x40 && buf[i+1]<=0xfe){
						temp[0]=buf[i++];
						temp[1]=buf[i];
						temp[2]='\0';
						insert(temp);
					}
					else if(buf[i]>=0xaa && buf[i]<=0xfe && buf[i+1]>=0x40 && buf[i+1]<=0xa0){
						temp[0]=buf[i++];
						temp[1]=buf[i];
						temp[2]='\0';
						insert(temp);
					}
					else i++;
				}
				else if(buf[i]=='|') {
						m=0;temp[m++]=buf[i++];
						while(buf[i]!='|') {
							if ((m>19)||(buf[i]=='\0')) {
								printf("\" error in %s!",filename);
								system("pause");
								return 0;
							}						
							temp[m++]=buf[i++];
						}
						temp[m++]=buf[i];
						temp[m]='\0';
						insert("　");
					}
					else if(buf[i]=='{') {
						while(buf[i]!='}') i++;
					}					
			}
			fgets(buf,1499,txt_fp);
		}
		fclose(txt_fp);
	}
closedir(dir);
}

//统计剧情文本文字
	insert("info char below:");

	dir =opendir("..\\convert\\conv\\");
if(dir!=NULL)
{	
	while((dirptr=readdir(dir))!=NULL)
	{
		if(dirptr->d_name[0]=='.') continue;
		strcpy(filename,"..\\convert\\conv\\");
		strcat(filename,dirptr->d_name);
		if((txt_fp=fopen(filename,"rt"))==NULL){
			printf("error on open %s!\n",filename);
			continue;
		}		
		
		fgets(buf,1499,txt_fp);
		while(!feof(txt_fp)) {
			for(i=0;buf[i]!='\0';i++) {
				if(buf[i]>0x7f) {
					if(buf[i]>=0xb0 && buf[i]<=0xf7 && buf[i+1]>=0xa1 && buf[i+1]<=0xfe) {
						temp[0]=buf[i++];
						temp[1]=buf[i];
						temp[2]='\0';
						insert(temp);
					}
					else if(buf[i]>=0x81 && buf[i]<=0xA0 && buf[i+1]>=0x40 && buf[i+1]<=0xfe){
						temp[0]=buf[i++];
						temp[1]=buf[i];
						temp[2]='\0';
						insert(temp);
					}
					else if(buf[i]>=0xaa && buf[i]<=0xfe && buf[i+1]>=0x40 && buf[i+1]<=0xa0){
						temp[0]=buf[i++];
						temp[1]=buf[i];
						temp[2]='\0';
						insert(temp);
					}
					else i++;
				}
				else if(buf[i]=='|') {
						m=0;temp[m++]=buf[i++];
						while(buf[i]!='|') {
							if ((m>19)||(buf[i]=='\0')) {
								printf("\" error in %s!",filename);
								system("pause");
								return 0;
							}						
							temp[m++]=buf[i++];
						}
						temp[m++]=buf[i];
						temp[m]='\0';
						insert("　");
					}
					else if(buf[i]=='{') {
						while(buf[i]!='}') i++;
					}					
			}
			fgets(buf,1499,txt_fp);
		}
		fclose(txt_fp);
	}
	closedir(dir);
}

//统计图鉴文本
	
	insert("zukan chars below:");

	dir =opendir("..\\convertzukan\\txt\\");
if(dir!=NULL)
{	
	while((dirptr=readdir(dir))!=NULL)
	{
		if(dirptr->d_name[0]=='.') continue;
		strcpy(filename,"..\\convertzukan\\txt\\");
		strcat(filename,dirptr->d_name);
		if((txt_fp=fopen(filename,"rt"))==NULL){
			printf("error on open %s!\n",filename);
			continue;
		}		
		
		fgets(buf,1499,txt_fp);
		while(!feof(txt_fp)) {
			for(i=0;buf[i]!='\0';i++) {
				if(buf[i]>0x7f) {
					if(buf[i]>=0xb0 && buf[i]<=0xf7 && buf[i+1]>=0xa1 && buf[i+1]<=0xfe) {
						temp[0]=buf[i++];
						temp[1]=buf[i];
						temp[2]='\0';
						insert(temp);
					}
					else if(buf[i]>=0x81 && buf[i]<=0xA0 && buf[i+1]>=0x40 && buf[i+1]<=0xfe){
						temp[0]=buf[i++];
						temp[1]=buf[i];
						temp[2]='\0';
						insert(temp);
					}
					else if(buf[i]>=0xaa && buf[i]<=0xfe && buf[i+1]>=0x40 && buf[i+1]<=0xa0){
						temp[0]=buf[i++];
						temp[1]=buf[i];
						temp[2]='\0';
						insert(temp);
					}
					else i++;
				}
				else if(buf[i]=='|') {
						m=0;temp[m++]=buf[i++];
						while(buf[i]!='|') {
							if ((m>19)||(buf[i]=='\0')) {
								printf("\" error in %s!",filename);
								system("pause");
								return 0;
							}						
							temp[m++]=buf[i++];
						}
						temp[m++]=buf[i];
						temp[m]='\0';
						insert("　");
					}
					else if(buf[i]=='{') {
						while(buf[i]!='}') i++;
					}					
			}
			fgets(buf,1499,txt_fp);
		}
		fclose(txt_fp);
	}
closedir(dir);
}


//统计武器名,怪物名等all.txt文本
	printf("now counts all.txt\n");
	insert("all.txt char below");
	txt_fp=fopen("all.txt","rt");
if(txt_fp!=NULL)
{
	fgets(buf,1499,txt_fp);
	while(!feof(txt_fp)) {
		for(i=0;buf[i]!='\0';i++) {
			if(buf[i]>0x7f) {
				if(buf[i]>=0xb0 && buf[i]<=0xf7 && buf[i+1]>=0xa1 && buf[i+1]<=0xfe) {
					temp[0]=buf[i++];
					temp[1]=buf[i];
					temp[2]='\0';
					insert(temp);
				}
				else if(buf[i]>=0x81 && buf[i]<=0xA0 && buf[i+1]>=0x40 && buf[i+1]<=0xfe){
					temp[0]=buf[i++];
					temp[1]=buf[i];
					temp[2]='\0';
					insert(temp);
				}
				else if(buf[i]>=0xaa && buf[i]<=0xfe && buf[i+1]>=0x40 && buf[i+1]<=0xa0){
					temp[0]=buf[i++];
					temp[1]=buf[i];
					temp[2]='\0';
					insert(temp);
				}
				else i++;
			}
			else if(buf[i]=='|') {
					m=0;temp[m++]=buf[i++];
					while(buf[i]!='|') {
		   				if ((m>19)||(buf[i]=='\0')) {
							printf("\" error in %s!",filename);
							system("pause");
							return 0;
						}						
						temp[m++]=buf[i++];
					}
					temp[m++]=buf[i];
					temp[m]='\0';
					insert("　");
				}
				else if(buf[i]=='{') {
					while(buf[i]!='}') i++;
				}					
		}
		fgets(buf,1499,txt_fp);
	}
	fclose(txt_fp);	
}
	
	sort(hz_head);
	output(count_fp);
	fclose(count_fp);
	system("pause");
	return 0;
}			

