using System;
using System.IO;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;

namespace DynaBlasterVita
{
	public class Map
	{
		public static int TILESIZE = 16;
		private static String path1 = "/Application/res/maps/";
		private static String path2 = ".txt";
	
		private String mapPath;
	
		private int x;
		private int y;
	
		private int tileSize;
		private int[,] map;
		private int mapWidth;
		private int mapHeight;
		private Sprite finalMap;
		private GraphicsContext graphics;
		private Vector2 scale;
	
		private SpriteSheet tileset;
		private Tile[,] tiles;
	
		public Map(String s, int tileSize, Vector2 scale, GraphicsContext graphics) {
			if (s != null) {
				this.mapPath = path1 + s + path2;
				this.tileSize = tileSize;
				this.graphics = graphics;
				this.scale = scale;
	
				try {
					
					using (StreamReader sr = new StreamReader(File.OpenRead(mapPath))) 
					{
						this.mapWidth = Convert.ToInt32(sr.ReadLine());
						this.mapHeight = Convert.ToInt32(sr.ReadLine());
						this.map = new int[mapHeight, mapWidth];
						
						for (int row = 0; row < mapHeight; row++) {
							String line = sr.ReadLine();
							String[] tokens = line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
							for (int col = 0; col < mapWidth; col++) {
								this.map[row, col] = Convert.ToInt32(tokens[col]);
							}
						}
					}
				} catch (Exception e) {
					Console.Error.Write(e.Message);
				}
			}
			
			String map = null;
			switch (s) {
				case "map1_2_square": case "map1_2_height":
					map = "/Application/res/maps/map1.png";
					break;
				case "map2_width": case "map2_square": case "map2_height":
					map = "/Application/res/maps/map2.png";
					break;
				case "map3_width": case "map3_height":
					map = "/Application/res/maps/map3.png";
					break;
				case "map4_width": case "map4_height":
					map = "/Application/res/maps/map4.png";
					break;
				case "map5_width": case "map5_height":
					map = "/Application/res/maps/map5.png";
					break;
				case "map6_width": case "map6_height":
					map = "/Application/res/maps/map6.png";
					break;
				case "map7_width": case "map7_height":
					map = "/Application/res/maps/map7.png";
					break;
				case "map8_width": case "map8_height":
					map = "/Application/res/maps/map8.png";
					break;
				default:
					map = "/Application/res/maps/map1.png";
					break;
					
			}
			
			SpriteLoader loader = new SpriteLoader();
			Image spriteSheet = loader.cargarImagen(map);
			SpriteSheet ss = new SpriteSheet(spriteSheet);
			loadTiles(ss);
			saveImagetoFile(scale);
		}
	
		public void loadTiles(SpriteSheet s) {
			try {
				tileset = s;
	
				int numTilesAcross = (tileset.getWidth() + 1) / tileSize;
				tiles = new Tile[2, numTilesAcross];
	
				Image subimage;
				for (int col = 0; col < numTilesAcross; col++) {
					subimage = tileset.obtenerSprite(col * tileSize, 0, tileSize,
							tileSize);
					tiles[0, col] = new Tile(subimage, false);
					subimage = tileset.obtenerSprite(col * tileSize, tileSize,
							tileSize, tileSize);
					tiles[1, col] = new Tile(subimage, true);
				}
	
			} catch (Exception e) {
				Console.Error.WriteLine(e.Message);
			}
	
		}
	
		public void saveImagetoFile(Vector2 scale) {
			
			int width = this.mapWidth*this.tileSize;
	        int height = this.mapHeight*this.tileSize;
	        Image newImage = new Image(ImageMode.Rgba, new ImageSize(width, height), new ImageColor(0, 0, 0, 0));
			
			int error = 0;
			for (int row = 0; row < this.mapHeight; row++) {
				for (int col = 0; col < this.mapWidth; col++) {
	
					int rc = map[row, col];
	
					int r = rc / tiles.GetLength(1);
					int c = rc % tiles.GetLength(1);
	
					// Apaño provisional, el techo izquierdo no esta rotado en los
					// sprites
					if (rc >= 25 && rc <= 27)
						error = 8;
					else
						error = 0;
					newImage.DrawImage(tiles[r, c].getImage(), new ImagePosition(error + x + col * tileSize,
							y + row * tileSize));
	
				}
			}
			Image aux = newImage.Resize(new ImageSize((int)(width*scale.X), (int)(height*scale.Y)));
			Texture2D texture = new Texture2D(aux.Size.Width, aux.Size.Height, false, PixelFormat.Rgba);
			texture.SetPixels(0, aux.ToBuffer(), PixelFormat.Rgba);
			this.finalMap = new Sprite(graphics, texture);
			//this.finalMap.Position.X = -8*getScale();
			//this.finalMap.Position.Y = 24*getScale();
		}
	
		public int getx() {
			return x;
		}
	
		public int gety() {
			return y;
		}
		
		public int getmapWidth() {
			return mapWidth;
		}
		
		public int getmapHeight() {
			return mapHeight;
		}
		
	
		public int getTile(int row, int col) {
			return map[row, col];
		}
	
		public int getTileSize() {
			return tileSize;
		}
		
		public Vector2 getMapSize() {
			return new Vector2(finalMap.Width, finalMap.Height);
		}
		
		public SpriteSheet getSpriteSheet() {
			return tileset;
		}
		
		public Vector2 getScale() {
			return scale;
		}
	
		public bool isBlocked(int row, int col) {
			int rc = map[row, col];
			int r = rc / tiles.GetLength(1);
			int c = rc % tiles.GetLength(1);
			return tiles[r, c].isBlocked();
		}
	
		public void render(GraphicsContext g) {
	
			int error = 0;
			for (int row = 0; row < mapHeight; row++) {
				for (int col = 0; col < mapWidth; col++) {
	
					int rc = map[row, col];
	
					int r = rc / tiles.GetLength(1);
					int c = rc % tiles.GetLength(1);
	
	
					// Apaño provisional, el techo izquierdo no esta rotado en los
					// sprites
					if (rc >= 25 && rc <= 27)
						error = 8;
					else
						error = 0;
					
					//g.drawImage(tiles[r][c].getImage(), error + x + col * tileSize,
					//		y + row * tileSize, null);
	
				}
			}
	
		}
		
		public void renderMap() {
			this.finalMap.Position.X = graphics.Screen.Width/2 - finalMap.Width/2;
			this.finalMap.Position.Y = 24*scale.Y;
			this.finalMap.Render();
		}
	}
}