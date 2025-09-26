#include <iostream>



#include "sum_and_division.h"
#include "Losses.h"
#include "activations.h"
// #include "mnist_train.h"
int main( void )
{
     auto* sad = new Sum_and_division( {2,5,3,2}, 0.0001, 5 );
  //  auto* sad = new Mnist_train( { 784, 165, 65, 10 } , 0.01, 5 );
    sad->setLoss( Losses::SSE);
    sad->setLossDerivation ( Losses::SSEDerivation);
	sad->chooseActivation( Activations::ReLu);
    sad->setActivation( [sad]( std::vector< double >& vec, std::size_t p ) { sad->classicActivations(vec,p); });
	sad->chooseActivationDerivation( Activations::ReLuDerivation);
    sad->setActivationDerivation( [sad]( std::vector< double >& vec, std::size_t p  ) { sad->classicActivationsDerivation( vec, p); })	;

	//sad->train( 1000000 );
    sad->run();

    delete sad;
    return 1;
}



