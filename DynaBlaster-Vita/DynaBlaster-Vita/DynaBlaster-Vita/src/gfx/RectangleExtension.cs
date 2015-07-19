using System;

using Sce.PlayStation.Core;

namespace DynaBlasterVita
{
	public static class RectangleExtension
	{
		
		public static bool contains(this Rectangle rectA, Rectangle rectB)
	    {
			return (rectB.X >= rectA.X) && (rectB.Y >= rectA.Y) &&
				(rectB.X + rectB.Width <= rectA.X + rectA.Width) && (rectB.Y + rectB.Height <= rectA.Y + rectA.Height);
	    }
		
		public static bool intersects(this Rectangle rectA, Rectangle rectB) {
	        return !((rectB.X + rectB.Width <= rectA.X) ||
	                (rectB.Y + rectB.Height <= rectA.Y) ||
	                (rectB.X >= rectA.X + rectA.Width) ||
	                (rectB.Y >= rectA.Y + rectA.Height));
		}
	}
}

