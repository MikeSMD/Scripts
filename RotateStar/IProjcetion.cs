using System;

namespace Star
{

	public interface IProjection
	{
	(double x, double y, double depth ) projectPoint( Point p );
	}
}
