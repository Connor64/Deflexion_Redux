using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Deflexion_Redux {
    class Turret : Enemy {
        private Sprite baseSprite;
        private Sprite gunSprite;

        private float elapsedTime = 0;

        public Turret(Vector2 position) {
            this.position = position;

            Texture2D baseTex = enemyManager.turret_base;
            Texture2D gunTex = enemyManager.turret_gun;

            baseSprite = new Sprite(baseTex, position, 0, Vector2.One, Sprite.Layers["Turret"], Vector2.Zero);
            gunSprite = new Sprite(gunTex, position + new Vector2(baseTex.Width/2, baseTex.Height/2), 0, Vector2.One, Sprite.Layers["Turret"] - 0.01f, new Vector2(gunTex.Width/2, gunTex.Height/2));
        }

        public override void Update(Vector2 playerPosition, float deltaTime) {
            Vector2 camPos = cam.position;
            bool inBounds = !(position.Y < -camPos.Y || position.Y > -camPos.Y + cam._Height || position.X < -camPos.X || position.X > -camPos.X + cam._Width);
            if (inBounds) {
                shoot(deltaTime);
                followPlayer(playerPosition);
            }
        }

        public void followPlayer(Vector2 playerPosition) {
            float x = gunSprite.Position.X - playerPosition.X;
            float y = gunSprite.Position.Y - playerPosition.Y;

            gunSprite.Rotation = -(float)Math.Atan2(x, y);
        }

        public void shoot(float deltaTime) {
            elapsedTime += deltaTime;
            if (elapsedTime >= 2f) {
                enemyManager.enemyBullets.Add(new Bullet(enemyManager.bullet, position, gunSprite.Rotation, BulletType.Enemy));
                elapsedTime = 0;
            }
        }

        public override void Draw(SpriteBatch spriteBatch) {
            baseSprite.draw(spriteBatch);
            gunSprite.draw(spriteBatch);
        }
    }
}
