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

        public Bullet(Texture2D bulletTexture, Vector2 startingPosition, float rotation, Vector2 bounds) {
            this.bounds = bounds;
            mass = 0.01f;
            baseSpeedLimit = 750f;
            speedLimit = baseSpeedLimit;
            collisionBoxSize = 8f;
            player = false;
            position = startingPosition;
            resistance = 0f;
            bulletSprite = new Sprite(bulletTexture, position, 0, new Vector2(1, 1), 1);
            Vector2 direction = new Vector2(MathF.Cos(rotation - MathF.PI / 2), MathF.Sin(rotation - MathF.PI / 2));
            addForce(direction, 750f, 0);
        }

        public void update() {
            if (isActive) {
                if (position.Y > bounds.Y || position.Y < 0 || position.X > bounds.X || position.X < 0) {
                    isActive = false;
                    //Debug.Print("inactive");
                }
                bulletSprite.Position = position;
            }
        }

        public void draw(SpriteBatch spriteBatch) {
            bulletSprite.draw(spriteBatch);
        }
    }
}
