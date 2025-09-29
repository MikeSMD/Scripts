#include <vector>
#include <Eigen/Dense>
#include <math.h>

#include <algorithm>
class Activations 
{
    
    public:
    static double sinus( double input )
    {
        return sin( input );
    }    

     static double sinusDerivation( double input )
    {
        return cos( input );
    } 
     
     static double sigmoid(double x) {
         return 1.0 / (1.0 + std::exp(-x));
     }

     
     static double sigmoid_derivative(double x) {
         double s = sigmoid(x);
         return s * (1.0 - s);
     }
    static double ReLu( double input )
    {
        if ( input < 0 )
        {
            return 0;
        }
        else return input ;
    }    

     static double ReLuDerivation( double input )
     {
         if ( input <= 0 )
         {
             return 0;
         }
         else return 1;
     } 

     static Eigen::VectorXd softMax ( const Eigen::VectorXd& vec )
     {
         // -maxValue neni v klasicke matematicke funkci avsak tu se to uziva kvuli overflow apod e^x jde ruychle do velkcyh hodnot - odecet je numericka stabilizace
         double maxValue = vec.maxCoeff();
         double sum = (vec.array() - maxValue).exp().sum();


         Eigen::VectorXd result = (vec.array() - maxValue).exp();
         result /= sum;
         return result;
     }

     static Eigen::Matrix< double, Eigen::Dynamic, Eigen::Dynamic > softMaxDerivate( const Eigen::VectorXd& vec ) {

         Eigen::VectorXd s = Activations::softMax(vec); 
         int n = s.size();
         Eigen::MatrixXd J = Eigen::MatrixXd::Zero(n, n);

         for (int i = 0; i < n; ++i) {
             for (int j = 0; j < n; ++j) {
                 if (i == j) {
                     J(i, j) = s(i) * (1 - s(i));
                 } else {
                     J(i, j) = -s(i) * s(j);
                 }
             }
         }
         return J;
     }



};



