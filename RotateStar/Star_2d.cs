using System;

namespace Star
{
	class Star_2d : Renderable
	{
	
		public Star_2d( char sign, ConsoleColor cc = ConsoleColor.White) : base ( sign, cc )
		{
			//
		}

		public void getPointed(int size = 12, Plane plane = Plane.xy, char fill = '.' )
		{
			if ( size <= 2 )
			{
				throw new ArgumentException ("size musi byt alespon 3");
			}
			if ( size % 2 == 0 )
			{
				size -= 1;
			}
			int val = 1;
			int mid = (size + 1) / 2;
			int offset = 1;
			int last = 1;
			List< Point > points_list = new List< Point > ();
			while( offset <= mid )
			{
				int stars = val;
				if ( stars % 2 == 0 )
				{
					if ( offset != mid || stars - 1 != last )
					{
						stars -= 1;
					}
					else stars += 1;
				}
				for ( int row_index = 0; row_index < 2; ++row_index)
				{
					if ( mid == offset && row_index == 1 )
						continue;
					int row = (row_index == 0)? offset : 2*mid - offset;

					for ( int i = 0; i < stars; ++i )
					{
						char ksign = sign;
						if ( !( i == 0 || i == stars -1 ) )
						{
							ksign = fill;
						}
						Point k = new Point( 3 , ksign, cc);

						switch ( plane )
						{
							case Plane.xy:
								{
									k.SetCoordinates([i - stars/2, row, 0.0]);
									break;
								}
							case Plane.yz:
								{
									k.SetCoordinates([0.0, i - stars/2, row]); 
									break;
								}
							case Plane.xz:
								{
									k.SetCoordinates([i - stars/2, 0.0, row]);
									break;
								}
						}
						points_list.Add( k );
					}
				}
				last = stars;
				val = val << 1;
				offset += 1;
			}
			points = points_list.ToArray();
		}
		}
}
