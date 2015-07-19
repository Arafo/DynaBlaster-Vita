using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;

namespace DynaBlasterVita
{
	public class FrogYellow : Enemy
	{
		private String ANIMATION = "/Application/res/image/frog_yellow.png"; 
		private static int w = 16;
		private static int h = 18;
	
		public FrogYellow(GenerateObstacles obs, Map map, Bomberman player, GraphicsContext g, Vector2 scale) :
		base(obs, map, player, g, scale) {
	
			this.position.Width = 12*scale.X;
			this.position.Height = 14*scale.Y;
			this.health = 10;
			this.score = 1000;
			
			this.sl = new SpriteLoader();			
			this.sl.cargarImagen(ANIMATION);
			this.sl.setImage(sl.getImage().Resize(new ImageSize((int)(sl.getImage().Size.Width*scale.X), 
			                                                    (int)(sl.getImage().Size.Height*scale.Y))));
			this.ss = new SpriteSheet(this.sl.getImage());
			
			Sprite[] mov = {ss.obtenerSprite((int)(0*w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.Y), g), 
					ss.obtenerSprite((int)(1*w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.Y), g),
					ss.obtenerSprite((int)(2*w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.Y), g), 
					ss.obtenerSprite((int)(1*w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.Y), g)};
			Sprite[] die = {ss.obtenerSprite((int)(3*w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.Y), g),
					ss.obtenerSprite((int)(4*w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.Y), g), 
					ss.obtenerSprite((int)(5*w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.Y), g),
					ss.obtenerSprite((int)(6*w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.Y), g), 
					ss.obtenerSprite((int)(7*w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.Y), g),
					ss.obtenerSprite((int)(8*w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.Y), g), 
					ss.obtenerSprite((int)(9*w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.Y), g),
					new Score(score, w, h, g, scale).getImage()};
			
			this.down = new Animation(mov, 8, Animation.Direction.DOWN);
			this.death = new Animation(die, 14, Animation.Direction.DOWN);
			
			// Animacion inicial
			animation = down;
			animation.start();
	
		}
		
		public override Rectangle getBounds() {
			return new Rectangle(position.X, position.Y, w, h);
		}
	}
}

