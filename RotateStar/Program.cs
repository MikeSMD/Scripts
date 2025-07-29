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
			PrespectiveProjection rj = new PrespectiveProjection(Plane.xy, 1.0 );
			ConsoleRenderer cr = new ConsoleRenderer(200, 55, 30, rj);
		//	Scale sc = new Scale([0.5, 0.5, 0.5]);
			Rotation r = new Rotation (1, Osa.y);
			Move m2 = new Move([0.0, -5.0, -50]);
		//	Ball b = new Ball(5, 'o', ConsoleColor.Green, 2);
		//	Cylinder b = new Cylinder(5, 12, 'k', '.', Osa.y, ConsoleColor.DarkYellow, 2);
			Tree b = new Tree (2,15,8, Osa.y, 5, 2);
			Move m= new Move([0.0, 1.0, -17]);
			Plane_2d kr = new Plane_2d(100,100, Plane.xz , '.', ConsoleColor.Green, 3 );
			Move mk=new Move([-50.0, 7.0, -50.0]);
			Scale scr = new Scale([1.1, 1.1, 1.1]);
			kr.addTransformation(mk);
			b.Koruna.addTransformation( r );
			b.Koruna.addTransformation( scr );
			 b.addTransformationAll( m );
			s2.addTransformation( m2 );
			b.RegisterAll(cr);
			cr.Register(s2);
			cr.Register(kr);
			int k = -1;

			bool l = 1==1;
			while( k-- != 0 )
			{
				cr.RenderScene();

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
				 Thread.Sleep(60);
			}

		}
	}
}
