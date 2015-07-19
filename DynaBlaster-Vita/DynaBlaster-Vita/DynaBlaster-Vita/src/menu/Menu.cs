using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

namespace DynaBlasterVita
{
	public class Menu
	{
		protected GraphicsContext graphics;
		protected Vector2 scales;
		protected Vector4 bgColor;
		
		public Menu(GraphicsContext g, Vector2 scales, Vector4 color) {
			this.graphics = g;
			this.scales = scales;
			this.bgColor = color;
		}
		
		public virtual void tick(InputHandler input) {
		}
		
		public virtual void Render() {
		}
		
		public virtual void Resize(Vector2 scale) {
		}
		
	}
}

