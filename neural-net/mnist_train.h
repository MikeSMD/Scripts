#include "file_reader.h"
#include "ITrain.h"
#include <cmath>
#include <eigen3/Eigen/src/Core/Matrix.h>

class Mnist_train : public ITrain 
{
    private:
        std::size_t layer_count;
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
        this->layer_count = layers.size();
    }

    void test()
    {
        //
    }

    virtual void train( int count ) // po radcich - online read
    {
        std::vector < Eigen::VectorXd > input = {};
        std::vector< Eigen::VectorXd > expected = {};

        input = FileReader::readCSV( this->file_train_input );
        expected = FileReader::readCSV( this->file_train_expected );
    
        std::cout << input.size() << "," << expected.size() << std::endl;
        if ( count > input.size() )
       {
            count = input.size();
        }

        
        int percentage = 0;
        for ( int i = 0; i < count; ++i )
        {
            Eigen::VectorXd res = ann->ForwardPass( input[ i ] , expected[ i ] );
            
            int percentage_now = i*100/ count;
            if ( percentage != percentage_now )
            {
                percentage = percentage_now;
                std::cout << percentage << std::endl;
            }
            
      /*      std::cout << res.transpose()  << ",";
            std::cout << std::endl;
            
            std::cout << expected[ i ].transpose() << ",";
            std::cout << std::endl;
            std::cout << std::endl;
            */
        }
    }

    std::function< double ( double )> activation_hidden;  
    std::function< Eigen::VectorXd ( const Eigen::VectorXd& )> activation_output;

    std::function< double ( double )> activation_hidden_derivation;  
    std::function< Eigen::MatrixXd ( Eigen::VectorXd )> activation_output_derivation;

    Eigen::VectorXd classic_activation (const Eigen::VectorXd& vec, std::size_t i )
    {
         if ( i == this->layer_count - 1 )
        {
            return (Eigen::VectorXd) activation_output( vec );
        }

        
            return (Eigen::VectorXd) vec.unaryExpr( [this](double inp) { return activation_hidden( inp ); });
    }

    std::function < Eigen::VectorXd ( Eigen::VectorXd, Eigen::VectorXd) > loss;
    std::function < Eigen::VectorXd ( Eigen::VectorXd, Eigen::VectorXd) > loss_derivative;
    Eigen::VectorXd ClassicLosses( const Eigen::VectorXd& vec, const Eigen::VectorXd& exp )
    {

        return loss( vec, exp );
        //return  vec.binaryExpr(exp, [this](double x, double y) {
          //          return this->loss( x, y ); 
            //});
    }
    Eigen::VectorXd ClassicLossesDerivation( const Eigen::VectorXd& vec, const Eigen::VectorXd& exp )
    {
        return loss_derivative( vec, exp );
    }
    Eigen::MatrixXd classic_activation_derivation (const Eigen::VectorXd& vec, std::size_t i )
    {
         if ( i == this->layer_count - 1 )
        {
             return this->activation_output_derivation( vec );
        }

         return (Eigen::MatrixXd) vec.unaryExpr( [this](double inp) { return activation_hidden_derivation( inp ); }).eval().asDiagonal(); // pro matici
    }

    Eigen::VectorXd readPythonMNIST(const std::string& scriptPath) {
        std::string command = "python3 " + scriptPath;
        std::array<char, 128> buffer;
        std::string result;

        FILE* pipe = popen(command.c_str(), "r");
        if (!pipe) throw std::runtime_error("popen failed!");
        while (fgets(buffer.data(), buffer.size(), pipe) != nullptr) {
            result += buffer.data();
        }
        pclose(pipe);

        std::istringstream iss(result);
        std::vector<double> values;
        double v;
        while (iss >> v) values.push_back(v);

        Eigen::VectorXd vec(values.size());
        for (size_t i=0; i<values.size(); ++i) vec(i) = values[i];
        return vec;
    }
    void run()
    {
        while( true )
        {
            Eigen::VectorXd mnist_input = readPythonMNIST("mnist_player.py");
            Eigen::VectorXd result = ann->ForwardPass( mnist_input, {} );
            std::cout << result.transpose() << std::endl;
        }
       

    }
};

