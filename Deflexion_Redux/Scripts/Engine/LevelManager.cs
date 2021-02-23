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
        public Tile[,] tileArray;
        //public List<Sprite> tileSprites = new List<Sprite>();
        //public List<Tile> tiles = new List<Tile>();

        public Vector2 startingPosition = Vector2.Zero;
        //private Camera cam = Camera.Instance;
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
                tileArray = new Tile[200, 100];
                for (int x = 0; x < 200; x++) {
                    for (int y = 0; y < 100; y++) {
                        tileArray[x, y] = new Tile();
                    }
                }
            } else {
                Level currentLevel = AssetManager.levels[level];
                int width = currentLevel.width;
                int height = currentLevel.height;
                tileArray = new Tile[width, height];
                Tile[] tiles = currentLevel.tiles;

                int i = 0;
                for (int x = 0; x < width; x++) {
                    for (int y = 0; y < height; y++) {
                        tileArray[x, y] = tiles[i];
                        i++;
                    }
                }


                //int x = 0, y = 0;
                //for (int i = 0; i < tiles.Length; i++) {

                //    tileArray[x, y] = tiles[i];

                //    x++;
                //    if (x >= width) {
                //        x = 0;
                //        y++;
                //    }
                //}
            }

            //int[,] levelData = readLevel((Bitmap)Bitmap.FromFile(levelPath)); // make sure file is set to always copy to output in build actions in its properties in the solution explorer
            //int[,] entityData = readLevel((Bitmap)Bitmap.FromFile(entityPath));
            //int width = levelData.GetLength(0);
            //int height = levelData.GetLength(1);
            //tileArray = new Tile[width, height];
            //Debug.Print("X: " + width + "    Y: " + height);
            //for (int x = 0; x < width; x++) {
            //    for (int y = 0; y < height; y++) {
            //        Vector2 pos = new Vector2((x * 16) - (width * 8) - (width * 8) % 16, (y * 16) - (height * 8) - (height * 8) % 16);
            //        if (levelData[x, y] == 1) {
            //            //tileSprites.Add(new Sprite(tileTexture, pos, 0f, new Vector2(1, 1), Sprite.Layers["Tiles"]));
            //            tileArray[x, y] = new Tile(TextureType.test_tile, pos, true, Sprite.Layers["Tiles"], 16, 0);
            //            //tiles.Add(new Tile(tileTexture, pos, true, Sprite.Layers["Tiles"], 16, 0));
            //        }

            //        switch (entityData[x, y]) {
            //            case 1:
            //                //enemyManager.enemies.Add(new Turret(pos + new Vector2(8, 8)));
            //                break;
            //            case 2:
            //                //enemyManager.enemies.Add(new Drone(pos));
            //                break;
            //            case 3:
            //                startingPosition = pos;
            //                break;
            //        }
            //    }
            //}
        }

        public void Draw(SpriteBatch spriteBatch) {
            //foreach (Sprite tile in tileSprites) {
            //    tile.draw(spriteBatch);
            //}
            foreach (Tile tile in tileArray) {
                if (!tile.empty) {
                    tile.Draw(spriteBatch);
                }
            }
        }

        public Tile[,] getTiles() {
            return tileArray;
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