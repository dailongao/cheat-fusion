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
		cout<<tbl.GetLinkBytes("1a",ret);
		cout<<ret<<"\n";
		ret.clear();

		cout<<tbl.DefAutoFill<<"\n";
		
		cout<<tbl.GetLinkBytes("8790",ret);
		cout<<ret;
		
        tbl.OutputError();
	}
	else{
        tbl.OutputError();
    }
	return 0;
}

