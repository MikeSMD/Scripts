using System;

namespace Star
{
	class Camera
	{
		public Point up {get; set;}
		public Point lookAt{get; set;}
		public Point position{ get; set;}
		public Camera(Point position, Point lookAt, Point up )
		{
			this.up = up; //kde je pro kameru "nahoru" bez ohledu na další vektory - je jen orientační
			this.position = position; // pozice kamery
			this.lookAt = lookAt; // kam se kamera kouká
		}

		public Point applyViewMatrix( Point p ) //vrací souřadnicový systém tak, že svět pčetočí tak, že kamera je v 0,0,0 a kouká se na zápornou osu Z nahoru je Y a X doprava
		{
			// přepočet hodnor na konkrétní přesné hodnory
			Point look = ( this.lookAt - this.position );
			look.Normalize();
			Point right = (Point.Cross( this.up, look )) ;//sklon do stran
			right.Normalize();
			Point newup= (Point.Cross( right, look )); //sklon nahoru a dolu
			newup.Normalize();
			if ( p.getDimensions() <= 3 )
				throw new ArgumentException ("chyba");
			//pohledová matice je inverzní ke kamere
			//[ x.x  x.y  x.z  dot(x, eye) ]    [ p[0] ]
			//[ y.x  y.y  y.z  dot(y, eye) ] *  [ p[1] ]
			//[ z.x  z.y  z.z  dot(z, eye) ]    [ p[2] ]
			//[  0    0    0        1      ]     [ p[3] ]

			p[ 0 ] = right[ 0 ] * p [ 0 ] + right[ 1 ] * p[ 1 ] + right [ 2 ] * p [ 2 ] + Point.Dot( right, this.position );
			p[ 1 ] = newup[ 0 ] * p [ 0 ] + newup[ 1 ] * p[ 1 ] + newup[ 2 ] * p [ 2 ] + Point.Dot( newup, this.position );
			p[ 2 ] = -look[ 0 ] * p [ 0 ] - look[ 1 ] * p[ 1 ] - look[ 2 ] * p [ 2 ] + Point.Dot( -look, this.position );
			p[ 3 ] = p [ 3 ];

			return p;
		}

	}
}
