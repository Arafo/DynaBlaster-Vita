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
		public Sprite cu, bg, dy, bl, backgroundColor;
	
		private static String cursor = "/Application/res/image/flecharoja.png";
		private static String background = "/Application/res/image/bg1.png";
		private static String dyna = "/Application/res/image/dyna.png";
		private static String blaster = "/Application/res/image/blaster.png";
		private static String[] options = { "Game start", "Credits", "Setup", "Password" };
		private static String push = "Push fire button !";
		private static String copyright = "COPYRIGHT 2015.2015";
		private static String company = "VidejuegosG5 SA";
		private static SpriteLoader sl;
		public MP3Player sound;
	
		public TitleMenu(GraphicsContext g, Vector2 scales) : base(g, scales, new Vector4(73, 102, 192, 255)) {
			font1 = new Font(new Vector4(255, 255, 255, 255), true, scales, g);
			font2 = new Font(new Vector4(255, 255, 0, 255), true, scales, g);
			
			sl = new SpriteLoader();
			sl.cargarImagen(cursor);
			sl.setImage(sl.getImage().Resize(new ImageSize((int)(sl.getImage().Size.Width*scales.X), (int)(sl.getImage().Size.Height*scales.Y))));
			cu = sl.ImageToSprite(g);
			
			sl.cargarImagen(background);
			sl.setImage(sl.getImage().Resize(new ImageSize((int)(sl.getImage().Size.Width*scales.X), (int)(sl.getImage().Size.Height*scales.Y))));
			bg = sl.ImageToSprite(g);
			
			sl.cargarImagen(dyna);
			sl.setImage(sl.getImage().Resize(new ImageSize((int)(sl.getImage().Size.Width*scales.X), (int)(sl.getImage().Size.Height*scales.Y))));
			dy = sl.ImageToSprite(g);
				
			sl.cargarImagen(blaster);
			sl.setImage(sl.getImage().Resize(new ImageSize((int)(sl.getImage().Size.Width*scales.X), (int)(sl.getImage().Size.Height*scales.Y))));
			bl = sl.ImageToSprite(g);	
			
			backgroundColor = Textures.CreateTexture(g.Screen.Width, g.Screen.Height, g, bgColor);
			bg.Position.X = graphics.Screen.Width / 2 - bg.Width / 2;
			bg.Position.Y = graphics.Screen.Height;
			
			this.sound = new MP3Player("/Application/res/sound/title.mp3");
			sound.play();
	
		}
	
		public override void tick(InputHandler input) {
			// Transicion vertical
			if (graphics.Screen.Height - (bg.Position.Y + bg.Height) <= 0) {
				ybg = (int)(ybg + 2);
				if (input.fire.clicked) {
					//ybg = (int)(graphics.Screen.Height - bg.Height - 2);
					bg.Position.Y = graphics.Screen.Height - bg.Height - 2;
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
						sound.close();
						//MP3Player.title.stop();
						AppMain.startLevel(1, 5);
						//AppMain.setMenu(new LevelMenu(1, graphics, scale));
					}
					if (selected == 1) {
						sound.close();
						//MP3Player.title.stop();
						AppMain.setMenu(new CreditsMenu(graphics, scales));
					}
					if (selected == 2) {
						AppMain.setMenu(new SetupMenu(this, graphics, scales));
					}
					if (selected == 3) {
						AppMain.setMenu(new PasswordMenu(this, graphics, scales));
					}
				}
	
			}
		}
	
		public override void Render() {			
			// Color de fondo
			backgroundColor.Position.X = 0;
			backgroundColor.Position.Y = 0;
			backgroundColor.Render();

			
		    // Transicion vertical
			if (graphics.Screen.Height - (bg.Position.Y + bg.Height) <= 0) {
				bg.Position.X = graphics.Screen.Width / 2 - bg.Width / 2;
				bg.Position.Y = graphics.Screen.Height - ybg;
				bg.Render();
				return;
			}

			// Fondo
			bg.Position.X = graphics.Screen.Width / 2 - bg.Width / 2;
			bg.Position.Y = graphics.Screen.Height - bg.Height - 2;
			bg.Render();
			
			// Dyna
			dy.Position.X = graphics.Screen.Width / 2 - bg.Width /2 + 25 * scales.X;
			dy.Position.Y = 15 * scales.Y;
			dy.Render();
			
			// Blaster
			bl.Position.X = graphics.Screen.Width / 2 - bg.Width /2 + 80 * scales.X;
			bl.Position.Y = 45 * scales.Y;
			bl.Render();
	
			for (int i = 0; i < options.Length; i++) {
				String msg = options[i]; 
				if (i == selected) {
					cu.Position.X = graphics.Screen.Width / 2 - ((options[0].ToString().Length / 2) * font1.getTilesize() * scales.X + cu.Width + 5);
					cu.Position.Y = graphics.Screen.Height / 2 + (font1.getTilesize() * i) * 3/2 *  scales.Y + font1.getTilesize() * scales.Y * 33 / 10;
					cu.Render();
					font1.render(msg,
							(int)(graphics.Screen.Width / 2 - (options[0].ToString().Length / 2) * font1.getTilesize() * scales.X), 
							(int)(graphics.Screen.Height / 2 + (font1.getTilesize() * i) * 3/2 * scales.Y + font1.getTilesize() * scales.Y * 33/10));
				} else {
					font1.render(msg, 
							(int)(graphics.Screen.Width / 2 - (options[0].ToString().Length / 2) * font1.getTilesize() * scales.X), 
							(int)(graphics.Screen.Height / 2 + (font1.getTilesize() * i) * 3/2 * scales.Y + font1.getTilesize() * scales.Y * 33/10));
				}
			}
			
			font2.render(push, 
					(int)(graphics.Screen.Width / 2 - (push.Length / 2) * font1.getTilesize() * scales.X), 
					(int)(graphics.Screen.Height / 2 - font2.getTilesize() * scales.Y));
			font2.render(copyright, 
					(int)(graphics.Screen.Width / 2 - (copyright.Length / 2) * font1.getTilesize() * scales.X),
					(int)(graphics.Screen.Height - font2.getTilesize() * 7/2 * scales.Y));
			font2.render(company, 
					(int)(graphics.Screen.Width / 2 - (company.Length / 2) * font1.getTilesize() * scales.X),
					(int)(graphics.Screen.Height - font2.getTilesize() * 2 * scales.Y));
		}
		
		public override void Resize(Vector2 scales) {
			base.scales = scales;
			sl.cargarImagen(cursor);
			sl.setImage(sl.getImage().Resize(new ImageSize((int)(sl.getImage().Size.Width*scales.X), (int)(sl.getImage().Size.Height*scales.Y))));
			cu = sl.ImageToSprite(graphics);		
			
			sl.cargarImagen(background);
			sl.setImage(sl.getImage().Resize(new ImageSize((int)(sl.getImage().Size.Width*scales.X), (int)(sl.getImage().Size.Height*scales.Y))));
			bg = sl.ImageToSprite(graphics);
			
			sl.cargarImagen(dyna);
			sl.setImage(sl.getImage().Resize(new ImageSize((int)(sl.getImage().Size.Width*scales.X), (int)(sl.getImage().Size.Height*scales.Y))));
			dy = sl.ImageToSprite(graphics);
				
			sl.cargarImagen(blaster);
			sl.setImage(sl.getImage().Resize(new ImageSize((int)(sl.getImage().Size.Width*scales.X), (int)(sl.getImage().Size.Height*scales.Y))));
			bl = sl.ImageToSprite(graphics);	
			
			font1 = new Font(new Vector4(255, 255, 255, 255), true, scales, graphics);
			font2 = new Font(new Vector4(255, 255, 0, 255), true, scales, graphics);
		}
	}
}

