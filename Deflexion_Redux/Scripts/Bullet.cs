using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Deflexion_Redux {
    class Bullet : Physics {

        private Sprite bulletSprite;
        private Vector2 bounds;
        public bool isActive = true;
        private float elapsedTime = 0;

        Camera cam = Camera.Instance;

        public Bullet(Texture2D bulletTexture, Vector2 startingPosition, float rotation, Vector2 bounds) {
            this.bounds = bounds;
            mass = 0.01f;
            baseSpeedLimit = 750f;
            speedLimit = baseSpeedLimit;
            collisionBoxSize = 8f;
            position = startingPosition;
            resistance = 0f;
            bulletSprite = new Sprite(bulletTexture, position, 0, new Vector2(1, 1), 0.9f);
            Vector2 direction = new Vector2(MathF.Cos(rotation - MathF.PI / 2), MathF.Sin(rotation - MathF.PI / 2));
            addForce(direction, 750f, 0);
        }

        public void update(float deltaTime) {
            if (isActive) {
                Vector2 camPos = cam.position;

                if (elapsedTime > 5 && (position.Y < -camPos.Y || position.Y > -camPos.Y + cam._Height || position.X < -camPos.X || position.X > -camPos.X + cam._Width)) {
                    isActive = false;
                }

                elapsedTime += deltaTime;

                bulletSprite.Position = position;
            }
        }

        public void draw(SpriteBatch spriteBatch) {
            bulletSprite.draw(spriteBatch);
        }
    }
}
