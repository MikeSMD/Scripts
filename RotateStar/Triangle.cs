using System;

namespace Star
{
	class Triangle_2d : Renderable
	{

		public Triangle_2d(  char sign, char fill = '.', ConsoleColor cc = ConsoleColor.White) : base ( sign, cc )
		{
			//
		}

		public void GetPointed(int size = 5, Plane plane=Plane.xy)
		{
			points = new Point[ 3 ];
			Point a = new Point( 3 , 'p', cc );
			Point b = new Point ( 3, 'q', cc );
			Point c = new Point ( 3, 'r', cc );

			switch ( plane )
			{
				case Plane.xy:
					{
						a.SetCoordinates([ -size / 2, 0.0, 0.0 ]);
						b.SetCoordinates([ size / 2, 0.0, 0.0 ]);
						c.SetCoordinates([ 0 , size / 2, 0.0 ]);
					}
					break;
				case Plane.xz:
					{
						a.SetCoordinates([-size / 2, 0.0, 0.0 ]);
						b.SetCoordinates([size / 2, 0.0, 0.0 ]);
						c.SetCoordinates([ 0 , 0.0, size ]);
					}	
					break;
				case Plane.yz:
					{
						a.SetCoordinates([ 0.0, -size / 2, 0.0 ]);
						b.SetCoordinates([ 0.0 ,size / 2 , 0.0 ]);
						c.SetCoordinates([ 0.0 , 0.0, size ] );
					}
					break;
			}
			points [ 0 ] = a;
			points [ 1 ] = b;
			points [ 2 ] = c;
		}

		}
}
