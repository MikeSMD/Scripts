using System;

namespace Star
{
	class Camera
	{
		public Point up {get; set;}
		public Point lookAt{get; set;}
		public Point position{ get; set;}

		//souřadnice jež budou užity v rámci kamera space - zde se kamera kouká na -z takže yaw bude otočení všech objektů kolem z (v kamera space) a y je nahoru takže se všechny objekty v kamera space již převedené otočí kolem x pro pitch - musí se teda aplikovat PO převodu všech objektů (bodů) do KAMERA SPACE
		public double angleYaw {get; set;}
		public double anglePitch {get; set;}
		public double speedWalk {get;set;} = 0.5;
		//pokud bych otáčel kameru samotnou nebo její vektory před uvedením každého objektu do kamera space, pak bych otáčel kolem up a right kamery což by vyýadovalo otáčemí směru forward kolem obecné osy. a vuči něm by se pak nastavili objekry do kamera space
		public Camera(Point position, Point lookAt, Point up )
		{
			this.up = up; //kde je pro kameru "nahoru" bez ohledu na další vektory - je jen orientační
			this.position = position; // pozice kamery
			this.lookAt = lookAt; // kam se kamera kouká

			this.angleYaw = 0.0;
			this.anglePitch = 0.0;
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
			
			// 2x prevod homogeni -> kartezsky. -.-
			p = RotateYaw( p );
			p = RotatePitch( p );
			return p;
		}

		private Point RotateYaw(Point p)
		{


			p.MakeKartezs(); //--.-- udelat pak transformace pro homogenni i
					 //rotace kolem Y daného bodu v KAMERA SPACE
			Rotation r = new Rotation(this.angleYaw, Osa.y);
			p = r.transformPoint( p );
			p.MakeHomogenous(); // -.-
			return p;
		}
		private Point RotatePitch(Point p)
		{
			if ( anglePitch > 89.0 ) // zamezení převrácení
				anglePitch = 89.0;	
	if ( anglePitch < -89.0 ) // zamezení převrácení
				anglePitch = -89.0;				p.MakeKartezs(); //-.-
					 //rotace kolem X daného bodu v KAMERA SPACE
			Rotation r = new Rotation(this.anglePitch, Osa.x);
			p = r.transformPoint( p );
			p.MakeHomogenous(); // -.-
			return p;
		}
	
		public void GoForward()
			{
			//jednoduše přeneseme viektor kam kamera míří do kamera sapce a posuneme
			Point forward = lookAt - position; //zde neni jeste forward ale v camera space,ono ro funguje ale jen pro ty pripady kdy by kamera mela stejne proporce up apod jako po aplikaci viewmatrix takze pohled na -z a y nahoru

			//dosraneme forward do camera space
			forward.MakeHomogenous();

			forward = RotatePitch(forward);
			forward = RotateYaw(forward);
			forward.MakeKartezs();
			forward.Normalize();

			for ( int i = 0; i < 3; ++i )
			{
				position[ i ] = position[ i ] - forward[ i ] * this.speedWalk;
    				lookAt[i] = lookAt[i] - forward[i] *  this.speedWalk;  // forward je normovaný vektor
			}	
		}
	
		
		public void GoBackward() //to same -.- - do vice funkci..
		{
			Point forward = lookAt - position;
			forward.MakeHomogenous();
			forward = RotatePitch(forward);
			forward = RotateYaw(forward);
			forward.MakeKartezs();
			forward.Normalize();

			for ( int i = 0; i < 3; ++i )
			{
				position[ i ] = position[ i ] + forward[ i ] * this.speedWalk;
    				lookAt[i] = lookAt[i] + forward[i] *  this.speedWalk;  // forward je normovaný vektor
			}	
		}	

		//pro test only
		public void GoRighr() //to same -.- - do vice funkci..
		{
			Point forward = lookAt - position;
			forward.MakeHomogenous();
			forward = RotatePitch(forward);
			forward = RotateYaw(forward);
			forward.MakeKartezs();
			forward.Normalize();

			Point right = Point.Cross(forward, up); //up je zde jen orientační,potřebuju real one? asi ne..
			right.Normalize();

			for (int i = 0; i < 3; ++i)
			{
				position[i] += right[i] * speedWalk;
				lookAt[i] += right[i] * speedWalk;
			}
		}	

	public void GoLefr() //to same -.- - do vice funkci..
	{
		Point forward = lookAt - position;
		forward.MakeHomogenous();
		forward = RotatePitch(forward);
		forward = RotateYaw(forward);
		forward.MakeKartezs();
		forward.Normalize();

		Point right = Point.Cross(forward, up);
		right.Normalize();

		for (int i = 0; i < 3; ++i)
		{
			position[i] -= right[i] * speedWalk;
			lookAt[i] -= right[i] * speedWalk;
		}
	}	

	}
}
