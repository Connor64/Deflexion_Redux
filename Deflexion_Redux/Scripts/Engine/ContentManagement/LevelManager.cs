using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Deflexion_Redux {
    class LevelManager {
        public Tile[,] tiles;

        public Vector2 startingPosition = Vector2.Zero;
        private EnemyManager enemyManager = EnemyManager.Instance;

        private static LevelManager instance;
        public static LevelManager Instance {
            get {
                if (instance == null) {
                    instance = new LevelManager();
                }
                return instance;
            }
        }

        public void Load(LevelType level, string entityPath) {
            if (level == LevelType.new_level) {
                tiles = new Tile[200, 100];
                for (int x = 0; x < 200; x++) {
                    for (int y = 0; y < 100; y++) {
                        tiles[x, y] = new Tile();
                    }
                }
            } else {
                Level currentLevel = AssetManager.levels[level];
                int width = currentLevel.width;
                int height = currentLevel.height;
                tiles = new Tile[width, height];
                Tile[] newTiles = currentLevel.tiles;

                int i = 0;
                for (int x = 0; x < width; x++) {
                    for (int y = 0; y < height; y++) {
                        tiles[x, y] = newTiles[i];
                        i++;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            foreach (Tile tile in tiles) {
                if (!tile.empty) {
                    tile.Draw(spriteBatch);
                }
            }
        }

        int[,] readLevel(Bitmap image) {
            int[,] data = new int[image.Width, image.Height];
            System.Drawing.Color pixel;
            string htmlColor;

            Debug.Print("" + image.Width);

            for (int x = 0; x < image.Width; x++) {
                for (int y = 0; y < image.Height; y++) {
                    pixel = image.GetPixel(x, y);
                    htmlColor = ColorTranslator.ToHtml(pixel);

                    switch (htmlColor) {
                        case "#000000": // Black (technically also transparent so backgrounds must be a color) - Turret
                            data[x, y] = 1;
                            break;
                        case "#FF0000": // Red - Drone
                            data[x, y] = 2;
                            break;
                        case "#00FF00": // Green - Player
                            data[x, y] = 3;
                            break;
                        default:
                            data[x, y] = 0;
                            break;
                    }
                }
            }
            return data;
        }
    }
}