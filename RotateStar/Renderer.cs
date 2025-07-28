using System;

namespace Star
{
	class ConsoleRenderer
	{
		private List < IRenderable > _registered = new List < IRenderable > ();
		private int _width;
		private int _heighr;
		private double _scale;
		private (char sign, bool changed)[][] _scene;
		private IProjection _projection;

		public ConsoleRenderer( int width, int heighr, double scale, IProjection projection )
		{
			if ( width < 0 || heighr < 0 || scale <= 0.0 )
			{
				throw new ArgumentException("chyba");
			}

			_width = width;
			_heighr = heighr;
			_scale = scale;
			_projection = projection;
			_scene = Enumerable.Range(0, _heighr).Select(_ => Enumerable.Repeat((' ',true), _width).ToArray()).ToArray();
		}





		public void RenderScene()
		{
			(char sign, double depth)[][] grid = Enumerable.Range(0, _heighr).Select(_ => Enumerable.Repeat((' ', double.PositiveInfinity), _width).ToArray()).ToArray();

			foreach( IRenderable r in _registered )
			{
				Point[] points = TransformSequence.applyMultipleTransformations(r.transformations, r.points);
				for ( int i = 0; i < points.Length; ++i )
				{
					(double x, double y, double depth ) newpoints= _projection.projectPoint( points[ i ] );
					newpoints.x = newpoints.x * _scale + (_width / 2);
					newpoints.y = -newpoints.y * _scale + (_heighr / 2);

					int x =(int) Math.Floor(newpoints.x);
					int y =(int) Math.Floor( newpoints.y);
					if ( x >= _width || y >= _heighr || x < 0 || y < 0.1 )
						continue;

					if ( grid [ y ][ x ].depth > newpoints.depth )
					{
						grid[ y ][ x ].sign = r.points[ i ].sign;
						grid[ y ][ x ].depth = newpoints.depth;
					}
				}
			}

			for ( int i = 0; i < _heighr; ++i )
			{
				for ( int j = 0; j < _width; ++j )
				{
					if ( grid[ i ][ j ].sign != _scene[ i ][ j ].sign )
					{
						_scene[ i ][ j ].sign = grid[ i ][ j ].sign;
						_scene[ i ][ j ].changed = true;
					}
				}
			}
			for ( int i = 0; i < _heighr ; ++i )
			{
				for ( int j = 0; j < _width; ++j )
				{
					if ( _scene[ i ][ j ].changed )
					{
						Console.SetCursorPosition(j,i);
						Console.WriteLine($"{_scene[ i ][ j ].sign}");
						_scene[ i ][ j ].changed = false;
					}
				}
			}
		}

		public void Register(IRenderable q)
		{
			_registered.Add( q );
		}
	}
}
