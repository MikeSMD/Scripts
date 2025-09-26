#include <iostream>



//#include "sum_and_division.h"
#include "Losses.h"
#include "activations.h"
#include "mnist_train.h"
int main( void )
{

    // auto* sad = new Sum_and_division( {2,5,3,2}, 0.0000001, 5 );
    auto* sad = new Mnist_train( { 784, 165, 65, 10 } , 0.01, 5 );
    sad->setLoss( Losses::MSE );
    sad->setLossDerivation ( Losses::MSEDerivation );
	sad->setActivation( Activations::sigmoid);
	sad->setActivationDerivation( Activations::sigmoid_derivative);
	

	sad->train( 100000 );
    sad->run();

    delete sad;
    return 0;
}



