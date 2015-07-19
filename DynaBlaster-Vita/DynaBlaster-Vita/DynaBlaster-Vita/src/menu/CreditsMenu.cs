using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

namespace DynaBlasterVita
{
	public class CreditsMenu : Menu
	{
		private static String DAVID = "DAVID OREA";
		private static String RAFA = "RAFAEL MARCEN";
		private static String RUBEN = "RUBEN TOMAS";
		
		private Font font;
		private MP3Player music;
		private String[] credits = {"DYNABLASTER", "CREDITS", "", "", "",
				"VIDEOJUEGOS 2014-2015", "GRUPO 5", "", "",
				"PRODUCTION", DAVID, RAFA, RUBEN, "", "",
				"DESIGN", DAVID, RAFA, RUBEN, "", "",
				"CODING", DAVID, RAFA, RUBEN, "", "",
				"GRAPHICS", DAVID, RAFA, RUBEN, "", "",
				"MUSIC", DAVID, RAFA, RUBEN, "", "",
				"SOUND FX", DAVID, RAFA, RUBEN, "", "",
				"3D", DAVID, RAFA, RUBEN, "", "",
				"GAMETESTING", DAVID, RAFA, RUBEN, "", "",
				"", "ORIGINAL GAME", "1991", "HUDSON SOFT ", "", "",
				"", "", "", "", "", "", "THE END"};
		private float y;
	
		public CreditsMenu(GraphicsContext g, Vector2 scales) : base(g, scales, new Vector4(0, 0, 0, 255)) {
			this.font = new Font(new Vector4(255, 255, 255, 255), true, scales, g);
			this.y = 0;
			music = new MP3Player("/Application/res/sound/credits.mp3");
			music.play();
		}
		
		public override void tick(InputHandler input) {
			if (input.exit.clicked || (music != null && !music.isPlaying())) {
				music.close();
				AppMain.setMenu(new TitleMenu(graphics, scales));
			}
	
			if (graphics.Screen.Height + y + (credits.Length-1)*font.getTilesize()*2*scales.Y > graphics.Screen.Height/2) {
				y -= 0.5f;
			}
	
		}
		
		public override void Render () {
			for (int i = 0; i<credits.Length; i++) {
				font.render(credits[i], 
						(int)(graphics.Screen.Width/2 - (credits[i].ToString().Length/2)*font.getTilesize()*scales.X), 
						(int)(graphics.Screen.Height + y + i*font.getTilesize()*2*scales.Y));
			}
	
		}
	}
}

