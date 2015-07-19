using System;
using System.Diagnostics;

using Sce.PlayStation.Core;
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
		
		public MapMenu (int level, int map, GraphicsContext g, Vector2 scales) : base(g, scales, new Vector4(0, 0, 0, 255)) {	
						
			this.sl = new SpriteLoader();			
			this.sl.cargarImagen(sprites);
			this.sl.setImage(sl.getImage().Resize(new ImageSize((int)(sl.getImage().Size.Width*scales.X),
			                                                    (int)(sl.getImage().Size.Height*scales.Y))));
			this.ss = new SpriteSheet(this.sl.getImage());
			
			this.stage = ss.obtenerSprite(0, 0, (int)(79*scales.X), (int)(13*scales.Y), g);
			this.game_start = ss.obtenerSprite(0, (int)(14*scales.Y), (int)(107*scales.X), (int)(13*scales.Y), g);
			this.level = ss.obtenerSprite((int)(8*level*scales.X), (int)(28*scales.Y), (int)(6*scales.X), (int)(13*scales.Y), g);
			this.map = ss.obtenerSprite((int)(8*map*scales.X), (int)(28*scales.Y), (int)(6*scales.X), (int)(13*scales.Y), g);	
			
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
			
			this.stage.Position.X = (int)(graphics.Screen.Width/2 - this.stage.Width/2 - 3*scales.X);
			this.stage.Position.Y = (int)(graphics.Screen.Height/2 - this.stage.Height - 4*scales.Y);
			this.stage.Render();
			
		    this.level.Position.X = (int)(graphics.Screen.Width/2 + this.stage.Width/2 - 18*scales.X);
			this.level.Position.Y = (int)(graphics.Screen.Height/2 - this.stage.Height - 4*scales.Y);
			this.level.Render();
		    
			this.map.Position.X = graphics.Screen.Width/2 + this.stage.Width/2;
			this.map.Position.Y = (int)(graphics.Screen.Height/2 - this.stage.Height - 4*scales.Y);
			this.map.Render();
			
			this.game_start.Position.X = graphics.Screen.Width/2 - this.game_start.Width/2;
			this.game_start.Position.Y = graphics.Screen.Height/2;
			this.game_start.Render(); 
		 
		}
	}
}

