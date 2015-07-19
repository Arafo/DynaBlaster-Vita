using System;
using System.Diagnostics;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;


namespace DynaBlasterVita
{
	public class Bomberman : Mob
	{
		private String ANIMATION = "/Application/res/image/bomberman.png";
		private static int w = 24;
		private static int h = 22;
		
		private int score;
		private int lives;
		private int velocity;
		private int max_velocity;
		private int bombs;
		private int max_bombs;
		private int potency;
		private int max_potency;
		
		private bool canPassWalls, canPassBombs, remoteDetonator, deathPlayed;
		
		private int invincible;
		private Stopwatch invincibleClock;
		
		private Animation teleport, sparks;
		private Map map;
		
		private bool cheats;
		
		public Bomberman(GraphicsContext g, Vector2 scales, Map map, bool cheats) : base(g, scales) {
			
			this.position.X = g.Screen.Width/2 - map.getMapSize().X/2 + 33*scales.X;
			this.position.Y = 45*scales.Y;
			this.position.Width = 14*scales.X;
			this.position.Height = 11*scales.Y;
			this.map = map;
			this.cheats = cheats;
	
			this.score = 0;
			this.lives = 1;
			this.velocity = 0;
			this.max_velocity = 1;
			this.bombs = 1;
			this.max_bombs = 8;
			this.potency = 1;
			this.max_potency = 8;
			this.canPassWalls = false;
			this.canPassBombs = false;
			this.remoteDetonator = false;
			this.deathPlayed = false;
			this.invincible = 0;
			this.invincibleClock = new Stopwatch();
			this.invincibleClock.Start();
			
			if (cheats) {
				this.lives = 9;
				this.bombs = 8;
				this.potency = 8;
				this.invincible = 999999999;
			}
			
			this.sl = new SpriteLoader();			
			this.sl.cargarImagen(ANIMATION);
			this.sl.setImage(sl.getImage().Resize(new ImageSize((int)(sl.getImage().Size.Width*scales.X), 
			                                                    (int)(sl.getImage().Size.Height*scales.Y))));
			this.ss = new SpriteSheet(this.sl.getImage());
	
			Sprite[] walkingLeft = {
					ss.obtenerSprite((int)(6*w*scales.X), 0, (int)(w*scales.X), (int)(h*scales.Y), g),
					ss.obtenerSprite((int)(7*w*scales.X), 0, (int)(w*scales.X), (int)(h*scales.Y), g),
					ss.obtenerSprite((int)(8*w*scales.X), 0, (int)(w*scales.X), (int)(h*scales.Y), g) };
			Sprite[] walkingRight = {
					ss.obtenerSprite((int)(3*w*scales.X), 0, (int)(w*scales.X), (int)(h*scales.Y), g),
					ss.obtenerSprite((int)(4*w*scales.X), 0, (int)(w*scales.X), (int)(h*scales.Y), g),
					ss.obtenerSprite((int)(5*w*scales.X), 0, (int)(w*scales.X), (int)(h*scales.Y), g) };
			Sprite[] walkingUp = {
					ss.obtenerSprite((int)(9*w*scales.X), 0, (int)(w*scales.X), (int)(h*scales.Y), g),
					ss.obtenerSprite((int)(10*w*scales.X), 0, (int)(w*scales.X), (int)(h*scales.Y), g),
					ss.obtenerSprite((int)(11*w*scales.X), 0, (int)(w*scales.X), (int)(h*scales.Y), g) };
			Sprite[] walkingDown = {
					ss.obtenerSprite(0, 0, (int)(w*scales.X), (int)(h*scales.Y), g),
					ss.obtenerSprite((int)(1*w*scales.X), 0, (int)(w*scales.X), (int)(h*scales.Y), g),
					ss.obtenerSprite((int)(2*w*scale.X), 0, (int)(w*scales.X), (int)(h*scales.Y), g) };
			Sprite[] die = {
					ss.obtenerSprite(0, (int)(h*scales.Y), (int)(w*scales.X), (int)(h*scales.Y), g),
					ss.obtenerSprite((int)(1*w*scales.X), (int)(h*scales.Y), (int)(w*scales.X), (int)(h*scales.Y), g),
					ss.obtenerSprite((int)(2*w*scales.X), (int)(h*scales.Y), (int)(w*scales.X), (int)(h*scales.Y), g),
					ss.obtenerSprite((int)(3*w*scales.X), (int)(h*scales.Y), (int)(w*scales.X), (int)(h*scales.Y), g),
					ss.obtenerSprite((int)(4*w*scales.X), (int)(h*scales.Y), (int)(w*scales.X), (int)(h*scales.Y), g),
					ss.obtenerSprite((int)(5*w*scales.X), (int)(h*scales.Y), (int)(w*scales.X), (int)(h*scales.Y), g),
					ss.obtenerSprite((int)(6*w*scales.X), (int)(h*scales.Y), (int)(w*scales.X), (int)(h*scales.Y), g),
					ss.obtenerSprite((int)(7*w*scales.X), (int)(h*scales.Y), (int)(w*scales.X), (int)(h*scales.Y), g),
					ss.obtenerSprite((int)(8*w*scales.X), (int)(h*scales.Y), (int)(w*scales.X), (int)(h*scales.Y), g),
					ss.obtenerSprite((int)(9*w*scales.X), (int)(h*scales.Y), (int)(w*scales.X), (int)(h*scales.Y), g) };
			
			Sprite[] tp = new Sprite[(int)(h*scales.Y + 1)];
			for(int i=0; i<h*scales.Y/4; i++) {
				tp[i*4] = ss.obtenerSprite((int)(6*w*scales.X), i*4, (int)(w*scales.X), (int)(h*scales.Y-i*4), g);
				tp[i*4+1] = ss.obtenerSprite((int)(9*w*scales.X), i*4, (int)(w*scales.X), (int)(h*scales.Y-i*4), g);
				tp[i*4+2] = ss.obtenerSprite((int)(3*w*scales.X), i*4, (int)(w*scales.X), (int)(h*scales.Y-i*4), g);
				tp[i*4+3] = ss.obtenerSprite((int)(0*w*scales.X), i*4, (int)(w*scales.X), (int)(h*scales.Y-i*4), g);
			}
			//tp[tp.length - 1] = new BufferedImage(tp[0].getWidth(), tp[0].getHeight(), BufferedImage.TYPE_INT_ARGB);
	
	
			Sprite[] sp = new Sprite[(int)(h*scales.Y) + 1];
	
			for (int i = 0; i < h*scales.Y / 4; i++) {
				sp[i*4] = ss.obtenerSprite((int)(8 * w * scales.X), (int)(h*scales.Y), (int)(w*scales.X), (int)(h*scales.Y), g);
				sp[i*4+1] = ss.obtenerSprite((int)(9 * w * scales.X), (int)(h*scales.Y), (int)(w*scales.X), (int)(h*scales.Y), g);
				sp[i*4+2] = ss.obtenerSprite((int)(10 * w * scales.X), (int)(h*scales.Y), (int)(w*scales.X), (int)(h*scales.Y), g);
				sp[i*4+3] = ss.obtenerSprite((int)(11 * w * scales.X), (int)(h*scales.Y), (int)(w*scales.X), (int)(h*scales.Y), g);
			}
	
			this.left = new Animation(walkingLeft, 10, Animation.Direction.LEFT);
			this.right = new Animation(walkingRight, 10, Animation.Direction.RIGHT);
			this.up = new Animation(walkingUp, 10, Animation.Direction.UP);
			this.down = new Animation(walkingDown, 10, Animation.Direction.DOWN);
			this.death = new Animation(die, 10, Animation.Direction.DOWN);
			
			this.teleport = new Animation(tp, (int)(8-scales.X), Animation.Direction.DOWN);
			this.sparks = new Animation(sp, 10, Animation.Direction.DOWN);
	
			// Animacion inicial
			this.animation = down;
		}
	
		public override void tick(InputHandler input) {
			if(isInvincible())
				invincible--;
			else 
				invincibleClock.Reset();
			if(animation != teleport) {
				if (health <= 0) {
					if (!deathPlayed) {
						//Sound.death.play(); // Para que solo se reproduzca una vez
						deathPlayed = true;
					}
					die();
				} else {
		
					if (input.left.down) {
						position.X -= (scale.X + velocity);
						animation = left;
						animation.start();
					}
		
					else if (input.right.down) {
						position.X += (scale.X + velocity);
						animation = right;
						animation.start();
					}
		
					else if (input.up.down) {
						position.Y -= (scale.Y + velocity);
						animation = up;
						animation.start();
					}
		
					else if (input.down.down) {
						position.Y += (scale.Y + velocity);
						animation = down;
						animation.start();
					}
		
					else {
						animation.stop();
						animation.reset();
					}
				}
				animation.tick();
			}
			else {
				teleport.tick();
				sparks.tick();
			}
		}
	
		public override void Render() {
			if(animation != teleport) {
				if((invincible - invincibleClock.ElapsedMilliseconds) / 5 % 2 == 0) {
					// g.fillRect(position.x, position.y, 14 * scale, 11 * scale);
					Sprite f = animation.getSprite();
					f.Position.X = position.X + position.Width/2 - (f.Width - 1*scale.X)/ 2;
					f.Position.Y = position.Y + position.Height/2 - (f.Height + 11* scale.Y)/2;
					f.Render();
					// g.fillRect(position.x+position.width/2-f.getWidth()/2,
					// position.y+position.height/2-f.getHeight()/2, w*scale, h*scale);
				}
				else {
					Sprite f = animation.getSprite();
					f.SetColor(255, 255, 255, 255);
					f.Position.X = position.X + position.Width/2 - (f.Width - 1*scale.X)/ 2;
					f.Position.Y = position.Y + position.Height/2 - (f.Height + 11* scale.Y)/2;
					f.Render();
				}
			}
			else {
				Sprite f = teleport.getSprite();
				if (f != null) {
					f.Position.X = position.X + position.Width/2 - (f.Width - 1*scale.X)/2;
					f.Position.Y = position.Y + position.Height/2 - (f.Height + 11*scale.Y)/2 + teleport.getCurrentFrame()/scale.Y;
					f.Render();
				}
				f = sparks.getSprite();
				f.Position.X = position.X + position.Width/2 - (f.Width - 1*scale.X)/2;
				f.Position.Y = position.Y + position.Height/2 - (f.Height + 11*scale.Y)/2;
				f.Render();
			}
	
		}
	
		public override void touchedBy(Entity go) {
			if (go is Flare) {
				if(!isInvincible())
					this.hurt(go, 10);
			}
			if(go is Exit) {
				//this.position.x = go.position.x;
				//this.position.y = go.position.y;
				this.animation = teleport;
				teleport.start();
				sparks.start();
			}
		}
	
		public override Rectangle getBounds() {
			return new Rectangle(position.X + 5*scale.X, position.Y + 10 * scale.Y,
					14 * scale.X, 11 * scale.Y);
		}
	
		public void addPowerUp(Entity powerup) {
			// Añadir mejoras del power up
			//Sound.powerup.play();
			switch (powerup.getType()) {
			case 0:
				if(potency < max_potency)
					potency += 1;
				break;
			case 1:
				if(bombs < max_bombs)
					bombs += 1;
				break;
			case 2:
				remoteDetonator = true;
				break;
			case 3:
				if (velocity < max_velocity)
					velocity += 1;
				break;
			case 4:
				canPassBombs = true;
				break;
			case 5:
				canPassWalls = true;
				break;
			case 6:
				invincible = 600000;
				break;
			case 7:
				lives += 1;
				break;
			case 8:
				break;
			}
			powerup.remove();
		}
	
		public int getLives() {
			return this.lives;
		}
	
		public override int getScore() {
			return this.score;
		}
	
		public void setScore(int score) {
			this.score = score;
		}
	
		public void setLives(int lives) {
			this.lives = lives;
		}
	
		public int getPotency() {
			return potency;
		}
	
		public int getBombs() {
			return bombs;
		}
		
		public bool isTeleporting() {
			return animation == teleport;
		}
		
		public bool endLvl() {
			return animation == teleport && animation.finalFrame();
		}
		
		public bool endLvlFirst() {
			return animation == teleport;
		}
		
		public void reset() {
			if(removed) {
		   		this.canPassBombs = false;
		   		this.canPassWalls = false;
		   		this.remoteDetonator = false;
		   		this.deathPlayed = false;
	   		}
			this.position.X = g.Screen.Width/2 - map.getMapSize().X/2 + 33*scale.X;
			this.position.Y = 45*scale.Y;
			this.removed = false;
			this.health = 10;
	   		this.animation = down;
	   		this.invincible = 0;
			if (cheats) {
				this.lives = 9;
				this.bombs = 8;
				this.potency = 8;
				this.invincible = 999999999;
			}
		}
		
		public void resetAnim() {
	   		this.animation = down;
		}
		
		public bool playerCanPassWalls() {
			return canPassWalls;
		}
		
		public bool playerCanPassBombs() {
			return canPassBombs;
		}
		
		public bool hasRemoteDetonator() {
			return remoteDetonator;
		}
		
		public override bool isInvincible() {
			return (invincible - invincibleClock.ElapsedMilliseconds) > 0;
		}
	
		public bool isDying() {
			return animation == death;
		}
		
		public bool isDyingFirst() {
			return deathPlayed;
		}
	}
}

