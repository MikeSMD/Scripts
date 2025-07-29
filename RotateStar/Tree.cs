using System;

namespace Star
{
	class Tree
	{
		public Cylinder Kmen {get; set;}
		public Ball Koruna {get; set;}
		public List < Cylinder > Vetvicky { get; set; }
		
		/**
		 * plane - v jake rovine mit tu 2d hvezdu
		 * velist - pocet radku
		 */
		public Tree( double kmen_radius, double kmen_height,double koruna_radius, Osa osa = Osa.y, int roKmen = 2, int roKoruna = 2)
		{
			Kmen = new Cylinder(kmen_radius, kmen_height,'|', '.', osa, ConsoleColor.DarkYellow ,roKmen);
			Koruna = new Ball(koruna_radius, '\0', ConsoleColor.Green,roKoruna);
			Move move;
			if ( osa == Osa.x )
			{
			move = new Move([ kmen_height / 3.0, 0.0, 0.0 ]);
			}
			if ( osa == Osa.y )
			{
			move = new Move([0.0, - kmen_height / 3.0 , 0.0 ]);
			}
			else
			{
			move = new Move([0.0, 0.0, kmen_height / 3.0]);
			}
			Koruna.addTransformation( move );
		}
		public void addTransformationAll( Transformation q )
		{
			Koruna.addTransformation( q );
			Kmen.addTransformation( q );
		}
		public void RegisterAll( ConsoleRenderer k )
		{
			k.Register(Kmen);
			k.Register(Koruna);
			//k.Register(kmen);
		}
	}
}
