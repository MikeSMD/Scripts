using System;
using System.Collections.Generic;
using System.Linq;
namespace Star
{
	public static class TransformSequence
	{
		public static Point[] applyMultipleTransformations(List< Transformation > r, Point[] p)
		{
			if ( r == null ) return p;
			if ( r.Count == 0 ) return p;
			Point[] result = p.Select(point => point.Copy()).ToArray();
			foreach( var rr in r )
			{
				result = rr.applyTransformation( result );
			}
			return result;

		}
	}

	public abstract class Transformation
	{
		public abstract Point transformPoint( Point p );
		public Point[] applyTransformation( Point[] p )
		{
			Point[] result = new Point[p.Length];
			for (int i = 0; i < p.Length; i++)
			{
				result[i] = transformPoint(p[i]);
			}
			return result;
		}
	
	}
	// prvni postupne, pak s integraci do jedine matice
       class Move : Transformation
	{
		public double[] vecmove {get; set;}
		public Move( double[] vecmove )
		{
			this.vecmove = vecmove;
		}
		public override Point transformPoint( Point p )
		{
			if ( p.getDimensions() != vecmove.Length )
				throw new ArgumentException("chyba");
			Point k = p.Copy();
			for ( int i = 0; i < k.getDimensions(); ++i )
			{
				k[ i ] += vecmove[ i ];
			}
			return k;
		}
	}

}
