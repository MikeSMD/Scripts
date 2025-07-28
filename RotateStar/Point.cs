using System;

namespace Star
{

	struct Coordinate
	{
	
	
	}

	public struct Point
	{
		private double[] _dimensions;
		public char sign;
		public ConsoleColor cc;
		public Point(int dimensions, char sign )
		{
			if (dimensions <= 0)
				throw new ArgumentException("Count must be positive.");
			_dimensions = new double[ dimensions ];
			this.sign = sign;
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


		public Point Copy()
		{
			Point res = new Point(this._dimensions.Length, this.sign);
			res.SetCoordinates( this._dimensions );
			return res;
		}
		public void SetCoordinates(double[] coordinates)
		{
			if (coordinates == null || coordinates.Length != _dimensions.Length)
				throw new ArgumentException("Invalid coordinates array.");
			_dimensions = (double[])coordinates.Clone();
		}

		public int getDimensions()
		{
			return _dimensions.Length;
		}
	}
}
