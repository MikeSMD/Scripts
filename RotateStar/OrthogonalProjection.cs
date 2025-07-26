using System;

namespace Star
{

	public enum Plane
	{
		xy, yz, xz
	}
	class OrthogonalProjection
	{
		private Plane _projectionPlane;
		private double _width;
		private double _height;

		public OrthogonalProjection(double width, double height, Plane projection)
		{
			_width = width;
			_height = height;
			_projectionPlane = projection;
		}

		public (double x, double y) projectPoint( Point p )
		{
			switch( _projectionPlane )
			{
				case xy:
					return ( p[0], p[1] );
				case yz: 
					return ( p[1], p[2] );
				case xz:
					return ( p[0], p[2] );
			}
		}
	}
}
