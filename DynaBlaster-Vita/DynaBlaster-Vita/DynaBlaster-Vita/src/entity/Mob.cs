using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

namespace DynaBlasterVita
{
	public class Mob : Entity
	{
		public int health = 10;
		public int tickTime = 0;
		protected Animation up, down, left, right, death;
		protected GraphicsContext g;
	
		public Mob(GraphicsContext g, Vector2 scale) : base(scale) {
			this.g = g;
		}
	
		public virtual void tick(InputHandler input) {
			tickTime++;
			animation.tick();
		}
		
//		public virtual void Render() {
//		}
		
		public virtual void die() {
			animation = death;
			animation.start();
			if (animation.finalFrame())
				remove();
		}
	
//		public bool blocks(Entity e) {
//			return e.isBlockableBy(this);
//		}
	
		public override void hurt(Entity mob, int damage) {
			doHurt(damage);
		}
	
		public virtual void doHurt(int damage) {
			health -= damage;
		}
	
		public virtual bool findStartPos(Map map) {
			return false;
		}
	}
}

