



#include "neuralnet.h"
#include <functional>
class ITrain 
{
    protected:
        ANN* ann;
        double learning_rate;
        double batch_size;

    public:
       ITrain( std::vector< int > layers, double learning_rate, int batch_size )
       {
            ann = new ANN( layers, learning_rate, batch_size );
       }

       void setActivation ( std::function< void ( std::vector< double >&, std::size_t  ) > activation )
       {
           ANN::Layer::SetMethod( activation );
       }

       void setActivationDerivation ( std::function< void ( std::vector< double >&, std::size_t  ) > activationDerivation )
       {
           ANN::Layer::SetDerivative( activationDerivation );
       } 

        void setLoss( std::function< double ( double, double ) > loss )
       {
          ann->setLoss( loss );
       } 
        void setLossDerivation( std::function< double ( double, double ) > loss_derivative )
       {
          ann->setLossDerivative( loss_derivative );
       } 
       virtual void train( int count ) = 0;

       virtual void run() = 0;

       virtual ~ITrain()
       {
        delete ann;
       }
       
    
};
std::function< void ( std::vector< double >&, std::size_t  )> ANN::Layer::activation_method = nullptr;
std::function< void ( std::vector< double >&, std::size_t  )> ANN::Layer::derivative = nullptr;
