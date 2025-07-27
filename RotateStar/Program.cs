using System;
using System.Threading;

namespace Star
{


	class MainStar
	{
		public static void Main( string[] args )
		{
			Console.WriteLine("predone");
			Star_2d s = new Star_2d(9, Plane.xy, '*' );
			Star_2d s2 = new Star_2d(9, Plane.xy, 'o' );
			Console.WriteLine("done");
			OrthogonalProjection pj = new OrthogonalProjection( Plane.xy );
			ConsoleRenderer cr = new ConsoleRenderer(100, 25, 1, pj);
			Move m = new Move([-20.0, 1.0, 1.0]);
			Move m2 = new Move([20.0, -1.0, 5.0]);
			s.addTransformation( m );
			s2.addTransformation( m2 );
			cr.Register(s);
			cr.Register(s2);
			int k = 60;


			while( k-- != 0 )
			{
				cr.RenderScene();
				m.vecmove[ 0 ] = m.vecmove[ 0 ] + 1; 
				m.vecmove[ 1 ] = m.vecmove[ 1 ] - 0.2; 

				m2.vecmove[ 0 ] = m2.vecmove[ 0 ] - 1; 
				m2.vecmove[ 1 ] = m2.vecmove[ 1 ] - 0.1;
				Thread.Sleep(60);
			}

		}
	}
}
