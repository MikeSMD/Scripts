#include <vector>
#include <unordered_map>
#include <random>
struct Node
{
	int count;
	std::unordered_map< std::string, Node* > nodes;

	Node()
	{
		count = 1;
		nodes = std::unordered_map< std::string, Node* >();
	}
};

class Tree
{
	private:
	Node* root ;
	int depth;
	std::unordered_map< std::string, std::vector< std::pair < std::string, double > > > cache;
	public:
	Tree( int n_value )
	{
		this->cache = {};
		this->depth = n_value - 1;
		root = new Node();
	}

	void Process( std::vector< std::string > data )
	{
		int index = 0;
		int tmp_index = 0;
		Node* p = this->root;

		while(index + tmp_index < data.size())
		{
			std::string value = data[index + tmp_index];
			Node* r;

		//	std::cout << value << std::endl;

			auto it = p->nodes.find(value);
			if(it == p->nodes.end())
			{
	//			std::cout << "novy r do dalsi urovne" << std::endl;
				p->nodes[value] = new Node();
				r = p->nodes[value];
			}
			else
			{
		//		std::cout << "nalezen - presun r do dalsi urovne" << std::endl;
				r = it->second;
			}

			if(tmp_index == depth )
			{

			//	std::cout << "hloubka - reser;" << std::endl;
				r->count += 1;
				tmp_index = 0;
				index += 1;     
				p = this->root;
				continue;
			}
			//	std::cout << "p je r, tmp index++" << std::endl;
			p = r;
			tmp_index++;
		}
	}


	void ResetCache()
	{
		cache = {};
	}
	std::string prediction( std::vector< std::string > data  )
	{

			std::vector< std::pair < std::string, double > > datar;
		std::string key;
		for (const auto &w : data) {
			key += w + ",";
		}

		auto it = cache.find(key);
		if (it == cache.end()) {

			Node* position = this->root;
			int index = 0;
			while( position->nodes.size() != 0 && index < data.size() )
			{
				auto it = position->nodes.find(data[index]);
			//	std::cout << "hledam v position" << data[ index ] << " ";
				if(it == position->nodes.end())
				{
			//		std::cout << "nenalezen" << data[ index ]<< " ";;
					break;
				}
				Node* s = it->second;
			//	std::cout << "prirazuji position = s" << data[ index ]<< " ";;
				
				index++;
				position = s;
			}

			if ( position == this-> root)
			{
				return "qwe";
			}
			else if ( position->nodes.size() == 0 )
				return "ewq";


			std::vector< std::pair < std::string, double > > counter = {};
			int summer = 0;
			for ( const auto pr : position->nodes )
			{
				summer += pr.second->count;
				counter.emplace_back( std::make_pair ( pr.first, pr.second->count ) );
			}

			for ( int i = 0; i < counter.size(); ++i )
			{
				counter[ i ].second /= ( double )summer;
			}

			cache[ key ] = counter;
			datar = counter;
		}
		else {
	 		datar = cache [ key ];
		}
		std::random_device rd;                  
		std::mt19937 gen(rd());              
		std::uniform_real_distribution<> dis(0.0, 1.0); 

		double ps = dis( gen );

		for ( auto& i : datar )
		{
			ps -= i.second;
			if ( ps <= 0 ) return i.first;
		}

		return "unknown";
		
	}

};
