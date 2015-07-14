using System;

using Sce.PlayStation.Core.Audio;

namespace DynaBlasterVita
{
	public class MP3Player
	{
//		public static  MP3Player title = new MP3Player("/Application/res/sound/title.mp3");
//		public static  MP3Player intro = new MP3Player("/Application/res/sound/intro.mp3");
//		public static  MP3Player level_start = new MP3Player("/Application/res/sound/level_start.mp3");
//		public static  MP3Player map_start = new MP3Player("/Application/res/sound/map_start.mp3");
//		public static  MP3Player invincible = new MP3Player("/Application/res/sound/invincible.mp3");
//		public static  MP3Player no_continue = new MP3Player("/Application/res/sound/no_continue.mp3");
//		public static  MP3Player level_clear = new MP3Player("/Application/res/sound/level_clear.mp3");
		
		private static BgmPlayer bgmPlayer;
		private static Bgm bgm;
		
		public MP3Player(String path) {
			bgm = new Bgm(path);
			bgmPlayer = bgm.CreatePlayer();
			bgmPlayer.Volume = 0.2f;

		}
		
		public void play() {
			if (bgmPlayer == null || bgm == null) return;
			if (bgmPlayer.Status != BgmStatus.Playing) {
				if (bgmPlayer.Status == BgmStatus.Paused) bgmPlayer.Resume();
				else bgmPlayer.Play();
			}
		}
		
		public void stop() {
			if (bgmPlayer != null && bgmPlayer.Status == BgmStatus.Playing) {
				bgmPlayer.Stop();
			}
		}
		
		public void pause() {
			if (bgmPlayer != null && bgmPlayer.Status == BgmStatus.Playing) {
				bgmPlayer.Pause();
			}
		}
		
		public void close() {
			stop();
			bgmPlayer.Dispose();
			bgm.Dispose();
		}
	
		public bool isPlaying() {
			return bgmPlayer.Status == BgmStatus.Playing;
		}
	}
}

