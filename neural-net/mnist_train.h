#include "file_reader.h"



#include "ITrain.h"

class Mnist_train : public ITrain 
{
    private:
        const std::string file_train_input = "MNIST/mnist_train_images.csv";
        const std::string file_train_expected = "MNIST/mnist_train_labels.csv";
        const std::string file_test_input = "MNIST/mnist_test_labels.csv";
        const std::string file_test_expected = "MNIST/mnist_test_labels.csv";


        const std::string file_user_input = "MNIST/???.csv";
        const std::string file_user_label = "MNIST/???.csv";
        const std::string file_input_program = "MNIST/???.csv";

    public:
    Mnist_train ( std::vector < int > layers, double learning, int batch_size ) : ITrain( layers, learning, batch_size )
    {
        //
    }

    void test()
    {
        //
    }

    virtual void train( int count )
    {
        std::cout << "strating training mnist " << std::endl;
        // po radcich - predelat..
        std::vector < std::vector< double > > input = {};
        std::vector< std::vector< double > > expected = {};

        input = FileReader::readCSV( this->file_train_input );
        expected = FileReader::readCSV( this->file_train_expected );
    
        std::cout << input.size() << "," << expected.size() << std::endl;
        if ( count > input.size() )
        {
            count = input.size();
        }

        
        for ( int i = 0; i < count; ++i )
        {
            std::vector< double > res = ann->ForwardPass( input[ i ], expected[ i ]  );
            
            for ( int j = 0; j < res.size(); ++j )
            {
                std::cout << res[ j ]  << ",";
            }
            std::cout << std::endl;
            
            for ( int j = 0; j < expected[ i ].size(); ++j )
            {
                std::cout << expected[ i ][ j ]  << ",";
            }
            std::cout << std::endl;
            std::cout << std::endl;
            
        }
    }

    void run()
    {
        //
    }
};

