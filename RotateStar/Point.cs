using System;

namespace Star
{

	struct Coordinate
	{
	
	
	}

	struct Point
	{
		private double[] _dimensions;

		public Point(int dimensions)
		{
			if (dimensions <= 0)
				throw new ArgumentException("Count must be positive.");
			_dimensions = new double[ dimensions ];
		}

		public double this[ int index ]
		{
			get 
			{
				if ( index < 0 || index >= _dimensions.Length )
					throw new IndexOutOfRangeException();
				return _dimensions[ index ];
			}
			set  
			{
				if ( index < 0 || index > _dimensions.Length )
					throw new IndexOutOfRangeException();
				_dimensions[ index ] = value;
			}
		}



		public void SetCoordinates(double[] coordinates)
		{
			if (coordinates == null || coordinates.Length != _dimensions.Length)
				throw new ArgumentException("Invalid coordinates array.");
			_dimensions = (double[])coordinates.Clone();
		}
	}
}
