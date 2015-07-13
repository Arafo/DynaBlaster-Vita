using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;

namespace DynaBlasterVita
{
	public class Font
	{
		private int TILE_SIZE = 8;
		private String FONT_SPRITE = "/Application/res/image/font.png";
		
		private Sprite[,] font;
		private Sprite[,] shad;
		private SpriteSheet tileset;
		private int scale;
		private Vector4 color;
		private bool shadows;
		private static String chars = "" +
				"ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
				"!.<#/>0123456789  =------- ";
		private GraphicsContext g;
		
		public Font(Vector4 color, bool shadows, int scale, GraphicsContext g) {
			this.scale = scale;
			this.color = color;
			this.shadows = shadows;
			this.g = g;
			SpriteLoader loader = new SpriteLoader();
			Image spriteSheet = loader.cargarImagen(FONT_SPRITE);
			tileset = new SpriteSheet(spriteSheet);
			loadTiles();
		}
		
		private void loadTiles() {
			try {
				int numTilesAcross = (tileset.getWidth() + 1) / TILE_SIZE;
				font = new Sprite[2, numTilesAcross];
				shad = new Sprite[2, numTilesAcross];
	
				Image subimage, shadowImage;
				Sprite aux;
				
				for (int col = 0; col < numTilesAcross; col++) {
					subimage = tileset.obtenerSprite(col * TILE_SIZE, 0, TILE_SIZE, TILE_SIZE);
					aux = ImageToSprite(subimage.Resize(new ImageSize(subimage.Size.Width*scale, subimage.Size.Height*scale)), g);
					aux.SetColor(color);
					font[0, col] = aux;
					
					subimage = tileset.obtenerSprite(col * TILE_SIZE, TILE_SIZE, TILE_SIZE, TILE_SIZE);
					aux = ImageToSprite(subimage.Resize(new ImageSize(subimage.Size.Width*scale, subimage.Size.Height*scale)), g);
					aux.SetColor(color);
					font[1, col] = aux;
					
					if (shadows) {
						shadowImage = tileset.obtenerSprite(col * TILE_SIZE, 0, TILE_SIZE, TILE_SIZE);
						aux = ImageToSprite(shadowImage.Resize(new ImageSize(shadowImage.Size.Width*scale, shadowImage.Size.Height*scale)), g);
						aux.SetColor(new Vector4(0, 0, 0, 255));
						shad[0, col] = aux;
						
						shadowImage = tileset.obtenerSprite(col * TILE_SIZE, TILE_SIZE, TILE_SIZE, TILE_SIZE);
						aux = ImageToSprite(shadowImage.Resize(new ImageSize(shadowImage.Size.Width*scale, shadowImage.Size.Height*scale)), g);
						aux.SetColor(new Vector4(0, 0, 0, 255));
						shad[1, col] = aux;
					}
				}
	
			} catch (Exception e) {
				Console.Error.WriteLine(e.Message);
			}
		}
		
		public int getTilesize() {
			return this.TILE_SIZE;
		}
		
		private Sprite ImageToSprite(Image image, GraphicsContext graphics) {
			Texture2D texture = new Texture2D(image.Size.Width, image.Size.Height, false, PixelFormat.Rgba);
			texture.SetPixels(0, image.ToBuffer(), PixelFormat.Rgba);
			return new Sprite(graphics, texture);
		}
		
		public void render(String msg, int x, int y) {
			msg = msg.ToUpper();
			for (int i = 0; i < msg.Length; i++) {
				int ix = chars.IndexOf(msg[i]);
				if (ix >= 0) {
					int r = ix / font.GetLength(1);
					int c = ix % font.GetLength(1);
					if (shadows) {
						shad[r, c].Position.X = x + i * + TILE_SIZE * scale + 2;
						shad[r, c].Position.Y = y + 2;
						shad[r, c].Render();
					}
					font[r, c].Position.X = x + i * + TILE_SIZE * scale;
					font[r, c].Position.Y = y;
					font[r, c].Render();
				}
			}
		}
	}
}

