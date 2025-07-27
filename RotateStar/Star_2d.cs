using System;

namespace Star
{
	class Star_2d : IRenderable
	{
		public Point[] points {get; private set;}

		/**
		 * plane - v jake rovine mit tu 2d hvezdu
		 * velist - pocet radku
		 */
		public Star_2d( int size, Plane plane, char sign )
		{
			if ( size >= 2 )
			{
				throw new ArgumentException ("size musi byt alespon 3");
			}
			if ( size % 2 == 0 )
			{
				size -= 1;
			}
			int val = 1;
			int mid = (size + 1) / 2;
			int offset = 0;
			int last = 1;
			List< Point > points_list = new List< Point > ();
			while( offset <= mid )
			{
				if ( val % 2 == 0 )
				{
					if ( offset != mid || val - 1 != last ) )
					{
						val -= 1;
					}
				}
				for ( int row_index = 0; row_index < 2; ++i )
				{
					if ( mid == offset && row_index == 1 )
						continue;
					int row = (row_index == 0)? offset : mid + offset;
				

					for ( int i = 0; i < val; ++i )
					{
						Point k = new Point( 3 , sign );

						switch ( plane )
						{
							case Plane.xy
							{
								k.SetCoordinates([i - val/2, row, 0.0]);
							}
							case Plane.yz
							{
								k.SetCoordinates([0.0, i - val/2, row]); 
							}
							case Plane.xz
							{
								k.SetCoordinates([i - val/2, 0.0, row]);
							}
						}
						points_list.Append( k );
					}
				}
				last = val;
				val = val >> 1;
			}
			points = points_list.ToArray();
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
