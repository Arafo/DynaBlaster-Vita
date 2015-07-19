using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;

namespace DynaBlasterVita
{
	public class Score
	{
		private String score = "/Application/res/image/score.png";
		private static int W = 15;
		private static int H = 5;
		
		private SpriteLoader sl;
		private SpriteSheet ss;
		private Image scoreImg;
		private Image scoreFinal;
		private Sprite scoreSprite;
		private GraphicsContext g;
		private Vector2 scale;
		
		public Score(int score, int w, int h, GraphicsContext g, Vector2 scale) {
			this.g = g;
			this.scale = scale;
			int pos = 0;
			if (score%1000 == 0) {
				pos = (int) (Math.Log(score/1000)/Math.Log(2)) + 7;
			}
			else {
				pos = (int) (Math.Log(score/100)/Math.Log(2));
			}
			
			this.sl = new SpriteLoader();			
			this.sl.cargarImagen(this.score);
			this.sl.setImage(sl.getImage().Resize(new ImageSize((int)(sl.getImage().Size.Width*scale.X), 
			                                                    (int)(sl.getImage().Size.Height*scale.Y))));
			this.ss = new SpriteSheet(this.sl.getImage());
			
			scoreImg = ss.obtenerSprite((int)(pos*W*scale.X), 0, (int)(W*scale.X), (int)(H*scale.Y));
			
			scoreFinal = new Image(ImageMode.Rgba, new ImageSize((int)(w*scale.X), (int)(h*scale.Y)), new ImageColor(0, 0, 0, 0));
			scoreFinal.DrawImage(scoreImg, new ImagePosition((int)(w*scale.X/2 - W*scale.X/2),
							(int)(h*scale.Y/2 - H*scale.Y/2)));
			
			Texture2D texture = new Texture2D(scoreFinal.Size.Width, scoreFinal.Size.Height, false, PixelFormat.Rgba);
			texture.SetPixels(0, scoreFinal.ToBuffer(), PixelFormat.Rgba);
			scoreSprite = new Sprite(g, texture);
		}
		
		public void Render() {
	
			//g.drawImage(scoreImg, 0, 0, null);
		}
		
		public Sprite getImage() {
			return scoreSprite;
		}
	}
}

