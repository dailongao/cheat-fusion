#include "stringutil.h"

namespace UT_Utils
{

    const LPSTR StringUtil::EmptyString ="";

	bool StringUtil::StringRealloc(char** pstrSrc,const char* strValue)
	{
		if(pstrSrc==NULL ) return false;

		try
		{
			if((*pstrSrc)!=NULL) free((*pstrSrc));

			if(strValue==NULL)
			{
				(*pstrSrc)=NULL;
			}
			else
			{
				(*pstrSrc)=(char*)malloc(sizeof(char)*(strlen(strValue)+1));
				strcpy((*pstrSrc),strValue);
			}
		}
		catch(...)
		{
			return false;
		}
		return true;
	}
    
    //字符串延长函数
    //pstrSrc，字符串指针变量的地址，strValue，添加的部分
    //pstrSrc保存的指针会发生变化
    bool StringUtil::StringAppend(char** pstrSrc, const char* strValue)
    {
        if(pstrSrc ==NULL) return false;

		try
		{
			if(strValue==NULL)
			{
				(*pstrSrc)=NULL;
			}
			else
			{
                if((*pstrSrc)==NULL)
                {
                    (*pstrSrc) = (char*)malloc(sizeof(char)*(strlen(strValue)+1));
                    (*pstrSrc)[0]=0;
                }
                else (*pstrSrc) = (char*)realloc(*pstrSrc, strlen(*pstrSrc) + strlen(strValue) + 1);
				strcat((*pstrSrc),strValue);
			}
		}
		catch(...)
		{
			return false;
		}
		return true;
    }

    /**
    *
    *从一个文本文件fp中读取字符串，直到遇见指定字符end，结果保存在strOut中
    *
    *返回-1表示读取失败，否则返回实际读取的字符数量。
    */
    int StringUtil::ReadUntil(FILE* fp, const char end, char* strOut)
    {
        if(fp==NULL || strOut == NULL) return -1;

        strOut[0] = '\0';
        char chr;
        try
        {
            //逐个字符读取比较
            while(!feof(fp))
            {
                if(fscanf(fp,"%c",&chr)<=0) break;
                if(chr==end)
                {
                    break;
                }
                else
                {
                    int len = (int)strlen(strOut);
                    strOut[len]=chr;
                    strOut[len+1] = '\0';
                }
            }
            return (int)strlen(strOut);
        }
        catch(...)
        {
            return -1;
        }
    }

    /*
    *
    *去掉一个字符串中的\r\r\n和空格
    *strIn，输入字符串，strOut，输出字符串
    */
    void StringUtil::Trim(char* strIn, char* strOut)
    {
        if(strIn == NULL || strOut==NULL) return;
        try
        {
            int ptr = 0;
            int cnt = 0;
            while(strIn[ptr]!='\0')
            {
                if(strIn[ptr]!=' ' && strIn[ptr]!='\t' && strIn[ptr]!='\r' && strIn[ptr]!='\n')
                {
                    strOut[cnt++]=strIn[ptr];
                }
                ptr++;
            }
            strOut[cnt]='\0';
        }
        catch(...)
        {
        }
    }

    /*
    *从文件流读取一个ANSI字符
    *返回实际读取字符数量
    *note: charOut应该至少能承接3个字符
    */
    int StringUtil::ReadANSIChar(FILE* fp, char* charOut)
    {
        if(feof(fp) || fread(charOut,sizeof(char), 1, fp)<=0)
        {
            charOut[0] = 0; 
            return 0;
        }

        if(charOut[0]<0 && !feof(fp))
        {
             if(fread(charOut+1,sizeof(char), 1, fp)<=0)
             {
                 charOut[1] = 0;
                 return 1;
             }
        }
        else
        {
            charOut[1] = 0;
            return 1;
        }

        if(charOut[1]>0)
        {
            fseek(fp, -1, SEEK_CUR);
            charOut[1]=0;
        }
        else
            charOut[2] = 0;
        return strlen(charOut);
        
    }

    /*
    *获取字符串中第一个ANSI字符
    *返回实际读取字符数量
    *note: charOut应该至少能承接3个字符
    */
    int StringUtil::NextANSIChar(char* strIn, char* charOut)
    {
        if(strIn[0]==0)
        {
            charOut[0]=0;
            return 0;
        }

        charOut[0] = strIn[0];

        if(strIn[0]>0)
        {
            charOut[1]=0;
            return 1;
        }

        charOut[1] = strIn[1];

        if(strIn[1]==0)
        {
            return 1;
        }

        if(strIn[1]>0)
        {
            charOut[1]=0;
            return 1;
        }

        charOut[2]=0;
        return 2;
    }
}
