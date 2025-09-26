

#include <math.h>
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


};



