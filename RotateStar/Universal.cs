using System;

namespace Star
{
	static class DataMiner
	{
		public static List < Point > getData( string filePath, char sign = '.', ConsoleColor cc = ConsoleColor.White)
		{
		
			if ( !File.Exists( filePath ) )
			{
			 	throw new ArgumentException($"sohbor {filePath} neexistuje");
			}
			
			var points_list = new List < Point > ();
			using ( StreamReader reader = new StreamReader(filePath) )
			{
				string line;
				while( ( line = reader.ReadLine() ) != null )
				{
             			   string[] parts = line.Split(',');
				   Point p = new Point( 3, sign, cc);
				   if ( p.sign == Specials.randomness )
				   {
					p.sign = (char)Specials.random.Next(32, 125);   
				}
				   double[] k = new double[ 3 ];

				   int k_index = 0;
				   foreach ( var part in parts )
				   {
					   if ( k_index >= 3 )
						   break;
					   if (double.TryParse(part.Trim(), out double number))
					   {
						k [ k_index++ ] = number;
					   }
					   else
					   {
						   throw new ArgumentException ("chyba v souboru 1");
					   }
				   }
				   if ( k_index != 3 )
				   {
					   throw new ArgumentException ("chyba v souboru 2");
				   }
				   p.SetCoordinates ( k );
				   points_list.Add( p );
				}	
				return points_list;
			}
		}

	
		}
}
