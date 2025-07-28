using System;

namespace Star
{

	class PrespectiveProjection : IProjection
	{
		private Plane _projectionPlane;
		private double focal;

		public PrespectiveProjection(Plane projection, double focal )
		{
			this.focal = focal;
			_projectionPlane = projection;
		}

		public (double x, double y, double depth ) projectPoint( Point p )
		{
            		if (p[2] == 0) p[2] = 1e-6;
			switch( _projectionPlane )
			{
				case Plane.xy:
					if (p[2] == 0) p[2] = 1e-6; // ochrana před dělením nulou
					return (focal * p[0] / p[2], focal * p[1] / p[2], p[2]);
				case Plane.yz: 
					if (p[0] == 0) p[0] = 1e-6;
					return (focal * p[1] / p[0], focal * p[2] / p[0], p[0]);
				case Plane.xz:
					if (p[1] == 0) p[1] = 1e-6;
					return (focal * p[0] / p[1], focal * p[2] / p[1], p[1]);
			}
			return (0.0, 0.0, double.PositiveInfinity);
		}
	}
}
