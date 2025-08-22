
#include <vector>
#include <unordered_map>
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

	public:
	Tree( int n_value )
	{
		this->depth = n_value - 1;
		root = new Node();
	}

	void Process( std::vector< std::string > data )
	{
		int index = 0;
		int tmp_index = 0;
		Node* p = this->root;
		while( index + tmp_index < data.Size() )
		{
			std::string value = data [ index + tmp_index ];
			Node* r = p->nodes[ value ];
			if ( r == nullptr )
			{
				p->nodes[ value ] = new Node( );
			}
			else if ( tmp_index == depth )
			{
				r->count += 1;
				tmp_index = 0;
				index = 1;
				p = this->root;
				continue;
			}
			p = r;
			tmp_index ++;
		}
	}

	std::vector < std::pair< std::string, double > > makeProp()
	{
		std::vector < std::pair< std::string, double > > res = {};
		return res;
	}

};
