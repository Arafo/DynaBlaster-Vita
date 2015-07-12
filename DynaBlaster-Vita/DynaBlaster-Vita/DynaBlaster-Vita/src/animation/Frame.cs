using System;

using Sce.PlayStation.Core.Imaging;

namespace DynaBlasterVita
{
	public class Frame {
		protected long length;
		protected Sprite img;
	
		public Frame(long length) {
			this.length = length;
			this.img = null;
		}
		
		public Frame(Sprite img, long time) {
			this.length = time;
			this.img = img;
		}
	
		public int compareTo(Frame f) {
			//return Long.compare(length, f.length);
			return length.CompareTo(f.length);
		}
		
		public Sprite getImg() {
			return img;
		}
	}
}

