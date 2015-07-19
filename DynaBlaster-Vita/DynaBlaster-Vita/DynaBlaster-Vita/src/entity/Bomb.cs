using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;

namespace DynaBlasterVita
{
	public class Bomb : Entity
	{
		private String ANIMATION = "/Application/res/image/bomb.png"; 
		private static int w = 16;
		private static int h = 16;
		
		private HashSet<Entity> bombsOut = new HashSet<Entity>();
		
		private int potency;
		private bool explode;
		
		//private Animation animation;
	
		public Bomb(Bomberman player, GraphicsContext g, Vector2 scale) : base(scale) {
			//super(obs, map, player);
			
			this.position.X = 2 -(w*scale.X)/2 + (int)Math.Round(((float)(player.position.X+(float)w*scale.X/2)/(w*scale.X)))*(w*scale.X);
			this.position.Y = -(h*scale.Y)/2 + (int)Math.Round(((float)(player.position.Y+(float)h*scale.Y/2)/(h*scale.Y)))*(h*scale.Y);
	
			this.position.Width = 12*scale.X;
			this.position.Height = 14*scale.Y;
			
			explode = !player.hasRemoteDetonator();
			
			this.potency = player.getPotency();
			
			this.sl = new SpriteLoader();			
			this.sl.cargarImagen(ANIMATION);
			this.sl.setImage(sl.getImage().Resize(new ImageSize((int)(sl.getImage().Size.Width*scale.X), 
			                                                    (int)(sl.getImage().Size.Height*scale.Y))));
			this.ss = new SpriteSheet(this.sl.getImage());
			
			Sprite[] anim = {ss.obtenerSprite((int)(1*w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.Y), g), 
					ss.obtenerSprite((int)(0*w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.Y), g), 
					ss.obtenerSprite((int)(2*w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.Y), g),
					ss.obtenerSprite((int)(0*w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.Y), g),
					ss.obtenerSprite((int)(1*w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.Y), g),
					ss.obtenerSprite((int)(0*w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.Y), g),
					ss.obtenerSprite((int)(2*w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.Y), g),
					ss.obtenerSprite((int)(0*w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.Y), g),
					ss.obtenerSprite((int)(1*w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.Y), g),
					ss.obtenerSprite((int)(0*w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.Y), g),
					ss.obtenerSprite((int)(2*w*scale.X), 0, (int)(w*scale.X), (int)(h*scale.Y), g)};
			
			// Animacion inicial
			animation = new Animation(anim, 10, Animation.Direction.DOWN);
			animation.start();
	
		}
	
		public override void tick() {
			base.tick();
			//super.tick();
			
			if(explode && animation.finalFrame()) removed = true;
			// TODO VOLVER A PONER
			if (removed) Sounds.bomb.play();
	
			//else if (!removed) animation.tick();
		}
	
		public override void Render() {
			if(!removed) {
				Sprite f = animation.getSprite();
				f.Position.X = position.X + position.Width/2 - (f.Width-2*scale.X)/2;
				f.Position.Y = position.Y + position.Height/2 - (f.Height-2*scale.Y)/2;
				f.Render();
			}
	
		}
		
		public void setOut(Entity e) {
			bombsOut.Add(e);
		}
		
		public bool isOut(Entity e) {
			return bombsOut.Contains(e);
		}
		
		public override Rectangle getBounds() {
			if(!removed)
				return new Rectangle(position.X, position.Y, w, h);
			else
				return new Rectangle(0, 0, 0, 0);
		}
	
		public int getPotency() {
			return potency;
		}
		
		public override void touchedBy(Entity entity) {
			if(entity is Flare) {
				removed = true;
			}
		}
	}
}

