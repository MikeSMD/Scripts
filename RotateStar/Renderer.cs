using System;

namespace Star
{
	enum PrimitiveType
	{
		PT_Point, PT_Triangle
	}
	class ConsoleRenderer
	{
		private int _width;
		private int _heighr;
		private double _scale;
		private (char sign, bool changed, ConsoleColor cc)[][] _scene;
		private IProjection _projection;
		private Camera camera;
		private PrimitiveType primitive;
		public ConsoleRenderer( int width, int heighr, double scale, IProjection projection, Camera camera = null, PrimitiveType primitive = PrimitiveType.PT_Point )
		{
			if ( width < 0 || heighr < 0 || scale <= 0.0 )
			{
				throw new ArgumentException("chyba");
			}
			this.primitive = primitive;
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

			List <(double x, double y, double depth, char sign, ConsoleColor cc)> triangles = new List <(double x, double y, double depth, char sign, ConsoleColor cc)>();
			foreach( IRenderable r in render_obects )
			{
				//spojení matic do homogenní matice- případ na dodělání
				Point[] points = TransformSequence.applyMultipleTransformations(r.transformations, r.points);

				if ( this.primitive == PrimitiveType.PT_Triangle )
				{
					triangles.Clear();
				}
				for ( int i = 0; i < points.Length; ++i )
				{
					Point p = points[ i ];
					if ( this.camera != null )
					{
						points[ i ].MakeHomogenous();
						p = this.camera.applyViewMatrix( points[ i ] );
					}
				
					(double x, double y, double depth ) newpoints= _projection.projectPoint( p );
					
					if ( double.IsInfinity( newpoints.x ) || double.IsInfinity ( newpoints.y ) || double.IsInfinity ( newpoints.depth ) )
					{
						continue;
					}

					if ( primitive == PrimitiveType.PT_Triangle )
					{
			
						triangles.Add( ( newpoints.x, newpoints.y * 0.5, newpoints.depth, r.points[ i ].sign, r.points[ i ].cc ) );
						if ( triangles.Count == 3 )
						{
							
							
							// rasterizcae - velmi ale velmi zjednodusene -prevede se trojuhlenik ze 3 bodu na grid prvky
							//jak? vyberu nejmensi crverec kde je trojuhelnik a jedoduse otesruji zda kazdy bod tam patri.
							int xmin = (int)Math.Floor(triangles.Min(r => r.x));
							int xmax = (int)Math.Ceiling(triangles.Max(r => r.x));
							int ymin = (int)Math.Floor(triangles.Min(r => r.y));
							int ymax = (int)Math.Ceiling(triangles.Max(r => r.y));

							// meze do gridu
							xmin = Math.Clamp(xmin, 0, _width - 1);
							xmax = Math.Clamp(xmax, 0, _width - 1);
							ymin = Math.Clamp(ymin, 0, _heighr - 1);
							ymax = Math.Clamp(ymax, 0, _heighr - 1);

							//testovani vsech bodu a pripadna interpolace hloubky - bez barycentrickych koordinar - protoze nevim co toe :)
							// Pixel (x, y) je uvnitř trojúhelníku, pokud leží na stejné straně všech tří hran (AB, BC, CA)-vekrrorovy soucin.
							// vektor os
							( double x, double y )osaAB = ( triangles [ 1 ].x - triangles[ 0 ].x, triangles[ 1 ].y - triangles[ 0 ].y ); 
							( double x, double y )osaBC = ( triangles [ 2 ].x - triangles[ 1 ].x, triangles[ 2 ].y - triangles[ 1 ].y ); 
							( double x, double y )osaCA = ( triangles [ 0 ].x - triangles[ 2 ].x, triangles[ 0 ].y - triangles[ 2 ].y ); 

							// double toleration = 0.0;
							for ( int x = xmin; x < xmax; ++x )
							{
								for ( int y = ymin; y < ymax; ++y )
								{
									// směr k bodu AP = P - A
									( double x, double y ) vec = ( x - triangles[ 0 ].x , y - triangles[ 0 ].y);
									// cross producr
									double p1 = osaAB.x * vec.y - osaAB.y * vec.x;

									// 2. hrana
									// směr k bodu BP = P - B
									( double x, double y ) vec2 = ( x - triangles[ 1 ].x , y - triangles[ 1 ].y);
									// cross producr
									double p2 = osaBC.x * vec2.y - osaBC.y * vec2.x;
								
									// 3. hrana
									// směr k bodu CP = P - C
									( double x, double y ) vec3 = ( x - triangles[ 2 ].x , y - triangles[ 2 ].y);
									// cross producr
									double p3 = osaCA.x * vec3.y - osaCA.y * vec3.x;
									

									if ( ( p1 > 0 || p2 > 0 || p3 > 0 ) && ( p1 < 0 || p2 < 0 || p3 < 0 ) )
										continue;
									// pokud vse projde - aktualizace hloubky gridu, pokud je blize a pokud je na hrane dam i znak - drateny model prozarim, pak pridam normaly a interpolaci jejich
									// interpolace hloubky - velmi zjednodusena opet.. jinak dle barycenrrickych souradnic
									// dle vzdalenosti od vrcholu ABC bodu P
									double z1 = Math.Sqrt(vec.x * vec.x + vec.y * vec.y);
									double z2 = Math.Sqrt(vec2.x * vec2.x + vec2.y * vec2.y);
									double z3 = Math.Sqrt(vec3.x * vec3.x + vec3.y * vec3.y);
									double eps = 0.0001; // Zabrání dělení nulou
									double w1 = 1.0 / (z1 + eps);
									double w2 = 1.0 / (z2 + eps);
									double w3 = 1.0 / (z3 + eps);
									
								double depth = (w1 * triangles[0].depth + w2 * triangles[1].depth + w3 * triangles[2].depth) / (w1 + w2 + w3);
									if (depth < 0.0 || depth > 1.0) continue;
									if ( grid[ y ] [ x ].depth > depth )
									{
										int index = 0; // vusde kde z1 prvni

										var q = new List< ( double z, int index ) >();
										q.Add((z1, 0));
										q.Add((z2, 1));
										q.Add((z3, 2));
										(double z, int index) wq = q.MinBy(r=>r.z);
										index = wq.index;
										grid[ y ][ x ].sign = triangles[ index ].sign;
										grid[ y ][ x ].cc = triangles[ index ].cc;
										grid[ y ][ x ].depth = depth;
									}
								}

							}
						 	// triangles.RemoveAt( 0 );//trianglestrip
						 	triangles.Clear(); // triangle
						}
					}
					else
					{
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
							continue;
						}
						if ( newpoints.depth < 0.0 || newpoints.depth > 1.0)
						{
							continue;
						}
						if ( grid [ y ][ x ].depth > newpoints.depth )
						{
							if ( newpoints.depth > 0.9993 )
							{
								grid[ y ][ x ].sign = '.';
							}
							else grid[ y ][ x ].sign = r.points[ i ].sign;
							grid[ y ][ x ].depth = newpoints.depth;
							grid[ y ] [ x ].cc = r.points[ i ].cc;
						}
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
						Console.Write($"{_scene[ i ][ j ].sign}");
						_scene[ i ][ j ].changed = false;
						
					}
				}

			}
		}
	}
}
