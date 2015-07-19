using System;
using System.Collections.Generic;

using Sce.PlayStation.Core.Input;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace DynaBlasterVita
{
	public class InputHandler
	{
		public class Key {
			public int presses, absorbs;
			public bool down, clicked;
	
			public Key() {
				keys.Add(this);
			}
	
			public void toggle(bool pressed) {
				if (pressed != down) {
					down = pressed;
				}
				if (pressed) {
					presses++;
				}
				else {
					presses = 0;
				}
			}
	
			public void tick() {

				if (absorbs < presses) {
					absorbs++;
					presses = absorbs - 64;
					clicked = true;
				} else {
					clicked = false;
					absorbs = 0;
				}
			}
		}

		public static List<Key> keys = new List<Key>();
		public Key up = new Key();
		public Key down = new Key();
		public Key left = new Key();
		public Key right = new Key();
		public Key fire = new Key();
		public Key remote = new Key();
		public Key pause = new Key();
		public Key exit = new Key();
	
		public void releaseAll() {
			for (int i = 0; i < keys.Count; i++) {
				keys[i].down = false;
			}
		}
	
		public void tick(GamePadData gamePadData) {
			for (int i = 0; i < keys.Count; i++) {
				keys[i].tick();
				checkKeys(gamePadData);
			}
		}
	
		public InputHandler() {
		}
	
		private void checkKeys(GamePadData gamePadData) {
			if((gamePadData.Buttons & GamePadButtons.Left) != 0) left.toggle(true);
			else left.toggle(false);
			if((gamePadData.Buttons & GamePadButtons.Up) != 0) up.toggle(true);
			else up.toggle(false);
			if((gamePadData.Buttons & GamePadButtons.Right) != 0) right.toggle(true);
			else right.toggle(false);
			if((gamePadData.Buttons & GamePadButtons.Down) != 0) down.toggle(true);
			else down.toggle(false);
			if((gamePadData.Buttons & GamePadButtons.Cross) != 0) fire.toggle(true);
			else fire.toggle(false);
			if((gamePadData.Buttons & GamePadButtons.Circle) != 0) exit.toggle(true);
			else exit.toggle(false);
			if((gamePadData.Buttons & GamePadButtons.Start) != 0) pause.toggle(true);
			else pause.toggle(false);
		}
	}
}

