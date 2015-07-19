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
		private Sprite backgroundColor;
	
		private static String cursor = "/Application/res/image/flecharoja.png";
		private static String background = "/Application/res/image/bg1.png";
		private static String dyna = "/Application/res/image/dyna.png";
		private static String blaster = "/Application/res/image/blaster.png";
		private static String[] options = { "Sound", "Zoom", "Cheats", "Debug Info", "Exit" };
		private static String[] onoff = { "On", "Off" };
		private static String[] offon = { "Off", "On" };
		private static String[] zoom = { "Original", "Streched" };
		private static String push = "Push fire button !";
		private static String copyright = "COPYRIGHT 2015.2015";
		private static String company = "VidejuegosG5 SA";
		private static SpriteLoader sl;
		private Menu menu;
	
		public SetupMenu(Menu menu, GraphicsContext g, Vector2 scales) : base(g, scales, new Vector4(73, 102, 192, 255)) {
			this.menu = menu;
			
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
				int lenCheats = offon.Length;
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
					if (selectedZoom == 0) {
						AppMain.scales.X = 2.0f; 
						AppMain.scales.Y = 2.0f;
					}
					if (selectedZoom == 1) {
						AppMain.scales.X = (float)graphics.Screen.Width/256;  
						AppMain.scales.Y = (float)graphics.Screen.Height/232;
					}
					menu.Resize(AppMain.scales);
					AppMain.ResizeGui();
					AppMain.setMenu(new SetupMenu(menu, graphics, AppMain.scales));
				}
				if (selected == 2) {
					if (offon[selectedCheats].Equals("On")) AppMain.cheats = true;
					else AppMain.cheats = false;
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
			backgroundColor.Position.X = 0;
			backgroundColor.Position.Y = 0;
			backgroundColor.Render();
			
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
					cu.Position.X = graphics.Screen.Width/2 - ((options[3].ToString().Length/2)*font1.getTilesize()*scales.X*3/2 + cu.Width + 5);
					cu.Position.Y = graphics.Screen.Height/2 + (font1.getTilesize()*i)*3/2*scales.Y + font1.getTilesize()*scales.Y*33/10;
					cu.Render();
					font1.render(msg,
							(int)(graphics.Screen.Width/2 - (options[3].ToString().Length/2) * font1.getTilesize()* scales.X*3/2), 
							(int)(graphics.Screen.Height/2 + (font1.getTilesize()*i)*3/2*scales.Y + font1.getTilesize()*scales.Y*33/10));
				} else {
					font1.render(msg, 
							(int)(graphics.Screen.Width/2 - (options[3].ToString().Length/2)*font1.getTilesize()*scales.X*3/2), 
							(int)(graphics.Screen.Height/2 + (font1.getTilesize()*i)*3/2*scales.Y + font1.getTilesize()*scales.Y*33/10));
				}
			}
			// Sound
			font1.render("<" + onoff[selectedSound] + ">", 
				(int)(graphics.Screen.Width/2 + 26*scales.X), 
				(int)(graphics.Screen.Height/2 + font1.getTilesize()*scales.Y*33/10));
			
			// Zoom 
			font1.render("<" + zoom[selectedZoom] + ">", 
				(int)(graphics.Screen.Width/2 + 26*scales.X), 
				(int)(graphics.Screen.Height/2 + font1.getTilesize()*3/2*scales.Y + font1.getTilesize()*scales.Y*33/10));
			
			// Cheats
			font1.render("<" + offon[selectedCheats] + ">", 
				(int)(graphics.Screen.Width/2 + 26*scales.X), 
				(int)(graphics.Screen.Height/2 + font1.getTilesize()*3*scales.Y + font1.getTilesize()*scales.Y*33/10));
			
			// Debug info
			font1.render("<" + onoff[selectedDebug] + ">", 
				(int)(graphics.Screen.Width/2 + 26*scales.X), 
				(int)(graphics.Screen.Height/2 + font1.getTilesize()*9/2*scales.Y + font1.getTilesize()*scales.Y*33/10));
			
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
	}
}

