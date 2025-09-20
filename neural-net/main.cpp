#include <iostream>
#include "neuralnet.h"
#include <algorithm>


#include <math.h>

void train( int p,ANN& ann )
{
    auto& generator = Generator::GetInstance();
    for ( int i = 0; i < p; ++i )
    {
        const double q = generator.generateDouble(5.0, 10.0);
        const double r = generator.generateDouble(5.0, 10.0);

        ann.ForwardPass( { q,r }, {q+r} );
        std::cout << std::endl;
    }
}

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

	ANN* ann = new ANN( {2,5,1}, 0.001, 5 );
    ann->setLoss( loss );
    ann->setLossDerivative( loss_derivative );
	ANN::Neuron::SetMethod( ReLu );
    ANN::Neuron::SetDerivative( ReLuDerivation );
	

	while( true )
	{
		double k,p;
		std::cin >> k;
		std::cin >> p;

        if ( k == -1.0 )
        {
            train( (int)p, *ann);
        }
        else
        {
        const std::vector<double>& result = ann->ForwardPass( { k,p }, { k+p } );
        for ( int i = 0; i < result.size(); ++i )
        {
            std::cout << result[ i ] << ",";
        }
        }
        std::cout << std::endl;
	}	

	delete ann;
	return 0;
}

std::function<double(double)> ANN::Neuron::activation_method = nullptr;
std::function<double(double)> ANN::Neuron::derivative = nullptr;

