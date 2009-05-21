#include <cstdlib>
#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <map>
#include <list>
#include <algorithm>
#include "TableReader.h"

int main()
{
	TableReader tbl(ReadTypeDump);
	string ret;
	
	if(tbl.OpenTable("test.tbl")==true){
		cout<<"open ok\n";
		tbl.GetTextValue("7896543210",ret);
		cout<<ret<<"\n";
		ret.clear();
		if(tbl.GetHexValue("{end}",ret)==true)
			cout<<ret;
		else cout<<"err";
		
        tbl.OutputError();
	}
	else{
        tbl.OutputError();
    }
	return 0;
}

