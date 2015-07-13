using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

namespace DynaBlasterVita
{
	public class Menu
	{
		protected GraphicsContext graphics;
		protected int scale;
		protected Vector4 bgColor;
		
		public Menu(GraphicsContext g, int scale) {
			this.graphics = g;
			this.scale = scale;
			this.bgColor = new Vector4(73, 102, 192, 255);
		}
		
		public virtual void tick(InputHandler input) {
		}
		
		public virtual void Render() {
		}
		
	}
}

