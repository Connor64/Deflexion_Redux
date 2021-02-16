using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public enum BulletType { Player, Enemy, World };

namespace Deflexion_Redux {
    class Bullet : Physics {

        private Sprite bulletSprite;
        public bool isActive = true;
        private float elapsedTime = 0;

        private BulletType bulletType;

        Camera cam = Camera.Instance;

        public Bullet(Texture2D bulletTexture, Vector2 startingPosition, float rotation, BulletType bulletType) {
            mass = 0.01f;
            baseSpeedLimit = 750f;
            speedLimit = baseSpeedLimit;
            bodyType = BodyType.Bullet;
            collisionBoxSize = 1f;
            position = startingPosition;
            resistance = 0f;
            bulletSprite = new Sprite(bulletTexture, position, 0, Vector2.One, Sprite.Layers["Bullets"], new Vector2(8, 8));
            Vector2 direction = new Vector2(MathF.Cos(rotation - MathF.PI / 2), MathF.Sin(rotation - MathF.PI / 2));
            addForce(direction, 750f, 0);

            this.bulletType = bulletType;
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
