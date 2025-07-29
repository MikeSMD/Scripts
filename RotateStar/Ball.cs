using System;

namespace Star
{
	class Ball : IRenderable
	{
		public Point[] points {get; private set;}
		public List < Transformation > transformations {get; private set; }

		/**
		 * plane - v jake rovine mit tu 2d hvezdu
		 * velist - pocet radku
		 */
		public Ball( double radius, char sign, ConsoleColor cc = ConsoleColor.White, int ro = 1 )
		{
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
					Point p = new Point( 3, sign, cc);
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
			
		/** 	
		 * pomoci exponenicalni funkce..
		 *	
		 * bitovy posun (-1 u sudych pokud neni stred pak stejny jak ostatni) 
		 * 1 -> 2 -> 4 -> 8 -> 16 - velikost - 9/10
		 *
		 * 		*
		 * 	        *
		 * 	       ***
		 * 	     *******
		 * 	 ***************
		 * 	     *******
		 * 	       ***
		 * 	        *
		 * 	        *
		 *
		 * 	       //velikost 3
		 *
		 *		*
		 *	       ***
		 *	        *
		 *
		 *
		 *
		 */	
		}
}
