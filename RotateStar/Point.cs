using System;

namespace Star
{


	public class Point
	{
		private double[] _dimensions;
		public char sign;
		public ConsoleColor cc;
		public Point(int dimensions, char sign, ConsoleColor cc = ConsoleColor.White )
		{
			if (dimensions <= 0)
				throw new ArgumentException("Count must be positive.");
			_dimensions = new double[ dimensions ];
			this.sign = sign;
			this.cc = cc;
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

		public void MakeHomogenous()
		{
			List< double > p = new List < double >();
			for ( int i = 0 ; i < this.getDimensions(); ++i )
			{
				p.Add(this[ i ]);
			}
			p.Add( 1 );
			this.SetCoordinates( p.ToArray() );
		}

		public void MakeKartezs()
		{
			int dimensions =  this.getDimensions() - 1;
			List< double > p = new List < double >();
			for ( int i = 0 ; i < dimensions; ++i )
			{
				p.Add(this[ i ] / this[dimensions]);
			}
			this.SetCoordinates( p.ToArray() );
		}

		public Point Copy()
		{
			Point res = new Point(this._dimensions.Length, this.sign);
			res.SetCoordinates( this._dimensions );
			return res;
		}
		public void SetCoordinates(double[] coordinates)
		{
			if (coordinates == null)
				throw new ArgumentException("Invalid coordinates array.");
			_dimensions = (double[])coordinates.Clone();
		}

		public int getDimensions()
		{
			return _dimensions.Length;
		}
		public void Normalize()
		{
			double k = this[ 0 ] * this[ 0 ]+ this [ 1 ] * this[ 1 ] + this[ 2 ] * this[ 2 ];
			k = Math.Sqrt( k );
			if ( k == 0 ) return;
			this[ 0 ] /= k;
			this[ 1 ] /= k;
			this [ 2 ] /= k;
		}
		public static Point operator -(Point a, Point b)
		{
			Point k = new Point (a.getDimensions(), a.sign);
			for ( int i = 0; i < a.getDimensions(); ++i )
				k [ i ] = a[ i ] - b[ i ];
			return k;
		}
		public static Point operator -(Point a)
		{
			Point k = new Point (a.getDimensions(), a.sign);
			for ( int i = 0; i < a.getDimensions(); ++i )
				k [ i ] = -a[ i ];
			return k;
		}
		public static Point Cross(Point a, Point b)
		{
			Point k = new Point (a.getDimensions(), a.sign);
			int p = 1;
			for ( int i = 0; i < a.getDimensions(); ++i )
			{
				int pi = ( p+i ) % a.getDimensions();
				int pi1 = ( p+i+1 ) % a.getDimensions();
				k [ i ] = a[ pi ]* b[ pi1 ] - a[ pi1 ]* b [ pi ];
			}
			return k;
		}
		public static double Dot(Point a, Point b)
		{
			double k = 0.0;
			for ( int i = 0; i < a.getDimensions(); ++ i )
			{
				k += a[ i ] * b[ i ];
			}
			return k;
		}
	}
}
