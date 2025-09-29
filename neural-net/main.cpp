#include <iostream>



//#include "sum_and_division.h"
#include "Losses.h"
#include "activations.h"
#include "mnist_train.h"
int main( void )
{
   //  auto* sad = new Sum_and_division( {2,5,3,2}, 0.0001, 5 );
    auto* sad = new Mnist_train( { 784, 165, 65, 10 } , 0.1, 5 );
    sad->loss = Losses::Cross_entropy;
    sad->loss_derivative = Losses::Cross_entropy_derivate;
    sad->setLoss( [sad](const Eigen::VectorXd& p, const Eigen::VectorXd& q ) { return sad->ClassicLosses( p,q ); } );
    sad->setLossDerivation( [sad](const Eigen::VectorXd& p, const Eigen::VectorXd& q ) { return sad->ClassicLossesDerivation( p,q ); } );
	sad->activation_hidden = Activations::ReLu;
    sad->activation_output = Activations::softMax;
    sad->activation_hidden_derivation = Activations::ReLuDerivation;
    sad->activation_output_derivation = Activations::softMaxDerivate;
    sad->setActivation( [sad]( const Eigen::VectorXd& vec, std::size_t p ) { return sad->classic_activation(vec,p); });
	sad->setActivationDerivation([sad]( const Eigen::VectorXd& vec, std::size_t p ) { return sad->classic_activation_derivation(vec,p); } );

	sad->train( 60000 );
    sad->run();

    delete sad;
    return 1;
}



