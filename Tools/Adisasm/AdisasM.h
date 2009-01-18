typedef unsigned long word;
typedef unsigned short uhword;
typedef unsigned char ubyte;
typedef signed long sword;
typedef signed short shword;
typedef signed char sbyte;

#include "mipscrap.h"

struct CPsxHeader
{
	char ID[8];        
	word Pad1[2];     
	word JumpAddress;  
	word Pad2; 
	word LoadAddress;
	word TextLength;
	char  Pad3[16];
	word StackAddress;
	char  Pad4[61];
	char  Territory; 
	char  Pad5[1934];
};


#define IF_CODE          0x8000 // reachable code that is, except if the 'consider unreachable code'-option is specified
 #define IFC_LABEL       0x0001 // target of normal branch
 #define IFC_TERM        0x0002 // terminator: delay slot of J(r), but not jr ra
	
 #define IFC_PROC        0x4000 // procedure body, if set:
		#define IFP_BEGIN      0x2000
		#define IFP_END        0x1000

 #define IFC_LOOP        0x0800 // body of loop	
  #define IFL_BEGIN      0x0400
  #define IFL_END        0x0200

 #define IFC_BRANCH      0x0080 // any kind of branching
  #define IFB_VAR        0x0040 // j(al)r, but not jr ra
  #define IFB_LINK       0x0020
  #define IFB_UNCOND     0x0010
	
 

#define IF_DATA         0x0000 // data
	// the IFD_ defines are only guesses (as far as a program can take a guess)
	#define IFD_UNKNOWN     0x0000 // i don't know ! :-/
	#define IFD_ADDRESS		   0x0002 // probably an address (ie. a pointer)
	#define IFD_STRING      0x0004 // string


struct CCodeLine
{
 uhword flag; // see IFx_ constants above
	word instruction; // index in our pool o' strings if flag&IFD_STRING
	
	CCodeLine()	{  memset(this,0,sizeof(*this));	}
};

class CCrossRefTbl
{
public:
 void addRef(word target, word src);
	void printAllRefs(FILE* f);
private:
};


#define ST_PROC_EP 1
#define ST_LABEL   2
#define ST_MEM_VAR 3

typedef map<word, string> SYMTBL;

class CPsxExe;

class CSymbolTbl
{
public:
	CSymbolTbl() {inited=false;}
 void addSym(word val, string name, ubyte type);
	bool lookup(word val, string& name);
	void load(FILE* f);
	void psyLoad(FILE* f);
	void markCode(CPsxExe* exe);
private:
	SYMTBL symbols;
	void handlePsySym8a(FILE* f);
	bool inited;
};


#define INIT_STRINGS 40
#define MAX_STRINGLENGTH 255


class CPsxExe
{
public:
	friend CSymbolTbl;
	CPsxExe();
	bool doCmdLine(int argc, char* argv[]);

 virtual ~CPsxExe();

	word loadAddress;
	word entryPoint;
	word codeLength;
	void loadLines(word* lines);
 void loadExe(char* name);
	void loadSymTbl(char* name);
	bool isValidInstruction(word d, word pc);
	void decodeInstruction(word d, word pc);
	void disassemble(char *name, word from, word to);
	void disassemble(char *name);
	word extractASCIIZStr(word start, word end, char* buffer);

	// options:
	static bool optVerbose;
	static bool optForceBin; // always handle as binary file
	static bool optForceMachCode; // display the codeword 
	static bool optDumbString; // <sniff> don't try to extract a string.. just be dumb
	static bool optConsiderUnreachableCode; // also disasm code (that seems) unreachable
	static bool optResourceC; // high level disasm
 static word optLoadAddress;
	static word optCodeLength;
 static word optJumpAddress;
private:
	CCodeLine* lines;
	CCrossRefTbl crossRef;
	CSymbolTbl syms;
	char strings[INIT_STRINGS][MAX_STRINGLENGTH]; // swimming pool for our ladies wearing nothing but strings #-D
 word nextFreeString; // next chick in our pool
};