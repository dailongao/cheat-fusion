#include "ppfmaker.h"
#include "stringutil.h"
#include <conio.h>

using namespace UT_Utils;

int main()
{
    FILE* fp = fopen("list.txt", "r");
    if(fp != NULL)
    {
        char cbuf[128];
		
		int count = 0;
		while(StringUtil::ReadUntil(fp, '\n', cbuf)>0) count++;
		fseek(fp, 0, SEEK_SET);
		count-=2;
		
        char fiso[1024];
        char fppf[1024];
		
        StringUtil::ReadUntil(fp, '\n', fiso);
        StringUtil::Trim(fiso,fiso);
        StringUtil::ReadUntil(fp, '\n', fppf);
        StringUtil::Trim(fppf,fppf);
		
        char** flist = new char*[count];
        for(int i=0; i<count; i++)
        {
            flist[i] = new char[256];
        }
		
        unsigned long long *offsetlist = new unsigned long long[count];
		
        for(int i=0; i<count; i++)
        {
            int c = 0;
            char buf[256],buf2[256];
            //fname
            c = StringUtil::ReadUntil(fp,',',buf2);
            if(c<0) break;
            StringUtil::Trim(buf2,buf2);
            //offset
            c = StringUtil::ReadUntil(fp, '\n',buf);
            if(c<=0) break;
            StringUtil::Trim(buf,buf);
            
            if(strlen(buf2)>0)
            {
                strcpy(flist[i], buf);
				sscanf(buf2, "%I64X", offsetlist+i);
            }
        }
        MakePPF(count, flist, offsetlist, fiso, fppf);
        fclose(fp);
        _getch();
    }
}

