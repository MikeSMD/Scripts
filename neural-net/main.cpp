#include <iostream>
#include "neuralnet.h"
#include <algorithm>


#include <math.h>


double loss(double predicted, double target )
{
    return pow( predicted - target , 2 );
}

double loss_derivative(double predicted, double target )
{
    return 2*( predicted - target );
}



double ReLu( double input )
{
    if ( input < 0 )
    {
        return 0;
    }
    else return input ;
}
double ReLuDerivation( double input )
{
    if ( input <= 0 )
    {
        return 0;
    }
    else return 1;
}


int main( void )
{

	ANN* ann = new ANN( {2,5,1} );
    ann->setLoss( loss );
    ann->setLossDerivative( loss_derivative );
	ANN::Neuron::SetMethod( ReLu );
    ANN::Neuron::SetDerivative( ReLuDerivation );
	

	while( true )
	{
		double k,p;
		std::cin >> k;
		std::cin >> p;
        const std::vector<double>& result = ann->ForwardPass( { k,p } );
        for ( int i = 0; i < result.size(); ++i )
        {
            std::cout << result[ i ] << ",";
        }
        ann->gradientCounter({ p });
        std::cout << std::endl;
	}	

	delete ann;
	return 0;
}

std::function<double(double)> ANN::Neuron::activation_method = nullptr;
std::function<double(double)> ANN::Neuron::derivative = nullptr;

