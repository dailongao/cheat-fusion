#include "ppfmaker.h"
#include <memory.h>
#include <string.h>
#include <conio.h>

static int patch_count=0;

/*make a ppf file*/
void MakePPF(int listlength, char** flist,int* offsetlist,char* fiso,char* fppf)
{	
    if(listlength>0)
    {
        printf("\nwriting...");
	  /*  try
        {*/
		    //the iso file
            char* fbuf = 0, *fbuf2 = 0, *fibuf = 0 ;
		    FILE* isoin = fopen(fiso, "rb");
            fibuf = new char[32768];
            setvbuf(isoin, fibuf, _IOFBF, 32768);
    	    fseek(isoin,0, SEEK_END);
            int isolength = ftell(isoin);
            rewind(isoin);


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
    		
		    //Ð´ppfÎÄ¼þÍ·
		    char *tempbytes="PPF20";
		    fwrite(tempbytes, strlen((char*)tempbytes),1,ppfout);//ppf20
		    fwrite(tempbytes, strlen((char*)tempbytes),1,backupout);
            int version = 1;
		    fwrite(&version,1,1, ppfout);//ppf version
		    fwrite(&version,1,1, backupout);
		    tempbytes=new char[0x32];
            memset(tempbytes, 0, 0x32);
		    fwrite(tempbytes, 0x32, 1, ppfout);//description, temporily empty
		    fwrite(tempbytes, 0x32, 1, backupout);
            delete[] (char*)tempbytes;
            tempbytes = 0;
		    fwrite(&isolength, 4, 1, ppfout);//size of iso file
		    fwrite(&isolength, 4, 1, backupout);
		    fseek(isoin, 0x9320, SEEK_SET);//read 1024 bytes from 0x9320 of iso file
		    tempbytes=new char[1024];
		    fread(tempbytes, 1024, 1, isoin);
		    fwrite(tempbytes, 1024, 1, ppfout);//write the 1024 bytes into ppf
		    fwrite(tempbytes, 1024, 1, backupout);
            delete[] (char*)tempbytes;
            tempbytes = 0;
    		
		    //read & write each file in the list
		    for(int i=0;i<listlength;i++)
            {
                printf("\nfile: %s", flist[i]);
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
void _WritePPFData(FILE* ppfout,FILE* backupout,FILE* isoin,char* fsrc, int offset)
{
    //create iobuf
    char* fdbuf = new char[32768];
    FILE* datain=fopen(fsrc, "rb");
    if(datain==NULL) return;
    setvbuf(datain, fdbuf, _IOFBF, 32768);
    fseek(datain, 0, SEEK_END);
    int length = ftell(datain);
    rewind(datain);

    char buffer[PART_SIZE];//the buffer
    char bbuffer[PART_SIZE];//the backup buffer
    int position=offset;//position in iso file
    int buffersize=0;//counter, write buffer into ppf when reached PART_SIZE

    for(int i=0;i<length;position++,i++){
	    buffersize++;
	    //printf("%d\n",buffersize);
	    if(position%LBA_SIZE >= LBA_DATA_SIZE + LBA_HEADER_SIZE-1 || buffersize == PART_SIZE || i == length - 1 ){// reach max buffersize, eof or end of lbas
		    int p = position-buffersize+1;
		    fread(buffer, buffersize, 1, datain);//read data
            fseek(isoin, p, SEEK_SET);
            fread(bbuffer, buffersize, 1, isoin);//read backup data
            //compare with original
            //printf(" %x %d %x,",position,buffersize,p);getch();
            if(memcmp(bbuffer, buffer, buffersize)!=0)
            {
				//printf("!");
		        fwrite(&p, 4, 1, ppfout);//position in iso file
		        fwrite(&p, 4, 1, backupout);
		        fwrite(&buffersize, 1, 1, ppfout );//size
		        fwrite(&buffersize, 1, 1, backupout );
		        fwrite(buffer, buffersize, 1, ppfout);//write to ppf
		        fwrite(bbuffer, buffersize, 1, backupout);//write to backup file
            }
            if(position%LBA_SIZE >= LBA_DATA_SIZE + LBA_HEADER_SIZE-1){
                position += LBA_SIZE - LBA_DATA_SIZE;//jump to next lba
			}
            buffersize=0;//reset buffer
	    }
    }
    fclose(datain);
    delete[] fdbuf;
}


