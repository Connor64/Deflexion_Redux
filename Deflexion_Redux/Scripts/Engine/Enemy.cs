using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Deflexion_Redux {
    class Enemy : Physics {
        public Camera cam = Camera.Instance;
        public EnemyManager enemyManager = EnemyManager.Instance;
        public bool isAlive = true;

        public float elapsedTime = 0;
        public Random rnd = new Random();

        public virtual void Update(Vector2 playerPosition, float deltaTime) {}
        public virtual void Draw(SpriteBatch spriteBatch) {}
        public virtual void Attack(float deltaTime) {}

        public void deathCheck() {
            if (collisionCheck(position, enemyManager.player.playerBullets, out Bullet bullet)) {
                isAlive = false;
                bullet.isActive = false;
                enemyManager.enemies.Remove(this);
            }
        }
    }
}
