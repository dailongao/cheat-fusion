#include <stdio.h>
#include <fcntl.h>
#include <string.h>
#include <stdlib.h>
#include <conio.h>
unsigned char buffer[0x100000];
int buf_ptr;
void Decode(FILE *fp, int offset, FILE *out)
{
	unsigned int data,bits,flag,ch,bitmask,nextch,testch;
	int win_pos,win_len;
	fseek( fp, offset, SEEK_SET );
	buf_ptr=1;
	flag=0;
	while(1)
	{
		if(flag==0){
			data = fgetc( fp );
			bits = 8;
			flag = 0xff;
		}
		ch=flag&data;
		flag>>=1;
		testch=ch&(flag^0xff);
		bits--;		
		bitmask=8;
		if(flag==0){
			flag=0xff;
			bits=8;
			data = fgetc( fp );
		}
		ch=flag&data;
		bitmask-=bits;
		flag=0xff;
		bits=8;
		data = fgetc( fp );
		if(bitmask!=0){
			flag>>=bitmask;
			bits-=bitmask;
			ch=(ch<<bitmask)|(((flag^0xff)&data)>>bits);
		}
		printf("%02X,",ch);getch();
		if((int)testch>0){		//raw byte
			buffer[buf_ptr++]=ch;
			buf_ptr&=0xff;
			fputc(ch,out);
		}
		else{			
			if(ch==0) break;
			bitmask=4;
			win_len=0;
			if(flag==0){
				flag=0xff;
				bits=8;
				data = fgetc( fp );
			}
			if((int)(bitmask-bits)>0){
				bitmask=bitmask-bits;
				win_len=flag&data;
				flag=0xff;
				bits=8;
				data = fgetc( fp );
			}
			nextch=flag&data;
			flag>>=bitmask;
			bits-=bitmask;
			nextch=nextch&(flag^0xff);
			win_len=(win_len<<bitmask)|(nextch>>bits);
			printf("winlen:%02x,",win_len);getch();
			win_len++;
			win_pos=ch&0xff;
			while(win_len>=0){
				buffer[buf_ptr++]=buffer[win_pos];
				fputc(buffer[win_pos],out);
				win_pos++;
				win_pos&=0xff;
				buf_ptr&=0xff;
				win_len--;
			}
		}
	}
}



///////////////////////////////////////////////////////

void LZ_Decode(FILE *fp, int offset, FILE *out)
{
	buf_ptr = 1;
	fseek( fp, offset, SEEK_SET );

	unsigned int testch,ch;
	int pos,len;
	while(1){
		testch=fgetc(fp);
		//printf("%02x",testch);getch();
		if(testch==1){
			ch=fgetc(fp);
			buffer[buf_ptr++]=ch;
			buf_ptr&=0xff;
			fputc(ch,out);
		}
		else if(testch==0){
			pos=fgetc(fp);
			len=fgetc(fp)+1;
			while(len>=0){
				buffer[buf_ptr++]=buffer[pos];
				fputc(buffer[pos],out);
				buf_ptr&=0xff;
				pos++;
				pos&=0xff;
				len--;
			}
		}
		else return;
	}
}

int main()
{
	FILE *in,*out;
	char name[128];
	char outname[128];
	printf("input infile\n");
	scanf("%s",name);
	sprintf(outname,"%s.dec",name);
	in=fopen(name,"rb");
	out=fopen(outname,"wb");
	
	Decode(in,0,out);
	//LZ_Decode(in,0,out);
	fclose(in);fclose(out);
	return 0;
}

