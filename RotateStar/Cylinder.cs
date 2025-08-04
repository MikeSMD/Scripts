using System;

namespace Star
{
	class Cylinder : Renderable
	{
	
		public Cylinder( char sign, ConsoleColor cc = ConsoleColor.White ) : base ( sign, cc )
		{
			//
		}

		public void GetPointed( char signP, double radius, double height, Osa osa = Osa.y, int ro = 1 )
		{
			char signR = sign;
			if ( radius <= 0.0 )
			{
				throw new ArgumentException ("size musi byt alespon 3");
			}

			List< Point > points_list = new List< Point > ();
			int steps = (int)( radius * ro);
			for (int i = 0; i <= steps; i++)
			{
				double z = height * i / steps;
				for (int j = 0; j <= steps; j++)
				{
					Point p = new Point ( 3, signP,cc );
					double theta = 2 * Math.PI * j / steps;

					switch ( osa )
					{
						case Osa.x:
							{
								p[ 0 ] = z;	
								p [ 1 ] = radius * Math.Cos(theta);
								p [ 2 ] = radius * Math.Sin(theta);
								points_list.Add ( p );
							}
							break;
						case Osa.y:
							{
								p[ 1 ] = z;	
								p [ 2 ] = radius * Math.Cos(theta);
								p [ 0 ] = radius * Math.Sin(theta);
								points_list.Add ( p );
							}	
							break;
						case Osa.z:
							{
								p[ 2 ] = z;	
								p [ 0 ] = radius * Math.Cos(theta);
								p [ 1 ] = radius * Math.Sin(theta);
								points_list.Add ( p );
							}
							break;
					}
				}
			}

			// Podstavy (kruh v rovině z=0 a z=height)
			for (int k = 0; k <= 1; k++) // 0 = spodní, 1 = horní podstava
			{
				double z = k * height;

				for (int i = 0; i <= steps; i++)
				{
					double rho = radius * i / steps;
					for (int j = 0; j <= steps; j++)
					{
						Point p = new Point ( 3, signR,cc );
						double theta = 2 * Math.PI * j / steps;

						switch ( osa )
						{
							case Osa.x:
								{
									p [ 0 ] = z;		
									p [ 1 ] = rho * Math.Cos(theta);
									p [ 2 ] = rho * Math.Sin(theta);
								}
								break;
							case Osa.y:
								{
									p [ 1 ] = z;		
									p [ 2 ] = rho * Math.Cos(theta);
									p [ 0 ] = rho * Math.Sin(theta);
								}
								break;
							case Osa.z:
								{
									p [ 2 ] = z;		
									p [ 0 ] = rho * Math.Cos(theta);
									p [ 1 ] = rho * Math.Sin(theta);
								}
								break;
						}

						points_list.Add ( p );

					}
				}
			}
			points = points_list.ToArray();
		}

	}
}
