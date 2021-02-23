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

        public AnimatedSprite(TextureType textureType, Vector2 startingPosition, float rotation, Vector2 scale, int rows, int columns, int pixelWidth, float layer) {
            this.textureType = textureType;
            Position = startingPosition;
            Rows = rows;
            Columns = columns;
            Rotation = rotation;
            Origin = Vector2.Zero;
            Layer = layer;
            Scale = scale;
            width = AssetManager.textures[textureType].Width;
            height = AssetManager.textures[textureType].Height;
            totalFrames = rows * columns;
            Size = new Vector2((AssetManager.textures[textureType].Width / columns) * scale.X, (AssetManager.textures[textureType].Height / rows) * scale.Y); // The true size of the scaled sprite shown on screen (not the raw texture)
            this.pixelWidth = pixelWidth;
        }

        public AnimatedSprite(TextureType textureType, Vector2 startingPosition, float rotation, Vector2 origin, Vector2 scale, int rows, int columns, int pixelWidth, int startingFrame, float layer) {
            this.textureType = textureType;
            Position = startingPosition;
            Rows = rows;
            Columns = columns;
            Rotation = rotation;
            Origin = origin;
            Layer = layer;
            Scale = scale;
            width = AssetManager.textures[textureType].Width;
            height = AssetManager.textures[textureType].Height;

            sheetSize = new Vector2(width * scale.X, height * scale.Y);
            totalFrames = rows * columns;
            Size = new Vector2((AssetManager.textures[textureType].Width / columns) * scale.X, (AssetManager.textures[textureType].Height / rows) * scale.Y);
            this.pixelWidth = pixelWidth;

            currentFrame = startingFrame;
        }

        public AnimatedSprite(TextureType textureType, Vector2 startingPosition, float rotation, Vector2 origin, Vector2 scale, int rows, int columns, int pixelWidth, int startingFrame, int frameRate, float layer) {
            this.textureType = textureType;
            Position = startingPosition;
            Rows = rows;
            Columns = columns;
            Rotation = rotation;
            Origin = origin;
            Layer = layer;
            Scale = scale;
            width = AssetManager.textures[textureType].Width;
            height = AssetManager.textures[textureType].Height;

            sheetSize = new Vector2(width * scale.X, height * scale.Y);

            currentFrame = startingFrame;
            totalFrames = rows * columns;
            Size = new Vector2((AssetManager.textures[textureType].Width / columns) * scale.X, (AssetManager.textures[textureType].Height / rows) * scale.Y);
            this.pixelWidth = pixelWidth;
            this.frameRate = frameRate;
        }

        public AnimatedSprite() {}

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
            return new AnimatedSprite(textureType, Position, Rotation, Origin, Scale, Rows, Columns, pixelWidth, currentFrame, frameRate, Layer);
        }
    }
}