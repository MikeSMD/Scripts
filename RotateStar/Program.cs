using System;
using System.Threading;
namespace Star
{//-
	class MainStar 
	{
		public static void Main( string[] args )
		{
			Scene foresrScene = makeScene();
		//Scene foresrScene = makeForesrScene();	
			while( 1 < 2 )
			{
				 foresrScene.UpdateScene();
				foresrScene.ShowScene();
			double rs = 0.01;

				if (Console.KeyAvailable)
				{
					ConsoleKeyInfo key = Console.ReadKey(true);

					switch (key.Key)
					{
						case ConsoleKey.W:
							foresrScene.camera.GoForward();
							break;
						case ConsoleKey.A:
							foresrScene.camera.GoLefr();
							break;
						case ConsoleKey.S:
							foresrScene.camera.GoBackward();
							break;
						case ConsoleKey.D:
							foresrScene.camera.GoRighr();
							break;
						case ConsoleKey.UpArrow:
							foresrScene.camera.anglePitch -= rs;
							break;
						case ConsoleKey.DownArrow:
							foresrScene.camera.anglePitch += rs;
							break;
						case ConsoleKey.RightArrow:
							foresrScene.camera.angleYaw += rs;
							break;
						case ConsoleKey.LeftArrow:
							foresrScene.camera.angleYaw -= rs;
							break;
					}

					while (Console.KeyAvailable)
						Console.ReadKey(true);
				}
			//	break;
			}
		}
	public static Scene makeScene()
		{
			Point lookAt = new Point(3, ' ');
			Point eye = new Point(3, ' ');
			Point up = new Point(3, ' ');

			lookAt.SetCoordinates([0.0, 7.0, -1.0]);
			eye.SetCoordinates([0.0, 7.0, 0.0]);
			up.SetCoordinates([0.0, 1.0, 0.0 ]);
			Camera c = new Camera(eye, lookAt, up);

			Scene scene = new Scene( c, 311, 82, Math.PI / 20, 0.1, 500.1, PrimitiveType.PT_Triangle ); //311 vs 52
			Move m = new Move([0.0, -5.0, -25]);
			Rotation q = new Rotation( -Math.PI/2, Osa.x);
			Kuzel kuzel = new Kuzel( '.', ConsoleColor.Red );
			Tree tree = new Tree(0.3, 1.5);
			kuzel.BuildTriangulated();

			kuzel.addTransformation(m);
			scene.AddObject( kuzel );
			tree.addTransformationAll(q);
			tree.addTransformationAll(m);
			tree.RegisterAll( scene );
			return scene;
		}
	/*
		public static Scene makeForesrScene()
		{
				Point lookAt = new Point(3, ' ');
			Point eye = new Point(3, ' ');
			Point up = new Point(3, ' ');

			lookAt.SetCoordinates([0.0, 7.0, -1.0]);
			eye.SetCoordinates([0.0, 7.0, 0.0]);
			up.SetCoordinates([0.0, 1.0, 0.0 ]);
			Camera c = new Camera(eye, lookAt, up);

			Scene foresrScene = new Scene( c, 311, 82, Math.PI / 20, 0.1, 500.1, PrimitiveType.PT_Point ); //311 vs 52
			Star_2d s = new Star_2d(9, Plane.xy, 'o', ' ', ConsoleColor.Red);
			Move m = new Move([0.0, -5.0, -50]);
			s.addTransformation( m );

			Random rnd = new Random();
			for ( int i = 0; i < 1; ++i )
			{
					double z = ( double ) - rnd.Next() % 200;
					double x = (double) rnd.Next() % 251 - 125;
					double height = (double) rnd.Next() % 22 + 3;
					Tree br = new Tree (2*height/15,height,height/2, Osa.y, 7, 3);
					Move mr = new Move([x, - height , z]);
					br.addTransformationAll( mr );
					br.RegisterAll( foresrScene );
			}
			foresrScene.Updator = () =>
			{
				m.vecmove[ 2 ] += 0.1;
			};
			Plane_2d kr = new Plane_2d(200,200, Plane.xz , '.', ConsoleColor.Green, 5 );
			Move mk=new Move([-100.0, 0.0, -100.0]);
			kr.addTransformation( mk );
			foresrScene.AddObject( s );
			//foresrScene.AddObject( kr );

			return foresrScene;
		}
		*/
	}
}
