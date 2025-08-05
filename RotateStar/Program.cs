using System;
using System.Threading;
namespace Star
{
	class MainStar //r
	{
		public static void Main( string[] args )
		{
Console.CursorVisible = false; // Skryje kurzor v konzoli
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
Console.CursorVisible = true;
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
			kuzel.GetTriangulated();

			Tree pq = new Tree( '.', ConsoleColor.Green );
			pq.GetTriangulated();

			Rotation rq = new Rotation ( Math.PI / 2 , Osa.x );
			kuzel.addTransformation(rq);
			kuzel.addTransformation(m);
			pq.addTransformation(rq);
			pq.addTransformation(m);
			scene.AddObject( kuzel );
			scene.AddObject( pq );
	
			return scene;
		}
	}
}
