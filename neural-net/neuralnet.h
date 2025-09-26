#include <iostream>
#include <vector>
#include <memory>
#include <random>
#include <functional>

#include "generator.h"

class ANN 
{
	public:
	
	struct Neuron
	{
		double bias;
		bool activation;
		static std::function< double( std::vector< double > ) > activation_method;
		static std::function< double( std::vector< double > ) > derivative;

	    double last_output;
        double last_input_to;
        Neuron ( double bias, bool activation = false ) : bias( bias ), activation(activation)
		{
			//
		}

		double Pass( double value )
		{
            double output_value = 0.0;
            last_input_to = value+bias;
			if ( activation )
			{
				output_value = Neuron::activation_method( value + bias );
			}
            else 
            {
			    output_value = (value + bias);
            }
            last_output = output_value;

            return output_value;
		}

		static void SetMethod(std::function< double(double) > p)
		{
			Neuron::activation_method = p;
		}
        static void SetDerivative( std::function < double ( double ) > p )
        {
            Neuron::derivative = p;
        }
	};


	struct Layer
	{
		std::vector< Neuron > neurons;

		/*
		 * weights[ c(l+1) * i + j ], kde
		 * c je pocet neuronu v levelu 
		 * l - ajtualni level
		 * i - cislo neuronu v aktualnim levelu
		 * j - neuron v nasledujicim levelu
		 *priklad - z neuronu 5 z 6 v levelu l vede weight na neuron 2 v nasledujicim levelu s 6 neurony s hodnototu na indexu 6*5+2 = 32; z 36 neuronu (NEURONY SE INDEXUJI OD 0 TAKZE NEURON 5 JE POSLEDNI)
		 */
		std::vector< double > weights;

		Layer( int neurons_count, bool activation = false, bool hasBias=false )
		{
			neurons.reserve( neurons_count );
			for ( std::size_t i = 0; i < neurons_count; ++i )
			{
                double bias = 0.0;
                if ( hasBias )
                {
                    bias = Generator::GetInstance().generateDouble( -1.0, 1.0 );
                }
                neurons.emplace_back(bias ,activation);
			}
		}

		void InitWeights( size_t sizeOfFollowingLayer )
		{
			size_t count = sizeOfFollowingLayer * this->neurons.size();
			Generator& p = Generator::GetInstance();

			for ( size_t i = 0; i < count; ++i )
			{
				weights.emplace_back( p.generateDouble( 0, 1.0 ) );
			}
		}

		void PassLayer( std::vector< double >& input, std::vector< double >& result )
		{
			if ( input.size() != this->neurons.size() )
			{
				throw std::runtime_error("input neni roven poctu neuronu");
			}
			std::vector<double> activated_inputs;
			activated_inputs.reserve(neurons.size());

			for ( std::size_t i = 0; i < neurons.size(); ++i )
			{
				activated_inputs.emplace_back(neurons[ i ].Pass(input[ i ]));
			}
			result.clear();
			if ( weights.size() == 0 )
			{
				result.clear();
				result = std::move( activated_inputs );
				return;
			}
			std::size_t following_layer_size = weights.size() / neurons.size();
			result.resize(following_layer_size);
			for ( std::size_t i = 0 ; i < following_layer_size; ++i )
			{
				double sum = 0.0;
				for ( std::size_t j = 0; j < input.size(); ++j )
				{
					sum += activated_inputs [ j ] * weights[following_layer_size * j + i ];
				}
				result[ i ] = sum ;
			}
		}
	};

	std::vector< Layer > layers;


	std::vector<double> ForwardPass( std::vector< double > inputs, std::vector< double > expected = {} )
	{
		for ( std::size_t i = 0; i < layers.size(); ++i )
		{
			std::vector< double > result ;
			layers[ i ].PassLayer( inputs, result );
			inputs= std::move(result );
		}
        if ( updating && expected.size() >= 1)
            {
                backpropagate( expected );
            }
		return inputs;
	}















    int current_iterator;
    std::vector< std::vector< double > > bd_weights;
    std::vector< std::vector< double > > bd_biases; 

    void backpropagate( std::vector< double > expected )
    {
      //  std::cout << "JEDU BACKPROPAGACE"<< std::endl;
        current_iterator++;
        std::vector< std::vector< double > > d_weights = {};
        std::vector< std::vector< double > > d_biases = {};
        
        gradientCounter(d_weights, d_biases, expected);
        
        for ( int i = 0; i < bd_weights.size(); ++i )
        {
            for ( int j = 0; j < bd_weights[ i ].size(); ++j )
            {
                bd_weights[ i ][ j ] += d_weights[ i ][ j ];
            }

            for ( int j = 0; j < bd_biases[ i ].size(); ++j )
            {
                bd_biases[ i ][ j ] += d_biases[ i ][ j ];
            }
        }
     //   std::cout << current_iterator << " / " << batch << std::endl;
        if ( current_iterator >= this->batch )
        {
            current_iterator = 0;

            for ( int i = 0; i < bd_weights.size(); ++i )
            {
                for ( int j = 0; j < bd_weights[ i ].size(); ++j )
                {
                    bd_weights[ i ][ j ] /= batch;
                }

                for ( int j = 0; j < bd_biases[ i ].size(); ++j )
                {
                    bd_biases[ i ][ j ] /= batch;
                }
            }
            updateANN( bd_weights, bd_biases );

             for ( int i = 0; i < bd_weights.size(); ++i )
            {
                for ( int j = 0; j < bd_weights[ i ].size(); ++j )
                {
                    bd_weights[ i ][ j ] = 0.0;
                }

                for ( int j = 0; j < bd_biases[ i ].size(); ++j )
                {
                    bd_biases[ i ][ j ] = 0.0;
                }
            }
        }
    }

    void gradientCounter( std::vector< std::vector< double > >& d_weights, std::vector< std::vector< double > >& d_biases,   std::vector< double > expected )
    {

        //delty vah + biasu dle vrstev - celkove to tvori gradient 
        d_weights = {};
        d_biases = {};

        // pocet vrstev v siti
        const std::size_t layers_count = layers.size();

        d_weights.resize(layers_count);
        d_biases.resize(layers_count);

        // pruchod vrstvami
        for ( std::size_t index = 0; index < layers_count; ++index )
        {
            // index aktualni vrstvy
            const std::size_t current = layers_count - index - 1;
            d_weights[current].resize(this->layers[current].weights.size());
               
            // pruchod vsech vah mezi aktualni a nasledujici vrstvou
            for ( std::size_t weight = 0; weight < this->layers[ current ].weights.size(); ++weight )
            {
                    std::size_t count = this->layers[ current + 1 ].neurons.size();
                    
                    // delta biasu kam vaha ukazuje
                    double delta_next = d_biases[ current + 1 ][ weight % count ];

                    // input odkud vaha ukazuje
                    double input = this->layers[ current ].neurons[ weight / count ].last_output;

                    //vysledna delta pro vahu
                    double delta = delta_next * input ;
                    d_weights[ current ][ weight ] = delta;
            }


            // prochazeni biasu v aktualni vrstve
            for ( std::size_t bias = 0; bias < this->layers[ current ].neurons.size(); ++bias )
            {

                d_biases[ current ].resize( this->layers[ current ].neurons.size() );
                Neuron actual = this->layers[ current ].neurons[ bias ]; //neuron ve kterem je aktualni bias
                double delta = 0.0; //delta pro bias
                
                //pokud posledni vrsrva
                if ( index == 0 )
                {
                    delta = loss_derivative( actual.last_output, expected [ bias ]);
                    std::cout <<"loss - " << loss( actual.last_output, expected[ bias ] ) << ",";
                }
                else 
                {
                    std::size_t counter = layers[ current + 1].neurons.size();
                    // delty vsech nasledujicich neuronu vynasobene prislusnymi vahami
                    for ( int i = 0; i < layers[ current + 1 ].neurons.size(); ++i ) 
                    {
                        delta += d_biases[ current + 1 ][ i ] * layers[ current ].weights[bias*counter+i]; 
                    }
                }
                if ( actual.activation )
                {
                      delta *= Neuron::derivative( actual.last_input_to );
                }
                d_biases[ current ] [ bias ] = delta;

            }
        }
    }


    void updateANN( const std::vector< std::vector< double > > d_weights, const std::vector< std::vector< double > > d_biases)
    {
        std::cout << "update.." << std::endl;
         const int layers_count = this->layers.size();
        for ( int i = 0; i < layers_count; ++i )
        {
            for ( int bias = 0; bias < layers[ i ].neurons.size(); ++bias )
            {
                layers[ i ].neurons[ bias ].bias -= learning * d_biases[ i ][ bias ];
            }
            for ( int w = 0; w < layers[ i ].weights.size(); ++w )
            {
                layers[ i ].weights[ w ] -= learning * d_weights[ i ][ w ];
            }
        }
    }

    std::function < double ( double, double ) > loss;
    std::function < double ( double, double ) > loss_derivative;

    void setLoss( std::function < double ( double, double ) > loss )
    {
        this->loss = loss;
    } 
    

    void setLossDerivative( std::function < double (double, double) > lossDerivative )
    {
        this->loss_derivative = lossDerivative;
    } 


    double learning;
    int batch;

    bool updating = true;
	ANN( const std::vector< int >& layers_vec, double learning, int batch_size ) : learning( learning ), batch( batch_size ), updating( true )
	{
		if ( layers_vec.size() < 2 )
		{
			throw std::runtime_error("layers musi byt alespon 2");
		}

        current_iterator = 0;

        bd_biases.resize(layers_vec.size());
        bd_weights.resize(layers_vec.size());

		for ( std::size_t i = 0; i < layers_vec.size(); ++i )
		{
            bd_biases[ i ].resize( layers_vec[ i ] );
			 layers.emplace_back( layers_vec [ i ], !( i == 0 || i == layers_vec.size() - 1 ), i != 0 );
        //    layers.emplace_back( layers_vec [ i ], !( i == 0 ), i != 0 ); //neurony v output vrstve maji aktivacni funkci
			if ( i < layers_vec.size() - 1 )
			{
                bd_weights[ i ].resize( layers_vec[ i + 1 ] * layers_vec[ i ] );
				layers[ i ].InitWeights(layers_vec[ i + 1 ]);
			}
		}
	}



};
