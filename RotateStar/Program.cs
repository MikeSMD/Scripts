using System;
using System.Threading;
namespace Star
{
	class MainStar
	{
		public static void Main( string[] args )
		{
			Console.WriteLine("predone");
			Star_2d s = new Star_2d(8, Plane.xy, '*'); 
			Star_2d s2 = new Star_2d(9, Plane.xy, 'o', ' ', ConsoleColor.Red);
			Console.WriteLine("done");
			OrthogonalProjection pj = new OrthogonalProjection( Plane.xy );
			Point lookAt = new Point(3, ' ');
			lookAt.SetCoordinates([0.0, 7.0, -1.0]);
			Point eye = new Point(3, ' ');
			eye.SetCoordinates([0.0, 7.0, 0.0]);
			Point up = new Point(3, ' ');
			up.SetCoordinates([0.0, 1.0, 0.0 ]);
			Camera c = new Camera(eye, lookAt, up);
			PrespectiveProjection rj = new PrespectiveProjection(Plane.xy, 1000, 150, Math.PI / 4, 0.1, 100.0 );
			ConsoleRenderer cr = new ConsoleRenderer(1000, 150, 1, rj,c);
		//	Scale sc = new Scale([0.5, 0.5, 0.5]);
			Rotation r = new Rotation (1, Osa.y);
			Move m2 = new Move([0.0, -5.0, -50]);
		//	Ball b = new Ball(5, 'o', ConsoleColor.Green, 2);
		//	Cylinder b = new Cylinder(5, 12, 'k', '.', Osa.y, ConsoleColor.DarkYellow, 2);
			Tree b = new Tree (2,15,8, Osa.y, 5, 2);
			Move m= new Move([0.0, -7.5, -25.0]);

			Random rnd = new Random();
			for ( int i = 0; i < 5; ++i )
			{
					double z = ( double ) - rnd.Next() % 100;
					double x = (double) rnd.Next() % 151 - 75;
					double height = (double) rnd.Next() % 22 + 3;
					Tree br = new Tree (2*height/15,height,height/2, Osa.y, 7, 3);
					Move mr = new Move([x, - height - 2, z]);
					br.addTransformationAll( mr );
					br.RegisterAll( cr );
					//iteratorova funkce pro nastaveni pohybu?
			}
			Plane_2d kr = new Plane_2d(1000,1000, Plane.xz , '.', ConsoleColor.Green, 5 );
			Move mk=new Move([-500.0, 0.0, -500.0]);
			Scale scr = new Scale([1.1, 1.1, 1.1]);
			kr.addTransformation(mk);
			b.Koruna.addTransformation( r );
			b.Koruna.addTransformation( scr );
			 b.addTransformationAll( m );
			s2.addTransformation( m2 );
			b.RegisterAll(cr);
			cr.Register(s2);
			cr.Register(kr);

			bool l = 1==1;
			while( 1 < 2 )
			{
				cr.RenderScene();
 if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true); // true = nezobrazí znak v konzoli

                switch (key.Key)
		{
			case ConsoleKey.W:
				Point forward = (lookAt - eye);
				forward.Normalize();
				for ( int i = 0; i < 3; ++i )
			{
				eye[ i ] -= forward[ i ] * 0.6;
				lookAt [ i ] -=  forward[ i ] * 0.6;

			}
                        break;
                    case ConsoleKey.A:
			Point k = Point.Cross( lookAt - eye, up);

			for ( int i = 0; i < 3; ++i )
			{
				eye[ i ] -= k[ i ];
				lookAt [ i ] -= k [ i ];
			}

                        break;
                    case ConsoleKey.S:
            		Point krr = (lookAt - eye);
			krr.Normalize();
				for ( int i = 0; i < 3; ++i )
			{
				eye[ i ] +=krr[ i ] * 0.6;
				lookAt [ i ] +=krr[ i ] * 0.6;

			}
                        break;
                    case ConsoleKey.D:
                        Point re = Point.Cross( lookAt - eye, up);

			for ( int i = 0; i < 3; ++i )
			{
				eye[ i ] += re[ i ];
				lookAt [ i ] += re [ i ];
			}
                        break;
                }
		 // vyčisti zbytek bufferu
        while (Console.KeyAvailable)
            Console.ReadKey(true);
            }


			//	for ( int i = 0; i < 3; ++i )
			//		sc.vecscale[ i ] = sc.vecscale[ i ] + 0.1;
				r.angle +=0.01;
				 m2.vecmove[ 2 ] = m2.vecmove[ 2 ] + 0.2; 
				 if ( scr.vecscale[ 1 ] >= 1.1 )
				 {
				 	l = 1==2;
				 }
				 if ( scr.vecscale[ 1 ] <= 1.0 )
				 {
				 	l = 1==1;
				 }
				 
					
					if ( l )
					{
						scr.vecscale[ 0 ] += 0.01;
						scr.vecscale[ 1 ] += 0.01;
						scr.vecscale[ 2 ] += 0.01;
					}
					else
					{
						scr.vecscale[ 0 ] -= 0.01;
						scr.vecscale[ 1 ] -= 0.01;
						scr.vecscale[ 2 ] -= 0.01;
					}
				// Thread.Sleep(60); fps korekce nutná. - rychlost výpisu ascci a výpočtů nesmí hrát roli na rychlost v prostředí - vlákno navíc či měření času mezi?
			}

		}

	}
}
