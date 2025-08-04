using System;

namespace Star
{
	class Kuzel : IRenderable
	{
		public Point[] points {get; private set;}
		public List < Transformation > transformations {get; private set; }
		private char sign;
		private ConsoleColor color;

		public double density {get; set;} = 1.0;
		public bool transparent {get; set; } = false;
		
		public Kuzel( char sign, ConsoleColor cc = ConsoleColor.White)
		{
			transformations = new List <Transformation> ();
			points = new Point[ 0 ];
			this.sign = sign;
			this.color = cc;
		}	

		public void GetPointed()
		{
			throw new NotImplementedException();
		}

		public void BuildTriangulated()
		{
		this.points = DataMiner.getData("objekry/kuzel.txt", sign, color).ToArray();
		}
		public void addTransformation( Transformation q )
		{
			transformations.Add( q );
		}

		}
}
