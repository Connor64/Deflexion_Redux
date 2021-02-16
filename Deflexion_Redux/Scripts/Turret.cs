using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Deflexion_Redux {
    class Turret {
        private Sprite baseSprite;
        private Sprite gunSprite;

        public Vector2 position;
        private Camera cam = Camera.Instance;

        private float elapsedTime = 0;
        public List<Bullet> bullets = new List<Bullet>();
        private Texture2D bulletTexture;

        public Turret(ContentManager content, Vector2 position) {
            this.position = position;
            Texture2D baseTexture = content.Load<Texture2D>("Sprites/turret_bottom");
            Texture2D gunTexture = content.Load<Texture2D>("Sprites/turret_top");

            bulletTexture = content.Load<Texture2D>("Sprites/shotgunBlast");

            baseSprite = new Sprite(baseTexture, position, 0, Vector2.One, Sprite.Layers["Turret"], Vector2.Zero);
            gunSprite = new Sprite(gunTexture, position + new Vector2(baseTexture.Width/2, baseTexture.Height/2), 0, Vector2.One, Sprite.Layers["Turret"] - 0.01f, new Vector2(gunTexture.Width/2, gunTexture.Height/2));

        }

        public void update(Vector2 playerPosition, float deltaTime) {
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
                bullets.Add(new Bullet(bulletTexture, position, gunSprite.Rotation, BulletType.Enemy));
                elapsedTime = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            baseSprite.draw(spriteBatch);
            gunSprite.draw(spriteBatch);
            foreach(Bullet bullet in bullets) {
                bullet.draw(spriteBatch);
            }
        }
    }
}
