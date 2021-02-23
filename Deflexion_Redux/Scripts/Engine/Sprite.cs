using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Deflexion_Redux {
    public class Sprite {

        public static Dictionary<string, float> Layers = new Dictionary<string, float>() {
            {"Background", 1.00f},
            {"Tiles",      0.95f},
            {"Bullets",    0.90f},
            {"Enemies",    0.75f},
            {"Turret",     0.70f},
            {"Player",     0.50f},
            {"UI",         0.10f},
        };

        public TextureType textureType;
        public Vector2 Position;
        public float Rotation;
        public Vector2 Origin;
        public Vector2 Scale;
        public float Layer;

        public Vector2 Size;
        public int width;
        public int height;

        private Camera cam = Camera.Instance;

        public Sprite(TextureType textureType, Vector2 position, float rotation, Vector2 scale, float layer) {
            this.textureType = textureType;
            Position = position;
            Rotation = rotation;
            Origin = Vector2.Zero;
            Scale = scale;
            Layer = layer;
            width = AssetManager.textures[textureType].Width;
            height = AssetManager.textures[textureType].Height;

            Size = new Vector2(AssetManager.textures[textureType].Width * scale.X, AssetManager.textures[textureType].Height * scale.Y);
        }

        public Sprite(TextureType textureType, Vector2 position, float rotation, Vector2 scale, float layer, Vector2 origin) {
            this.textureType = textureType;
            Position = position;
            Rotation = rotation;
            Origin = origin;
            Scale = scale;
            Layer = layer;
            width = AssetManager.textures[textureType].Width;
            height = AssetManager.textures[textureType].Height;

            Size = new Vector2(AssetManager.textures[textureType].Width * scale.X, AssetManager.textures[textureType].Height * scale.Y);
        }

        public bool isHovering() {
            Vector2 mousePosition = cam.getMousePosition();

            return (mousePosition.X > Position.X - (Origin.X * Scale.X) &&
                    mousePosition.X < Position.X + Size.X - (Origin.X * Scale.X) &&
                    mousePosition.Y > Position.Y - (Origin.Y * Scale.Y) &&
                    mousePosition.Y < Position.Y + Size.Y - (Origin.Y * Scale.Y));
        }

        protected Sprite() {}

        public void draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(AssetManager.textures[textureType], Position, new Rectangle(0, 0, width, height), Color.White, Rotation, Origin, Scale, SpriteEffects.None, Layer);
        }
    }
}
