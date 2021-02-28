using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Deflexion_Redux {
    class Drone : Enemy {
        private Sprite shipSprite;
        private float x;
        private float y;
        private float rotation;

        public Drone(Vector2 position) {
            collisionBoxSize = 6;
            rotation = 0;
            bodyType = BodyType.Enemy;
            elapsedTime = (float)rnd.NextDouble();
            baseSpeedLimit = 300;
            screenTolerance = new Vector2(128, 128);
            this.position = position;
            shipSprite = new Sprite(TextureType.test_drone, position, 0, Sprite.Layers[LayerType.Turret], Vector2.One, new Vector2(8, 8));
            
        }

        public override void Update(Vector2 playerPosition, float deltaTime) {
            if (cam.contains(position, screenTolerance)) {
                elapsedTime += deltaTime;
                rotation = -MathF.Atan2(x, y);
                flyTowardsPlayer(playerPosition);
                Attack(deltaTime);
                PhysicsUpdate(deltaTime);
                deathCheck();
            }

            shipSprite.Position = position;
        }

        public override void Attack(float deltaTime) {
            if (elapsedTime > 4.5f) {
                enemyManager.enemyBullets.Add(new Bullet(TextureType.test_enemy_blast, position, 450f, rotation, 1));
                elapsedTime = 0; // randomize?
            }
        }

        private void flyTowardsPlayer(Vector2 playerPosition) {
            if (elapsedTime < 4.25) {
                // acts as the enemy "aiming"
                x = position.X - playerPosition.X;
                y = position.Y - playerPosition.Y;
                shipSprite.Rotation = rotation;
            }
            addForce(new Vector2(-x, -y), 300f, 0);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            shipSprite.Draw(spriteBatch);
        }
    }
}
