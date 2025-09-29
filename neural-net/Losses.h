




#include <math.h>
#include <Eigen/Dense>
class Losses
{
    public:
        static Eigen::VectorXd Cross_entropy ( Eigen::VectorXd predicted, Eigen::VectorXd expected )
        {
            double epsilon = 1e-12;
            Eigen::VectorXd y = predicted.array().max(epsilon).min(1.0 - epsilon); 
            return -(expected.array() * y.array().log());
        }




        static Eigen::VectorXd Cross_entropy_derivate ( Eigen::VectorXd predicted, Eigen::VectorXd expected )
        {
            if (predicted.size() != expected.size()) {
                throw std::invalid_argument("Vektory predicted a expected musí mít stejnou velikost.");
            }

            // Softmax
            Eigen::VectorXd exp_logits = predicted.array().exp();
            Eigen::VectorXd y = exp_logits / exp_logits.sum();
            // Derivace: y - expected
            return y - expected;
        }
        static double SSE( double predicted, double expected )
        {
            return std::pow( predicted - expected, 2 );
        }
        static double SSEDerivation( double predicted, double expected )
        {
            return 2 * ( predicted - expected);
        }
        static double MSE( double predicted, double expected )
        {
            return std::abs ( predicted - expected );
        }
         static double MSEDerivation ( double predicted, double expected )
        {
            if ( predicted - expected > 0 )
                return 1;
            else if( predicted - expected < 0 )
                return -1;
            else return 0;
        }
};
