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
		static std::function< double(double) > activation_method;

		Neuron ( double bias, bool activation = false ) : bias( bias ), activation(activation)
		{
			//
		}

		double Pass( double value )
		{
			if ( activation )
			{
				return Neuron::activation_method( value + bias );
			}

			return (value + bias);
		}

		static void SetMethod(std::function< double(double) > p)
		{
			Neuron::activation_method = p;
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

		Layer( int neurons_count, bool activation = false )
		{
			neurons.reserve( neurons_count );
			for ( std::size_t i = 0; i < neurons_count; ++i )
			{
				double bias = Generator::GetInstance().generateDouble( -1.0, 1.0 );
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
			std::cout << "passing" << std::endl;
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
			int following_layer_size = weights.size() / neurons.size();
			result.resize(following_layer_size);
			std::cout << "result" << std::endl;
			for ( int i = 0 ; i < following_layer_size; ++i )
			{
				double sum = 0.0;
				for ( std::size_t j = 0; j < input.size(); ++j )
				{
					sum += activated_inputs [ j ] * weights[following_layer_size * j + i ];
				}
				result[ i ] = sum ;
			}
			std::cout << "done" << std::endl;
		}
	};

	std::vector< Layer > layers;


	std::vector<double> ForwardPass( std::vector< double > inputs )
	{
		for ( std::size_t i = 0; i < layers.size(); ++i )
		{
		std::cout << inputs.size() << std::endl;
			std::vector< double > result ;
			layers[ i ].PassLayer( inputs, result );
			inputs= std::move(result );
		}
		std::cout << "returning" << std::endl;
		return inputs;
	}
	
	ANN( const std::vector< int >& layers_vec )
	{
		if ( layers_vec.size() < 2 )
		{
			throw std::runtime_error("layers musi byt alespon 2");
		}
		for ( std::size_t i = 0; i < layers_vec.size(); ++i )
		{
			layers.emplace_back( layers_vec [ i ], !( i == 0 || i == layers_vec.size() - 1 ) );
			if ( i < layers_vec.size() - 1)
			{
				layers[ i ].InitWeights(layers_vec[ i + 1 ]);
			}
		}
	}



};
