using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;

namespace DynaBlasterVita
{
	public class Enemy : Mob
	{
		
		protected int xdir, ydir, speed;
	
		protected GenerateObstacles obs;
		protected Bomberman player;
		protected int score;
	
		protected static Random r = new Random();
	
		protected Rectangle lastpos;
		
		protected bool move = true;
	
		public Enemy(GenerateObstacles obs, Map map, Bomberman player, GraphicsContext g, Vector2 scale) : base(g ,scale) {
			this.obs = obs;
			this.player = player;
			this.speed = (int)(1*scale.X/2);
			while (!located)
				findStartPos(map);
	
			switch (r.Next(3)) {
			case 0:
				xdir = speed;
				ydir = 0;
				break;
			case 1:
				xdir = -speed;
				ydir = 0;
				break;
			case 2:
				xdir = 0;
				ydir = speed;
				break;
			case 3:
				xdir = 0;
				ydir = -speed;
				break;
			}
		}
	
		public override void tick() {
			if (!removed)
				animation.tick();
			if (health <= 0)
				die();
			else {
				if (move) {
					this.position.X += xdir;
					this.position.Y += ydir;
	
					bool isinsquare = position.Equals(lastpos);
					lastpos = new Rectangle(position.Position, position.Size);
					if (!isinsquare)
						foreach (Obstacle rect in obs.getPath()) {
							if (rect.getBounds().contains(position)) {
								isinsquare = true;
								break;
							}
						}
	
					if (isinsquare) {
						switch (r.Next(30)) {
						case 0:
							xdir = speed;
							ydir = 0;
							break;
						case 1:
							xdir = -speed;
							ydir = 0;
							break;
						case 2:
							xdir = 0;
							ydir = speed;
							break;
						case 3:
							xdir = 0;
							ydir = -speed;
							break;
						}
					}
				}
			}
			/*
			 * else { // IA muy rudimentaria if (player.position.x >
			 * this.position.x) { this.position.x++; } else { this.position.x--; }
			 * 
			 * if (player.position.y > this.position.y) { this.position.y++; } else
			 * { this.position.y--; } }
			 */
		}
		
		public override void Render() {
			//g.setColor(Color.CYAN);
			//g.fillRect(position.x, position.y, 12*scale, 14*scale);
			Sprite f = animation.getSprite();
			f.Position.X = position.X+position.Width/2 - (f.Width)/2;
			f.Position.Y = position.Y+position.Height/2 - (f.Height+3*scale.Y)/2;
			f.Render();
		}
	
		public override void touchedBy(Entity entity) {
			if (health > 0 && entity is Bomberman) {
				if(!entity.isInvincible())
					entity.hurt(this, 10); // BIEN
			}
		}
	
		public override bool findStartPos(Map map) {
			for (int y = 0; y < map.getmapHeight() - obs.getOffsetY() * 2; y++) {
				for (int x = 0; x < map.getmapWidth() - obs.getOffsetX() * 2; x++) {
					if (x == 0 && y == 0 || x == 1 && y == 0 || x == 0 && y == 1) {
						break;
					}
					else {
						if (!obs.obstacleAt(x, y, scale) && random.NextDouble() >= 0.99) {
							this.position.X = g.Screen.Width/2 - map.getMapSize().X/2 + x*obs.getTileSize().X + obs.getOffsetX()*obs.getTileSize().X;// - 8*scale.X;
							this.position.Y = y*obs.getTileSize().Y + obs.getOffsetY()*obs.getTileSize().Y + 24*scale.Y;
							located = true;
							goto located;
						}
					}
				}
			}
		located: return true;
		}
	
		public override int getScore() {
			return score;
		}
	
		public static Enemy createEnemy(int type, GenerateObstacles obs, Map map, Bomberman player, GraphicsContext g, Vector2 scale) {
			switch (type) {
			case 0:
				return new Balloon(obs, map, player, g, scale);
			case 1:
				return new BalloonPurple(obs, map, player, g, scale);
			case 2:
				return new BalloonBlue(obs, map, player, g, scale);
			case 3:
				return new BalloonRed(obs, map, player, g, scale);
			case 4:
				return new DragonPurple(obs, map, player, g, scale);
			case 5:
				return new GhostYellow(obs, map, player, g, scale);
//			case 6:
//				return new SnakeHead(obs, map, player);
			case 7:
				return new CrocodileGreen(obs, map, player, g, scale);
			case 8:
				return new GarlicBlue(obs, map, player, g, scale);
			case 9:
				return new FishGreen(obs, map, player, g, scale);
			case 10:
				return new GhostBlue(obs, map, player, g, scale);
			case 11:
				return new CoinYellow(obs, map, player, g, scale);
			case 12:
				return new GhostPink(obs, map, player, g, scale);
			case 13:
				return new Flame(obs, map, player, g, scale);
			case 14:
				return new DuckPurple(obs, map, player, g, scale);
			case 15:
				return new GhostWhite(obs, map, player, g, scale);
			case 16:
				return new Spinning(obs, map, player, g, scale);
			case 17:
				return new FrogYellow(obs, map, player, g, scale);
			case 18:
				return new GhostGreen(obs, map, player, g, scale);
			}
			return null;
		}
	}
}

