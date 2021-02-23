using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Deflexion_Redux {
    class EnemyManager {
        public List<Bullet> enemyBullets;
        // Global enemy projectiles

        public Texture2D bullet;
        public Texture2D turret_base;
        public Texture2D turret_gun;
        public Texture2D drone_ship;
        // Textures

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
            bullet = AssetManager.textures[TextureType.test_enemy_blast];
            turret_base = AssetManager.textures[TextureType.turret_bottom];
            turret_gun = AssetManager.textures[TextureType.turret_top];
            drone_ship = AssetManager.textures[TextureType.test_drone];
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
    }
}