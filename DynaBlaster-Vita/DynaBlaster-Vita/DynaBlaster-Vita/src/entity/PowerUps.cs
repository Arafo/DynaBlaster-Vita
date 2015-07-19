using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;

namespace DynaBlasterVita
{
	public class PowerUps : Entity
	{
		private String POWERUPS = "/Application/res/image/powerups.png"; 
		private static int w = 16;
		private static int h = 16;
			
		private int type;
		
		public PowerUps(int type, List<Obstacle> obs, GraphicsContext g, Vector2 scale) : base(scale) {
			
			this.type = type;
			this.position.Width = w;
			this.position.Height = h;
			
			this.sl = new SpriteLoader();			
			this.sl.cargarImagen(POWERUPS);
			this.sl.setImage(sl.getImage().Resize(new ImageSize((int)(sl.getImage().Size.Width*scale.X), 
			                                                    (int)(sl.getImage().Size.Height*scale.Y))));
			this.ss = new SpriteSheet(this.sl.getImage());
			
			Sprite[] powerup = new Sprite[]{ss.obtenerSprite((int)(type*w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.Y), g),
						ss.obtenerSprite((int)(type*w*scale.X), (int)(h*scale.Y), (int)(w*scale.X), (int)(h*scale.Y), g)};
			if (type != -1) while (!located) findStartPos(obs);
			this.animation = new Animation(powerup, 10, Animation.Direction.DOWN);
			this.animation.start();
			
		}
		
		public override void tick() {
			this.animation.tick();
		}
		
		public override void Render() {
			animation.getSprite().Position.X = position.X;
			animation.getSprite().Position.Y = position.Y;
			animation.getSprite().Render();
	//		g.setColor(Color.ORANGE);
	//		g.fillRect(position.x, position.y, w*scale, h*scale);
	
		}
		
		public override int getType() {
			return type;
		}
		
		private bool findStartPos(List<Obstacle> obs) {
			for (int i = 0; i < obs.Count; i++) {
				if (obs[i].isSolid() && random.NextDouble() >= 0.95) {
					this.position.X = obs[i].position.X;
					this.position.Y = obs[i].position.Y;
					located = true;
					break;
				}
			}
			return true;
			
		}
	}
}

