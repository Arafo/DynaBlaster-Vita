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
		private Animation bomb;
		private SpriteLoader sl;
		private SpriteSheet ss;
		
		private Menu menu;
		
		private String[,] passwords;
	
		public PasswordMenu(TitleMenu titleMenu, GraphicsContext g, int scale) : base(g, scale) {
			this.menu = titleMenu;
			this.scale = scale;
			this.font1 = new Font(new Vector4(255, 255, 255, 255), true, scale, g);
			this.password = new String[8];
			
			this.sl = new SpriteLoader();
			this.sl.cargarImagen(cursor);
			this.sl.setImage(sl.getImage().Resize(new ImageSize(sl.getImage().Size.Width*scale, sl.getImage().Size.Height*scale)));
			
			this.ss = new SpriteSheet(this.sl.getImage());
			
			Sprite[] bomb = {ss.obtenerSprite(0, 0, 20*scale, 23*scale, g), 
					ss.obtenerSprite(20*scale, 0, 20*scale, 23*scale, g), 
					ss.obtenerSprite(40*scale, 0, 22*scale, 23*scale, g)};
			this.bomb = new Animation(bomb, 10);
				
			passwords = new String[8, 8];
			
			using (StreamReader sr = new StreamReader(File.OpenRead("/Application/res/maps/definitions/passwords.txt"))) 
			{
				for(int i=0; i<8;i++) {
					for(int j=0; j<8; j++) {
						sr.Read();
						sr.Read();
						passwords[i, j] = Convert.ToString(sr.Read());
					}			
				}
			}
			
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

					found: for(int i=0; i<8; i++) {
						for(int j=0; j<8; j++) {
							if(passwords[i, j].Equals(pass)) {
								i++;
								j++;
								//MP3Player.title.stop();
								//game.startLevel(i, j);
								goto found;
							}
						}
					}
					// TODO crear nivel i j
					/*for (int i = 0; i<password.length; i++) {
						System.out.print(password[i]);
					}*/
					password = new String[password.Length];
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
		 	graphics.SetClearColor(bgColor);
		    //g.fillRect(0, 0, game.getWidth(), game.getHeight());
		    
		    // Titulo del menu
		    font1.render(title, 
		    		graphics.Screen.Width / 2 - (title.Length / 2)*font1.getTilesize()*scale,
		    		graphics.Screen.Height / 2 - font1.getTilesize()*8*scale);
		    
			this.x = graphics.Screen.Width / 2 - ((19/2)*font1.getTilesize())*scale;
			this.y = graphics.Screen.Height / 2 - font1.getTilesize()*3*scale;
			
		    // Cursor de todas los caracteres
			bomb.getSprite().Position.X = x + selected%10 * font1.getTilesize()*2*scale - 6*scale;
			bomb.getSprite().Position.Y = y + selected/10 * font1.getTilesize()*2*scale - 10*scale;
			bomb.getSprite().Render();

			// Caracteres
		    for (int i = 0; i < chars.Length; i++) {
		    	font1.render(chars[i],
		    			x + i%10 * font1.getTilesize()*2*scale, 
		    			y + i/10 * font1.getTilesize()*2*scale);
		    }
		    
		    this.x = graphics.Screen.Width / 2 - password.Length/2*font1.getTilesize()*scale;
		    this.y = graphics.Screen.Height - font1.getTilesize()*5*scale;
		    
			for (int i = 0; i < password.Length; i++) {
				if (password[i] != null)
					font1.render(password[i],  x + i*font1.getTilesize()*scale, y);
				
				// Guiones
				font1.render("-", 
						x + i*font1.getTilesize()*scale, y + font1.getTilesize() * scale);
				// Cursor de la letra seleccionada
				font1.render("=", x+ index*font1.getTilesize()*scale, y + font1.getTilesize() * scale);
				
			}
		}
	}
}

