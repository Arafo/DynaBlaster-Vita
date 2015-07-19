using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;

namespace DynaBlasterVita
{
	public class SnakeHead : Boss
	{
		private String ANIMATION = "/Application/res/image/blue_snake.png"; 
		private static int w = 16;
		private static int h = 18;
		
		protected int state = 0;
		
		protected Animation[] up, down, left, right;
		
		public LinkedList<PosDir> positions = new LinkedList<PosDir>();
		protected SnakeBody child;
		
		protected int invincible = 0;
		
		public class PosDir {
			public PosDir(Rectangle position, int dirx, int diry) {
				this.position = new Rectangle(position.X, position.Y, position.Width, position.Height);
				this.dirx = dirx;
				this.diry = diry;
			}
			public Rectangle position;
			public int dirx;
			public int diry;
		}
	
		public SnakeHead(GenerateObstacles obs, Map map, Bomberman player, GraphicsContext g, Vector2 scale) :
		base(obs, map, player, g, scale) {
	
			this.position.Width = 12*scale.X;
			this.position.Height = 14*scale.Y;
			this.health = 80;
			this.score = 100;
			
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
				Sprite[] u = {ss.obtenerSprite((int)(0*w*scale.X), (int)((2*i+1)*h*scale.Y+(i+1)*scale.Y), (int)(w*scale.X), (int)(h*scale.X), g), 
						ss.obtenerSprite((int)(1*w*scale.X), (int)((2*i+1)*h*scale.Y+(i+1)*scale.Y), (int)(w*scale.X), (int)(h*scale.X), g)};
				
				Sprite[] d = {ss.obtenerSprite((int)(6*w*scale.X), (int)((2*i+1)*h*scale.Y+(i+1)*scale.Y), (int)(w*scale.X), (int)(h*scale.X), g), 
						ss.obtenerSprite((int)(7*w*scale.X), (int)((2*i+1)*h*scale.Y+(i+1)*scale.Y), (int)(w*scale.X), (int)(h*scale.X), g)};
				
				Sprite[] l = {ss.obtenerSprite((int)(8*w*scale.X), (int)((2*i+1)*h*scale.Y+(i+1)*scale.Y), (int)(w*scale.X), (int)(h*scale.X), g), 
						ss.obtenerSprite((int)(9*w*scale.X), (int)((2*i+1)*h*scale.Y+(i+1)*scale.Y), (int)(w*scale.X), (int)(h*scale.X), g)};
				
				Sprite[] r = {ss.obtenerSprite((int)(2*w*scale.X), (int)((2*i+1)*h*scale.Y+(i+1)*scale.Y), (int)(w*scale.X), (int)(h*scale.X), g), 
						ss.obtenerSprite((int)(3*w*scale.X), (int)((2*i+1)*h*scale.Y+(i+1)*scale.Y), (int)(w*scale.X), (int)(h*scale.X), g)};
				
				up[i] = new Animation(u, 20, Animation.Direction.UP);
				down[i] = new Animation(d, 20, Animation.Direction.DOWN);
				left[i] = new Animation(l, 20, Animation.Direction.LEFT);
				right[i] = new Animation(r, 20, Animation.Direction.RIGHT);
			}
			
			Sprite[] die = {ss.obtenerSprite((int)(0*w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.X), g),
					ss.obtenerSprite((int)(1*w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.X), g),
					ss.obtenerSprite((int)(2*w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.X), g),
					ss.obtenerSprite((int)(3*w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.X), g),
					ss.obtenerSprite((int)(4*w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.X), g),
					ss.obtenerSprite((int)(5*w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.X), g),
					ss.obtenerSprite((int)(6*w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.X), g),
					ss.obtenerSprite((int)(7*w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.X), g),
					new Score(score, w, h, g, scale).getImage()};
			
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
			if(invincible > 0) 
				invincible--;
			if(health <= 20) 
				state = 3;
			else if(health <= 40)
				state = 2;
			else if(health <= 60)
				state = 1;
			else
				state = 0;
			base.tick();
			if(animation != death) {
				if(xdir < 0)
					animation = left[state];
				else if(xdir > 0)
					animation = right[state];
				else if (ydir < 0)
					animation = up[state];
				else
					animation = down[state];
			}
			
			positions.AddLast(new PosDir(position,xdir,ydir));
			
			animation.start();
		}
	
//		public void Render() {
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
		
		public override bool isInvincible() {
			return invincible > 0;
		}
	
		public void setChild(SnakeBody child) {
			this.child = child;
		}
		
		public override void doHurt(int damage) {
			if(!isInvincible()) {
				base.doHurt(damage);
				invincible = 60;
			}
		}
		
		public override void die() {
			if(animation != death) {
				move = false;
				animation = death;
				child.notifyParentDeath();
			}
			if (animation.finalFrame())
				remove();
		}
		
		public void notifyChildDeath() {
			die();
		}
	}
}

