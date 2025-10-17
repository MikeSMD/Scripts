#include <eigen3/Eigen/src/Core/Matrix.h>
#include <eigen3/Eigen/src/Core/util/Constants.h>
#include <iostream>
#include <vector>
#include <memory>
#include <random>
#include <functional>
#include <Eigen/Dense> // VectorXd - sloupcovy

#include "generator.h"

class ANN 
{
	public:

    void setLearningRate(double learning)
    {
        this->learning = learning;
    }	
	struct Layer
	{
        Eigen::VectorXd biases; // sloupcovy vektor
		static std::function< Eigen::VectorXd ( const Eigen::VectorXd&, std::size_t ) > activation_method;
		static std::function< Eigen::MatrixXd ( const Eigen::VectorXd&, std::size_t ) > derivative;
 
        static void SetMethod(std::function< Eigen::VectorXd(const Eigen::VectorXd&, std::size_t ) > p)
        {
            Layer::activation_method = p;
        }
        static void SetDerivative( std::function < Eigen::MatrixXd( const Eigen::VectorXd&, std::size_t ) > p )
        {
            Layer::derivative = p;
        }
		/*
		 * weights[ c(l+1) * i + j ], kde
         * proste radky - aktualni vrstva sloupce - pro neuron kam
		 * c je pocet neuronu v levelu 
		 * l - ajtualni level
		 * i - cislo neuronu v aktualnim levelu
		 * j - neuron v nasledujicim levelu
		 *priklad - z neuronu 5 z 6 v levelu l vede weight na neuron 2 v nasledujicim levelu s 6 neurony s hodnototu na indexu 6*5+2 = 32; z 36 neuronu (NEURONY SE INDEXUJI OD 0 TAKZE NEURON 5 JE POSLEDNI)
		 */
        Eigen::Matrix< double, Eigen::Dynamic, Eigen::Dynamic > weights;

        Eigen::VectorXd last_input ;
        Eigen::VectorXd last_output ;
        std::size_t current;
		Layer( int biases_count, bool hasBias=false, std::size_t current = 0 ) : current( current )
		{
            db_biases = Eigen::VectorXd::Zero( biases_count );
			biases.resize( biases_count );
			for ( std::size_t i = 0; i < biases_count; ++i )
			{
                double bias = 0.0;
                if ( hasBias )
                {
                    bias = Generator::GetInstance().generateDouble( -1.0, 1.0 );
                }
                biases(i) = bias;
			}
		}

		void InitWeights( const size_t sizeOfFollowingLayer )
		{
			size_t count = this->biases.size();
			Generator& p = Generator::GetInstance();
            this->weights.resize( count, sizeOfFollowingLayer );//radky, sloupce
            this->db_weights = Eigen::Matrix< double, Eigen::Dynamic, Eigen::Dynamic >::Zero( count, sizeOfFollowingLayer );
            for ( std::size_t i = 0; i < count; ++i )
            {
                for ( std::size_t j = 0; j < sizeOfFollowingLayer; ++j )
                {
                    this->weights( i, j ) = p.generateDouble(-1.0, 1.0);
                }
            }
		}

		
        
        Eigen::VectorXd db_biases;
        Eigen::Matrix< double, Eigen::Dynamic, Eigen::Dynamic > db_weights;


        Eigen::VectorXd GradientCounter(Eigen::VectorXd next_biases)
        {
            Eigen::VectorXd d_biases;
            if ( this->weights.size() != 0 )
                d_biases = this->weights * next_biases;
            else
                d_biases = next_biases;
            Eigen::VectorXd inputs = this->last_input;

            Eigen::MatrixXd jacobian = Layer::derivative( inputs, current );
            d_biases =  jacobian.transpose() * d_biases ;
            db_biases += d_biases;
            if ( db_weights.size() > 0 )
                db_weights +=  this->last_input * next_biases.transpose();
            
            return d_biases;
        }

        void GradientApply( int batch, double learning )
        {
            db_biases /= (double) batch;
            db_weights /= (double) batch;
    
            db_biases*=learning;
            db_weights *= learning;

            this->weights -= db_weights;
            this->biases -= db_biases;

            db_biases.setZero();
            db_weights.setZero();
        }

        void PassLayer( Eigen::VectorXd& input, Eigen::VectorXd& result )
		{
			if ( input.size() != this->biases.size() )
			{
				throw std::runtime_error("input neni roven poctu neuronu");
			}

            Eigen::VectorXd activated_inputs = input + biases;
            
            this->last_input = activated_inputs;

            activated_inputs = Layer::activation_method( activated_inputs, current );

            this->last_output = activated_inputs;
            
			if ( this->weights.size() == 0 )
			{
				result = activated_inputs;
				return;
			}
			std::size_t following_layer_size = this->weights.cols();
			result.resize(following_layer_size);
			
            result = activated_inputs.transpose() * weights;
		}
	};

	std::vector< Layer > layers;


    Eigen::VectorXd ForwardPass( Eigen::VectorXd inputs, Eigen::VectorXd expected )
	{
		for ( std::size_t i = 0; i < layers.size(); ++i )
		{
            Eigen::VectorXd result ;
			layers[ i ].PassLayer( inputs, result );
			inputs= result;
		}
        if ( updating && expected.size() >= 1)
            {
                backpropagate( inputs, expected );
            }
		return inputs;
	}

    int current_iterator;
    Eigen::Matrix<double , Eigen::Dynamic, Eigen::Dynamic > bd_weights;
    Eigen::Matrix<double , Eigen::Dynamic, Eigen::Dynamic > bd_biases; 


    void backpropagate(const Eigen::VectorXd& returned, const Eigen::VectorXd& expected )
    {
        current_iterator++;
        
        Eigen::VectorXd biases = this->loss_derivative( returned, expected );

        for ( int i = 0; i < this->layers.size(); ++i )
        {
          biases = this->layers[ this->layers.size() - 1 - i ].GradientCounter( biases );
        }
       


        if ( current_iterator >= this->batch )
        {
            this->updateANN();
        }
    }

    void updateANN()
    {
        for ( int i = 0; i < layers.size(); ++i )
        {
            this->layers[ i ].GradientApply( this->batch, this->learning );
        }
    }

    std::function < Eigen::VectorXd ( const Eigen::VectorXd&,const Eigen::VectorXd& ) > loss;
    std::function < Eigen::VectorXd ( const Eigen::VectorXd&,const Eigen::VectorXd& ) > loss_derivative;

    void setLoss( std::function < Eigen::VectorXd ( Eigen::VectorXd, Eigen::VectorXd ) > loss )
    {
        this->loss = loss;
    } 
    

    void setLossDerivative( std::function < Eigen::VectorXd ( Eigen::VectorXd, Eigen::VectorXd ) > lossDerivative )
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

		for ( std::size_t i = 0; i < layers_vec.size(); ++i )
		{
			 layers.emplace_back( layers_vec [ i ], i != 0, i ); // meni se bias v backpropagaci pozor, vsechny i ty co tam nemaji byt..
			if ( i < layers_vec.size() - 1 )
			{
				layers[ i ].InitWeights(layers_vec[ i + 1 ]);
			}
		}
	}



};
