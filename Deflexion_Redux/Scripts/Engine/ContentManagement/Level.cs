using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework.Content;
using System.Xml.Serialization;

namespace Deflexion_Redux {
    public class Level {
        public int width;
        public int height;
        public string name;
        public Tile[] foregroundTiles;
        public Tile[] backgroundTiles;
        public EnemyTile[] enemies;

        public Level(Tile[,] foregroundTileArray, Tile[,] backgroundTileArray, EnemyTile[,] enemyArray) {
            width = foregroundTileArray.GetLength(0);
            height = foregroundTileArray.GetLength(1);
            foregroundTiles = new Tile[width * height];
            backgroundTiles = new Tile[width * height];
            enemies = new EnemyTile[width * height];
            int i = 0;
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    if (foregroundTileArray[x, y] == null) {
                        foregroundTiles[i] = new Tile();
                    } else {
                        foregroundTiles[i] = foregroundTileArray[x, y];
                    }

                    if (backgroundTileArray[x, y] == null) {
                        backgroundTiles[i] = new Tile();
                    } else {
                        backgroundTiles[i] = foregroundTileArray[x, y];
                    }

                    enemies[i] = enemyArray[x, y];
                    backgroundTiles[i] = backgroundTileArray[x, y];
                    i++;
                }
            }
        }

        public Level() {} // to allow for serialization
    }
}
