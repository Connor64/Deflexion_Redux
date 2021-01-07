using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Deflexion_Redux {
    class Bullet : Physics {

        public Sprite bulletSprite;

        public Bullet(Texture2D bulletTexture, Vector2 startingPosition, float rotation) {
            mass = 0.01f;
            baseSpeedLimit = 750f;
            speedLimit = baseSpeedLimit;
            collisionBoxSize = 8f;
            player = false;
            position = startingPosition;
            resistance = 0f;
            bulletSprite = new Sprite(bulletTexture, position, 0, new Vector2(2, 2), 0);
            Vector2 direction = new Vector2(MathF.Cos(rotation - MathF.PI / 2), MathF.Sin(rotation - MathF.PI / 2));
            addForce(direction, 750f, 0);
        }

        public void update() {
            //if (position.Y > bulletSprite.Bounds)
            bulletSprite.Position = position;
        }

        public void draw(SpriteBatch spriteBatch) {
            bulletSprite.draw(spriteBatch);
        }


    }
}
