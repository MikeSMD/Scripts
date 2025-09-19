#include <iostream>
#include "neuralnet.h"
#include <algorithm>
int main( void )
{

	ANN* ann = new ANN( {2,3,5,12,15,22,1} );
	ANN::Neuron::SetMethod( [](double x) { return std::max(0.0,x ); } );
	

	while( true )
	{
		double k,p;
		std::cin >> k;
		std::cin >> p;
		std::cout << ann->ForwardPass( { k,p })[ 0 ] << std::endl;
	}	

	delete ann;
	return 0;
}

std::function<double(double)> ANN::Neuron::activation_method = nullptr;

