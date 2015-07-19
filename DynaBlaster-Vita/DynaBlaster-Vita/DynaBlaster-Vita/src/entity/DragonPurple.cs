using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;

namespace DynaBlasterVita
{
	public class DragonPurple : Enemy
	{
		private String ANIMATION = "/Application/res/image/dragon_purple.png"; 
		private static int w = 16;
		private static int h = 18;
	
		public DragonPurple(GenerateObstacles obs, Map map, Bomberman player, GraphicsContext g, Vector2 scale) :
		base(obs, map, player, g, scale) {
	
			this.position.Width = 12*scale.X;
			this.position.Height = 14*scale.Y;
			this.health = 10;
			this.score = 400;
			
			this.sl = new SpriteLoader();			
			this.sl.cargarImagen(ANIMATION);
			this.sl.setImage(sl.getImage().Resize(new ImageSize((int)(sl.getImage().Size.Width*scale.X), 
			                                                    (int)(sl.getImage().Size.Height*scale.Y))));
			this.ss = new SpriteSheet(this.sl.getImage());
			
			Sprite[] movDown = {
					ss.obtenerSprite((int)(9*w*scale.X), 0,(int)(w*scale.X),(int)(h*scale.Y), g),
					ss.obtenerSprite((int)(10*w*scale.X), 0,(int)(w*scale.X),(int)(h*scale.Y), g),
					ss.obtenerSprite((int)(11*w*scale.X), 0,(int)(w*scale.X),(int)(h*scale.Y), g),
					ss.obtenerSprite((int)(10*w*scale.X), 0,(int)(w*scale.X),(int)(h*scale.Y), g) };
			Sprite[] movUp = {
					ss.obtenerSprite((int)(3*w*scale.X), 0,(int)(w*scale.X),(int)(h*scale.Y), g),
					ss.obtenerSprite((int)(4*w*scale.X), 0,(int)(w*scale.X),(int)(h*scale.Y), g),
					ss.obtenerSprite((int)(5*w*scale.X), 0,(int)(w*scale.X),(int)(h*scale.Y), g),
					ss.obtenerSprite((int)(4*w*scale.X), 0,(int)(w*scale.X),(int)(h*scale.Y), g) };
			Sprite[] movLeft = {
					ss.obtenerSprite((int)(0*w*scale.X), 0,(int)(w*scale.X),(int)(h*scale.Y), g),
					ss.obtenerSprite((int)(1*w*scale.X), 0,(int)(w*scale.X),(int)(h*scale.Y), g),
					ss.obtenerSprite((int)(2*w*scale.X), 0,(int)(w*scale.X),(int)(h*scale.Y), g),
					ss.obtenerSprite((int)(0*w*scale.X), 0,(int)(w*scale.X),(int)(h*scale.Y), g) };
			Sprite[] movRight = {
					ss.obtenerSprite((int)(6*w*scale.X), 0,(int)(w*scale.X),(int)(h*scale.Y), g),
					ss.obtenerSprite((int)(7*w*scale.X), 0,(int)(w*scale.X),(int)(h*scale.Y), g),
					ss.obtenerSprite((int)(8*w*scale.X), 0,(int)(w*scale.X),(int)(h*scale.Y), g),
					ss.obtenerSprite((int)(7*w*scale.X), 0,(int)(w*scale.X),(int)(h*scale.Y), g) };
			Sprite[] die = {
					ss.obtenerSprite((int)(12*w*scale.X), 0,(int)(w*scale.X),(int)(h*scale.Y), g),
					ss.obtenerSprite((int)(13*w*scale.X), 0,(int)(w*scale.X),(int)(h*scale.Y), g),
					ss.obtenerSprite((int)(14*w*scale.X), 0,(int)(w*scale.X),(int)(h*scale.Y), g),
					ss.obtenerSprite((int)(15*w*scale.X), 0,(int)(w*scale.X),(int)(h*scale.Y), g),
					ss.obtenerSprite((int)(16*w*scale.X), 0,(int)(w*scale.X),(int)(h*scale.Y), g),
					ss.obtenerSprite((int)(17*w*scale.X), 0,(int)(w*scale.X),(int)(h*scale.Y), g),
					ss.obtenerSprite((int)(18*w*scale.X), 0,(int)(w*scale.X),(int)(h*scale.Y), g),
					new Score(score, w, h, g, scale).getImage() };
	
			this.down = new Animation(movDown, 8, Animation.Direction.DOWN);
			this.up = new Animation(movUp, 8, Animation.Direction.UP);
			this.left = new Animation(movLeft, 8, Animation.Direction.LEFT);
			this.right = new Animation(movRight, 8, Animation.Direction.RIGHT);
			this.death = new Animation(die, 14, Animation.Direction.DOWN);
			
			// Animacion inicial
			animation = down;
			animation.start();
	
		}
		
		public override void tick() {
			base.tick();
			if (health > 0) {
				if (xdir > 0) {
					animation = right;
					animation.start();
				} else if (xdir < 0) {
					animation = left;
					animation.start();
				} else if (ydir < 0) {
					animation = up;
					animation.start();
				} else if (ydir > 0) {
					animation = down;
					animation.start();
				}
			}
		}
	
//		public override void Render() {
//			base.render();
//		}
	
		public override Rectangle getBounds() {
			return new Rectangle(position.X, position.Y, w, h);
		}
		
		public  override bool canPassWalls() {
			return true;
		}
		
		public override bool canPassBombs() {
			return true;
		}
	}
}



