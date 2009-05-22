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
	int jumpbytes;
	
	if(tbl.OpenTable("test.tbl")==true){
		cout<<"open ok\n";
		if(tbl.GetTextValue("7890",ret,jumpbytes)==true)
			cout<<ret<<","<<jumpbytes<<"\n";
		else
			cout<<"cant find";
		if(tbl.GetTextValue("0e",ret,jumpbytes)==true)
			cout<<ret<<","<<jumpbytes<<"\n";
		else
			cout<<"cant find";
		if(tbl.GetTextValue("90",ret,jumpbytes)==true)
			cout<<ret<<","<<jumpbytes<<"\n";
		else
			cout<<"cant find";
		
        tbl.OutputError();
	}
	else{
        tbl.OutputError();
    }
	return 0;
}

