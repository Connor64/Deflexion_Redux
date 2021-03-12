using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Deflexion_Redux {

    public enum EnemyType {
        Turret,
        Drone,
        none,
    }

    public class EnemyManager {
        // Global enemy projectiles
        public List<Bullet> enemyBullets;

        public List<Enemy> enemies = new List<Enemy>();
        public Player player;

        private static EnemyManager instance = null;
        public static EnemyManager Instance {
            get {
                if (instance == null) {
                    instance = new EnemyManager();
                }
                return instance;
            }
        }

        public EnemyManager() {
            enemyBullets = new List<Bullet>();
        }

        public void Load(Player player) {
            this.player = player;
        }

        public void Update(Vector2 playerPosition, float deltaTime) {
            List<Enemy> toRemove = enemies;
            for (int i = 0; i < enemies.Count; i++) {
                if (enemies[i].isAlive) {
                    enemies[i].Update(playerPosition, deltaTime);
                } else {
                    toRemove.Remove(enemies[i]);
                }
            }
            enemies = toRemove;
        }

        public void appendEnemies(EnemyTile[,] newEnemyList) {
            enemies.Clear();
            enemyBullets.Clear();

            foreach(EnemyTile enemy in newEnemyList) {
                if (enemy.enemyType != EnemyType.none) {
                    SpawnEnemy(enemy.enemyType, enemy.position);
                }
            }
        }

        public void SpawnEnemy(EnemyType enemy, Vector2 position) {
            switch (enemy) {
                case EnemyType.Turret:
                    enemies.Add(new Turret(position));
                    break;
                case EnemyType.Drone:
                    enemies.Add(new Drone(position));
                    break;
            }
        }

        void enemyCleanup() {
            List<Enemy> toRemove = enemies;
            for (int i = 0; i < enemies.Count; i++) {
                Enemy enemy = enemies[i];
                if (!enemy.isAlive) {
                    toRemove.RemoveAt(i);
                }
                enemies = toRemove;
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            foreach (Enemy enemy in enemies) {
                if (enemy.isAlive) {
                    enemy.Draw(spriteBatch);
                }
            }
            foreach (Bullet bullet in enemyBullets) {
                bullet.draw(spriteBatch);
            }
        }
    }
}