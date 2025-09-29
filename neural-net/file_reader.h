#include <fstream>
#include <vector>
#include <sstream>


#include <Eigen/Dense>

class FileReader 
{
    public:
    static std::vector< Eigen::VectorXd > readCSV( std::string path )//qweqwe
    {
        std::fstream file_stream( path );
        if( ! file_stream.is_open() )
        {
            throw std::runtime_error( "soubor " + path + " nelze otevrit " );
        }
        std::vector< Eigen::VectorXd > data = {};

        std::string line_data;
        while( std::getline( file_stream, line_data ) )
        {
            std::vector< double > data_values = {};
            std::stringstream stream_data ( line_data );
            
            std::string value_str;
            while( getline( stream_data, value_str, ',' ) )
            {
                data_values.push_back( std::stod( value_str ) );
            }
            data.emplace_back( Eigen::Map<Eigen::VectorXd>( data_values.data(), data_values.size() ) );
        }
        return data;
    }
};

