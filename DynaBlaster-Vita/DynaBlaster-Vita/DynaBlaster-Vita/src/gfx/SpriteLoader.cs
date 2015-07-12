using System;
using System.IO;

using Sce.PlayStation.Core.Imaging;

namespace DynBlasterVita
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
	}
}

