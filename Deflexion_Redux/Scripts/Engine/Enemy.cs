using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Deflexion_Redux {
    public abstract class Enemy : Physics {
        public bool isAlive = true; // returns false if killed/health is depleted
        public TextureType mainTexture; // used to identify in level editor

        public float elapsedTime = 0;
        public Random rnd = new Random();
        public Vector2 startingPosition; // Will not be updated -> used to determine if a tile is already occupied by another enemy in the level editor

        public Enemy() {} // To allow for serialization

        public abstract void Update(Vector2 playerPosition, float deltaTime);
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void Attack(float deltaTime);

        public void deathCheck() {
            if (collisionCheck(position, GameplayScreen.player.playerBullets, out Bullet bullet)) {
                isAlive = false;
                bullet.isActive = false;
                EnemyManager.Instance.enemies.Remove(this);
            }
        }
    }
}
