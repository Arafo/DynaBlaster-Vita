using System;

using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;

namespace DynBlasterVita
{
	public class Obstacle : GameObject
	{
		private bool solid;
		private Animation a1, a2, a3;
	
		public Obstacle(int x, int y, int size, Animation animation,
				SpriteSheet ss, int scale, bool solid, GraphicsContext g) : base (x - 8*scale, y + 24*scale, size, size, animation) {
			//this.position.x = this.position.x - 8 * scale;
			//this.position.y = this.position.y + 24 * scale;
			this.solid = solid;
			
			ss = new SpriteSheet(ss.getImage().Resize(new ImageSize(ss.getWidth()*scale, ss.getHeight()*scale)));
			
			Sprite[] obstacle = {ss.obtenerSprite(2*size, 0, size, size, g)};
			a1 = new Animation(obstacle, 1, Animation.Direction.DOWN);
			
			Sprite[] explosion = {ss.obtenerSprite(size*7, size, size, size, g),
					ss.obtenerSprite(size*8, size, size, size, g),
					ss.obtenerSprite(size*9, size, size, size, g),
					ss.obtenerSprite(size*10, size, size, size, g),
					ss.obtenerSprite(size*11, size, size, size, g),
					ss.obtenerSprite(size*12, size, size, size, g),
					ss.obtenerSprite(size*13, size, size, size, g)};
			
			a2 = new Animation(explosion, 6, Animation.Direction.DOWN);
			
			Sprite[] blink = {ss.obtenerSprite(2*size, 0, size, size, g),
					ss.obtenerSprite(size*7, size, size, size, g)};
			
			a3 = new Animation(blink, 20, Animation.Direction.DOWN);
	
			this.animation = a1;
		}
		
		public Obstacle(Obstacle singleton, int x, int y, bool solid, int size, int scale) : base(x - 8*scale, y + 24*scale, size, size, null) {
			this.solid = solid;
			this.a1 = singleton.getA1();
			this.a2 = singleton.getA2();
			this.a3 = singleton.getA3();
			this.animation = a1;
		}
	
	//	public boolean intersects(GameObject o) {
	//		return position.intersects(o.position);
	//	}
		
		public bool isSolid() {
			return solid;
		}
		
//		public void die() {
//			if (!removed) {
//				if (!(animation == a2)) {
//					animation = a2;
//					animation.start();
//				}
//			}
//		}
	
		public void update(long ms) {
			//animation.update(ms);
		}
		
		public void tick() {
//			animation.tick();
//			if (animation.equals(a2) && animation.finalFrame()) {
//				remove();
//			}
		}
	
		public void draw(GraphicsContext g) {
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
//			if(animation == a1) {
//				animation = a3;
//				a3.start();
//			}
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
	
