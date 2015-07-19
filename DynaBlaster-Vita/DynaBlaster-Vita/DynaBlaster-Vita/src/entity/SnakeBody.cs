using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;

namespace DynaBlasterVita
{
	public class SnakeBody : SnakeHead
	{
		private String ANIMATION = "/Application/res/image/blue_snake.png"; 
		private static int w = 16;
		private static int h = 18;
	
		protected SnakeHead parent;
	
		public SnakeBody(GenerateObstacles obs, Map map, Bomberman player, SnakeHead parent, GraphicsContext g, Vector2 scale) :
		base(obs, map, player, g, scale) {
			this.parent = parent;
			this.move = false;
			this.health = 80;
			this.score = 100;
			
			this.position = new Rectangle(parent.position.X, parent.position.Y, parent.position.Width, parent.position.Height);
			
			this.sl = new SpriteLoader();			
			this.sl.cargarImagen(ANIMATION);
			this.sl.setImage(sl.getImage().Resize(new ImageSize((int)(sl.getImage().Size.Width*scale.X), 
			                                                    (int)(sl.getImage().Size.Height*scale.Y))));
			this.ss = new SpriteSheet(this.sl.getImage());
			
			up = new Animation[4];
			down = new Animation[4];
			left = new Animation[4];
			right = new Animation[4];
			for(int i=0; i < 4; i++) {
				Sprite[] u = {ss.obtenerSprite((int)(12*w*scale.X), (int)((2*i+1)*h*scale.Y+(i+1)*scale.Y), (int)(w*scale.X), (int)(h*scale.Y), g), 
						ss.obtenerSprite((int)(13*w*scale.X), (int)((2*i+1)*h*scale.Y+(i+1)*scale.Y), (int)(w*scale.X), (int)(h*scale.Y), g),
						ss.obtenerSprite((int)(14*w*scale.X), (int)((2*i+1)*h*scale.Y+(i+1)*scale.Y), (int)(w*scale.X), (int)(h*scale.Y), g),
						ss.obtenerSprite((int)(13*w*scale.X), (int)((2*i+1)*h*scale.Y+(i+1)*scale.Y), (int)(w*scale.X), (int)(h*scale.Y), g)};
				
				Sprite[] d = {ss.obtenerSprite((int)(0*w*scale.X), (int)((2*i+2)*h*scale.Y+(i+1)*scale.Y), (int)(w*scale.X), (int)(h*scale.Y), g), 
						ss.obtenerSprite((int)(19*w*scale.X), (int)((2*i+1)*h*scale.Y+(i+1)*scale.Y), (int)(w*scale.X), (int)(h*scale.Y), g),
						ss.obtenerSprite((int)(18*w*scale.X), (int)((2*i+1)*h*scale.Y+(i+1)*scale.Y), (int)(w*scale.X), (int)(h*scale.Y), g),
						ss.obtenerSprite((int)(19*w*scale.X), (int)((2*i+1)*h*scale.Y+(i+1)*scale.Y), (int)(w*scale.X), (int)(h*scale.Y), g)};
				
				Sprite[] l = {ss.obtenerSprite((int)(3*w*scale.X), (int)((2*i+2)*h*scale.Y+(i+1)*scale.Y), (int)(w*scale.X), (int)(h*scale.Y), g), 
						ss.obtenerSprite((int)(2*w*scale.X), (int)((2*i+2)*h*scale.Y+(i+1)*scale.Y), (int)(w*scale.X), (int)(h*scale.Y), g),
						ss.obtenerSprite((int)(1*w*scale.X), (int)((2*i+2)*h*scale.Y+(i+1)*scale.Y), (int)(w*scale.X), (int)(h*scale.Y), g),
						ss.obtenerSprite((int)(2*w*scale.X), (int)((2*i+2)*h*scale.Y+(i+1)*scale.Y), (int)(w*scale.X), (int)(h*scale.Y), g)};
				
				Sprite[] r = {ss.obtenerSprite((int)(15*w*scale.X), (int)((2*i+1)*h*scale.Y+(i+1)*scale.Y), (int)(w*scale.X), (int)(h*scale.Y), g), 
						ss.obtenerSprite((int)(16*w*scale.X), (int)((2*i+1)*h*scale.Y+(i+1)*scale.Y), (int)(w*scale.X), (int)(h*scale.Y), g),
						ss.obtenerSprite((int)(17*w*scale.X), (int)((2*i+1)*h*scale.Y+(i+1)*scale.Y), (int)(w*scale.X), (int)(h*scale.Y), g),
						ss.obtenerSprite((int)(16*w*scale.X), (int)((2*i+1)*h*scale.Y+(i+1)*scale.Y), (int)(w*scale.X), (int)(h*scale.Y), g)};
				
				up[i] = new Animation(u, 20, Animation.Direction.UP);
				down[i] = new Animation(d, 20, Animation.Direction.DOWN);
				left[i] = new Animation(l, 20, Animation.Direction.LEFT);
				right[i] = new Animation(r, 20, Animation.Direction.RIGHT);
			}
			
			Sprite[] die = {ss.obtenerSprite((int)(8*w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.Y), g),
					ss.obtenerSprite((int)(9*w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.Y), g),
					ss.obtenerSprite((int)(10*w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.Y), g),
					ss.obtenerSprite((int)(11*w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.Y), g),
					ss.obtenerSprite((int)(12*w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.Y), g),
					ss.obtenerSprite((int)(13*w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.Y), g),
					ss.obtenerSprite((int)(14*w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.Y), g),
					ss.obtenerSprite((int)(15*w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.Y), g),
					ss.obtenerSprite((int)(16*w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.Y), g)};
			
			this.death = new Animation(die, 14, Animation.Direction.DOWN);
			animation = down[0];
			animation.start();
	
		}
	
		public override void tick() {
			if(child == null || child.removed)
				animation.start();
			else if(animation == death)
				animation.stop();
			if(animation == death && animation.finalFrame())
				remove();
			if (!removed)
				animation.tick();
			if (health <= 0)
				die();
			else {
				if (invincible > 0)
					invincible--;
				if (health <= 20)
					state = 3;
				else if (health <= 40)
					state = 2;
				else if (health <= 60)
					state = 1;
				else
					state = 0;
				if (animation != death) {
					if (parent.positions.Count > Map.TILESIZE * scale.X) {
						PosDir pd = parent.positions.First.Value;
						parent.positions.RemoveFirst();
						position = pd.position;
						xdir = pd.dirx;
						ydir = pd.diry;
						if(child != null)
							positions.AddLast(pd);
					}
				}
				// super.tick();
				animation.tick();
				if (animation != death) {
					if (xdir < 0)
						animation = left[state];
					else if (xdir > 0)
						animation = right[state];
					else if (ydir < 0)
						animation = up[state];
					else
						animation = down[state];
				}
	
				animation.start();
			}
		}
	
//		public void render(Graphics2D g) {
//			super.render(g);
//		}
		
		public override Rectangle getBounds() {
			return new Rectangle(position.X, position.Y, w, h);
		}
		
		public override bool canPassWalls() {
			return true;
		}
		
		public override bool canPassBombs() {
			return true;
		}
		
		public override void die() {
			if(animation != death) {
				animation = death;
				parent.notifyChildDeath();
				if(child != null)
					child.notifyParentDeath();
			}
			if (animation.finalFrame())
				remove();
		}
		
		public void notifyChildDeath() {
			die();
			parent.notifyChildDeath();
		}
		
		public void notifyParentDeath() {
			die();
			if(child != null)
				child.notifyParentDeath();
		}
	}
}

