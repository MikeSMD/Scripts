using System;

namespace Star
{
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
	}
}
