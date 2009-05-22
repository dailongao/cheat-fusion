//#include "stdafx.h" // Needed for MSVC++ Precompiled Headers option
#include <iostream>
#include <algorithm>
#include <fstream>
#include <string>
#include <vector>
#include <map>
#include <list>
#include <cctype>
#include "TableReader.h"

const char* HexAlphaNum = "ABCDEFabcdef0123456789";
TBL_ERROR err;

TableReader::TableReader(const TableReaderType type)
{
	DefAutoFill = "00";
	DefAlignFill = "00";
	DefEndLine = "{N}";
	DefEndString = "{end}";
	LongestHex = 1;
	memset(LongestText, 0, 256*4);
	ReaderType = type;
	LineNumber = 0;

	if(type == ReadTypeInsert)
		InitHexTable();
}

TableReader::~TableReader()
{
	// Clear the maps
	if(!LookupText.empty())
		LookupText.clear();
	if(!LookupHex.empty())
		LookupHex.clear();
	if(!LinkedEntries.empty())
		LinkedEntries.clear();
}

bool TableReader::OpenTable(const char* TableFilename)
{
	list<string> EntryList;
	string Line;

	ifstream tablefile(TableFilename);

	if(!tablefile.is_open())
	{
		err.LineNumber = -1;
		err.Description = "Table file cannot be opened";
		TableErrors.push_back(err);
		return false;
	}

	// Gather text into the list
	while(!tablefile.eof())
	{
		getline(tablefile, Line);
		EntryList.push_back(Line);
	}
	
	tablefile.close();

	for(list<string>::iterator i = EntryList.begin(); i != EntryList.end(); i++)
	{
		LineNumber++;
		if(i->length() == 0) // Blank line
			continue;

		switch((*i)[0]) // first character of the line
		{
		case '$':
			parselink(*i);
			break;
		case '(': // Bookmark (not implemented)
			break;
		case '[': // Script dump (not implemented)
			break;
		case '{': // Script insert (not implemented)
			break;
		case '@':	//insert auto fill char
			parseautofill(*i);
			break;
		case '~':	//insert test alignment fill char
			break;
		case '/': // End string value
			break;
		case '*': // End line value
			break;
		case '0': case '1': case '2': case '3': case '4': case '5': case '6': case '7': case '8': case '9':
		case 'a': case 'b': case 'c': case 'd': case 'e': case 'f': case 'A': case 'B': case 'C': case 'D':
		case 'E': case 'F': // Normal entry value
			parseentry(*i);
			break;
		default:
			err.LineNumber = LineNumber;
			err.Description = "First character of the line is not a recognized table character";
			TableErrors.push_back(err);
			break;
		}
	}

	if(TableErrors.empty())
		return true;
	else
		return false;
}

//-----------------------------------------------------------------------------
// GetTextValue() - Returns a Text String from a Hexstring from the table
//-----------------------------------------------------------------------------

bool TableReader::GetTextValue(const string& Hexstring, string& RetTextString)
{
	StringPairMapIt it = LookupText.find(Hexstring);
	if(it != LookupText.end()) // Found
	{
		RetTextString = it->second;
		return true;
	}
	return false;
}

//-----------------------------------------------------------------------------
// GetHexValue() - Returns a Hex value from a Text string from the table
//-----------------------------------------------------------------------------

bool TableReader::GetHexValue(const string& Textstring, string& RetHexString)
{
	StringPairMapIt it = LookupHex.find(Textstring);
	if(it != LookupHex.end()) // Found
	{
		RetHexString = it->second;
		return true;
	}
	return false;
}

int TableReader::GetLinkedBytes(const string& Hexstring, string& RetTextString)
{
	LinkedEntryMapIt it = LinkedEntries.find(Hexstring);
	if(it != LinkedEntries.end())
	{
		RetTextString = it->second.Text;
		return it->second.Number;
	}
	return 0;
}

bool TableReader::OutputError(const char* filename)
{
	ofstream logtxt(filename, ios::out);
	vector<TBL_ERROR>::iterator it;
	for(it = TableErrors.begin();it!=TableErrors.end();it++)
		logtxt<<"line"<<it->LineNumber<<":"<<it->Description<<"\n";
	logtxt.close();
}
bool TableReader::OutputError()
{
	vector<TBL_ERROR>::iterator it;
	for(it = TableErrors.begin();it!=TableErrors.end();it++)
		cout<<"line"<<it->LineNumber<<":"<<it->Description<<"\n";
}

//-----------------------------------------------------------------------------
// AddToMaps() - Adds an entry to the table, if there aren't duplicates
//-----------------------------------------------------------------------------

inline bool TableReader::AddToMaps(string& HexString, string& TextString)
{
	string ModString = TextString;

	if(ReaderType == ReadTypeDump) // Check for multiple HexString entries (Dumping confliction)
	{
		transform(HexString.begin(), HexString.end(), HexString.begin(), (int(*)(int))toupper);
		StringPairMapIt it = LookupText.lower_bound(HexString);
		if(it != LookupText.end() && !(LookupText.key_comp()(HexString, it->first))) // Multiple entry
		{
			err.LineNumber = LineNumber;
			err.Description = "Hex token part of the string is already in the table.";
			TableErrors.push_back(err);
			return false;
		}
		else // Dumpers only need to look up text
			LookupText.insert(it, StringPairMap::value_type(HexString, ModString));
	}

	if(ReaderType == ReadTypeInsert) // Check for multiple TextString entries (Insertion confliction)
	{
		StringPairMapIt it = LookupHex.lower_bound(ModString);
		if(it != LookupHex.end() && !(LookupHex.key_comp()(ModString, it->first))) // Multiple entry
		{
			err.LineNumber = LineNumber;
			err.Description = "Text token part of the string is already in the table.";
			TableErrors.push_back(err);
			return false;
		}
		else // Inserters only need to look up hex values
			LookupHex.insert(it, StringPairMap::value_type(ModString, HexString));
	}

	// Update hex/text lengths
	if(LongestHex < (int)HexString.length())
		LongestHex = (int)HexString.length();
	if(ModString.length() > 0)
	{
		if(LongestText[(unsigned char)ModString[0]] < (int)ModString.length())
			LongestText[(unsigned char)ModString[0]] = (int)ModString.length();
	}

	return true;
}

//-----------------------------------------------------------------------------
// InitHexTable() - Adds the <$XX> strings for insertion
//-----------------------------------------------------------------------------

inline void TableReader::InitHexTable()
{
	char textbuf[16];
	char hexbuf[16];

	// Add capital case <$XX> entries to the lookup map
	for(unsigned int i = 0; i < 0x100; i++)
	{
		sprintf(textbuf, "<$%02X>", i);
		sprintf(hexbuf, "%02X", i);
		LookupHex.insert(map<string, string>::value_type(string(textbuf), string(hexbuf)));
	}
	// Add lower case <$xx> entries to the lookup map
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
// parseautofill() - parses a auto fill table value: ex, @00
//-----------------------------------------------------------------------------

inline bool TableReader::parseautofill(string line)
{
	line.erase(0, 1);
	size_t pos = line.find_first_not_of(HexAlphaNum, 0);
	string hexstr;
	
	hexstr = line.substr(0, pos);

	if((hexstr.length() % 2) != 0)
	{
		err.LineNumber = LineNumber;
		err.Description = "Hex token length is not a multiple of 2";
		TableErrors.push_back(err);
		return false;
	}

	DefAutoFill = hexstr;
	return true;
}

//-----------------------------------------------------------------------------
// parseentry() - parses a hex=text line
//-----------------------------------------------------------------------------

inline bool TableReader::parseentry(string line)
{
	size_t pos = line.find_first_not_of(HexAlphaNum, 0);

	if(pos == string::npos)
	{
		err.LineNumber = LineNumber;
		err.Description = "Entry only contains hex values";
		TableErrors.push_back(err);
		return false;
	}

	string hexstr = line.substr(0, pos);

	if((hexstr.length() % 2) != 0)
	{
		err.LineNumber = LineNumber;
		err.Description = "Hex token length is not a multiple of 2";
		TableErrors.push_back(err);
		return false;
	}

	string textstr;
	pos = line.find_first_of("=", 0);
	if(pos == line.length() - 1) // End of the line, blank entry means it's an error
	{
		err.LineNumber = LineNumber;
		err.Description = "Contains a blank entry";
		TableErrors.push_back(err);
		return false;
	}
	else
	{
		line.erase(0, pos+1);
		textstr = line;
	}

	return AddToMaps(hexstr, textstr);
}

//-----------------------------------------------------------------------------
// parselink() - Parsed a linked entry, like $10F0={text}:2
//             - Prints both the {text} part and 2 bytes afterwards
//-----------------------------------------------------------------------------

inline bool TableReader::parselink(string line)
{
	LINKED_ENTRY l;
	size_t pos;

	line.erase(0, 1);
	pos = line.find_first_not_of(HexAlphaNum, 0);

	string hexstr = line.substr(0, pos);
	if((hexstr.length() % 2) != 0)
	{
		err.LineNumber = LineNumber;
		err.Description = "Hex token length is not a multiple of 2";
		TableErrors.push_back(err);
		return false;
	}

	string textstr;
	pos = line.find_first_of("=", 0);

	if(pos == string::npos)
	{
		err.LineNumber = LineNumber;
		err.Description = "No equal sign, linked entry format is $XX={text}:num";
		TableErrors.push_back(err);
		return false;
	}

	line.erase(0, pos+1);

	pos = line.find_first_of(":",0);

	if(pos == string::npos)
	{
		err.LineNumber = LineNumber;
		err.Description = "No comma, linked entry format is $XX={text}:num";
		TableErrors.push_back(err);
		return false;
	}

	textstr = line.substr(0, pos);
	line.erase(0, pos+1);

	pos = line.find_first_not_of("0123456789", 0);
	if(pos != string::npos)
	{
		err.LineNumber = LineNumber;
		err.Description = "Nonnumeric characters in num field, linked entry format is $XX={text}:num";
		TableErrors.push_back(err);
		return false;
	}

	l.Text = textstr;
	l.Number = strtoul(line.c_str(), NULL, 10);
	string ModString = textstr;
	if(ReaderType == ReadTypeDump) // Add linked entry to the dumper
	{
		transform(hexstr.begin(), hexstr.end(), hexstr.begin(), (int(*)(int))toupper);
		LinkedEntryMapIt it = LinkedEntries.lower_bound(hexstr);
		if(it != LinkedEntries.end() && !(LinkedEntries.key_comp()(hexstr, it->first))) // Multiple entries
		{
			err.LineNumber = LineNumber;
			err.Description = "Hex token part of the linked entry is already in the table.";
			TableErrors.push_back(err);
			return false;
		}
		else // Inserters only need to look up hex values
			LinkedEntries.insert(it, LinkedEntryMap::value_type(hexstr, l));
	}
	else if(ReaderType == ReadTypeInsert) // Add hexstr=textstr portion to the inserter
	{
		StringPairMapIt it = LookupHex.lower_bound(ModString);
		if(it != LookupHex.end() && !(LookupHex.key_comp()(ModString, it->first))) // Check for multiple TextString entries (Insertion confliction)
		{
			err.LineNumber = LineNumber;
			err.Description = "Text token part of the linked entry is already in the table.";
			TableErrors.push_back(err);
			return false;
		}
		else // Inserters only need to look up hex values
			LookupHex.insert(it, StringPairMap::value_type(ModString, hexstr));
	}
	
	if(LongestHex < (int)hexstr.length())
		LongestHex = (int)hexstr.length();
	if(ModString.length() > 0)
	{
		if(LongestText[(unsigned char)ModString[0]] < (int)ModString.length())
		LongestText[(unsigned char)ModString[0]] = (int)ModString.length();
	}

	return true;
}


