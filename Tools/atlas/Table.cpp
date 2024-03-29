//-----------------------------------------------------------------------------
// Table - A table library by Klarth, http://rpgd.emulationworld.com/klarth
// email - stevemonaco@hotmail.com
// Open source and free to use
//-----------------------------------------------------------------------------

#include "stdafx.h"
#include <string>
#include <vector>
#include <fstream>
#include <cstdlib>
#include <hash_map>
#include <utility>
#include "Table.h"
#include "AtlasCore.h"

using namespace std;

// Constructor


Table::Table()
{
	TblEntries = 0;
	//DefEndLine = "<line>";
	//DefEndString = "<end>";
	memset(LongestText, 0, 256*4);
	LongestText[(int)'<'] = 5; // Length of <$XX>
	LongestHex = 1;
	StringCount = 0;
	bAddEndToken = true;

	bAddFillChar=false;
	FillChar=0;
	TestFillChar=0;
}

// Destructor

Table::~Table()
{
	// Clear Errors
	if(!Errors.empty())
		Errors.clear();

	// Clear the hash maps
	if(!LookupHex.empty())
		LookupHex.clear();
	//if(!LookupText.empty())
	//	LookupText.clear();
}



//-----------------------------------------------------------------------------
// EncodeStream() - Encodes text in a vector to the string tables
//-----------------------------------------------------------------------------

unsigned int Table::EncodeStream(string& scriptbuf, unsigned int& BadCharOffset)
{
	TBL_STRING TblString;
	TXT_STRING TxtString;
	string hexstr;
	string subtextstr;
	unsigned char i;
	unsigned int EncodedSize = 0;
	bool bIsEndToken = false;
	std::map<string,string>::iterator hashit;
	unsigned int BufOffset = 0;
	
	hexstr.reserve(LongestHex * 2);

	if(scriptbuf.empty())
		return 0;

	if(!StringTable.empty())
	{
		TBL_STRING RestoreStr = StringTable.back();
		if(RestoreStr.EndToken.empty()) // No end string...restore and keep adding
		{
			StringTable.pop_back();
			TblString.Text = RestoreStr.Text;
		}
	}

	while(BufOffset < scriptbuf.size()) // Translate the whole buffer
	{
		bIsEndToken = false;
		i = LongestText[(unsigned char)scriptbuf[BufOffset]]; // Use LUT
		while(i)
		{
			subtextstr = scriptbuf.substr(BufOffset, i);
			hashit = LookupHex.find(subtextstr);
			if(hashit == LookupHex.end()) // if the entry isn't found
			{
				i--;
				continue;
			}

			hexstr = hashit->second;

			// Search to see if it's an end token, if it is, add to the string table
			for(unsigned int j = 0; j < EndTokens.size(); j++)
				if(EndTokens[j] == subtextstr)
				{
					bIsEndToken = true;
					if(bAddEndToken)
						//TblString.Text.insert(TblString.Text.end(), &hexstr[0], hexstr.end());
						AddToTable(hexstr, &TblString);

					TblString.EndToken = subtextstr;
					EncodedSize += (unsigned int)TblString.Text.size();
					StringTable.push_back(TblString);
					TblString.EndToken = "";
					TblString.Text.clear();
					break; // Only once
				}

			if(!bIsEndToken)
				//TblString.Text.insert(TblString.Text.end(), &hexstr[0], hexstr.end());
				AddToTable(hexstr, &TblString);

			BufOffset += i;
			break; // Entry is finished
		}
		if (i == 0) // no entries found
		{
			BadCharOffset = BufOffset;
			return -1;
		}
	}

	// Encode any extra data that doesn't have an EndToken
	if(!TblString.Text.empty())
	{
		StringTable.push_back(TblString);
	}

	EncodedSize += (unsigned int)TblString.Text.size();

	scriptbuf.clear();

	return EncodedSize;
}



inline void Table::InitHexTable()
{
	char textbuf[16];
	char hexbuf[16];

	for(unsigned int i = 0; i < 0x100; i++)
	{
		sprintf(textbuf, "<$%02X>", i);
		sprintf(hexbuf, "%02X", i);
		LookupHex.insert(map<string, string>::value_type(string(textbuf), string(hexbuf)));
	}
	for(unsigned int i = 0x0A; i < 0x100; i += 0x10)
	{
		for(unsigned int j = 0; j < 6; j++)
		{
			sprintf(textbuf, "<$%02x>", i+j);
			sprintf(hexbuf, "%02X", i+j);
			LookupHex.insert(map<string, string>::value_type(string(textbuf), string(hexbuf)));
		}
	}
}

//-----------------------------------------------------------------------------
// GetHexValue() - Returns a Hex value from a Text string from the table
//-----------------------------------------------------------------------------

inline string Table::GetHexValue(string& Textstring)
{
	return (LookupHex.find(Textstring))->second;
}



//-----------------------------------------------------------------------------
// GetTextValue() - Returns a Text String from a Hexstring from the table
//-----------------------------------------------------------------------------

//inline string& Table::GetTextValue(string& Hexstring)
//{
	//return (LookupText.find(Hexstring))->second;
//}



//-----------------------------------------------------------------------------
// OpenTable() - Opens, Parses, and Loads a file to memory
//-----------------------------------------------------------------------------

int Table::OpenTable(const char* TableFilename)
{
	string HexVal;
	char testchar;
	string TextString;

	LineNumber = 1;
	LookupHex.clear();
	InitHexTable();
	//LookupText.clear();

	ifstream TblFile(TableFilename);
	if(!TblFile.is_open()) // File can't be opened
		return TBL_OPEN_ERROR;

	// Read the Table File until eof
	while(!TblFile.eof())
	{
		HexVal.clear();
		TextString.clear();
		// Read the hex number, skip whitespace, skip equal sign
		parsews(TblFile);

		TblFile.get(testchar);
		if(TblFile.eof())
			break;
		TblFile.seekg(-1, ios::cur);

		switch(testchar)
		{
		case '(':
			if(parsebookmark(TblFile))
				break;
			else
				return TBL_PARSE_ERROR;
		case '[':
			parsescriptdump(TblFile);
			break;
		case '{':
			parsescriptinsert(TblFile);
			break;
		case '/':
			if(parseendstring(TblFile))
				break;
			else
				return TBL_PARSE_ERROR;
		case '*':
			if(parseendline(TblFile))
				break;
			else
				return TBL_PARSE_ERROR;
		case '@':
			if(parsefillchar(TblFile))
				break;
			else
				return TBL_PARSE_ERROR;
		case '~':
			if(parsetestfillchar(TblFile))
				break;
			else
				return TBL_PARSE_ERROR;
		default:
			if(parseentry(TblFile))
				break;
			else
				return TBL_PARSE_ERROR;
		}

		TblEntries++;

	} // End table reading loop

	return TBL_OK;
}



//"@xx"��ʽ xx��Ϊjmp�������������
inline bool Table::parsefillchar(ifstream& file)
{
	char testch;
	unsigned char temp[4],count=0;
	temp[0]=0;temp[1]=0;	//��ֻ��"@"����,Ĭ�����0
	
	file.get(testch); // the @
	parsews(file);
	
	// Get the hex
	while(true)
	{
		file.get(testch);
		if((testch == '\n') || file.eof() || (testch == '='))
			break;
		temp[count++]=HexToDec(testch);
	}
	
	if(testch == '\n') // ������"@xx"������
	{
		bAddFillChar = true;
		FillChar = temp[0]*16 + temp[1];
	}
	else return false;
	
	return true;
}

//"~xx"��ʽ xx��Ϊtest����������
inline bool Table::parsetestfillchar(ifstream& file)
{
	char testch;
	unsigned char temp[4],count=0;
	temp[0]=0;temp[1]=0;	//��ֻ��"~"����,Ĭ�����0
	
	file.get(testch); // the ~
	parsews(file);
	
	// Get the hex
	while(true)
	{
		file.get(testch);
		if((testch == '\n') || file.eof() || (testch == '='))
			break;
		temp[count++]=HexToDec(testch);
	}
	
	if(testch == '\n') // ������"~xx"������
	{
		TestFillChar = temp[0]*16 + temp[1];
	}
	else return false;
	
	return true;
}

//-----------------------------------------------------------------------------
// parsebookmark() - Parses a bookmark like (8000h)Text1
//-----------------------------------------------------------------------------

inline bool Table::parsebookmark(ifstream& file)
{
	char testch;
	string bookname;
	string hexaddress;
	unsigned int address;

	file.get(testch); // should be '('

	while(true)
	{
		file.get(testch);
		if((file.eof()) || (testch == 'h') || (testch == 'H') || (testch == '\n'))
			break;
		hexaddress += testch;
	}
	
	// Convert a hex string to an unsigned long
	address = strtoul(hexaddress.c_str(), NULL, 16);

	parsews(file);
	file.get(testch); // should be ')'
	if(testch != ')')
		return false;

	parsews(file);

	// Get the name
	while(true)
	{
		file.get(testch);
		if((testch == '\n') || file.eof())
			break;
		bookname += testch;
	}

	TBL_BOOKMARK bookmark;
	bookmark.address = address;
	bookmark.description = bookname;

	Bookmarks.push_back(bookmark);

	return true;
}

//-----------------------------------------------------------------------------
// parseendline() - parses a break line table value: ex, *FE
// You can also define messages like *FE=<End Text>
//-----------------------------------------------------------------------------

inline bool Table::parseendline(ifstream& file)
{
	char testch;
	string hexstr, textstr;
	
	file.get(testch); // the *
	parsews(file);

	// Get the hex
	while(true)
	{
		file.get(testch);
		if((testch == '\n') || file.eof() || (testch == '='))
			break;
		hexstr += testch;
	}

	if(testch != '=') // normal entry
	{
		// Add to the hash map
		LookupHex[DefEndString] = hexstr;
		//LookupText[hexstr] = DefEndString;
	}
	else
	{
		// Get what the string is
		while(true)
		{
			file.get(testch);
			if((testch == '\n') || file.eof())
				break;
			textstr += testch;
		}

		// Add custom message to the hash map
		LookupHex[textstr] = hexstr;
		//LookupText[hexstr] = textstr;
	}

	return true;
}



//-----------------------------------------------------------------------------
// parseendstring() - parses a string break table value
// Only ones like /FF=<end>
// and /<end> (gives a blank string value)
//-----------------------------------------------------------------------------

inline bool Table::parseendstring(ifstream& file)
{
	char testch;
	string hexstr, textstr;
	
	file.get(testch); // the /
	parsews(file);

	// Get the first part
	while(true)
	{
		file.get(testch);
		if((testch == '\n') || file.eof() || (testch == '='))
			break;
		hexstr += testch;
	}

	if(testch == '\n') // Must be a blank value string (/<end>)
	{
		size_t Pos = hexstr.find_first_of("0123456789ABCDEF");
		if(Pos == 0)
			return false;
		textstr = hexstr;
		hexstr.clear();
	}
	else if(testch == '=') // Must be a /FF=<end> type string
	{
		while(true)
		{
			file.get(testch);
			if((testch == '\n') || file.eof())
				break;
			textstr += testch;
		}
	}
	else
		return false;

	// Add custom string to the hash map
	LookupHex[textstr] = hexstr;
	EndTokens.push_back(textstr);
	//LookupText[hexstr] = textstr;

	return true;
}



//-----------------------------------------------------------------------------
// parseentry() - parses a hex = text line
//-----------------------------------------------------------------------------

inline bool Table::parseentry(ifstream& file)
{
	char testch;
	string Hex, Text;
	string PackHexStr;

	// get the hex
	while(true)
	{
		file.get(testch);
		if((testch == SPACE) || (testch == '='))
			break;
		else
			Hex += testch;
	}

	// get the equal sign
	if(testch != '=')
	{
		parsews(file);
		file.get(testch);
		if(testch != '=')
			return false; // bad formatting
	}

	// get the value
	file.get(testch);
	while(!file.eof() && testch != '\n')
	{
		Text += testch;
		file.get(testch);
	}
	
	// Hex entries are strings, so divide the length by two to get the bytes
	if(Hex.length() & 1) // Not a 8n bit hex number
		Hex.insert(0, "0");
	if((Hex.length() >> 1) > LongestHex)
		LongestHex = ((unsigned int)Hex.length() / 2);

	// Get the longest text string
	if(Text.length() > LongestText[(unsigned char)Text[0]])
		LongestText[(unsigned char)Text[0]] = (int)Text.length();

	// Add the entries into the hash maps
	// Make the Hex string all capital
	//for(unsigned int i = 0; i < Hex.length(); i++)
	//	Hex[i] = (char)toupper(Hex[i]);

	// Convert text hex to a chars
	//for(unsigned int k = 0; k < Hex.length(); k+=2)
	//	PackHexStr += char(HexToDec(Hex[k+1]) | (HexToDec(Hex[k]) << 4));
	//LookupHex[Text] = Hex;
	//LookupText[Hex] = Text;
	LookupHex.insert(std::map<string,string>::value_type(Text, Hex));
	//LookupText.insert(std::_map<string,string>::value_type(Hex, Text));

	return true;
}



//-----------------------------------------------------------------------------
// parsescriptdump() - Parses a script dump entry, like [8000h-8450h]Block 1
//-----------------------------------------------------------------------------

inline bool Table::parsescriptdump(ifstream& file)
{
	char testch;
	unsigned int HexAddr1, HexAddr2;
	string HexOff1, HexOff2, Description;
	TBL_DUMPMARK dumpmark;

	file.get(testch); // the '['

	// The first hex entry
	while(true)
	{
		file.get(testch);
		if((file.eof()) || (testch == '-') || (testch == '\n'))
			break;
		HexOff1 += testch;
	}

	HexAddr1 = strtoul(HexOff1.c_str(), NULL, 16);
	parsews(file);

	// The second hex entry
	while(true)
	{
		file.get(testch);
		if((file.eof()) || (testch == ']') || (testch == '\n'))
			break;
		HexOff2 += testch;
	}

	HexAddr2 = strtoul(HexOff2.c_str(), NULL, 16);
	parsews(file);

	// The name of the scriptdump
	while(true)
	{
		file.get(testch);
		if((testch == '\n') || file.eof())
			break;
		Description += testch;
	}

	if(HexAddr1 <= HexAddr2)
	{
		dumpmark.StartAddress = HexAddr1;
		dumpmark.EndAddress = HexAddr2;
	}
	else
	{
		dumpmark.StartAddress = HexAddr2;
		dumpmark.EndAddress = HexAddr1;
	}

	dumpmark.description = Description;

	Dumpmarks.push_back(dumpmark);

	return 1;
}



//-----------------------------------------------------------------------------
// parsescriptinsert() - Parses script insert bookmarks
//                       ex - {8000h-TextDump.txt}Block-e 1
//-----------------------------------------------------------------------------

inline bool Table::parsescriptinsert(ifstream& file)
{
	char testch;
	int HexAddress;
	string HexOff, FileName, Description;
	TBL_INSMARK insertmark;

	file.get(testch); // {

	// Get the hex offset
	while(true)
	{
		file.get(testch);
		if((file.eof()) || (testch == '-') || (testch == '\n'))
			break;
		HexOff += testch;
	}

	HexAddress = strtoul(HexOff.c_str(), NULL, 16);
	parsews(file);

	// Get the filename
	while(true)
	{
		file.get(testch);
		if((file.eof()) || (testch == ')') || (testch == '\n'))
			break;
		HexOff += testch;
	}

	parsews(file);

	// Get the description
	while(true)
	{
		file.get(testch);
		if((testch == '\n') || file.eof())
			break;
		Description += testch;
	}

	insertmark.address = HexAddress;
	insertmark.filename = FileName;
	insertmark.description = Description;

	Insertmarks.push_back(insertmark);

	return 1;
}



//-----------------------------------------------------------------------------
// parsews() - Eats all blanks and eoln's until a valid character or eof
//-----------------------------------------------------------------------------

inline void Table::parsews(ifstream& file)
{
	char testch;
	do{
		file.get(testch);
		if(testch == '\n')
			LineNumber++;
	}while(((testch == SPACE) || (testch == '\n')) && (!file.eof()));
	
	if((!file.eof()) || (testch != '\n'))
		file.seekg(-1, ios::cur);
}



//-----------------------------------------------------------------------------
// HexToDec(...) - Converts a hex character to its dec equiv
//-----------------------------------------------------------------------------

inline unsigned short HexToDec(char HexChar)
{
	switch(HexChar)
	{
	case '0': return 0;
	case '1': return 1;
	case '2': return 2;
	case '3': return 3;
	case '4': return 4;
	case '5': return 5;
	case '6': return 6;
	case '7': return 7;
	case '8': return 8;
	case '9': return 9;
	case 'a': case 'A': return 10;
	case 'b': case 'B': return 11;
	case 'c': case 'C': return 12;
	case 'd': case 'D': return 13;
	case 'e': case 'E': return 14;
	case 'f': case 'F': return 15;
	}

	// Never gets here
	printf("badstr");
	return 15;
}

inline void Table::AddToTable(string& Hexstring, TBL_STRING* TblStr)
{
	for(unsigned int k = 0; k < Hexstring.length(); k+=2)
		TblStr->Text += (HexToDec(Hexstring[k+1]) | (HexToDec(Hexstring[k]) << 4));
}