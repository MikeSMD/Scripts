#include <iostream>



//#include "sum_and_division.h"
#include "Losses.h"
#include "activations.h"
#include "mnist_train.h"

void qwe( double learning )
{
    //  auto* sad = new Sum_and_division( {2,5,3,2}, 0.0001, 5 );
    auto* sad = new Mnist_train( { 784, 128, 64, 10 } , learning,64);
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

  //  sad->setadams(0.9,0.999);
    double success = -1.0;
    double lsuccess = -1.0;
    while ( success <= 75)
    {
        double diff = -1.0;
        if( lsuccess > -1.0 )
        {
            diff = abs(success - lsuccess);
        }
        if ( diff >= 0 )
        {
              if ( diff <= 0.15 )
            {
                std::cout << "learning to 0.2" << std::endl;
                sad->setLearningRate(0.2);
            }
            if ( diff <= 0.3 )
            {
                std::cout << "learning to 0.001" << std::endl;
                sad->setLearningRate(0.001);
            }
            if ( diff <= 1 )
            {
                std::cout << "learning to 0.05" << std::endl;
                sad->setLearningRate(0.05);
            }
            else if ( diff <= 2 )
            {
                std::cout << "learning to 0.005" << std::endl;
                sad->setLearningRate(0.005);
            }
            else if ( diff <= 3 )
            {
                std::cout << "learning to 0.02" << std::endl;
                sad->setLearningRate(0.02);
            }
            else if ( diff <= 5.0 )
            {
                std::cout << "learning to 0.03" << std::endl;
                sad->setLearningRate(0.005);
            }
            else sad->setLearningRate(0.001);
        }
	    sad->train( 60000 );
        double cur = sad->testAnn();
        std::cout << "data trained with " << cur << "\% success percentage" << std::endl;
        
        lsuccess = success;
        success = cur;
    }
    sad->run();
    delete sad;
}

int main( void )
{
    qwe(0.1);
    return 1;
}



