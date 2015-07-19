using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Imaging;

namespace DynaBlasterVita
{
	public class Entity : GameObject
	{
		private int colisionlimit = 5;
		protected bool located = false;
		//protected int scale;
		protected Vector2 scale;
		protected SpriteLoader sl;
		protected SpriteSheet ss;
		
		
		public Entity(Vector2 scale) : base(0, 0, 0, 0, null) {
			this.scale = scale;
		}
	
//		public virtual void tick() {
//			animation.tick();
//		}
		
		public virtual void Render() {
		}
	
//		public virtual void remove() {
//			removed = true;
//		}
	
		public bool intersects(int x0, int y0, int x1, int y1) {
			//return super.intersects(new GameObject(x0,y0,x1-x0,y1-y0,null));
			//return !(position.x + xr < x0 || position.y + yr < y0 || position.x - xr > x1 || position.y - yr > y1);
			return false;
		}
	
		public bool blocks(Entity e) {
			return false;
		}
	
		public virtual void hurt(Entity mob, int dmg) {
		}
	
		public virtual void touchedBy(Entity entity) {
		}
	
		public bool isBlockableBy(Mob mob) {
			return true;
		}
		
		public void collide(GameObject obs) {
			//System.out.println("collision!" + animation.getAnimationDirection());
			switch(animation.getAnimationDirection()) {
			case Animation.Direction.UP:
			case Animation.Direction.DOWN:
				if(Math.Abs(this.position.X-(obs.position.X+obs.position.Width)) <= colisionlimit*scale.X/2)
					this.position.X = (obs.position.X+obs.position.Width) ;
				else if(Math.Abs((this.position.X+this.position.Width)-obs.position.X) <= colisionlimit*scale.X/2)
					this.position.X = (obs.position.X-this.position.Width) ;
				else if(Math.Abs(this.position.Y-(obs.position.Y+obs.position.Height)) <= colisionlimit*scale.Y/2)
					this.position.Y = (obs.position.Y+obs.position.Height) ;
				else if(Math.Abs((this.position.Y+this.position.Height)-obs.position.Y) <= colisionlimit*scale.Y/2)
					this.position.Y = (obs.position.Y-this.position.Height) ;
				break;
			case Animation.Direction.LEFT:
			case Animation.Direction.RIGHT:
				if(Math.Abs(this.position.Y-(obs.position.Y+obs.position.Height)) <= colisionlimit*scale.Y/2)
					this.position.Y = (obs.position.Y+obs.position.Height) ;
				else if(Math.Abs((this.position.Y+this.position.Height)-obs.position.Y) <= colisionlimit*scale.Y/2)
					this.position.Y = (obs.position.Y-this.position.Height) ;
				if(Math.Abs(this.position.X-(obs.position.X+obs.position.Width)) <= colisionlimit*scale.X/2)
					this.position.X = (obs.position.X+obs.position.Width) ;
				else if(Math.Abs((this.position.X+this.position.Width)-obs.position.X) <= colisionlimit*scale.X/2)
					this.position.X = (obs.position.X-this.position.Width) ;
				break;
				
			}
		}
	
		public virtual int getType() {
			return 0;
		}
	
		public virtual int getScore() {
			return 0;
		}
		
		public virtual bool canPassWalls() {
			return false;
		}
		
		public virtual bool canPassBombs() {
			return false;
		}
		
		public virtual bool isInvincible() {
			return false;
		}
	}
}

