using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Deflexion_Redux {
    public abstract class Enemy : Physics {
        public Camera cam = Camera.Instance;
        public EnemyManager enemyManager = EnemyManager.Instance;
        public bool isAlive = true;

        public float elapsedTime = 0;
        public Random rnd = new Random();

        public abstract void Update(Vector2 playerPosition, float deltaTime);
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void Attack(float deltaTime);

        public void deathCheck() {
            if (collisionCheck(position, enemyManager.player.playerBullets, out Bullet bullet)) {
                isAlive = false;
                bullet.isActive = false;
                enemyManager.enemies.Remove(this);
            }
        }
    }
}
