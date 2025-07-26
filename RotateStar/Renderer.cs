using System;

namespace Star
{
	class ConsoleRenderer
	{
		private List < IRenderable > _registered;
		private int _width;
		private int _heighr;
		private double _scale;
		private (char sign, bool changed)[][] _scene;
		private IProjection _projection;

		public ConsoleRenderer( int width, int heighr, int scale, IProjection projection )
		{
			if ( width < 0 || height < 0 || scale <= 0.0 )
			{
				throw new ArgumentException("chyba");
			}

			_width = width;
			_heighr = heighr;
			_scale = scale;
			_projection = projection;
			_scene = Enumerable.Range(0, height).Select(_ => Enumerable.Repeat((' ',false), width).ToArray()).ToArray();
		}





		public void RenderScene()
		{
			var (char sign, double depth)[][] grid = Enumerable.Range(0, height).Select(_ => Enumerable.Repeat((' ', double.PositiveInfinity), width).ToArray()).ToArray();

			foreach( IRenderable r in _registered )
			{
				for ( int i = 0; i < r.points.Length; ++i )
				{
					(double x, double y, double depth ) newpoints= _projection.projectPoint( r.points[ i ] );
					newpoints.x = newpoints.x * _scale + (width / 2);
					newpoints.y = newpoints.y * _scale + (height / 2);

					int x = Math.Round(newpoints.x);
					int y = Math.Round( newpoints.y);

					if ( grid [ y ][ x ].depth < newpoints.depth )
					{
						grid[ y ][ x ].sign = r.points[ i ].sign;
						grid[ y ][ x ].depth = newpoints.depth;
					}
				}
			}
			for ( int i = 0; i < height; ++i )
			{
				for ( int j = 0; j < width; ++i )
				{
					if ( grid[ i ][ j ].sign != _scene[ i ][ j ] )
					{
						_scene[ i ][ j ].sign = grid[ i ][ j ].sign;
						_scene[ i ][ j ].changed = true;
					}
				}
			}

			for ( int i = 0; i < height; ++i )
			{
				for ( int j = 0; j < width; ++i )
				{
					if ( _scene.changed )
					{
						Console.SetCursorPosition(i,j);
						Console.WriteLine($"{_scene[ i ][ j ].sign}");
					}
				}
			}
		}
	}
}
