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
       class Scale : Transformation
	{
		public double[] vecscale {get; set;}
		public Scale( double[] vecscale )
		{
			this.vecscale = vecscale;
		}
		public override Point transformPoint( Point p )
		{
			if ( p.getDimensions() != vecscale.Length )
				throw new ArgumentException("chyba");
			Point k = p.Copy();
			for ( int i = 0; i < k.getDimensions(); ++i )
			{
				k[ i ] *= vecscale[ i ];
			}
			return k;
		}
	}

       public enum Osa
       {
	       x,y,z
       }
       class Rotation : Transformation
	{
		public double angle {get; set;}
		public Osa osa {get; set;}
		public Rotation( double angle, Osa osa )
		{
			this.angle = angle;
			this.osa = osa;
		}
		public override Point transformPoint( Point p )
		{
			double cos = Math.Cos(angle);
			double sin = Math.Sin(angle);
			Point k = p.Copy();
			switch (osa)
			{
				case Osa.x:
					{
						double y = k[1];
						double z = k[2];
						k[1] = y * cos - z * sin;
						k[2] = y * sin + z * cos;
					}
					break;

				case Osa.y:
					{
						double x = k[0];
						double z = k[2];
						k[0] = x * cos + z * sin;
						k[2] = -x * sin + z * cos;
					}
					break;

				case Osa.z:
					{
						double x = k[0];
						double y = k[1];
						k[0] = x * cos - y * sin;
						k[1] = x * sin + y * cos;
					}
					break;
			}			return k;
		}
	}
}
