using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.HighLevel.UI;

using System.Diagnostics;

namespace DynaBlasterVita
{
	public class HUD
	{
		private Label position;
		private Label FPS;
		private Label enemies;
		private GraphicsContext graphics;
		private int frameCounter;
		private long elapseTime;

		public HUD (GraphicsContext gc)			
		{
			graphics = gc;
			UISystem.Initialize(graphics);
			Scene scene = new Scene();
			position = new Label();
			position.X = 10;
			position.Y = 10;
			position.Width = 600;
			position.Text = "Test Output";
			scene.RootWidget.AddChildLast(position);
			
			FPS = new Label();
			FPS.X = graphics.Screen.Rectangle.Width - 310;
			FPS.Y = 10;
			FPS.Width = 300;
			FPS.HorizontalAlignment = HorizontalAlignment.Right;
			FPS.Text = "FPS = ???";
			scene.RootWidget.AddChildLast(FPS);
			

			enemies = new Label();
			enemies.X = 10;
			enemies.Y = 500;
			enemies.Width = 300;
			enemies.Text = "Enemy Count = 0";
			scene.RootWidget.AddChildLast(enemies);

			UISystem.SetScene(scene, null);
			
			frameCounter = 0;
			elapseTime = 0;
		}
		
		public void Render()
		{
			UISystem.Render();
			frameCounter++;
		}
		
		public void UpdatePosition(string s)
		{
			position.Text = s;
		}

		public void UpdateEnemyCount(int count)
		{
			enemies.Text = "Enemy Count = " + count;
		}

		public void UpdateFPS(long timeDelta)
		{
			elapseTime += timeDelta;
			if (elapseTime >= 1000)
			{
				FPS.Text = "FPS = " + frameCounter;
				frameCounter = 0;
				elapseTime -= 1000;
			}			
		}
	}
}

