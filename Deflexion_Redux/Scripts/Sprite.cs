using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Deflexion_Redux {

    class Sprite {

        public static Dictionary<string, float> Layers = new Dictionary<string, float>() {
            {"Background", 1},
            {"Foreground", 0},
            {"Player", 0.5f},
            {"Enemies", 0.75f},
            {"Bullets", 0.9f},
            {"Tiles", 0.95f}
        };

        public Texture2D Texture;
        public Vector2 Position;
        public float Rotation;
        public Vector2 Origin;
        public Vector2 Scale;
        public float Layer;

        public Vector2 textureSize;

        public Sprite(Texture2D texture, Vector2 position, float rotation, Vector2 scale, float layer) {
            Texture = texture;
            Position = position;
            Rotation = rotation;
            Origin = Vector2.Zero;
            Scale = scale;
            Layer = layer;

            textureSize = new Vector2(texture.Width * scale.X, texture.Height * scale.Y);
        }

        public Sprite(Texture2D texture, Vector2 position, float rotation, Vector2 scale, float layer, Vector2 origin) {
            Texture = texture;
            Position = position;
            Rotation = rotation;
            Origin = origin;
            Scale = scale;
            Layer = layer;

            textureSize = new Vector2(texture.Width * scale.X, texture.Height * scale.Y);
        }

        protected Sprite() {}

        public void draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(Texture, Position, new Rectangle(0, 0, Texture.Width, Texture.Height), Color.White, Rotation, Origin, Scale, SpriteEffects.None, Layer);
        }
    }
}
