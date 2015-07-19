using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;

namespace DynaBlasterVita
{
	public class Exit : Entity
	{
		
		private String EXIT = "/Application/res/image/bomb.png"; 
		private static int w = 16;
		private static int h = 16;
		
		private bool activated;
		
		private List<Enemy> enemies;
		
		public Exit(List<Obstacle> obs, List<Enemy> enemies, GraphicsContext g, Vector2 scale) : base(scale) {
			this.enemies = enemies;
			this.activated = false;
			
			this.position.Width = w;
			this.position.Height = h;
			
			this.sl = new SpriteLoader();			
			this.sl.cargarImagen(EXIT);
			int ww = sl.getImage().Size.Width;
			int hh = sl.getImage().Size.Height;
			Image aux = sl.getImage().Resize(new ImageSize((int)(ww*scale.X), 
			                                               (int)(hh*scale.Y)));
			this.sl.setImage(aux);
			this.ss = new SpriteSheet(this.sl.getImage());
			
			Sprite[] exit = new Sprite[]{ss.obtenerSprite(0, 0, (int)(w*scale.X), (int)(h*scale.Y), g),
						ss.obtenerSprite((int)(w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.Y), g)};
			
			if (obs != null) while (!located) findStartPos(obs);
			else {
				this.position.X = 40*scale.X;
				this.position.Y = 72*scale.Y;
			}
			this.animation = new Animation(exit, 10, Animation.Direction.DOWN);
			
		}
		
		public override void tick() {
			this.animation.tick();
			if (enemies.Count <= 0) {
				activated = true;
				if (!animation.finalFrame()) animation.start();
			}
		}
		
		public override void Render() {
			animation.getSprite().Position.X = position.X;
			animation.getSprite().Position.Y = position.Y;
			animation.getSprite().Render();
		}
		
		private bool findStartPos(List<Obstacle> obs) {
			for (int i = 0; i < obs.Count; i++) {
				if (obs[i].isSolid() && random.NextDouble() >= 0.9) {
					this.position.X = obs[i].position.X;
					this.position.Y = obs[i].position.Y;
					located = true;
					break;
				}
			}
			return true;
			
		}
		
		public bool isActive() {
			return activated;
		}
	}
}

