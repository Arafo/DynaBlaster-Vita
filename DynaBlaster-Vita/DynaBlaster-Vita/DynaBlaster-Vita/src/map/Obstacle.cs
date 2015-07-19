using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;

namespace DynaBlasterVita
{
	public class Obstacle : GameObject
	{
		private bool solid;
		private Animation a1, a2, a3;
	
		public Obstacle(int x, int y, Vector2 size, Vector2 mapSize, Animation animation,
				SpriteSheet ss, Vector2 scale, bool solid, GraphicsContext g) : 
		base ((int)(g.Screen.Width/2 - mapSize.X/2 + x), (int)(y + 24*scale.Y), (int)(size.X), (int)(size.Y), animation) {
			//this.position.x = this.position.x - 8 * scale;
			//this.position.y = this.position.y + 24 * scale;
			this.solid = solid;
			
			ss = new SpriteSheet(ss.getImage().Resize(new ImageSize((int)(ss.getWidth()*scale.X),
			                                                        (int)(ss.getHeight()*scale.Y))));
			
			Sprite[] obstacle = {ss.obtenerSprite((int)(2*size.X), 0, (int)(size.X), (int)(size.Y), g)};
			a1 = new Animation(obstacle, 1, Animation.Direction.DOWN);
			
			Sprite[] explosion = {ss.obtenerSprite((int)(size.X*7), (int)(size.Y), (int)(size.X), (int)(size.Y), g),
					ss.obtenerSprite((int)(size.X*8), (int)(size.Y), (int)(size.X), (int)(size.Y), g),
					ss.obtenerSprite((int)(size.X*9), (int)(size.Y), (int)(size.X), (int)(size.Y), g),
					ss.obtenerSprite((int)(size.X*10), (int)(size.Y), (int)(size.X), (int)(size.Y), g),
					ss.obtenerSprite((int)(size.X*11), (int)(size.Y), (int)(size.X), (int)(size.Y), g),
					ss.obtenerSprite((int)(size.X*12), (int)(size.Y), (int)(size.X), (int)(size.Y), g),
					ss.obtenerSprite((int)(size.X*13), (int)(size.Y), (int)(size.X), (int)(size.Y), g)};
			
			a2 = new Animation(explosion, 6, Animation.Direction.DOWN);
			
			Sprite[] blink = {ss.obtenerSprite((int)(2*size.X), 0, (int)(size.X), (int)(size.Y), g),
					ss.obtenerSprite((int)(size.X*7), (int)(size.Y), (int)(size.X), (int)(size.Y), g)};
			
			a3 = new Animation(blink, 20, Animation.Direction.DOWN);
	
			this.animation = a1;
		}
		
		public Obstacle(Obstacle singleton, int x, int y, bool solid, Vector2 size, Vector2 scale, Vector2 mapSize, GraphicsContext g) : 
		base ((int)(g.Screen.Width/2 - mapSize.X/2 + x), (int)(y + 24*scale.Y), (int)(size.X), (int)(size.Y), null) {

			this.solid = solid;
			this.a1 = singleton.getA1();
			this.a2 = singleton.getA2();
			this.a3 = singleton.getA3();
			this.animation = a1;
		}
	
//		public bool intersects(GameObject o) {
//			return position.intersects(o.position);
//		}
		
		public bool isSolid() {
			return solid;
		}
		
		public void die() {
			if (!removed) {
				if (!(animation == a2)) {
					animation = a2;
					animation.start();
				}
			}
		}
	
		public void update(long ms) {
			//animation.update(ms);
		}
		
		public override void tick() {
			animation.tick();
			if (animation == a2 && animation.finalFrame()) {
				remove();
			}
		}
	
		public override void draw(GraphicsContext g) {
			if (solid) {
				if(animation == a3) {
					a1.getSprite().Position.X = position.X;
					a1.getSprite().Position.Y = position.Y;
					a1.getSprite().Render();
				}
				animation.getSprite().Position.X = position.X;
			  	animation.getSprite().Position.Y = position.Y;
				animation.getSprite().Render();
//	//			g.setColor(Color.BLACK);
//	//		 	g.fillRect(position.x, position.y, position.width, position.height);
			}
//	//		else {
//	//			g.setColor(Color.ORANGE);
//	//			g.fillRect(position.x, position.y, position.width, position.height);
//	//		}
		}
		
		public void blink() {
			if(animation == a1) {
				animation = a3;
				a3.start();
			}
		}
		
		public void setX (int x) {
			base.position.X = x;
		}
		
		public void setY(int y) {
			base.position.Y = y;
		}
		
		public void setState(bool state) {
			this.solid = state;
		}
		
		public Animation getA1() {	
			return a1;
		}
		
		public Animation getA2() {	
			return a2;
		}
		
		public Animation getA3() {	
			return a3;
		}
	}
}
	
