using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Deflexion_Redux {
    class TileManager {
        public List<Sprite> tileSprites = new List<Sprite>();

        public TileManager(ContentManager content) {
            int[,] levelData = readLevel((Bitmap)Bitmap.FromFile(@"Content\poopyLevel.bmp")); // make sure file is set to always copy to output in build actions in its properties in the solution explorer
            Texture2D tileTexture = content.Load<Texture2D>("Sprites/Tile");
            for (int x = 0; x < levelData.GetLength(0); x++) {
                for (int y = 0; y < levelData.GetLength(1); y++) {
                    if (levelData[x, y] == 1) {
                        tileSprites.Add(new Sprite(tileTexture, new Vector2(x * tileTexture.Width * 2, y * tileTexture.Height * 2), 0f, new Vector2(2, 2), 0f));
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            foreach (Sprite tile in tileSprites) {
                tile.draw(spriteBatch);
            }
        }

        int[,] readLevel(Bitmap image) {
            int[,] levelData = new int[image.Width, image.Height];
            for (int x = 0; x < image.Width; x++) {
                for (int y = 0; y < image.Height; y++) {
                    int pixel = image.GetPixel(x, y).ToArgb();

                    if (pixel == System.Drawing.Color.Black.ToArgb()) {
                        levelData[x, y] = 1;
                    } else {
                        levelData[x, y] = 0;
                    }
                }
            }
            return levelData;
        }
    }
}