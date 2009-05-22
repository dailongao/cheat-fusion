#pragma once
//#include "stdafx.h" // Needed for MSVC++ Precompiled Headers option
#include <string>
#include <vector>
#include <list>
#include <map>

using namespace std;

// Structure for errors
typedef struct TBL_ERROR
{
	int LineNumber; // The line number which the error occurred
	string Description; // A description of what the error was
} TBL_ERROR;

// Data structure for a linked entry
typedef struct LINKED_ENTRY
{
	string Text;
	int Number;
} LINKED_ENTRY;

typedef map<string,string> StringPairMap;
typedef StringPairMap::iterator StringPairMapIt;
typedef map<string,LINKED_ENTRY> LinkedEntryMap;
typedef map<string,LINKED_ENTRY>::iterator LinkedEntryMapIt;

enum TableReaderType { ReadTypeInsert, ReadTypeDump };

class TableReader
{
public:
	TableReader(const TableReaderType type);
	~TableReader();

	bool OpenTable(const char* TableFilename);

	bool GetTextValue(const string& Hexstring, string& RetTextString, int& linkbytes);
	bool GetHexValue(const string& Textstring, string& RetHexString);	

	bool OutputError(const char* filename);
	bool OutputError();

	int LongestHex;       // The longest hex entry, in bytes
	int LongestText[256]; // LUT for the longest text string starting with the first character
	
	string DefAutoFill;
	string DefAlignFill;
	
	string DefEndLine;
	string DefEndString;

private:
	inline bool parseautofill(string line);
	inline bool parseentry(string line);
	inline bool parselink(string line);

	inline bool AddToMaps(string& HexString, string& TextString);
	inline void InitHexTable();
	
	vector<TBL_ERROR> TableErrors;
	
	StringPairMap LookupText; // for looking up text values.  (Dumping)
	StringPairMap LookupHex;  // for looking up hex values.  (Insertion)
	
	LinkedEntryMap LinkedEntries;  // $FF=<text>:num type entries (Dumping)

	int LineNumber;

	TableReaderType ReaderType;
};
