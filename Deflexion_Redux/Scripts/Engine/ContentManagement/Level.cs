﻿using System;
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
        public Tile[] tiles;
        public EnemyTile[] enemies;

        public Level(Tile[,] tileArray, EnemyTile[,] enemyArray) {
            width = tileArray.GetLength(0);
            height = tileArray.GetLength(1);
            tiles = new Tile[width * height];
            enemies = new EnemyTile[width * height];
            int i = 0;
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    tiles[i] = tileArray[x, y];
                    enemies[i] = enemyArray[x, y];
                    i++;
                }
            }
        }

        public Level() {} // to allow for serialization
    }
}
