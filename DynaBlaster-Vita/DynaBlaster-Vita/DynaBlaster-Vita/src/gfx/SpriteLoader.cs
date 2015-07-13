using System;
using System.IO;

using Sce.PlayStation.Core.Imaging;
using Sce.PlayStation.Core.Graphics;

namespace DynaBlasterVita
{
	public class SpriteLoader
	{
		private Image imagen;
		
		public Image cargarImagen(String ruta) {
			try {
				imagen = new Image(ruta);
			} catch (IOException e) {
				Console.Error.WriteLine(e.Message);
			}
			return imagen;
		}
		
		public Sprite ImageToSprite(GraphicsContext graphics) {
			Texture2D texture = new Texture2D(imagen.Size.Width, imagen.Size.Height, false, PixelFormat.Rgba);
			texture.SetPixels(0, imagen.ToBuffer(), PixelFormat.Rgba);
			return new Sprite(graphics, texture);
		}
		
		public Image getImage() {
			return imagen;
		}
		
		public void setImage(Image image) {
			this.imagen = image;
		}
	}
}

