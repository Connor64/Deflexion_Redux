using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Deflexion_Redux {
    class AnimatedSprite : Sprite {

        public int Rows;
        public int Columns;

        public int pixelWidth;
        public int frameRate;
        private int currentFrame;
        private int totalFrames;

        public AnimatedSprite(Texture2D texture, Vector2 startingPosition, float rotation, Vector2 scale, int rows, int columns, int pixelWidth) {
            Texture = texture;
            Position = startingPosition;
            Rows = rows;
            Columns = columns;
            Rotation = rotation;
            Scale = scale;

            currentFrame = 0;
            totalFrames = rows * columns;
            textureSize = new Vector2((texture.Width / columns) * scale.X, (texture.Height / rows) * scale.Y); // The true size of the scaled sprite shown on screen (not the raw texture)
            this.pixelWidth = pixelWidth;
        }

        public AnimatedSprite(Texture2D texture, Vector2 startingPosition, float rotation, Vector2 origin, Vector2 scale, int rows, int columns, int pixelWidth) {
            Texture = texture;
            Position = startingPosition;
            Rows = rows;
            Columns = columns;
            Rotation = rotation;
            Origin = origin;
            Scale = scale;

            currentFrame = 0;
            totalFrames = rows * columns;
            textureSize = new Vector2((texture.Width / columns) * scale.X, (texture.Height / rows) * scale.Y);
            this.pixelWidth = pixelWidth;
        }

        public AnimatedSprite(Texture2D texture, Vector2 startingPosition, float rotation, Vector2 origin, Vector2 scale, int rows, int columns, int pixelWidth, int frameRate) {
            Texture = texture;
            Position = startingPosition;
            Rows = rows;
            Columns = columns;
            Rotation = rotation;
            Origin = origin;
            Scale = scale;

            currentFrame = 0;
            totalFrames = rows * columns;
            textureSize = new Vector2((texture.Width / columns) * scale.X, (texture.Height / rows) * scale.Y);
            this.pixelWidth = pixelWidth;
            this.frameRate = frameRate;
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

        public void Draw(SpriteBatch spriteBatch) {
            int row = currentFrame / Columns;
            int column = currentFrame % Columns;

            Rectangle sourceRectangle = new Rectangle(pixelWidth * column, pixelWidth * row, pixelWidth, pixelWidth);

            spriteBatch.Draw(Texture, Position, sourceRectangle, Color.White, Rotation, Origin, Scale, SpriteEffects.None, 0f);
        }
    }
}