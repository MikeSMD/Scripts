#include <iostream>
#include "Tree.h"
#include <fstream>
#include <sstream>
#include <vector>
#include <string>
#include <iostream>

#include <algorithm>
#include <cctype>
#include <locale>

std::string clean_word(const std::string &s)
{
    std::string result;
    for (unsigned char c : s)
    {
        // A-Z, a-z, 0-9 zůstane, cokoliv nad 127 (UTF-8 diakritika) taky
        if (std::isalnum(c) || c >= 128)
            result += c;
    }
    std::cout << result << " ";
    return result;
}

int main( void )
{

	std::ifstream file("corpusCzech.txt");
	if (!file.is_open()) {
		std::cerr << "Nelze otevrit soubor\n";
		return 1;
	}

	std::vector<std::string> words;
	std::string line;
	while (std::getline(file, line)) {
		std::istringstream iss(line);
		std::string word;
		while(iss >> word) {
    std::string w = clean_word(word);
    if(!w.empty())
        words.push_back(w);
}
	}
	file.close();

	int p;
	std::cin >> p;
	Tree tree(p);                 
	tree.Process(words);         
	std::vector<std::string> data2 = {};

	while( 1 )
	{
		data2 = {};
		std::string qw;
		for ( int i = 0 ; i < p - 1; ++i )
		{
			std::cin >> qw;
		}
		data2.emplace_back( qw );
	std::string pred = tree.prediction(data2);
	std::cout << "Predikce: " << pred << std::endl;
	}

	return 0;
}
