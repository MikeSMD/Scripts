using System;

namespace Star
{

	class PrespectiveProjection : IProjection
	{
		private double focal;
		private double width;
		private double height;
		private double near;
		private double far;
			public PrespectiveProjection(double width, double height, double focal, double near, double far  )
		{
			this.focal = focal;
			this.width = width;
			this.height = height;
			this.near = near;
			this.far = far;
		}
			/*
		public (double x, double y, double depth ) SimpleProjectPoint( Point p ) 
		{
			// pouze pro zobrazení z pohledu rovin. bez pohledové matice kdy chceme ji mít impleicitní v 0,0 natočenou na danou rovinu - není nutný viewmatrix,jen pro projekci rovin.
			
			switch( _projectionPlane )
			{
				case Plane.xy:
					if (p[2] == 0) p[2] = 1e-6; // ochrana před dělením nulou
					return (focal * p[0] / p[2], focal * p[1] / p[2], p[2]);
				case Plane.yz: 
					if (p[0] == 0) p[0] = 1e-6;
					return (focal * p[1] / p[0], focal * p[2] / p[0], p[0]);
				case Plane.xz:
					if (p[1] == 0) p[1] = 1e-6;
					return (focal * p[0] / p[1], focal * p[2] / p[1], p[1]);
			}
			return (0.0, 0.0, double.PositiveInfinity);
		}
		*/
		
		public (double x, double y, double depth ) SimpleViewMatrixProjectPoint( Point p ) 
		{
			// pokud použijeme view matrix. Po její aplikaci je vše natočeno na kameru tak, že kamera je v 0,0,0 a v klasické viewmatrix je natořena na -z (ty co jsou -z jsou před kamerou) a y je nahoru a x je doprava či doleva
			// teorie - při nejvyšším zjednodušení stačí vrátit to co výše ale s pojetím že máme vždy otočeno na -z
			if (p[2] == 0) p[2] = 1e-6;
			return (focal * p[0] / -p[2], focal * p[1] / -p[2], -p[2]);
 		}
			// ALE, můžou vznikat deformace, nemáme odkud kam kamera vidí, NEUVAŽUJE to ani zdaleka s ratio obrazovky - deformace.
		//aplikace projekční matice pro projekční prespektivu
		public (double x, double y, double depth ) projectPoint( Point p ) 
		{	
			/*
			 * aplikace plnohodnotné projekční matice
			 * počítá s - ratio okna, odkud kam kamera vidí, převádí na clamp souřadnice, normalizuje hloubku od 0 do 1. zde se opět počítá s tím že kamera "kouká" v camera space na -z
			 */
		/*
		 *
		 *
[ cot(fov/2)/aspect  0             0                    0                   ] x = jedná se o upravený fov pro ratio (x je většinou větší)
[ 0                  cot(fov/2)    0                    0                   ] y = fov yka
[ 0                  0             -(far+near)/(far-near) -2*far*near/(far-near) ] z - ohraničení
		[ 0                  0             -1                   0                   ] kamera kouka na -z
		 */
			double x = p [ 0 ] * ( 1 / Math.Tan( focal / 2 ) ) / (this.width / this.height );
		 	double y = p [ 1 ] * ( 1 / Math.Tan( focal / 2 ) );
			double z = p [ 2 ] * -(far+near)/(far-near) + p [ 3 ] *-2*far*near/(far-near);
			double w = - p [ 2 ];





			x/=w;
			y/=w;
			z/=w;
		//	Console.WriteLine($"x je {x}, y {y}, z {z}");
			return ( ( x + 1 )*this.width / 2, (1 - y) * this.height / 2, z ); //(-1,1), (-1,1), ( 0,1)
		
		}
	}
	}
