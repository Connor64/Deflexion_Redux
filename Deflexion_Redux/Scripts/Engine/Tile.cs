using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Deflexion_Redux {
    public class Tile {
        public bool empty;
        public AnimatedSprite sprite;
        public Vector2 position;
        public Vector2 bounds;
        public bool collidable;
        public float layer;
        public float rotation = 0;

        public Tile(TextureType textureType, Vector2 position, bool collidable, float layer, int pixelWidth, int tilePos) {
            empty = false;
            sprite = new AnimatedSprite(textureType, position, 0, Vector2.Zero, Vector2.One, AssetManager.textures[textureType].Height / pixelWidth, AssetManager.textures[textureType].Width / pixelWidth, pixelWidth, tilePos, layer);
            this.position = position;
            this.collidable = collidable;
            this.layer = layer;

            bounds = new Vector2(pixelWidth, pixelWidth);
        }

        public Tile() {
            empty = true;
        }

        public void Update() {
            sprite.Position = position;
            sprite.Rotation = rotation;
        }

        public void Draw(SpriteBatch spriteBatch) {
            sprite.animDraw(spriteBatch);
        }
    }
}