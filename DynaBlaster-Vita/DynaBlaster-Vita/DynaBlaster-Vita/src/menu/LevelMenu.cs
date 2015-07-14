using System;
using System.Diagnostics;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;

namespace DynaBlasterVita
{
	public class LevelMenu : Menu
	{
		private static  String sprites = "/Application/res/image/level_menu.png";
		
		private SpriteLoader sl;
		private SpriteSheet ss;
		private Sprite bg, round, level;
		private Animation head, roundFlicker, levelFlicker;
		private float x, y;
		private int lev;
		private Stopwatch clock;
		private MP3Player sound;

		public LevelMenu (int level, GraphicsContext g, int scale) : base(g, scale) {
			this.lev = level;
			
			this.sl = new SpriteLoader();			
			this.sl.cargarImagen(sprites);
			this.sl.setImage(sl.getImage().Resize(new ImageSize(sl.getImage().Size.Width*scale, sl.getImage().Size.Height*scale)));
			this.ss = new SpriteSheet(this.sl.getImage());
			
			this.bg = ss.obtenerSprite(0, 0, 256*scale, 160*scale, g);
			this.round = ss.obtenerSprite(76*scale, 161*scale, 49*scale, 14*scale, g);
			this.level = ss.obtenerSprite(12*(level-1)*scale + 134*scale, 160*scale, 12*scale, 14*scale, g);
			
			// TODO Arreglar parpadeo de ronda y nivel
			Sprite levelaux = ss.obtenerSprite(12*(level-1)*scale + 134*scale, 160*scale, 12*scale, 14*scale, g);
			levelaux.SetColor(new Vector4(0, 0, 0, 255));
			Sprite roundaux = ss.obtenerSprite(76*scale, 161*scale, 49*scale, 14*scale, g);
			roundaux.SetColor(new Vector4(0, 0, 0, 255));
			
			Sprite[] roundFlicker = {this.round, roundaux};
			Sprite[] levelFlicker = {this.level, levelaux};
			Sprite[] head = {ss.obtenerSprite(1, 160*scale, 23*scale, 23*scale, g), 
					ss.obtenerSprite(23*scale, 160*scale, 24*scale, 23*scale, g), 
					ss.obtenerSprite(48*scale, 160*scale, 24*scale, 23*scale, g)};
			
			this.head = new Animation(head, 12);
			this.roundFlicker = new Animation(roundFlicker, 5);
			this.levelFlicker = new Animation(levelFlicker, 5);
			
			switch (level) {
			case 1:
				this.x = graphics.Screen.Width/2 - this.bg.Width/2 + 18*scale;
				this.y = 134*scale;
				break;
			case 2:
				this.x = graphics.Screen.Width/2 - this.bg.Width/2 + 48*scale;
				this.y = 112*scale;
				break;
			case 3:
				this.x = graphics.Screen.Width/2 - this.bg.Width/2 + 66*scale;
				this.y = 168*scale;
				break;
			case 4:
				this.x = graphics.Screen.Width/2 - this.bg.Width/2 + 104*scale;
				this.y = 128*scale;
				break;
			case 5:
				this.x = graphics.Screen.Width/2 - this.bg.Width/2 + 154*scale;
				this.y = 120*scale;
				break;
			case 6:
				this.x = graphics.Screen.Width/2 - this.bg.Width/2 + 210*scale;
				this.y = 138*scale;
				break;
			case 7:
				this.x = graphics.Screen.Width/2 - this.bg.Width/2 + 210*scale;
				this.y = 104*scale;
				break;
			case 8:
				this.x = graphics.Screen.Width/2 - this.bg.Width/2 + 210*scale;
				this.y = 62*scale;
				break;
			}
			this.head.start();
			this.roundFlicker.start();
			this.levelFlicker.start();
			
			//MP3Player.level_start.play();
			this.sound = new MP3Player("/Application/res/sound/level_start.mp3");
			this.sound.play();
			
			this.clock = new Stopwatch();
			this.clock.Start();
		}
		
		public override void tick(InputHandler input) {
			if (this.clock.ElapsedMilliseconds > 5*1000 || input.fire.clicked) { // 5 segundos
				head.stop();
				roundFlicker.stop();
				levelFlicker.stop();
				this.clock.Stop();
				this.sound.close();
				//MP3Player.level_start.stop();
				//game.setMenu(new MapMenu(lev, 1));
				AppMain.setMenu(new MapMenu(lev, 1, graphics, scale));
			}
			head.tick();
			roundFlicker.tick();
			levelFlicker.tick();
		}
		
		public override void Render() {
		    
			this.bg.Position.X = graphics.Screen.Width/2 - this.bg.Width/2;
			this.bg.Position.Y = graphics.Screen.Height/2 - this.bg.Height/2;
			this.bg.Render();
			
			this.roundFlicker.getSprite().Position.X =  graphics.Screen.Width/2 - round.Width/2 - 12*scale;
		    this.roundFlicker.getSprite().Position.Y = graphics.Screen.Height - round.Height*2;
			this.roundFlicker.getSprite().Render();
			
			this.levelFlicker.getSprite().Position.X = graphics.Screen.Width/2 + round.Width/2;
			this.levelFlicker.getSprite().Position.Y = graphics.Screen.Height - round.Height*2;
			this.levelFlicker.getSprite().Render();
			
			this.head.getSprite().Position.X = x;
			this.head.getSprite().Position.Y = y;
			this.head.getSprite().Render();
		    
		}
	}
}

