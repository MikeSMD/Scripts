using System;

namespace Star
{
	interface IRenderable
	{
		public Point[] points {get;}
		public List < Transformation > transformations {get;}
		public void addTransformation( Transformation q );
	}	
}
