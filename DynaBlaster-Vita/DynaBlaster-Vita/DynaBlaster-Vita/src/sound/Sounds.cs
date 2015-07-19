using System;

using Sce.PlayStation.Core.Audio;

namespace DynaBlasterVita
{
	public class Sounds
	{
				
		public static  Sounds boom = new Sounds("/Application/res/sound/boom.wav");
		public static  Sounds bomb = new Sounds("/Application/res/sound/bomb.wav");
		//public static  Sounds powerup = new Sounds("/Application/res/sound/bonus.wav");
		//public static  Sounds death = new Sounds("/Application/res/sound/dying.wav");
		
		private static SoundPlayer soundPlayer;

		public Sounds(String path){
			try {
				soundPlayer = new Sound(path).CreatePlayer();
			} catch (Exception e) {
				Console.Error.WriteLine(e.Message);
			}	
		}
		
		public void play() {
			soundPlayer.Play();
		}
		
		public void stop() {
			soundPlayer.Stop();
		}
	}
}

