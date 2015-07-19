using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

namespace DynaBlasterVita
{
	public class Boss : Enemy
	{
		
	public Boss(GenerateObstacles obs, Map map, Bomberman player, GraphicsContext g, Vector2 scale) : base(obs, map, player, g, scale) {
			this.speed = (int)(1*scale.X/2);
		}
		
		public override void tick() {
			if (!removed)
				animation.tick();
			if (health <= 0)
				die();
			else {
				if (move) {
					this.position.X += xdir;
					this.position.Y += ydir;
	
					bool isinsquare = position.Equals(lastpos);
					lastpos = new Rectangle(position.X, position.Y, position.Width, position.Height);
					if (!isinsquare)
						foreach (Obstacle rect in obs.getPath()) {
							if (rect.getBounds().contains(position)) {
								isinsquare = true;
								break;
							}
						}
	
					int xsign = sign((int)(player.position.X - position.X));
					int ysign = sign((int)(player.position.Y - position.Y));
	
					if (isinsquare) {
						switch (r.Next(15)) {
						case 0:
							xdir = xsign * speed;
							ydir = 0;
							break;
						case 1:
							xdir = 0;
							ydir = ysign * speed;
							break;
						}
					}
				}
			}
		}
		
		protected int sign(int inS) {
			if(inS >= 0)
				return 1;
			return -1;
		}
	}
}

