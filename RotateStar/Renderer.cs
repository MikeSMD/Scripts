using System;

namespace Star
{
	class ConsoleRenderer
	{
		private int _width;
		private int _heighr;
		private double _scale;
		private (char sign, bool changed, ConsoleColor cc)[][] _scene;
		private IProjection _projection;
		private Camera camera;

		public ConsoleRenderer( int width, int heighr, double scale, IProjection projection, Camera camera = null )
		{
			if ( width < 0 || heighr < 0 || scale <= 0.0 )
			{
				throw new ArgumentException("chyba");
			}

			this.camera = camera;
			_width = width;
			_heighr = heighr;
			_scale = scale;
			_projection = projection;
			_scene = Enumerable.Range(0, _heighr).Select(_ => Enumerable.Repeat((' ',true, ConsoleColor.White), _width).ToArray()).ToArray();
		}



		public void SimplePlaneRenderScene()
		{
			//
		}


		public void RenderScene( List < IRenderable > render_obects )
		{
			(char sign, double depth, ConsoleColor cc)[][] grid = Enumerable.Range(0, _heighr).Select(_ => Enumerable.Repeat((' ', double.PositiveInfinity,ConsoleColor.White ), _width).ToArray()).ToArray();

			foreach( IRenderable r in render_obects )
			{
				//spojení matic do homogenní matice- případ na dodělání
				Point[] points = TransformSequence.applyMultipleTransformations(r.transformations, r.points);
				for ( int i = 0; i < points.Length; ++i )
				{
					Point p = points[ i ];
					if ( this.camera != null )
					{
						points[ i ].MakeHomogenous();
						p = this.camera.applyViewMatrix( points[ i ] );
					}
				
				//	p.MakeKartezs();

					(double x, double y, double depth ) newpoints= _projection.projectPoint( p );
				
					/* resi jiz projekce
					 * newpoints.x = newpoints.x * _scale + (_width / 2);
					newpoints.y = -newpoints.y * 0.5 * _scale + (_heighr / 2); //kompenzace osy y
					 * */
					// newpoints.x = newpoints.x ;
					newpoints.y = newpoints.y * 0.5;

					int x =(int) Math.Floor(newpoints.x);
					int y =(int) Math.Floor( newpoints.y);
					if ( x >= _width || y >= _heighr || x < 0 || y < 0.1 )
					{
					//	Console.WriteLine($"skip1{x},{y}");
						continue;
					}
					if ( newpoints.depth < 0.0 || newpoints.depth > 1.0)
					{
					//	Console.WriteLine($"skip2{newpoints.depth}");
						
						continue;
					}
					if ( grid [ y ][ x ].depth > newpoints.depth )
					{
						if ( newpoints.depth > 0.9993 )//?
						{
							grid[ y ][ x ].sign = '.';
						}
						else grid[ y ][ x ].sign = r.points[ i ].sign;
						grid[ y ][ x ].depth = newpoints.depth;
						grid[ y ] [ x ].cc = r.points[ i ].cc;
					}
				}
			}

			for ( int i = 0; i < _heighr; ++i )
			{
				for ( int j = 0; j < _width; ++j )
				{
					if ( grid[ i ][ j ].sign != _scene[ i ][ j ].sign || grid[ i ] [ j ].cc != _scene[ i ][ j ].cc)
					{
						_scene[ i ][ j ].sign = grid[ i ][ j ].sign;
						_scene[ i ][ j ].changed = true;
						_scene[ i ][ j ].cc = grid[ i ][ j ].cc;
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
						Console.ForegroundColor = _scene[i][j].cc;
						Console.WriteLine($"{_scene[ i ][ j ].sign}");
						_scene[ i ][ j ].changed = false;
					}
				}
			}
		}
	}
}
