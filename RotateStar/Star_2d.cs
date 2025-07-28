using System;

namespace Star
{
	class Star_2d : IRenderable
	{
		public Point[] points {get; private set;}
		public List < Transformation > transformations {get; private set; }

		/**
		 * plane - v jake rovine mit tu 2d hvezdu
		 * velist - pocet radku
		 */
		public Star_2d( int size, Plane plane, char sign, char fill = '.' )
		{
			transformations = new List <Transformation> ();
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
						Point k = new Point( 3 , ksign );

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
