using System;

namespace Star
{
	interface IRenderable
	{
		public Point[] points {get;}
		public bool transparent { get; set; }
		public double density { get; set; }
		public List < Transformation > transformations {get;}
		public void addTransformation( Transformation q );
	}	
}
