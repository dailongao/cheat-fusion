#pragma once
#include <windows.h>
#include <stdio.h>

namespace UT_Utils
{
	class StringUtil
	{
	public:
        static const LPSTR EmptyString ;
		static bool StringRealloc(char** pstrSrc, const char* strValue);
        static bool StringAppend(char** pstrSrc, const char* strValue);
        static int ReadUntil(FILE* fp, const char end, char* strOut);
        static int ReadANSIChar(FILE* fp, char* charOut);
        static void Trim(char* strIn, char* strOut);
        static void LTrim(char* strIn, char* strOut);
        static void RTrim(char* strin, char* strOut);
        static void LRTrim(char* strin, char* strOut);
        static int NextANSIChar(char* strIn, char* charOut);
	};
}

