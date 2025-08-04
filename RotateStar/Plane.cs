using System;

namespace Star
{
	class Plane_2d : Renderable
	{
		public Plane_2d(  char sign, ConsoleColor cc = ConsoleColor.White) : base ( sign, cc )
		{
			//
		}
		public void getPointed(double width, double heighr, Plane plane, int div = 1)
		{
			if ( width <= 2 || heighr <= 0.1 )
			{
				throw new ArgumentException ("size musi byt alespon 3");
			}
			List< Point > points_list = new List< Point > ();

			for ( int i = 0; i < width; ++i )
			{
				if ( i % div + 1 != 1 ) continue;
				for ( int j = 0; j < heighr; ++j )
				{
					if ( j % div + 1 != 1 ) continue;
					Point p = new Point ( 3, sign, cc );
					switch( plane )
					{
						case Plane.xy:
							{
								p [ 0 ] = i;
								p [ 1 ] = j;
								p [ 2 ] = 0.0;
							}
							break;
						case Plane.xz:
							{
								p [ 0 ] = i;
								p [ 2 ] = j;
								p [ 1 ] = 0.0;
							}
							break;
						case Plane.yz:
							{
								p [ 1 ] = i;
								p [ 2 ] = j;
								p [ 0 ] = 0.0;
							}
							break;
					}
					points_list.Add( p );
				}
			}
			points = points_list.ToArray();

		}

	
		}
}
