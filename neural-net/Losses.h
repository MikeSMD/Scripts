




#include <math.h>
class Losses
{
    public:
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
