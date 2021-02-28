using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Deflexion_Redux {
    public class AnimatedSprite : Sprite {

        public int Rows;
        public int Columns;

        public int pixelWidth;
        public int frameRate = 0;
        public int currentFrame = 0;
        private int totalFrames;

        public Vector2 sheetSize;

        public AnimatedSprite(TextureType textureType, Vector2 startingPosition, float rotation, int pixelWidth, float layer, Vector2 scale) {
            this.textureType = textureType;
            Position = startingPosition;
            Rotation = rotation;
            this.pixelWidth = pixelWidth;
            Layer = layer;
            Scale = scale;

            width = AssetManager.textures[textureType].Width;
            height = AssetManager.textures[textureType].Height;
            Rows = height / pixelWidth;
            Columns = width / pixelWidth;

            totalFrames = Rows * Columns;

            sheetSize = new Vector2(width * scale.X, height * scale.Y);
            Size = new Vector2((AssetManager.textures[textureType].Width / Columns) * scale.X, (AssetManager.textures[textureType].Height / Rows) * scale.Y); // The true size of the scaled sprite shown on screen (not the raw texture)
        }

        public AnimatedSprite(TextureType textureType, Vector2 startingPosition, float rotation, int pixelWidth, float layer, Vector2 scale, Vector2 origin, int startingFrame) : this(textureType, startingPosition, rotation, pixelWidth, layer, scale) {
            Origin = origin;
            currentFrame = startingFrame;
        }

        public AnimatedSprite(TextureType textureType, Vector2 startingPosition, float rotation, int pixelWidth, float layer, Vector2 scale, Vector2 origin, int startingFrame, int frameRate) : this(textureType, startingPosition, rotation, pixelWidth, layer, scale, origin, startingFrame) {
            this.frameRate = frameRate;
        }

        public AnimatedSprite() {}

        public void setScale(Vector2 scale) {
            Scale = scale;
            sheetSize = new Vector2(width * scale.X, height * scale.Y);
        }

        public void Update() {
            currentFrame++;
            if (currentFrame == totalFrames) {
                currentFrame = 0;
            }
        }

        public void setFrame(int frame) {
            currentFrame = frame;
        }

        public void animDraw(SpriteBatch spriteBatch) {
            int row = currentFrame / Columns;
            int column = currentFrame % Columns;

            Rectangle sourceRectangle = new Rectangle(pixelWidth * column, pixelWidth * row, pixelWidth, pixelWidth);

            spriteBatch.Draw(AssetManager.textures[textureType], Position, sourceRectangle, Color.White, Rotation, Origin, Scale, SpriteEffects.None, Layer);
        }

        public AnimatedSprite animCopyOf() {
            return new AnimatedSprite(textureType, Position, Rotation, pixelWidth, Layer, Scale, Origin, currentFrame, frameRate);
        }
    }
}