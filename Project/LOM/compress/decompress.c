//解压legend of mana的压缩文件
#include <stdio.h>
#include <fcntl.h>
#include <string.h>
#include <stdlib.h>
#include <conio.h>

unsigned char buf[0x200000];
unsigned int out_ptr;

int main(void)
{
	FILE *in,*out;
	unsigned int count,temp,temp1,temp2,temp3,temp4,pos,currentpos,inpos,v0;
	unsigned int f0=0,f1=0,f2=0,f3=0,f4=0,f5=0,f6=0,f7=0,f8=0,f9=0,fa=0,fb=0,fc=0,fd=0,fe=0,ff=0,etc=0;
	unsigned char flagchar,buffer[100],fatemp,fbtemp;
	char *inname=buffer;
	int t5;
	
	printf("input file to decompress:");
	scanf("%s",inname);
	
	in=fopen(inname,"rb");
	if(in==NULL) return 1;
	strcat(inname,".dec");
	out=fopen(inname,"wb");
	out_ptr=0;
	
	flagchar=fgetc(in);
	
	
	flagchar=fgetc(in);
	while(!feof(in))
	{
		//if(ftell(out)>0x7a1f) {printf("%x\n",ftell(in)); getch();}
		switch(flagchar)
		{
			case 0xf0:{
				f0++;
				buffer[0]=fgetc(in);
				count=(buffer[0]&0x0f)+3;
				temp=buffer[0]>>4;
				while(count--)	buf[out_ptr++]=temp;
				break;
			}
			case 0xf1:{
				f1++;
				fread(buffer,1,2,in);
				count=buffer[0]+4;
				temp=buffer[1];
				while(count--) buf[out_ptr++]=temp;
				break;
			}
			case 0xf2:{
				f2++;
				fread(buffer,1,2,in);
				count=buffer[0]+2;
				temp1=buffer[1]&0x0f;
				temp2=buffer[1]>>4;
				while(count--) {
					buf[out_ptr++]=temp1;
					buf[out_ptr++]=temp2;
				}
				break;
			}
			case 0xf3:{
				f3++;
				fread(buffer,1,3,in);
				count=buffer[0]+2;
				temp1=buffer[1];
				temp2=buffer[2];
				while(count--) {
					buf[out_ptr++]=temp1;
					buf[out_ptr++]=temp2;
				}
				break;
			}
			case 0xf4:{
				f4++;
				fread(buffer,1,4,in);
				count=buffer[0]+2;
				temp1=buffer[1];
				temp2=buffer[2];
				temp3=buffer[3];
				while(count--) {
					buf[out_ptr++]=temp1;
					buf[out_ptr++]=temp2;
					buf[out_ptr++]=temp3;
				}
				break;
			}
			case 0xf5:{
				f5++;
				fread(buffer,1,2,in);
				count=buffer[0]+4;
				temp1=buffer[1];
				while(count--) {
					buf[out_ptr++]=temp1;
					temp2=fgetc(in);
					buf[out_ptr++]=temp2;
				}
				break;
			}
			case 0xf6:{
				f6++;
				fread(buffer,1,3,in);
				count=buffer[0]+3;
				temp1=buffer[1];
				temp2=buffer[2];
				while(count--){
					buf[out_ptr++]=temp1;
					buf[out_ptr++]=temp2;
					temp3=fgetc(in);
					buf[out_ptr++]=temp3;
				}
				break;
			}
			case 0xf7:{
				f7++;
				fread(buffer,1,4,in);
				count=buffer[0]+2;
				temp1=buffer[1];
				temp2=buffer[2];
				temp3=buffer[3];
				while(count--){
					buf[out_ptr++]=temp1;
					buf[out_ptr++]=temp2;
					buf[out_ptr++]=temp3;
					temp4=fgetc(in);
					buf[out_ptr++]=temp4;
				}
				break;
			}
			case 0xf8:{
				f8++;
				fread(buffer,1,2,in);
				count=buffer[0]+4;
				temp1=buffer[1];
				while(count--){
					buf[out_ptr++]=temp1;
					temp1++;
				}
				break;
			}
			case 0xf9:{
				f9++;
				fread(buffer,1,2,in);
				count=buffer[0]+4;
				temp1=buffer[1];
				while(count--){
					buf[out_ptr++]=temp1;
					temp1--;
				}
				break;
			}
			case 0xfa:{
				fa++;
				fread(buffer,1,3,in);
				count=buffer[0]+5;
				temp1=buffer[1];
				fatemp=buffer[2];		//8位等差,temp 
				//currentpos=ftell(out);
				//printf("%x is 0xFA count=%d\n",currentpos,count);								
				while(count--){
					buf[out_ptr++]=temp1;
					temp1=temp1+fatemp;
				}
				break;
			}
			case 0xfb:{
				fb++;
				fread(buffer,1,4,in);
				count=buffer[0]+3;
				temp1=buffer[1];
				temp2=buffer[2];
				fbtemp=buffer[3];			//16位数等差
				inpos=ftell(in); 
				t5=fbtemp<<24;
				//printf("%x:0xfb>",inpos);
				while(count--){
					buf[out_ptr++]=temp1;
					buf[out_ptr++]=temp2;
					//printf("%x:%x,%x||",currentpos,temp1,temp2);getch();
					v0=t5>>24;
					temp2=temp2<<8;
					temp1=temp1&0x00ff;
					temp1=temp1|temp2;
					temp=temp1+v0;
					//if(currentpos==0xA097A) {printf("%x:%x,%x||",inpos,temp1,v0);getch();}
					temp1=temp;
					temp2=temp>>8;
				}
				//printf("\n");			
				break;
			}
			case 0xfc:{
				fc++;
				inpos=ftell(in);
				fread(buffer,1,2,in);
				count=(buffer[1]>>4)+4;
				temp=(buffer[1]&0x0f)<<8;
				temp=buffer[0]|temp;
				currentpos=out_ptr;
				pos=currentpos-temp-1;			//偏移为12bits---4096bytes
				//printf("0xfc,%x,%x,%x,%d>",inpos,currentpos,temp,count);
				while(count--){
					temp=buf[pos++];
					//printf("%x:%x,",pos-1,temp);getch();
					buf[out_ptr++]=temp;
				}
				//printf("\n");	
				break;
			}
			case 0xfd:{
				fd++;
				fread(buffer,1,2,in);
				count=buffer[1]+0x14;
				currentpos=out_ptr;
				pos=currentpos-buffer[0]-1;		//偏移为8bits---256bytes
				//printf("0xfd>");
				while(count--){
					temp=buf[pos++];
					//printf("%x:%x,",currentpos,temp);getch();
					buf[out_ptr++]=temp;
				}
				//printf("\n");				
				break;									
			}
			case 0xfe:{
				fe++;
				buffer[0]=fgetc(in);
				count=(buffer[0]&0x0f)+3;
				temp=(buffer[0]&0xf0)>>1;
				currentpos=out_ptr;
				pos=currentpos-temp-8;		//偏移为7bits---128bytes(8的整数倍)
				//if((pos%8)!=0)
				//{printf("%x,0xfe:%x,%x,%d>",ftell(in),currentpos,temp,count);getch();}
				while(count--){
					temp=buf[pos++];
					//printf("%x:%x|",pos-1,temp);getch();
					buf[out_ptr++]=temp;
				}
				//printf("\n");				
				break;						
			}
			case 0xff:{
				ff++;
				currentpos=out_ptr;
				inpos=ftell(in);
				printf("%x is 0xFF\n",inpos);
				//break;
				goto EXIT;
			}
			default:{
				etc++;
				count=flagchar+1;
				while(count--){
					temp=fgetc(in);
					buf[out_ptr++]=temp;
				}
				break;
			}
		}
		
		flagchar=fgetc(in);
	}
	
EXIT:
	fwrite(buf,1,out_ptr,out);
	fclose(in);fclose(out);
	
	printf("f0:%d\nf1:%d\nf2:%d\nf3:%d\nf4:%d\nf5:%d\nf6:%d\nf7:%d\nf8:%d\nf9:%d\nfa:%d\nfb:%d\n",f0,f1,f2,f3,f4,f5,f6,f7,f8,f9,fa,fb);
	printf("fc:%d\nfd:%d\nfe:%d\nff:%d\netc:%d\n",fc,fd,fe,ff,etc);	
	system("pause");
	return 0;
}

