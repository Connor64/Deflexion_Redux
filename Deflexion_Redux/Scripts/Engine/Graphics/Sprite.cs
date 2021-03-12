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

    public enum OriginType {
        Center,
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
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
        private Vector2 defaultScale;
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
            defaultScale = scale;
            Layer = layer;
            width = AssetManager.textures[textureType].Width;
            height = AssetManager.textures[textureType].Height;

            Size = new Vector2(AssetManager.textures[textureType].Width * scale.X, AssetManager.textures[textureType].Height * scale.Y) * cam.scalar;
        }

        public Sprite(TextureType textureType, Vector2 position, float rotation, float layer, Vector2 scale, Vector2 origin) : this(textureType, position, rotation, layer, scale) {
            Origin = origin;
        }

        public Sprite(TextureType textureType, Vector2 position, float rotation, float layer, Vector2 scale, OriginType origin) : this(textureType, position, rotation, layer, scale) {
            switch(origin) {
                case OriginType.Center:
                    Origin = new Vector2(width / 2, height / 2);
                    break;
                case OriginType.TopLeft:
                    Origin = Vector2.Zero;
                    break;
                case OriginType.TopRight:
                    Origin = new Vector2(width, 0);
                    break;
                case OriginType.BottomLeft:
                    Origin = new Vector2(0, height);
                    break;
                case OriginType.BottomRight:
                    Origin = new Vector2(width, height);
                    break;
            }
        }

        public void setScale(float scalar) {
            Scale = defaultScale * scalar;
            Size = new Vector2(AssetManager.textures[textureType].Width * Scale.X, AssetManager.textures[textureType].Height * Scale.Y) * cam.scalar;
        }

        public bool isHovering(float boundsScale, bool scaled) {
            Vector2 mousePosition = cam.getMousePosition(scaled);

            return (mousePosition.X > Position.X - (Origin.X * Scale.X * boundsScale) &&
                    mousePosition.X < Position.X + (Size.X * boundsScale) - (Origin.X * Scale.X) &&
                    mousePosition.Y > Position.Y - (Origin.Y * Scale.Y * boundsScale) &&
                    mousePosition.Y < Position.Y + (Size.Y * boundsScale) - (Origin.Y * Scale.Y));
        }

        protected Sprite() {}

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(AssetManager.textures[textureType], Position, new Rectangle(0, 0, width, height), Color.White, Rotation, Origin, Scale * cam.scalar, SpriteEffects.None, Layer);
        }
    }
}