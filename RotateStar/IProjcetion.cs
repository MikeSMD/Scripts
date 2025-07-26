using System;

namespace Star
{

	public interface IProjection
	{
	public (double x, double y, double depth ) projectPoint( Point p );
	}
}
