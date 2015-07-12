using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

using System.Diagnostics;

namespace DynaBlasterVita
{
	public class AppMain
	{
		private static GraphicsContext graphics;
		private static Map map;
		private static GenerateObstacles obstacles;
		private static HUD hud;
		private static Stopwatch clock;
		private static long startTime;
		private static long stopTime;
		private static long timeDelta;
		
		
		public static void Main (string[] args)
		{
			Initialize ();

			while (true) {
				startTime = clock.ElapsedMilliseconds;
				SystemEvents.CheckEvents();
				Update ();
				Render ();
				stopTime = clock.ElapsedMilliseconds;
				timeDelta = stopTime - startTime;
			}
		}

		public static void Initialize ()
		{
			// Set up the graphics system
			graphics = new GraphicsContext ();
			map = new Map("map1_2_square", 16, graphics);
			obstacles = new GenerateObstacles(map, true, graphics);
			hud = new HUD(graphics);
			clock = new Stopwatch();
			clock.Start();
			
			//dicTextureInfo = UnifiedTexture.GetDictionaryTextureInfo("/Application/image/unified_texture.xml");
			//textureUnified=new Texture2D("/Application/image/unified_texture.png", false);
		}

		public static void Update ()
		{
			// Query gamepad for current state
			var gamePadData = GamePad.GetData (0);
			hud.UpdateFPS(timeDelta);
		}

		public static void Render ()
		{
			// Clear the screen
			graphics.SetClearColor (0.0f, 0.0f, 0.0f, 0.0f);
			graphics.Clear ();
			
			map.renderMap();
			obstacles.Render();
			hud.Render();

			// Present the screen
			graphics.SwapBuffers ();
		}
	}
}
