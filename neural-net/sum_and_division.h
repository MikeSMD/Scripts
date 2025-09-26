

#include "ITrain.h"

class Sum_and_division : public ITrain
{
    private:
        int layers;
        std::function < double ( double ) > activation;
        std::function < double ( double ) > activationDerivation;
    public: 
        Sum_and_division( std::vector< int > layers , double learning, int batch ) : ITrain( layers, learning, batch )
        {
            this->layers = layers.size();
        }

        virtual void train( int count )
        {
            auto& generator = Generator::GetInstance();
            for ( int i = 0; i < count ; ++i )
            {
                const double q = generator.generateDouble(-25.0, 25.0);
                const double r = generator.generateDouble(-25.0, 25.0);

                ann->ForwardPass( { q,r }, {q-r, q+r} );
            }
        }   
        
        void chooseActivation ( std::function < double ( double ) > pq )
        {
            this->activation = pq;
         }
        void classicActivations (std::vector< double >& vec, std::size_t i )
        {
            if ( this->layers - 1 == i ) // posledni vrstva nema aktivaci
            {
                return;
            }
            for ( int v = 0; v < vec.size(); ++v )
            {
               vec[ v ] = this->activation( vec[ v ] );
            }
            return;
        }

        void chooseActivationDerivation ( std::function < double ( double ) > pq )
        {
            this->activationDerivation = pq;
         }
        void classicActivationsDerivation (std::vector< double >& vec, std::size_t i )
        {
            if ( this->layers == i )
            {
                return;
            }
            for ( int v = 0; v < vec.size(); ++v )
            {
               vec[ v ] = this->activationDerivation( vec[ v ] );
            }
            return;
        }

        virtual void run()
        {
            while( true )
            {
                double k,p;
                std::cin >> k;
                std::cin >> p;
                const std::vector<double>& result = ann->ForwardPass( { k,p }, { k-p, k+p } );
                std::cout << "results - ";
                for ( int i = 0; i < result.size(); ++i )
                {
                    std::cout << result[ i ] << ",";
                }
                std::cout << std::endl;
            }	
        }


};
