

#include "ITrain.h"

class Sum_and_division : public ITrain
{
    public: 
        Sum_and_division( std::vector< int > layers , double learning, int batch ) : ITrain( layers, learning, batch )
        {
            //
        }

        virtual void train( int count )
        {
            auto& generator = Generator::GetInstance();
            for ( int i = 0; i < count ; ++i )
            {
                const double q = generator.generateDouble(-1000.0, 1000.0);
                const double r = generator.generateDouble(-1000.0, 1000.0);

                ann->ForwardPass( { q,r }, {q-r, q+r} );
            }
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
