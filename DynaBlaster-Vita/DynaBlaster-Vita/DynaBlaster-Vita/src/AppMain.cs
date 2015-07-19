using System;
using System.IO;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;
using Sce.PlayStation.Core.Input;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

using System.Diagnostics;

namespace DynaBlasterVita
{
	public class AppMain
	{
		public static int scale = 2;
		public static Vector2 scales;
		public static bool debug = true;
		public static bool cheats = false;
		private static GraphicsContext graphics;
		private static Map map;
		private static GenerateObstacles obstacles;
		private static HUD hud;
		private static Menu menu;
		private static Bomberman player;
		private static Entity exit;
		private static List<Enemy> enemies = new List<Enemy>();
		private static List<Flare> flares = new List<Flare>();
		private static List<Bomb> bombs = new List<Bomb>();
		private static List<PowerUps> powerups = new List<PowerUps>();
		private static Dictionary<String, MP3Player> music = new Dictionary<String, MP3Player>();
		private static String keymusic = "";
		private static InputHandler input;
		
		private static Stopwatch clock;
		private static long startTime;
		private static long stopTime;
		private static long timeDelta;
		
		public static int offsetX = 0;
		public static int offsetY = 0;
		private static Stopwatch time;
		
		private static int level = 1;
		private static int levelmap = 1;
		private static int continues = 2;
	
		private static bool running = true;
		private static bool playing = false;
		private static bool  pause = false;
		private static Font font;
		private static Sprite gui = null;
		
		//private static MP3Player invincible, level_clear;
		
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
			scales = new Vector2(2.0f, 2.0f);

			clock = new Stopwatch();
			clock.Start();
			time = new Stopwatch();
			hud = new HUD(graphics);
			input = new InputHandler();
			
			
			Image aux = new Image("/Application/res/image/hud.png");
			Image aux1 = aux.Resize(new ImageSize(aux.Size.Width*scale, aux.Size.Height*scale));
			Texture2D texture = new Texture2D(aux1.Size.Width, aux1.Size.Height, false, PixelFormat.Rgba);
			texture.SetPixels(0, aux1.ToBuffer(), PixelFormat.Rgba);
			gui = new Sprite(graphics, texture);
			
//			music.Add("bgm_01", new MP3Player("/Application/res/sound/bgm_01.mp3"));
//			music.Add("bgm_02", new MP3Player("/Application/res/sound/bgm_02.mp3"));
//			music.Add("bgm_03", new MP3Player("/Application/res/sound/bgm_03.mp3"));
//			music.Add("bgm_boss", new MP3Player("/Application/res/sound/bgm_boss.mp3"));
//			
//			invincible = new MP3Player("/Application/res/sound/invincible.mp3");
//			level_clear = new MP3Player("/Application/res/sound/level_clear.mp3");

			setMenu(new TitleMenu(graphics, scales));
		}

		public static void Update ()
		{
			// Query gamepad for current state
			var gamePadData = GamePad.GetData (0);
			input.tick(gamePadData);
			
			if (time.ElapsedMilliseconds > 240*1000 && playing) {
				// Tiempo acabado, Game Over
				//if (player.isInvincible()) invincible.close();
				//else music[keymusic].stop();
				player.setLives(player.getLives() - 1);
				setMenu(new GameOverMenu(player.getLives(), level, levelmap, graphics, scales));
				time.Reset();
				time.Start();
			}
	
			if (menu != null) {
				menu.tick(input);
				playing = false;
			} else {
				// Si se han matado a todos los enemigos y no se ha cogido el powerup
				// el obstaculo sobre el que esta el powerup parpadea
				if(((Exit) exit).isActive() && powerups.Count > 0) {
					foreach (Obstacle obs in obstacles.getList()) {
						if (obs.intersects(powerups[0])) {
							obs.blink();
							break;
						}
					}
				}
				
				// Tecla ESC, salir al menu principal
				if (input.exit.clicked) {
					//if (player.isInvincible()) invincible.close();
					//else music[keymusic].stop();
					playing = false;
					pause = false;
					//initLevel();
					level = 1;
					levelmap = 1;
					offsetX = 0;
					offsetY = 0;
					setMenu(new TitleMenu(graphics, scales));
				}
				
				// Tecla P, pausa
				if (input.pause.clicked) {
					//if (player.isInvincible()) invincible.pause();
					//else music[keymusic].pause();
					pause = !pause;
				}
				
				if (!pause) {
					// Si bomberman no es invencible, musica normal
					if (!player.isInvincible()) {
						//if (playing) music[keymusic].play();
						//invincible.close();
					}
					// Si no, musica de invencibilidad
					else {
						//if (playing) invincible.play();
						//music[keymusic].pause();
					}
					
					// Primer frame en el que se ha acabado el nivel
					if (player.endLvlFirst()) {
						//music[keymusic].stop();
						//if (!playing) invincible.close();
						//level_clear.play();
					}
					
					playing = true;
					player.tick(input);
					
					// Si el jugador se ha muerto parar la musica de fondo
	//				if (player.isDyingFirst()) {
	//					music.get(keymusic).stop();
	//				}
					
					// Si se ha acabado el nivel
					if (player.endLvl()) {
						//level_clear.stop();
						levelmap++;
						if (levelmap > 8) {
							levelmap = 1;
							level++;
							if (level > 8) {
								setMenu(new CreditsMenu(graphics, scales));
								return;
							}
							else setMenu(new LevelMenu(level, graphics, scales));
						}
						else
							setMenu(new MapMenu(level, levelmap, graphics, scales));
						initLevel();
						return;
					}
					
					// Detonacion remota de las bombas
					if (player.hasRemoteDetonator() && input.remote.clicked && bombs.Count > 0) {
						bombs[0].removed = true;
						Sounds.bomb.play();
					}
					
					// Detonacion normal de las bombas
					if (input.fire.clicked && bombs.Count < player.getBombs()) {
						Bomb bomb = new Bomb(player, graphics, scales);
						bool found = false;
						foreach (Bomb b in bombs) {
							if (bomb.intersects(b)){
								found = true;
								break;
							}
						}
						if (!found)
							foreach (Obstacle obs in obstacles.getList()) {
								if (obs != null && obs.intersects(bomb)) {
									found = true;
									break;
								}
							}
						if (!found && !player.isTeleporting() && !player.isDying())
							bombs.Add(bomb);
					}
					
					// Tick de las salida
					exit.tick();
					
					// Comprobar si el jugador ha muerto
					if (player.removed) {
						//music[keymusic].stop();
						player.setLives(player.getLives() - 1);
						if (player.getLives() < 0) {
							initLevel();
							player.setLives(2);
							setMenu(new GameOverMenu(continues, level, levelmap, graphics, scales));
							continues--;
						}
						else {
							initLevel();
							setMenu(new MapMenu(level, levelmap, graphics, scales));
						}
					}
					
					// Comprobar si los enemigos han muerto y hacer tick
					for (int i = 0; i<enemies.Count; i++) {
						if (enemies[i] != null) {
							if (enemies[i].removed) {
								player.setScore(player.getScore() + enemies[i].getScore());
								enemies.Remove(enemies[i]);
								break;
							}	
						enemies[i].tick();
						}
					}
					
					// Tick de los powerups
					foreach (Entity e in powerups) {
						e.tick();
					}
					
					// Tick de las bombas
					for (int i = 0; i<bombs.Count; i++) {
						bombs[i].tick();
						if (bombs[i].removed) {
			
							addFlares(bombs[i]);
							bombs.Remove(bombs[i]);
							    //t.join();
						}
					}
					
					// Tick de las llamas
					for (int i = 0; i<flares.Count; i++) {
							if (flares[i].removed)
								flares.Remove(flares[i]);
							else
								flares[i].tick();
						}
					
					// Tick de los obstaculos
					for (int i = 0; i < obstacles.getList().Count; i++) {
						obstacles.getList()[i].tick();
						if (obstacles.getList()[i].removed) {
							obstacles.getList().Remove(obstacles.getList()[i]);
						}
					}
					
					// Comprobar colisiones
					checkCollisions();
				}
			}
			
			hud.UpdateFPS(timeDelta);
			hud.UpdatePosition(timeDelta + "ms");
			hud.UpdateEnemyCount(enemies.Count);
		}

		public static void Render ()
		{
											
			// Clear the screen
			graphics.SetClearColor (0.0f, 0.0f, 0.0f, 0.0f);
			//graphics.SetClearColor(73, 102, 192, 255);
			graphics.Clear ();
				
			if (menu != null) {
				menu.Render();
			} else {
				Scroll();
	
				// Pintar mapa
				map.renderMap();
				
				// Pintar salida
				//if (levelmap != 8 || ((Exit) exit).isActive())
				exit.Render();
				
				// Pintar bombas
				foreach (Entity e in bombs) {
					e.Render();
				}
				
				// Pintar PowerUps
				foreach (Entity e in powerups) {
					e.Render();
				}
				
				// Pintar obstaculos
				obstacles.Render();
				
				// Pintar enemigos
				foreach (Enemy e in enemies) {
					if (e != null) e.Render();
				}
	
				// Pintar llamas finales
				foreach (Flare e in flares) {
					if (e.isFinal())
						e.Render();
				}
					
				// Pintar llamas intermedias
				foreach (Flare e in flares) {
					if (!e.isFinal() && !e.isMid())
						e.Render();
				}
	
				// Pintar llamas iniciales
				foreach (Flare e in flares) {
					if (e.isMid())
						e.Render();
				}

				// Pintar bomberman
				player.Render();
				
				// Pintar gui
				RenderGui();
			}
			
			if (debug) hud.Render();
			// Present the screen
			graphics.SwapBuffers ();
		}
		
		public static void setMenu(Menu newMenu) {
			menu = newMenu;
		}
		
		public static void initLevel() {
			bool dead = (player  != null) ? player.removed : false;
			int powerupssize = powerups.Count;
			clear();
	
			StreamReader sr = new StreamReader(File.OpenRead("/Application/res/maps/definitions/"
			                                                 + level + "/" + level + "_" + levelmap + ".txt"));
			map = new Map(sr.ReadLine(), Map.TILESIZE, scales, graphics);
			// obstacles = new GenerateObstacles(map, true);
	
			PowerUps powerup = null;
			if (levelmap != 8) {
				obstacles = new GenerateObstacles(map, true, graphics);
				powerup = new PowerUps(Convert.ToInt32(sr.ReadLine()), obstacles.getList(), graphics, scales);
			} else {
				obstacles = new GenerateObstacles(map, false, graphics);
			}
	
			if (levelmap != 8) {
				if (level == 4 || level == 7)
					keymusic = "bgm_03";
				else if (level == 6 || level == 8)
					keymusic = "bgm_02";
				else
					keymusic = "bgm_01";
			} else
				keymusic = "bgm_boss";
	
			while (sr.Peek() >= 0) {
				String[] line = sr.ReadLine().Split();
				int type = Convert.ToInt32(line[0]);
				int count = Convert.ToInt32(line[1]);
				switch (type) {
				case 6: // blue snake

//					for (int x = 0; x < count; x++) {
//	
//						SnakeHead head = new SnakeHead(obstacles, map, player);
//						SnakeBody body = new SnakeBody(obstacles, map, player, head);
//	
//						for (int i = 0; i < 4; i++) {
//							head.setChild(body);
//							enemies.Add(head);
//							enemies.Add(body);
//							head = body;
//							body = new SnakeBody(obstacles, map, player, head);
//						}
//					}
	
					break;
				default:
					for (int i = 0; i < count; i++) 
						enemies.Add(Enemy.createEnemy(type, obstacles, map, player, graphics, scales));
					break;
				}
	
			}
			// in.close();
	
			if (powerup != null && levelmap != 8 && !dead) {
				powerups.Add(powerup);
			} else if (powerup != null && levelmap != 8 && dead && powerupssize > 0) {
				powerups.Add(powerup);
			}
	
			if (levelmap != 8) {
				do {
					exit = new Exit(obstacles.getList(), enemies, graphics, scales);
				} while (exit.intersects(powerup));
			} else
				exit = new Exit(null, enemies, graphics, scales);
			sr.Dispose();
		}
	 	
		private static void clear() {
			if (obstacles != null) obstacles.getList().Clear();
			if (enemies != null) enemies.Clear();
			if (powerups != null) powerups.Clear();
			if (bombs != null) bombs.Clear();
			if (flares != null) flares.Clear();
			if (player != null) player.reset();
			time.Reset();
			time.Start();
			offsetX = 0;
			offsetY = 0;
		}
		
		public static void checkCollisions() {
			// el jugador con los obstaculos
			foreach (Obstacle obs in obstacles.getList()) {
				if (obs != null && obs.intersects(player)) {
					if(!(player.canPassWalls() && obs.isSolid()))
						player.collide(obs);
				}
			}
			// los enemigos con los obstaculos
			foreach (Enemy enemy in enemies) {
				foreach (Obstacle obs in obstacles.getList()) {
					if (obs != null && obs.intersects(enemy)) {
						if(!(enemy.canPassWalls() && obs.isSolid()))
							enemy.collide(obs);
					}
				}
			}
			//try {
				// los enemigos con las llamas
				foreach (Enemy enemy in enemies) {
					foreach (Entity flare in flares) {
						if (flare.intersects(enemy)) {
							enemy.hurt(flare, 10);
						}
					}
				}
	
				// el jugador con las llamas
				foreach (Entity flare in flares) {
					if (flare.intersects(player)) {
						if (!player.isInvincible())
							player.touchedBy(flare);
						// break;
					}
				}
	
				// las bombas con las llamas
				foreach (Entity bomb in bombs) {
					foreach (Entity flare in flares) {
						if (flare.intersects(bomb)) {
							bomb.touchedBy(flare);
							// break;
						}
					}
				}
			//} catch (Exception e) {
			//}
	
			
			// los enemigos con las bombas
			foreach (Enemy enemy in enemies) {
				foreach (Bomb bomb in bombs) {
					if (bomb != null) {
						if (bomb.intersects(enemy)) {
							if (bomb.isOut(enemy) && !enemy.canPassBombs()) {
								enemy.collide(bomb);
							}
						}
						else {
							bomb.setOut(enemy);
						}
					}
				}
			}
	
			// el jugador con las bombas
			foreach (Bomb bomb in bombs) {
				if (bomb != null) {
					if (bomb.intersects(player)) {
						if (!bomb.isOut(player) && ! player.canPassBombs()) {
							player.collide(bomb);
						}
						else {
							bomb.setOut(player);
						}
					}
				}
			}
	
			// el jugador con los enemigos
			foreach (Enemy enemy in enemies) {
				if (enemy != null && enemy.intersects(player)) {
					enemy.touchedBy(player); // Normal
					//player.touchedBy(enemy); // Bomberman se carga a todos
					//break;
				}
			}
	
			// el jugador con los power ups
			for (int i = 0; i<powerups.Count; i++) {
				//PowerUps powerup = it.next();
				if (powerups[i] != null && powerups[i].intersects(player)) {
					player.addPowerUp(powerups[i]);
					if (powerups[i].removed)
						powerups.Remove(powerups[i]);
				}
			}
			
			// el jugador con la salida
			if (player.intersects(exit) && ((Exit) exit).isActive() /*&& exit.getBounds().contains(player.getBounds())*/) {
				bombs.Clear();
				player.touchedBy(exit);
				playing = false;
				//System.out.println("Level Complete");
			}
		}
		
		private static void addFlares(Bomb bomb) {
			bool obstaclefound;
			flares.Add(new Flare(bomb,0,0, graphics, scales));
			
			obstaclefound = false;
			for(int i=1; i <= bomb.getPotency(); i++) {
				Flare flare = new Flare(bomb, i, 0, graphics, scales);
				foreach (Obstacle obs in obstacles.getList()) {
					if (obs != null && obs.intersects(flare)) {
						if (obs.isSolid()) {
							flare.setAsFinal();
							obs.die();
							obstaclefound = true;
							break;
						}
						else
							goto flaresloop1;
					}
				}
				
				if(!obstaclefound) {
					for (int x = 0; x<powerups.Count; x++) {
						PowerUps pu = powerups[x];
						if (pu != null && pu.intersects(flare)) {
							flare.setAsFinal();
							powerups.Remove(powerups[x]);
							break;
						}
					}
				}
				flares.Add(flare);
				if(flare.isFinal())
					break;
			}
			
			flaresloop1: obstaclefound = false;
			for(int i=-1; i >= -bomb.getPotency(); i--) {
				Flare flare = new Flare(bomb,i,0, graphics, scales);
				foreach (Obstacle obs in obstacles.getList()) {
					if (obs != null && obs.intersects(flare)) {
						if (obs.isSolid()) {
							flare.setAsFinal();
							obs.die();
							obstaclefound = true;
							break;
						}
						else
							goto flaresloop2;
					}
				}
				
				if(!obstaclefound) {
					for (int x = 0; x<powerups.Count; x++) {
						PowerUps pu = powerups[x];
						if (pu != null && pu.intersects(flare)) {
							flare.setAsFinal();
							powerups.Remove(powerups[x]);
							break;
						}
					}
				}
				flares.Add(flare);
				if(flare.isFinal())
					break;
			}
			
		flaresloop2: obstaclefound = false;
			for(int i=1; i <= bomb.getPotency(); i++) {
				Flare flare = new Flare(bomb,0,i, graphics, scales);
				foreach (Obstacle obs in obstacles.getList()) {
					if (obs != null && obs.intersects(flare)) {
						if (obs.isSolid()) {
							flare.setAsFinal();
							obs.die();
							obstaclefound = true;
							break;
						}
						else
							goto flaresloop3;
					}
				}
				
				if(!obstaclefound) {
					for (int x = 0; x<powerups.Count; x++) {
						PowerUps pu = powerups[x];
						if (pu != null && pu.intersects(flare)) {
							flare.setAsFinal();
							powerups.Remove(powerups[x]);
							break;
						}
					}
				}
				flares.Add(flare);
				if(flare.isFinal())
					break;
			}
			
			flaresloop3: obstaclefound = false;
			for(int i=-1; i >= -bomb.getPotency(); i--) {
				Flare flare = new Flare(bomb,0,i, graphics, scales);
				foreach (Obstacle obs in obstacles.getList()) {
					if (obs != null && obs.intersects(flare)) {
						if (obs.isSolid()) {
							flare.setAsFinal();
							obs.die();
							obstaclefound = true;
							break;
						}
						else
							goto flaresloop4;
					}
				}
				
				if(!obstaclefound) {
					for (int x = 0; x<powerups.Count; x++) {
						PowerUps pu = powerups[x];
						if (pu != null && pu.intersects(flare)) {
							flare.setAsFinal();
							powerups.Remove(powerups[x]);
							break;
						}
					}
				}
				flares.Add(flare);
				if(flare.isFinal())
					break;
			}
			flaresloop4: ;
		}
		
		private static void Scroll() {
			if (player.position.X > graphics.Screen.Width/2
					&& player.position.X < map.getmapWidth()*map.getTileSize()*scale
			    	- (graphics.Screen.Width/ 2 + map.getTileSize()*scale)) {
				offsetX = (int)-player.position.X + graphics.Screen.Width/2;
			}
			if (player.position.Y > graphics.Screen.Height/2
					&& player.position.Y < map.getmapHeight()*map.getTileSize()*scale
			    	- graphics.Screen.Height/2) {
				offsetY = (int)-player.position.Y + graphics.Screen.Height/2;
			}
		}
		
		private static void RenderGui() {
	
			if (pause) {
				String p = "Pause";
				//g.fillRect(-offsetX, -offsetY, getWidth(), gui.getHeight());
				font.render("Pause", 
				            (int)(-offsetX + graphics.Screen.Width/2 - (p.Length*font.getTilesize()*scales.X)/2), 
				            (int)(-offsetY + (int)gui.Height/2 - (font.getTilesize()*scales.Y)/2));
			} else {
				
				int x = 0;
				int y = 0;
				
				gui.Position.X = graphics.Screen.Width/2 - map.getMapSize().X/2 - offsetX + 8*scales.X;
				gui.Position.Y = -offsetY;
				gui.Render();
	
				// Score
				x = 80 - 8 * (player.getScore().ToString().Length - 1);
				y = 8;
				font.render(Convert.ToString(player.getScore()), 
				            (int)(-offsetX + graphics.Screen.Width/2 - map.getMapSize().X/2 + 8*scales.X + x*scales.X),
				            (int)(-offsetY + y*scales.Y));
				x = 241 - 8 * (player.getScore().ToString().Length - 1);
				font.render(Convert.ToString(player.getScore()), 
				            (int)(-offsetX + graphics.Screen.Width/2 - map.getMapSize().X/2 + 8*scales.X + x*scales.X),
				            (int)(-offsetY + y*scales.Y));
	
				// Lives
				x = 153;
				font.render(Convert.ToString(player.getLives()), 
				            (int)(-offsetX + graphics.Screen.Width/2 - map.getMapSize().X/2 + 8*scales.X + x*scales.X), 
				            (int)(-offsetY + y*scales.Y));
	
				// Time
				int minutes = (int)System.Math.Floor(TimeSpan.FromMilliseconds((240*1000 - time.ElapsedMilliseconds)).TotalMinutes);
				int seconds = (int)System.Math.Floor(TimeSpan.FromMilliseconds((240*1000 - time.ElapsedMilliseconds) - minutes*60*1000).TotalSeconds);
				String seg = Convert.ToString(seconds);
				if (seconds < 10)
					seg = "0" + seconds;
	
				x = 106;
				font.render(Convert.ToString(minutes), 
				            (int)(-offsetX + graphics.Screen.Width/2 + 8*scales.X - map.getMapSize().X/2 + x*scales.X),
				            (int)(-offsetY + y * scales.Y));
				x = 120;
				font.render(seg, 
				            (int)(-offsetX + graphics.Screen.Width/2 - map.getMapSize().X/2 + 8*scales.X + x*scales.X),
				            (int)(-offsetY + y*scales.Y));
			}
		}
		
		public static void ResizeGui() {
			Image aux = new Image("/Application/res/image/hud.png");
			Image aux1 = aux.Resize(new ImageSize((int)(aux.Size.Width*scales.X), (int)(aux.Size.Height*scales.Y)));
			Texture2D texture = new Texture2D(aux1.Size.Width, aux1.Size.Height, false, PixelFormat.Rgba);
			texture.SetPixels(0, aux1.ToBuffer(), PixelFormat.Rgba);
			gui = new Sprite(graphics, texture);
		}
			
		public static void startLevel(int newLevel, int newMap) {
			level = newLevel;
			levelmap = newMap;
			continues = 2;
			initLevel();
			player = new Bomberman(graphics, scales, map, cheats);
			font = new Font(new Vector4(255, 255, 255, 255), true, scales, graphics);
			
			if (newMap > 1) setMenu(new MapMenu(newLevel, newMap, graphics, scales));
			else setMenu(new LevelMenu(newLevel, graphics, scales));
			}
		}
}
