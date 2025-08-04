using System;

namespace Star
{
	class Ball : IRenderable
	{
		public Point[] points {get; private set;}
		public List < Transformation > transformations {get; private set; }
		public bool transparent {get; set; } = false;
		public double density  { get; set; } = 1.0;
		private char sign;
		private ConsoleColor cc;
			public Ball( char sign, ConsoleColor cc = ConsoleColor.White )
		{
			transformations = new List <Transformation> ();
			this.sign = sign;
			this.cc = cc;
		}

		public void GetTriangulated()
		{
			this.points = DataMiner.getData("objekry/ball.txt", sign, cc ).ToArray();
		}


		public void GetPointed( double radius, int ro = 1)
		{
			Random rnd = new Random();
			transformations = new List <Transformation> ();
			if ( radius <= 0.0 )
			{
				throw new ArgumentException ("size musi byt alespon 3");
			}

			List< Point > points_list = new List< Point > ();
			int steps = (int)(radius * ro);
			for (int i = 0; i <= steps; i++)
			{
				double phi = Math.PI * i / steps; // zenitový úhel: 0 .. π

				for (int j = 0; j <= steps; j++)
				{
					char psign = sign;
					if ( psign == sign )
					{
						psign = (char)rnd.Next('a', 'z' + 1);

					}
					Point p = new Point( 3, psign, cc);
					double theta = 2 * Math.PI * j / steps; // azimut: 0 .. 2π

					p[ 0 ] = radius * Math.Sin(phi) * Math.Cos(theta);
					p [ 1 ] = radius * Math.Sin(phi) * Math.Sin(theta);
					p [ 2 ] = radius * Math.Cos(phi);
					points_list.Add( p );
				}
			}	
			points = points_list.ToArray();

		}

		public void addTransformation( Transformation q )
		{
			transformations.Add( q );
		}

		}
}
