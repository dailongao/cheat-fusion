#include "ppfmaker.h"
#include <memory.h>
#include <string.h>
#include <conio.h>

static int patch_count=0;

const char magic[]="PAF0";

/*make a ppf file*/
void MakePPF(int listlength, char** flist,unsigned long long* offsetlist,char* fiso,char* fppf)
{	
    if(listlength>0)
    {
        printf("\nwriting...%d files",listlength);
	  /*  try
        {*/
		    //the iso file
            char* fbuf = 0, *fbuf2 = 0, *fibuf = 0 ;
		    FILE* isoin = fopen(fiso, "rb");
            fibuf = new char[32768];
            setvbuf(isoin, fibuf, _IOFBF, 32768);
			
            //the ppf file
		    FILE* ppfout=fopen(fppf, "wb+");
            fbuf = new char[32768];
            setvbuf(ppfout, fbuf, _IOFBF, 32768);
            char fbackup[1024];
            fbackup[0] = 0;
            strcpy(fbackup, fppf);
            strcat(fbackup, ".bak");
		    //the backup ppf file
		    FILE* backupout = fopen(fbackup, "wb+");
            fbuf2 = new char[32768];
            setvbuf(backupout, fbuf2, _IOFBF, 32768);
			
			//header
			fwrite(magic,1,4,ppfout);
			fwrite(magic,1,4,backupout);
			fwrite(magic,1,4,ppfout);
			fwrite(magic,1,4,backupout);
			
			fseek(isoin,0,SEEK_END);
			unsigned long long isosize = ftell(isoin);
			fwrite(&isosize,1,8,ppfout);
			fwrite(&isosize,1,8,backupout);
			
			fseek(isoin,0x10000,SEEK_SET);
			char cmpbuf[256];
			fread(cmpbuf,1,240,isoin);
			fwrite(cmpbuf,1,240,ppfout);
			fwrite(cmpbuf,1,240,backupout);
			
		    //read & write each file in the list
		    for(int i=0;i<listlength;i++)
            {
                printf("\n%I64X,%s", offsetlist[i], flist[i]);
			    _WritePPFData(ppfout,backupout,isoin, flist[i],offsetlist[i]);
			    patch_count++;
		    }
    		
		    fclose(isoin);
		    fclose(ppfout);
		    fclose(backupout);
            isoin = ppfout = backupout = 0;
            delete[] fbuf;
            delete[] fbuf2;
            delete[] fibuf;
            fbuf = fbuf2 = fibuf = 0;
	  /*  }
        catch(...)
        {
		    printf("\nerror creating ppf");
            return;
	    }*/
    }
    else
    {
	    printf("\nnothing to write");
        return;
    }
    printf("\ntotally patch %d file\ndone.",patch_count);
}

/*write a file into ppf*/
void _WritePPFData(FILE* ppfout,FILE* backupout,FILE* isoin,char* fsrc, unsigned long long offset)
{
    //create iobuf
    char* fdbuf = new char[32768];
    FILE* datain=fopen(fsrc, "rb");
    setvbuf(datain, fdbuf, _IOFBF, 32768);
    fseek(datain, 0, SEEK_END);
    unsigned int length = ftell(datain);
    rewind(datain);

    char buffer[PART_SIZE];//the buffer
    char bbuffer[PART_SIZE];//the backup buffer
    unsigned long long position=offset;//position in iso file
    int buffersize=0;//counter, write buffer into ppf when reached PART_SIZE

    for(unsigned int i=0;i<length;position++,i++){
	    buffersize++;
	    if(buffersize == PART_SIZE /*|| position % LBA_SIZE >= LBA_DATA_SIZE + LBA_HEADER_SIZE - 1*/||i == length - 1){// reach max buffersize, eof or end of lbas
		    unsigned long long p = position-buffersize+1;
            //printf("\n%I64X %d %I64X,",position,buffersize,p);getch();
			fread(buffer, 1, buffersize, datain);//read data
            fseek(isoin, p, SEEK_SET);
            fread(bbuffer, 1, buffersize, isoin);//read backup data
            //compare with original
            if(memcmp(bbuffer, buffer, buffersize)!=0)
            {
				//printf("!");
		        fwrite(&p, 1, 5, ppfout);//position in iso file, 5bytes offset
		        fwrite(&p, 1, 5, backupout);
		        fwrite(&buffersize, 1, 1, ppfout );//size
		        fwrite(&buffersize, 1, 1, backupout );
		        fwrite(buffer, buffersize, 1, ppfout);//write to ppf
		        fwrite(bbuffer, buffersize, 1, backupout);//write to backup file
            }
/*            if(buffersize!=PART_SIZE)
                position += LBA_SIZE - LBA_DATA_SIZE;//jump to next lba*/
            buffersize=0;//reset buffer
	    }
    }
    fclose(datain);
    delete[] fdbuf;
}



