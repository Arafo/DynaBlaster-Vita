using System;
using System.Diagnostics;

using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;

namespace DynaBlasterVita
{
	public class MapMenu : Menu
	{
		private static String sprites = "/Application/res/image/map_menu.png";
		
		private SpriteLoader sl;
		private SpriteSheet ss;
		private Sprite stage, game_start, level, map;
		private Stopwatch clock;
		private MP3Player sound;
		
		public MapMenu (int level, int map, GraphicsContext g, int scale) : base(g, scale) {	
						
			this.sl = new SpriteLoader();			
			this.sl.cargarImagen(sprites);
			this.sl.setImage(sl.getImage().Resize(new ImageSize(sl.getImage().Size.Width*scale, sl.getImage().Size.Height*scale)));
			this.ss = new SpriteSheet(this.sl.getImage());
			
			this.stage = ss.obtenerSprite(0, 0, 79*scale, 13*scale, g);
			this.game_start = ss.obtenerSprite(0, 14*scale, 107*scale, 13*scale, g);
			this.level = ss.obtenerSprite(8*level*scale, 28*scale, 6*scale, 13*scale, g);
			this.map = ss.obtenerSprite(8*map*scale, 28*scale, 6*scale, 13*scale, g);	
			
			sound = new MP3Player("/Application/res/sound/map_start.mp3");
			sound.play();
			//MP3Player.map_start.play();
			this.clock = new Stopwatch();
			this.clock.Start();
		}
		
		public override void tick(InputHandler input) {
			if (this.clock.ElapsedMilliseconds> 3*1000 || input.fire.clicked) { // 3 segundos
				this.clock.Stop();
				AppMain.setMenu(null);
				sound.close();
				//MP3Player.map_start.stop();
			}
		}
		
		public override void Render() {
			
			this.stage.Position.X = graphics.Screen.Width/2 - this.stage.Width/2 - 3*scale;
			this.stage.Position.Y = graphics.Screen.Height/2 - this.stage.Height - 4*scale;
			this.stage.Render();
			
		    this.level.Position.X = graphics.Screen.Width/2 + this.stage.Width/2 - 18*scale;
			this.level.Position.Y = graphics.Screen.Height/2 - this.stage.Height - 4*scale;
			this.level.Render();
		    
			this.map.Position.X = graphics.Screen.Width/2 + this.stage.Width/2;
			this.map.Position.Y = graphics.Screen.Height/2 - this.stage.Height - 4*scale;
			this.map.Render();
			
			this.game_start.Position.X = graphics.Screen.Width/2 - this.game_start.Width/2;
			this.game_start.Position.Y = graphics.Screen.Height/2;
			this.game_start.Render(); 
		 
		}
	}
}

