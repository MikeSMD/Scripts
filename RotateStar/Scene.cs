using System;

namespace Star
{

	delegate void Prenos();
	class Scene
	{
		public List< Renderable > render_obects {get; set;} = new  List< Renderable >(); // var :(
		public Camera camera {get; set;}
		public readonly ConsoleRenderer consoleRenderer;
		public Prenos Updator {get; set;}
		public Scene( Camera camera, int width, int height, double focal, double pear, double far, PrimitiveType primirive = PrimitiveType.PT_Point  )
		{
			this.camera = camera;
			var pp = new PrespectiveProjection( width, height, focal, pear, far );
			consoleRenderer = new ConsoleRenderer( width, height,1.0, pp, this.camera, primirive );
		}

		public Scene( Plane plane, int width, int height, double scale, PrimitiveType primirive = PrimitiveType.PT_Point )
		{
			this.camera = null;
			var op = new OrthogonalProjection( plane );
			consoleRenderer = new ConsoleRenderer( width, height, scale, op, null, primirive );
		}

		public void AddObject ( Renderable obc )
		{
			render_obects.Add( obc );
		}

		public void ShowScene()
		{
			this.consoleRenderer.RenderScene( this.render_obects );
		}

		public void UpdateScene( )
		{
			this.Updator?.Invoke();
		}
	}
}
