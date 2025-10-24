#include <iostream>



//#include "sum_and_division.h"
#include "Losses.h"
#include "activations.h"
#include "mnist_train.h"

void qwe( double learning )
{
    //  auto* sad = new Sum_and_division( {2,5,3,2}, 0.0001, 5 );
    auto* sad = new Mnist_train( { 784, 128, 64, 10 } , learning,32);
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

    sad->setadams(0.9,0.999);
    double success = 0.1;
    while ( success <= 75)
    {
        /*
        if ( success > 50 )
        {
            std::cout << "learning to 0.03" << std::endl;
            sad->setLearningRate(0.01);
        }
        if (success > 70)
        {
            std::cout << "learning to 0.005" << std::endl;
            sad->setLearningRate(0.005);
        }
        if ( success > 75 ) sad->setLearningRate(0.001);
*/
	    sad->train( 60000 );
        double cur = sad->testAnn();
        std::cout << "data trained with " << cur << "\% success percentage" << std::endl;
        
        success = cur;
    }
    sad->run();
    delete sad;
}

int main( void )
{
    qwe(0.001);
    return 1;
}



