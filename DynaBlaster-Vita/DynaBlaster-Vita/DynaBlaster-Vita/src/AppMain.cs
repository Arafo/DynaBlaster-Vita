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
		private static int scale = 2;
		private static GraphicsContext graphics;
		private static Map map;
		private static GenerateObstacles obstacles;
		private static HUD hud;
		private static Menu menu;
		private static InputHandler input;
		
		private static Stopwatch clock;
		private static long startTime;
		private static long stopTime;
		private static long timeDelta;
		
		// TESTS
		
		
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
			map = new Map("map1_2_square", 16, scale, graphics);
			obstacles = new GenerateObstacles(map, true, graphics);
			hud = new HUD(graphics);
			clock = new Stopwatch();
			clock.Start();
			
			setMenu(new TitleMenu(graphics, scale));
			input = new InputHandler();
		}

		public static void Update ()
		{
			// Query gamepad for current state
			
			var gamePadData = GamePad.GetData (0);
			input.tick(gamePadData);
			
			if (menu != null) {
				menu.tick(input);
			} 
			else {
				
			}	
			hud.UpdateFPS(timeDelta);
			hud.UpdatePosition(timeDelta + "ms");
		}

		public static void Render ()
		{
			// Clear the screen
			graphics.SetClearColor (0.0f, 0.0f, 0.0f, 0.0f);
			graphics.SetClearColor(73, 102, 192, 255);
			graphics.Clear ();
			
			if (menu != null) {
				menu.Render();
			}
			else {
				map.renderMap();
				obstacles.Render();
			}
			
			hud.Render();
			
			// Present the screen
			graphics.SwapBuffers ();
		}
		
		public static void setMenu(Menu newMenu) {
			menu = newMenu;
		}
	}
}
