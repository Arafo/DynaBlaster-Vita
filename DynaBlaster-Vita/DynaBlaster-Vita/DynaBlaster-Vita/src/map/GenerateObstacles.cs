using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;


namespace DynBlasterVita
{
	public class GenerateObstacles
	{

		private double LIMIT = 0.8;
	
		private List<Obstacle> obstacles;
		private List<Obstacle> path;
	
		private int currentX;
		private int currentY;
		private int offsetX = 2;
		private int offsetY = 1;
		private int tileSize = 16;
		private bool canPlace = false;
		private GraphicsContext g;
		
		private Obstacle singleObstacle;
	
		public GenerateObstacles(Map map, bool solidObstacles, GraphicsContext g) {
			int finX = map.getmapWidth();
			int finY = map.getmapHeight();
			this.tileSize *= map.getScale();
			this.g = g;
			
			this.singleObstacle = new Obstacle(0, 0, tileSize, null, map.getSpriteSheet(), map.getScale(), true, g);
			
			if (finY > finX) finY--;
			
			obstacles = new List<Obstacle>();
			path = new List<Obstacle>();
	
			Random rn = new Random();
			while(true) {
				if (currentX >= (finX - offsetX * 2)) {
					currentY++;
					currentX = 0;
				}
				if (currentY >= (finY - offsetY * 2))
					break;
	
				if (solidObstacles && rn.NextDouble() >= LIMIT) {
	
					// Posiciones protegidas
					if (currentX == 0 && currentY == 0 || 
						currentX == 1 && currentY == 0 || 
						currentX == 0 && currentY == 1) continue;
	
					if (currentY % 2 == 0) { // Si la fila es par puedo colocar
						canPlace = true;
					} else {
						if (currentX % 2 == 0) { // Si la columna es par puedo colocar
							canPlace = true;
						} 
					}
	
					if (canPlace) {
						obstacles.Add(new Obstacle(singleObstacle, 
						                           currentX*tileSize + offsetX*tileSize, 
						                           currentY*tileSize + offsetY*tileSize,
						                           true,
						                           tileSize,
						                           map.getScale()));

						canPlace = false;
					}
				}
				// Camino libre con obstaculos
				if (currentY % 2 == 0 || currentX % 2 == 0) {
//					obstacles.Add(new Obstacle(singleObstacle, 
//					                           currentX*tileSize + offsetX*tileSize, 
//					                           currentY*tileSize + offsetY*tileSize,
//						                       true,
//						                       tileSize,
//						                       map.getScale()));
	
				}
				// Obstaculos fijos
				if (currentY%2 != 0 && currentX%2 != 0) {
					obstacles.Add(new Obstacle(singleObstacle, 
					                           currentX*tileSize + offsetX*tileSize, 
					                           currentY*tileSize + offsetY*tileSize,
						                       false,
						                       tileSize,
						                       map.getScale()));					
				}
				
				// Paredes laterales
				// Pared izquierda
				if (currentX == 1) {
					obstacles.Add(new Obstacle(singleObstacle, 
					                           currentX*tileSize, 
					                           currentY*tileSize + offsetY*tileSize,
						                       false,
						                       tileSize,
						                       map.getScale()));					
				}
				
				// Pared Derecha
				if (currentX == ((finX - offsetX*2) - 1)) {
					obstacles.Add(new Obstacle(singleObstacle, 
					                           currentX*tileSize + (offsetX+1)*tileSize, 
					                           currentY*tileSize + offsetY*tileSize,
						                       false,
						                       tileSize,
						                       map.getScale()));
				}
				
				// Pared superior
				if (currentY == 0) {
					obstacles.Add(new Obstacle(singleObstacle, 
					                           currentX*tileSize + offsetX*tileSize, 
					                           currentY*tileSize,
						                       false,
						                       tileSize,
						                       map.getScale()));					
				}
				
				// Pared inferior
				if (currentY == (finY - offsetY*2) - 1) {
					obstacles.Add(new Obstacle(singleObstacle, 
					                           currentX*tileSize + offsetX*tileSize, 
					                           currentY*tileSize + (offsetY+1)*tileSize,
						                       false,
						                       tileSize,
						                       map.getScale()));					
				}	
	
				currentX++;
			}
		}
	
		public void Render() {
			for (int i = 0; i < obstacles.Count; i++) {
				obstacles[i].draw(g);
			}
		}
		
		public List<Obstacle> getList() {
			return obstacles;
		}
		
		public List<Obstacle> getPath() {
			return path;
		}
		
		public bool obstacleAt(int x, int y, int scale) {
//			x = x*tileSize + offsetX*tileSize - 8*scale;
//			y = y*tileSize + offsetY*tileSize + 24*scale;
//			foreach (Obstacle obs in obstacles) {
//				if (new Rectangle(x, y, tileSize, tileSize).IntersectsWith(obs.position))
//					return true;
//			}
			return false;
		}
		
		public int getOffsetX() {
			return offsetX;
		}
		
		public int getOffsetY() {
			return offsetY;
		}
		
		public int getTileSize() {
			return tileSize;
		}
	}
}

