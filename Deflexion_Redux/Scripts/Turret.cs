using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Deflexion_Redux {
    class Turret : Enemy {
        private Sprite baseSprite;
        private Sprite gunSprite;

        public Turret(Vector2 position) {
            mainTexture = TextureType.turret_bottom;

            bodyType = BodyType.Enemy;
            startingPosition = position;
            this.position = position;
            screenTolerance = new Vector2(128, 128);

            elapsedTime = (float)rnd.NextDouble();

            baseSprite = new Sprite(TextureType.turret_bottom, position, 0, Sprite.Layers[LayerType.Turret], Vector2.One, new Vector2(8, 8));
            gunSprite = new Sprite(TextureType.turret_top, position, 0, Sprite.Layers[LayerType.Turret] - 0.01f, Vector2.One, 
                                   new Vector2(AssetManager.textures[TextureType.turret_top].Width/2, AssetManager.textures[TextureType.turret_top].Height/2));
        }

        public override void Update(Vector2 playerPosition, float deltaTime) {
            if (Camera.Instance.contains(position, screenTolerance)) {
                Attack(deltaTime);
                followPlayer(playerPosition);
            }
            deathCheck();
        }
        public override void Attack(float deltaTime) {
            elapsedTime += deltaTime;
            if (elapsedTime >= 3f) {
                EnemyManager.Instance.enemyBullets.Add(new Bullet(TextureType.test_enemy_blast, position, 400, gunSprite.Rotation, 1));
                elapsedTime = 0;
            }
        }

        public void followPlayer(Vector2 playerPosition) {
            float x = gunSprite.Position.X - playerPosition.X;
            float y = gunSprite.Position.Y - playerPosition.Y;

            gunSprite.Rotation = -(float)Math.Atan2(x, y);
        }


        public override void Draw(SpriteBatch spriteBatch) {
            baseSprite.Draw(spriteBatch);
            gunSprite.Draw(spriteBatch);
        }
    }
}
