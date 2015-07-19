using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Imaging;
using Sce.PlayStation.Core.Graphics;

namespace DynaBlasterVita
{
	public class Textures
	{
		public static Sprite CreateTexture(int width, int height, GraphicsContext g, Vector4 color)
		{
			Image aux = new Image(ImageMode.Rgba, new ImageSize(width, height), new ImageColor((int)color.R, (int)color.G, (int)color.B, (int)color.A));
			Texture2D texture = new Texture2D(width, height, false, PixelFormat.Rgba);
			texture.SetPixels(0, aux.ToBuffer(), PixelFormat.Rgba);
			return new Sprite(g, texture);
		}
	}
}

