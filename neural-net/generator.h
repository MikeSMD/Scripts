#include <iostream>
#include <random>     

class Generator
{

	private:
		std::mt19937 gen; 
		Generator() : gen(std::random_device{}())
		{
			//
		}	
	public:
		
		static Generator& GetInstance()
		{
			static Generator instance;
			return instance;
		}

		double generateDouble(double from, double to) 
		{
			std::uniform_real_distribution<double> distrib(from, to);
			return distrib(gen);
		}

		    Generator(const Generator&) = delete;
    Generator& operator=(const Generator&) = delete;
};
