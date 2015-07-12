using System;
using System.Collections.Generic;

using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;

namespace DynaBlasterVita
{
	public class SpriteSheet
	{
		private Image imagen;
	
		public SpriteSheet(Image bi) {
			this.imagen = bi;
		}
	
		public Image obtenerSprite(int x, int y, int width, int height) {
			return imagen.Crop(new ImageRect(x, y, width, height));
		}
		
		public Sprite obtenerSprite(int x, int y, int width, int height, GraphicsContext g) {
			Image aux = imagen.Crop(new ImageRect(x, y, width, height));
			Texture2D texture = new Texture2D(width, height, false, PixelFormat.Rgba);
			texture.SetPixels(0, aux.ToBuffer(), PixelFormat.Rgba);
			return new Sprite(g, texture);
		}
		
		public int getWidth() {
			return imagen.Size.Width;
		}
		
		public int getHeight() {
			return imagen.Size.Height;
		}
		
		public Image getImage() {
			return this.imagen;
		}
		
		public void setImage(Image  img) {
			this.imagen = img;
		}
	}
}

