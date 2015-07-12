using System;

using Sce.PlayStation.Core.Imaging;

namespace DynBlasterVita
{
	public class Tile
	{
		private Image image;
		private bool blocked;
		
		public Tile(Image image, bool blocked) {
			this.image = image;
			this.blocked = blocked;
		}

		public Image getImage() {
			return image;
		}

		public bool isBlocked() {
			return blocked;
		}
	}
}
