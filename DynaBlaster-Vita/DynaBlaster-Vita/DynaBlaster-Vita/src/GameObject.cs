using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;

namespace DynaBlasterVita
{
	public class GameObject
	{
		protected Random random = new Random();
		public Rectangle position;
		protected Animation animation;
		public bool removed;
		public int xr = 6;
		public int yr = 6;
		
		public GameObject(int x, int y, int width, int height, Animation animation) {
			position = new Rectangle(x, y, width, height);
			this.animation = animation;
		}
		
		public bool intersects(GameObject o) {
			return position.intersects(o.position);
		}
		
		public bool intersects(Rectangle bounds) {
			//return bounds.intersects(new Rectangle(position.x, position.y, position.width, position.height));
			return false;
		}
		
		public void remove() {
			removed = true;
		}
		
//		public void hurt(Entity mob, int dmg) {
//		}
	
		protected void touchedBy(GameObject go) {
		}
		
		public virtual Rectangle getBounds() {
			return position;
		}
		
		public virtual void tick() {
			animation.tick();
		}
		
		public virtual void render(GraphicsContext g) {
		}
		
		public void tick(long ms) {
			animation.tick();
		}
		
		public virtual void draw(GraphicsContext g) {
			//g.drawImage(animation.getSprite(), position.x, position.y, null);
		}
	}
}

