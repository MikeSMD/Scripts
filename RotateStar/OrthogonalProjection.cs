using System;

namespace Star
{

	public enum Plane
	{
		xy, yz, xz
	}
	class OrthogonalProjection : IProjection
	{
		private Plane _projectionPlane;

		public OrthogonalProjection(double width, double height, Plane projection)
		{
			_projectionPlane = projection;
		}

		public (double x, double y, double depth ) projectPoint( Point p )
		{
			switch( _projectionPlane )
			{
				case xy:
					return ( p[0], p[1], p[2] );
				case yz: 
					return ( p[1], p[2], p[0] );
				case xz:
					return ( p[0], p[2], p[1] );
			}
		}
	}
}
