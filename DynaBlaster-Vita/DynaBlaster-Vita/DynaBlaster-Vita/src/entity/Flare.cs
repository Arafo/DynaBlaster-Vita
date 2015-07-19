using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;

namespace DynaBlasterVita
{
	public class Flare : Entity
	{
		public String topflare = "/Application/res/image/topflare.png"; 
		public String botflare = "/Application/res/image/botflare.png"; 
		public String rightflare = "/Application/res/image/rightflare.png"; 
		public String leftflare = "/Application/res/image/leftflare.png"; 
		public String verticalflare = "/Application/res/image/verticalflare.png"; 
		public String horizontalflare = "/Application/res/image/horizontalflare.png"; 
		public String midflare = "/Application/res/image/midflare.png"; 
		private static int w = 16;
		private static int h = 16;
		
		private HashSet<Entity> flaresOut = new HashSet<Entity>();
		
		private String ANIMATION;
		private String FINALANIMATION;
		
		private bool draw;
		
		private GraphicsContext g;
		private Sprite[] anim;
	
		public Flare(Bomb bomb, int xdif, int ydif, GraphicsContext g, Vector2 scale) : base(scale) {
			//super(obs, map, player);
			this.g = g;
			this.position.X = -(w*scale.X)/2 + ((int)Math.Round(((float)(bomb.position.X+w*scale.X/2)/(w*scale.X)))+xdif)*(w*scale.X);
			this.position.Y = -(h*scale.Y)/2 + ((int)Math.Round(((float)(bomb.position.Y+h*scale.X/2)/(h*scale.Y)))+ydif)*(h*scale.Y);
	
			this.position.Width = 12*scale.X;
			this.position.Height = 14*scale.Y;
			
			this.sl = new SpriteLoader();	
			
			this.draw = true;
			
			
			if(xdif == 0 && ydif == 0) {
				ANIMATION = midflare;
			}
			else if(xdif == 0) {
				if(ydif > 0)
					FINALANIMATION = botflare;
				else
					FINALANIMATION = topflare;
				if(Math.Abs(ydif) == bomb.getPotency()) {
					if(ydif > 0)
						ANIMATION = botflare;
					else
						ANIMATION = topflare;
				}
				else
					ANIMATION = verticalflare;
			}
			else {
				if(xdif > 0)
					FINALANIMATION = rightflare;
				else
					FINALANIMATION = leftflare;
				if(Math.Abs(xdif) == bomb.getPotency()) {
					if(xdif > 0)
						ANIMATION = rightflare;
					else
						ANIMATION = leftflare;
				}
				else
					ANIMATION = horizontalflare;
			}
			
			this.sl = new SpriteLoader();			
			this.sl.cargarImagen(ANIMATION);
			this.sl.setImage(sl.getImage().Resize(new ImageSize((int)(sl.getImage().Size.Width*scale.X), 
			                                                    (int)(sl.getImage().Size.Height*scale.Y))));
			this.ss = new SpriteSheet(this.sl.getImage());
			
			Sprite[] anim;
			if(ANIMATION == midflare) {
				this.anim = new Sprite[]{ss.obtenerSprite(0, 0, (int)(w*scale.X), (int)(h*scale.Y), g), 
						ss.obtenerSprite((int)(0*w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.Y), g), 
						ss.obtenerSprite((int)(1*w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.Y), g),
						ss.obtenerSprite((int)(2*w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.Y), g),
						ss.obtenerSprite((int)(3*w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.Y), g),
						ss.obtenerSprite((int)(4*w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.Y), g)};
			}
			else {
				this.anim = new Sprite[]{ss.obtenerSprite(0, 0, (int)(w*scale.X), (int)(h*scale.Y), g), 
						ss.obtenerSprite((int)(0*w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.Y), g), 
						ss.obtenerSprite((int)(0*w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.Y), g),
						ss.obtenerSprite((int)(1*w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.Y), g),
						ss.obtenerSprite((int)(2*w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.Y), g),
						ss.obtenerSprite((int)(3*w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.Y), g)};
			}
			
			// Animacion inicial
			animation = new Animation(this.anim, 3, Animation.Direction.DOWN);
			animation.start();
	
		}
	
		public override void tick() {
			base.tick();
			
			if (animation.finalFrame()) removed = true;
			//else if (!removed) animation.tick();
		}
	
		public override void Render() {
			if(!removed && draw) {
				Sprite f = animation.getSprite();
				f.Position.X = position.X+position.Width/2 - (f.Width-2*scale.X)/2;
				f.Position.Y = position.Y+position.Height/2 - (f.Height-2*scale.Y)/2;
				f.Render();
			}
	
		}
		
		public void setOut(Entity e) {
			flaresOut.Add(e);
		}
		
		public bool isOut(Entity e) {
			return flaresOut.Contains(e);
		}
		
		public override Rectangle getBounds() {
			if(!removed)
				return new Rectangle(position.X, position.Y, w, h);
			else
				return new Rectangle(0, 0, 0, 0);
		}
		
		public void setAsFinal() {
			draw = false;
			ANIMATION = FINALANIMATION;
			
//			this.ss = new SpriteSheet(this.sl.getImage().Resize(new ImageSize(sl.getImage().Size.Widt(int)(h*scale.Y), sl.getImage().Size.Height*scale)));
//			Sprite[] anim;
//			anim = new Sprite[]{ss.obtenerSprite(0*(int)(w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.Y), g),
//					ss.obtenerSprite(0*(int)(w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.Y), g), 
//					ss.obtenerSprite(0*(int)(w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.Y), g),
//					ss.obtenerSprite(1*(int)(w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.Y), g),
//					ss.obtenerSprite(2*(int)(w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.Y), g),
//					ss.obtenerSprite(3*(int)(w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.Y), g)};
	
			
			// Animacion inicial
			animation = new Animation(this.anim, 3, Animation.Direction.DOWN);
			animation.start();
		}
		
		public bool isFinal() {
			return ANIMATION == FINALANIMATION;
		}
		
		public bool isMid() {
			return ANIMATION == midflare;
		}
	}
}

