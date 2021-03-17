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
        public Tile[,] foregroundTiles;
        public Tile[,] backgroundTiles;
        public EnemyTile[,] enemies;

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
                foregroundTiles = new Tile[200, 100];
                backgroundTiles = new Tile[200, 100];
                enemies = new EnemyTile[200, 100];
                for (int x = 0; x < 200; x++) {
                    for (int y = 0; y < 100; y++) {
                        foregroundTiles[x, y] = new Tile();
                        enemies[x, y] = new EnemyTile(EnemyType.none, Vector2.Zero);
                    }
                }
            } else {
                Level currentLevel = AssetManager.levels[level];
                int width = currentLevel.width;
                int height = currentLevel.height;

                foregroundTiles = new Tile[width, height];
                backgroundTiles = new Tile[width, height];
                enemies = new EnemyTile[width, height];
                
                Tile[] newTiles = currentLevel.foregroundTiles;
                Tile[] newBackTiles = currentLevel.backgroundTiles;
                EnemyTile[] newEnemies = currentLevel.enemies;

                int i = 0;
                for (int x = 0; x < width; x++) {
                    for (int y = 0; y < height; y++) {
                        foregroundTiles[x, y] = newTiles[i];
                        backgroundTiles[x, y] = newBackTiles[i];
                        enemies[x, y] = newEnemies[i];
                        i++;
                    }
                }
                enemyManager.appendEnemies(enemies);
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            foreach (Tile tile in foregroundTiles) {
                if (tile != null && !tile.empty) {
                    tile.Draw(spriteBatch);
                }
            }
        }

        public void DrawBackground(SpriteBatch spriteBatch) {
            foreach(Tile tile in backgroundTiles) {
                if (tile != null && !tile.empty) {
                    tile.Draw(spriteBatch);
                }
            }
        }
    }
}