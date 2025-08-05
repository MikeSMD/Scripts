using System;

namespace Star
{

	public static class Specials 
	{
		public static Random random = new Random();
		public static char transparency = '\t';
		public static char randomness = '\0';

		public static bool IsSpecial(char sign)
		{
			return ( Specials.randomness == sign || sign == Specials.transparency );
		}

	}


	public abstract class Renderable
	{
		public Point[] points {get; set;}
		public List < Transformation > transformations {get;}
		protected char sign;
		protected ConsoleColor cc;

		public Renderable( char sign, ConsoleColor cc )
		{
			this.transformations = new List < Transformation > ();
			this.sign = sign;
			this.cc = cc;
		}

		public void GetTriangulated( )
		{
			this.points = DataMiner.getData($"data/{this.GetType().Name}.txt", sign, cc ).ToArray();
		}

		public void addTransformation( Transformation q )
		{
			this.transformations.Add ( q );
		}

		public void ChangeTransparencyConsole( double opc )
		{
			if ( opc > 1.0 || opc < 0.0 || points == null )
			{
				return;
			}
			for ( int i = 0; i < points.Length; ++i )
			{
				if ( Specials.random.NextDouble() > opc )
				{
					points[ i ].sign = Specials.transparency;
				}
				else if ( points[ i ].sign != this.sign && ! Specials.IsSpecial( sign ) )
					points[ i ].sign = this.sign;

			}
		}
	}
}
