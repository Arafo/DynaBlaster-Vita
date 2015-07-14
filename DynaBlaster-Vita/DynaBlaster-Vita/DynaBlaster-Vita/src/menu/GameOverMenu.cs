using System;
using System.IO;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;

namespace DynaBlasterVita
{
	public class GameOverMenu : Menu
	{
		private static String sprites = "/Application/res/image/gameover_menu.png";
		private static String cursor = "/Application/res/image/flecharoja.png";
		
		private String[] options = { "Continue", "End"};
		private int selected = 0;
		private SpriteLoader sl;
		private SpriteSheet ss;
		private Sprite bg, title, bi, cu;
		private Font font, passfont;
		private String password;
		private int lives, level, map;
		
		public GameOverMenu(int lives, int level, int map, GraphicsContext g, int scale) : base(g, scale) {
			this.lives = lives;
			this.level = level;
			this.map = map;
			this.options[0] += " " + lives;
			this.font = new Font(new Vector4(255, 255, 255, 255), true, scale, g);
	
			// Ultimo continue
			if (lives == -1) {
				options[0] = "Continue last";
				this.font = new Font(new Vector4(255, 0, 0, 255), true, scale, g);
			}
			
			// No hay mas continues
			if (lives == -2) {
				options = new String[]{"End"};
			}
			
			// Buscar password
			using (StreamReader sr = new StreamReader(File.OpenRead("/Application/res/maps/definitions/passwords.txt"))) 
			{	
			for (int i=0; i<8;i++) {
				for (int j=0; j<8; j++) {
						if (level == i+1 && map == j+1)	{
							string line = Convert.ToString(sr.ReadLine());
							this.password = line.Substring(line.Length - 8);
							goto found;
						}
						//else 
							//sr.Read();
					}
				}
			}
			
			found: this.sl = new SpriteLoader();			
			this.sl.cargarImagen(sprites);
			this.sl.setImage(sl.getImage().Resize(new ImageSize(sl.getImage().Size.Width*scale, sl.getImage().Size.Height*scale)));
			this.ss = new SpriteSheet(this.sl.getImage());
			
			this.bg = ss.obtenerSprite(0, 0, 193*scale, 65*scale, g);
			this.title = ss.obtenerSprite(0, 65*scale, 156*scale, 27*scale, g);
			
			this.sl.cargarImagen(cursor);
			this.sl.setImage(sl.getImage().Resize(new ImageSize(sl.getImage().Size.Width*scale, sl.getImage().Size.Height*scale)));
			this.ss = new SpriteSheet(this.sl.getImage());
			
			this.cu =  this.sl.ImageToSprite(g);
			
			this.passfont = new Font(new Vector4(255, 255, 255, 255), true, scale, g);
			//MP3Player.no_continue.play();
		}
		
		public override void tick(InputHandler input) {
			if (input.up.clicked) selected--;
			if (input.down.clicked) selected++;
	
			int len = options.Length;
			if (selected < 0) selected += len;
			if (selected >= len) selected -= len;
	
			if (input.fire.clicked) {
				//MP3Player.no_continue.stop();
				if (selected == 0  && lives > -2) AppMain.setMenu(new MapMenu(level, map, graphics, scale));
				else AppMain.setMenu(new TitleMenu(graphics, scale));
				if (selected == 1) AppMain.setMenu(new TitleMenu(graphics, scale));
					
			}
		}
		
		public override void Render() {
			graphics.SetClearColor(new Vector4(0, 97, 146, 255));
		    //g.fillRect(0, 25*scale, game.getWidth(), game.getHeight() - 45*scale);
			
			this.bg.Position.X = graphics.Screen.Width/2 - this.bg.Width/2;
			this.bg.Position.Y = graphics.Screen.Height/2;
			this.bg.Render();
			
			this.title.Position.X = graphics.Screen.Width/2 - this.title.Width/2;
			this.title.Position.Y = graphics.Screen.Height/2 - 74*scale;
			this.title.Render();
		    
		    for (int i = 0; i < options.Length; i++) {
				String msg = options[i]; 
				if (i == selected) {
					this.cu.Position.X = graphics.Screen.Width/2 - ((options[0].ToString().Length/2)*font.getTilesize()*scale + cu.Width + 5);
					this.cu.Position.Y = graphics.Screen.Height/2 + (font.getTilesize()*i)*2*scale - font.getTilesize()*scale*7/2;
					this.cu.Render();

					font.render(msg,
							graphics.Screen.Width/2 - (options[0].ToString().Length/2)*font.getTilesize()*scale, 
							graphics.Screen.Height/2 + (font.getTilesize()*i)*2*scale - font.getTilesize()*scale*7/2);
				} else {
					font.render(msg, 
							graphics.Screen.Width/2 - (options[0].ToString().Length/2)*font.getTilesize()*scale, 
							graphics.Screen.Height/2 + (font.getTilesize()*i)*2*scale - font.getTilesize()*scale*7/2);
				}
			}
		    
		    passfont.render(password,
		    		graphics.Screen.Width/2 - (password.Length/2)*font.getTilesize()*scale - 9*scale,
		    		graphics.Screen.Height/2 + 39*scale);
			
		}
	}
}

