using System;
using System.IO;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;

namespace DynaBlasterVita
{
	public class PasswordMenu : Menu
	{
		private static String cursor = "/Application/res/image/bomb_cursor.png";
		private int selected = 0;
		private String title = "Enter password ......";
		private String[] chars = 
			{"A", "B", "C", "D", "E", "F", "G", "H", "I", "J",
				"K", "L", "M", "N", "O", "P", "Q", "R", "S" , "T",
				"U", "V", "W", "X", "Y", "Z", "<", ">", "#/" };
		private String[] password;
		private int index = 0;
		private Font font1;
		private int x, y;
		private Sprite backgroundColor;
		private Animation bomb;
		private SpriteLoader sl;
		private SpriteSheet ss;
		
		private TitleMenu menu;
		
		private String[,] passwords;
	
		public PasswordMenu(TitleMenu titleMenu, GraphicsContext g, Vector2 scales) : base(g, scales, new Vector4(73, 102, 192, 255)) {
			this.menu = titleMenu;
			//this.scale = scale;
			this.font1 = new Font(new Vector4(255, 255, 255, 255), true, scales, g);
			this.password = new String[8];
			
			this.sl = new SpriteLoader();
			this.sl.cargarImagen(cursor);
			this.sl.setImage(sl.getImage().Resize(new ImageSize((int)(sl.getImage().Size.Width*scales.X), 
			                                                          (int)(sl.getImage().Size.Height*scales.Y))));
			
			this.ss = new SpriteSheet(this.sl.getImage());
			
			Sprite[] bomb = {ss.obtenerSprite(0, 0, (int)(20*scales.X), (int)(23*scales.Y), g), 
					ss.obtenerSprite((int)(20*scales.X), 0, (int)(20*scales.X), (int)(23*scales.Y), g), 
					ss.obtenerSprite((int)(40*scales.X), 0, (int)(22*scales.X), (int)(23*scales.Y), g)};
			this.bomb = new Animation(bomb, 10);
				
			passwords = new String[8, 8];
			
			using (StreamReader sr = new StreamReader(File.OpenRead("/Application/res/maps/definitions/passwords.txt"))) 
			{
				for(int i=0; i<8;i++) {
					for(int j=0; j<8; j++) {
						string line = Convert.ToString(sr.ReadLine());
						passwords[i, j] = line.Substring(line.Length - 8);
					}			
				}
				sr.Dispose();
			}
			
			backgroundColor = Textures.CreateTexture(g.Screen.Width, g.Screen.Height, g, bgColor);
			
			this.bomb.start();
		}
		
		public override void tick(InputHandler input) {
			if (input.left.clicked) selected--;
			if (input.right.clicked) selected++;
			if (input.up.clicked) selected -= 10;
			if (input.down.clicked) selected += 10;
	
			int len = chars.Length;
			if (selected < 0) selected += len;
			if (selected >= len) selected -= len;
	
			if (input.fire.clicked) {
				
				if (chars[selected].Equals(chars[chars.Length - 3])) index--; // Caracter <
				else if (chars[selected].Equals(chars[chars.Length - 2])) index++; // Caracter >
				else if (chars[selected].Equals(chars[chars.Length - 1])) { // Caracter END
					
					// Comprobar password
					String pass = "";
					for (int i = 0; i<password.Length; i++) {
						pass += password[i];
					}

					for(int i=0; i<8; i++) {
						for(int j=0; j<8; j++) {
							if(passwords[i, j].Equals(pass)) {
								i++;
								j++;
								menu.sound.close();
								AppMain.startLevel(i, j);
								goto found;
							}
						}
					}
				found: password = new String[password.Length];
					index = 0;
				}
				else password[index++] = chars[selected]; // Cualquiera de los otros caracteres
				
				if (index < 0) index++;
				if (index >= password.Length) index--;
			}
			
			if (input.exit.clicked) {
				bomb.stop();
				AppMain.setMenu(menu);
			}
			bomb.tick();
		}
		
		public override void Render() {
			
			// Color de fondo
		 	backgroundColor.Position.X = 0;
			backgroundColor.Position.Y = 0;
			backgroundColor.Render();
		    
		    // Titulo del menu
		    font1.render(title, 
		    		(int)(graphics.Screen.Width / 2 - (title.Length / 2)*font1.getTilesize()*scales.X),
		    		(int)(graphics.Screen.Height / 2 - font1.getTilesize()*8*scales.Y));
		    
			this.x = (int)(graphics.Screen.Width / 2 - ((19/2)*font1.getTilesize())*scales.X);
			this.y = (int)(graphics.Screen.Height / 2 - font1.getTilesize()*3*scales.Y);
			
		    // Cursor de todas los caracteres
			bomb.getSprite().Position.X = x + selected%10 * font1.getTilesize()*2*scales.X - 6*scales.X;
			bomb.getSprite().Position.Y = y + selected/10 * font1.getTilesize()*2*scales.Y - 10*scales.Y;
			bomb.getSprite().Render();

			// Caracteres
		    for (int i = 0; i < chars.Length; i++) {
		    	font1.render(chars[i],
		    			(int)(x + i%10 * font1.getTilesize()*2*scales.X), 
		    			(int)(y + i/10 * font1.getTilesize()*2*scales.Y));
		    }
		    
		    this.x = (int)(graphics.Screen.Width / 2 - password.Length/2*font1.getTilesize()*scales.X);
		    this.y = (int)(graphics.Screen.Height - font1.getTilesize()*5*scales.Y);
		    
			for (int i = 0; i < password.Length; i++) {
				if (password[i] != null)
					font1.render(password[i],  (int)(x + i*font1.getTilesize()*scales.X), y);
				
				// Guiones
				font1.render("-", 
				             (int)(x + i*font1.getTilesize()*scales.X),
				             (int)(y + font1.getTilesize() * scales.Y));
				// Cursor de la letra seleccionada
				font1.render("=", 
				             (int)(x+ index*font1.getTilesize()*scales.X),
				             (int)(y + font1.getTilesize() * scales.Y));
				
			}
		}
	}
}

