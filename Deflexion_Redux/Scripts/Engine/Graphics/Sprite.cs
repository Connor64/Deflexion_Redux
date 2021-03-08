using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Deflexion_Redux {

    public enum LayerType {
        Canvas,
        Background,
        Tiles,
        Bullets,
        Enemies,
        Turret,
        Player,
        UI
    }

    public class Sprite {

        public static Dictionary<LayerType, float> Layers = new Dictionary<LayerType, float>() {
            {LayerType.Canvas,     1.00f},
            {LayerType.Background, 0.99f},
            {LayerType.Tiles,      0.95f},
            {LayerType.Bullets,    0.90f},
            {LayerType.Enemies,    0.75f},
            {LayerType.Turret,     0.70f},
            {LayerType.Player,     0.50f},
            {LayerType.UI,         0.10f},
        };

        public TextureType textureType;
        public Vector2 Position;
        public float Rotation;
        public Vector2 Origin = Vector2.Zero;
        public Vector2 Scale;
        public float Layer;

        public Vector2 Size;
        public int width;
        public int height;

        private Camera cam = Camera.Instance;

        public Sprite(TextureType textureType, Vector2 position, float rotation, float layer, Vector2 scale) {
            this.textureType = textureType;
            Position = position;
            Rotation = rotation;
            Scale = scale;
            Layer = layer;
            width = AssetManager.textures[textureType].Width;
            height = AssetManager.textures[textureType].Height;

            Size = new Vector2(AssetManager.textures[textureType].Width * scale.X, AssetManager.textures[textureType].Height * scale.Y);
        }

        public Sprite(TextureType textureType, Vector2 position, float rotation, float layer, Vector2 scale, Vector2 origin) : this(textureType, position, rotation, layer, scale) {
            Origin = origin;
        }

        public bool isHovering(float boundsScale) {
            Vector2 mousePosition = cam.getMousePosition();

            return (mousePosition.X > Position.X - (Origin.X * Scale.X * boundsScale) &&
                    mousePosition.X < Position.X + (Size.X * boundsScale) - (Origin.X * Scale.X) &&
                    mousePosition.Y > Position.Y - (Origin.Y * Scale.Y * boundsScale) &&
                    mousePosition.Y < Position.Y + (Size.Y * boundsScale) - (Origin.Y * Scale.Y));
        }

        protected Sprite() {}

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(AssetManager.textures[textureType], Position, new Rectangle(0, 0, width, height), Color.White, Rotation, Origin, Scale * cam.spriteScalar, SpriteEffects.None, Layer);
        }
    }
}
