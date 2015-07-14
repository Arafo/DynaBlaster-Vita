using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;
using Sce.PlayStation.Core.Input;

namespace DynaBlasterVita
{
	public class SetupMenu : Menu
	{

		private int selected = 0;
		private int selectedSound = 0;
		private int selectedZoom = 0;
		private int selectedCheats = 0;
	    private int selectedDebug = 0;
		private Font font1, font2;
		private Sprite cu, bg, dy, bl;
	
		private static String cursor = "/Application/res/image/flecharoja.png";
		private static String background = "/Application/res/image/bg1.png";
		private static String dyna = "/Application/res/image/dyna.png";
		private static String blaster = "/Application/res/image/blaster.png";
		private static String[] options = { "Sound", "Zoom", "Cheats", "Debug Info", "Exit" };
		private static String[] onoff = { "On", "Off" };
		private static String[] zoom = { "x1", "x2", "Streched" };
		private static String push = "Push fire button !";
		private static String copyright = "COPYRIGHT 2015.2015";
		private static String company = "VidejuegosG5 SA";
		private static SpriteLoader sl;
		private Menu menu;
	
		public SetupMenu(Menu menu, GraphicsContext g, int scale) : base(g, scale) {
			this.menu = menu;
			
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
			if (input.up.clicked) selected--;
			if (input.down.clicked) selected++;
	
			int len = options.Length;
			if (selected < 0) selected += len;
			if (selected >= len) selected -= len;
			
			// Sound
			if (selected == 0) {
				if (input.left.clicked) selectedSound--;
				if (input.right.clicked) selectedSound++;
				int lenSound = onoff.Length;
				if (selectedSound < 0) selectedSound += lenSound;
				if (selectedSound >= lenSound) selectedSound -= lenSound;
			}
			
			// Zoom
			if (selected == 1) {
				if (input.left.clicked) selectedZoom--;
				if (input.right.clicked) selectedZoom++;
				int lenZoom = zoom.Length;
				if (selectedZoom < 0) selectedZoom += lenZoom;
				if (selectedZoom >= lenZoom) selectedZoom -= lenZoom;
			}
			
			// Cheats
			if (selected == 2) {
				if (input.left.clicked) selectedCheats--;
				if (input.right.clicked) selectedCheats++;
				int lenCheats = onoff.Length;
				if (selectedCheats < 0) selectedCheats += lenCheats;
				if (selectedCheats >= lenCheats) selectedCheats -= lenCheats;
			}
			
			// Debug info
			if (selected == 3) {
				if (input.left.clicked) selectedDebug--;
				if (input.right.clicked) selectedDebug++;
				int lenDebug = onoff.Length;
				if (selectedDebug < 0) selectedDebug += lenDebug;
				if (selectedDebug >= lenDebug) selectedDebug -= lenDebug;
			}
	
			if (input.fire.clicked) {
				if (selected == 0) {
				}
				if (selected == 1) {
					if (selectedZoom == 0) AppMain.scale = 2;
					if (selectedZoom == 1) AppMain.scale = 3;
					AppMain.setMenu(new SetupMenu(menu, graphics, AppMain.scale));
				}
				if (selected == 2) {
				}
				if (selected == 3) {
					if (onoff[selectedDebug].Equals("On")) AppMain.debug = true;
					else AppMain.debug = false;
				}
				if (selected == 4) AppMain.setMenu(menu);
			}
		}
	
		public override void Render() {			
			// Color de fondo
			graphics.SetClearColor(bgColor);
			
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
					cu.Position.X = graphics.Screen.Width/2 - ((options[3].ToString().Length/2)*font1.getTilesize()*scale*3/2 + cu.Width + 5);
					cu.Position.Y = graphics.Screen.Height/2 + (font1.getTilesize()*i)*3/2*scale + font1.getTilesize()*scale*33/10;
					cu.Render();
					font1.render(msg,
							graphics.Screen.Width/2 - (options[3].ToString().Length/2) * font1.getTilesize()* scale*3/2, 
							graphics.Screen.Height/2 + (font1.getTilesize()*i)*3/2*scale + font1.getTilesize()*scale*33/10);
				} else {
					font1.render(msg, 
							graphics.Screen.Width/2 - (options[3].ToString().Length/2)*font1.getTilesize()*scale*3/2, 
							graphics.Screen.Height/2 + (font1.getTilesize()*i)*3/2*scale + font1.getTilesize()*scale*33/10);
				}
			}
			// Sound
			font1.render("<" + onoff[selectedSound] + ">", 
				graphics.Screen.Width/2 + 26*scale, 
				graphics.Screen.Height/2 + font1.getTilesize()*scale*33/10);
			
			// Zoom 
			font1.render("<" + zoom[selectedZoom] + ">", 
				graphics.Screen.Width/2 + 26*scale, 
				graphics.Screen.Height/2 + font1.getTilesize()*3/2*scale + font1.getTilesize()*scale*33/10);
			
			// Cheats
			font1.render("<" + onoff[selectedCheats] + ">", 
				graphics.Screen.Width/2 + 26*scale, 
				graphics.Screen.Height/2 + font1.getTilesize()*3*scale + font1.getTilesize()*scale*33/10);
			
			// Debug info
			font1.render("<" + onoff[selectedDebug] + ">", 
				graphics.Screen.Width/2 + 26*scale, 
				graphics.Screen.Height/2 + font1.getTilesize()*9/2*scale + font1.getTilesize()*scale*33/10);
			
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

