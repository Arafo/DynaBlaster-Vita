using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;
using Sce.PlayStation.Core.Input;

namespace DynaBlasterVita
{
	public class TitleMenu : Menu
	{

		private int selected = 0;
		private int ybg;
		private Font font1, font2;
		private Sprite cu, bg, dy, bl;
	
		private static String cursor = "/Application/res/image/flecharoja.png";
		private static String background = "/Application/res/image/bg1.png";
		private static String dyna = "/Application/res/image/dyna.png";
		private static String blaster = "/Application/res/image/blaster.png";
		private static String[] options = { "Game start", "Credits", "Setup", "Password" };
		private static String push = "Push fire button !";
		private static String copyright = "COPYRIGHT 2015.2015";
		private static String company = "VidejuegosG5 SA";
		private static SpriteLoader sl;
	
		public TitleMenu(GraphicsContext g, int scale) : base(g, scale) {
			font1 = new Font(new Vector4(255, 255, 255, 255), true, scale, g);
			font2 = new Font(new Vector4(255, 255, 0, 255), true, scale, g);
			
			sl = new SpriteLoader();
			sl.cargarImagen(cursor);
			sl.setImage(sl.getImage().Resize(new ImageSize(sl.getImage().Size.Width*scale, sl.getImage().Size.Height*scale)));
			cu = sl.ImageToSprite(g);		
			
			sl.cargarImagen(background);
			sl.setImage(sl.getImage().Resize(new ImageSize(sl.getImage().Size.Width*scale, sl.getImage().Size.Height*scale)));
			bg = sl.ImageToSprite(g);
			
			sl.cargarImagen(dyna);
			sl.setImage(sl.getImage().Resize(new ImageSize(sl.getImage().Size.Width*scale, sl.getImage().Size.Height*scale)));
			dy = sl.ImageToSprite(g);
				
			sl.cargarImagen(blaster);
			sl.setImage(sl.getImage().Resize(new ImageSize(sl.getImage().Size.Width*scale, sl.getImage().Size.Height*scale)));
			bl = sl.ImageToSprite(g);				
			
			//MP3Player.title.play();
	
		}
	
		public override void tick(InputHandler input) {
			// Transicion vertical
			if (graphics.Screen.Height - ybg >= 87 * scale) {
				ybg = ybg + 1 * scale;
				if (input.fire.clicked) {
					ybg = graphics.Screen.Height - 87*scale;
				}
			}
			else {
				
				if(input.up.clicked)
					selected--;
				if(input.down.clicked)
					selected++;
				
				int len = options.Length;
				if (selected < 0)
					selected += len;
				if (selected >= len)
					selected -= len;
	
				if (input.fire.clicked) {

					if (selected == 0) {
						//MP3Player.title.stop();
						//game.startLevel(1, 1);
					}
					if (selected == 1) {
						//MP3Player.title.stop();
						//game.setMenu(new CreditsMenu());
					}
					if (selected == 2) {
						//game.setMenu(new SetupMenu(this));
					}
					if (selected == 3) {

						AppMain.setMenu(null);
					}

						//game.setMenu(new PasswordMenu(this));
				}
	
			}
		}
	
		public override void Render() {			
			// Color de fondo
			graphics.SetClearColor(bgColor);
		    // Transicion vertical
			if (graphics.Screen.Height - ybg >= 87*scale) {
				bg.Position.X = graphics.Screen.Width / 2 - bg.Width / 2;
				bg.Position.Y = graphics.Screen.Height - ybg;
				bg.Render();
				return;
			}
			
			// Fondo
			bg.Position.X = graphics.Screen.Width / 2 - bg.Width / 2;
			bg.Position.Y = graphics.Screen.Height - 185*scale;
			bg.Render();
			
			// Dyna
			dy.Position.X = graphics.Screen.Width / 2 - bg.Width /2 + 25 * scale;
			dy.Position.Y = 15 * scale;
			dy.Render();
			
			// Blaster
			bl.Position.X = graphics.Screen.Width / 2 - bg.Width /2 + 80 * scale;
			bl.Position.Y = 45 * scale;
			bl.Render();
	
			for (int i = 0; i < options.Length; i++) {
				String msg = options[i]; 
				if (i == selected) {
					cu.Position.X = graphics.Screen.Width / 2 - ((options[0].ToString().Length / 2) * font1.getTilesize() * scale + cu.Width + 5);
					cu.Position.Y = graphics.Screen.Height / 2 + (font1.getTilesize() * i) * 3/2 *  scale + font1.getTilesize() * scale * 33 / 10;
					cu.Render();
					font1.render(msg,
							graphics.Screen.Width / 2 - (options[0].ToString().Length / 2) * font1.getTilesize() * scale, 
							graphics.Screen.Height / 2 + (font1.getTilesize() * i) * 3/2 * scale + font1.getTilesize() * scale * 33/10);
				} else {
					font1.render(msg, 
							graphics.Screen.Width / 2 - (options[0].ToString().Length / 2) * font1.getTilesize() * scale, 
							graphics.Screen.Height / 2 + (font1.getTilesize() * i) * 3/2 * scale + font1.getTilesize() * scale * 33/10);
				}
			}
			
			font2.render(push, 
					graphics.Screen.Width / 2 - (push.Length / 2) * font1.getTilesize() * scale, 
					graphics.Screen.Height / 2 - font2.getTilesize() * scale);
			font2.render(copyright, 
					graphics.Screen.Width / 2 - (copyright.Length / 2) * font1.getTilesize() * scale,
					graphics.Screen.Height - font2.getTilesize() * 7/2 * scale);
			font2.render(company, 
					graphics.Screen.Width / 2 - (company.Length / 2) * font1.getTilesize() * scale,
					graphics.Screen.Height - font2.getTilesize() * 2 * scale);
		}
	}
}

