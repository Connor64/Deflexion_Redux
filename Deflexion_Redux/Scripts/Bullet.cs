using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public enum BulletType { Player, Enemy, World };

namespace Deflexion_Redux {
    public class Bullet : Physics {

        private Sprite bulletSprite;
        public bool isActive = true;
        private float elapsedTime = 0;

        //private BulletType bulletType;

        Camera cam = Camera.Instance;

        public Bullet(TextureType textureType, Vector2 startingPosition, float force, float rotation, float collisionScale) {
            bodyType = BodyType.Bullet;
            mass = 0.01f;
            screenTolerance = new Vector2(128, 128);
            baseSpeedLimit = force;
            speedLimit = baseSpeedLimit;
            collisionBoxSize = 1f * collisionScale;
            position = startingPosition;
            resistance = 0f;
            bulletSprite = new Sprite(textureType, position, 0, Vector2.One, Sprite.Layers["Bullets"], new Vector2(8, 8));
            Vector2 direction = new Vector2(MathF.Cos(rotation - MathF.PI / 2), MathF.Sin(rotation - MathF.PI / 2));
            addForce(direction, force, 0);

            //this.bulletType = bulletType;
        }

        public void update(float deltaTime) {
            PhysicsUpdate(deltaTime);

            if (elapsedTime > 3 && !cam.contains(position, screenTolerance)) {
                isActive = false;
            }

            elapsedTime += deltaTime;

            bulletSprite.Position = position;
        }

        public void draw(SpriteBatch spriteBatch) {
            bulletSprite.draw(spriteBatch);
        }
    }
}
