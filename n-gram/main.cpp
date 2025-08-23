#include <iostream>
#include "Tree.h"
#include <fstream>
#include <sstream>
#include <vector>
#include <string>
#include <iostream>

#include <algorithm>
#include <cctype>

std::string clean_word(const std::string &s)
{
    std::string result;
    for(char c : s)
    {
        if(std::isalnum(c)) // ponechá písmena a čísla
            result += std::tolower(c); // můžeš i převést na malé písmeno
    }
    return result;
}

int main( void )
{

	std::ifstream file("corpus.txt");
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
