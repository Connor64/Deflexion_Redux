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
        // Textures

        public List<Enemy> enemies = new List<Enemy>();

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

        public void Initialize(ContentManager content) {
            bullet = content.Load<Texture2D>("Sprites/shotgunBlast");
            turret_base = content.Load<Texture2D>("Sprites/turret_bottom");
            turret_gun = content.Load<Texture2D>("Sprites/turret_top");
        }

        public void Update(Vector2 playerPosition, float deltaTime) {
            foreach(Enemy enemy in enemies) {
                enemy.Update(playerPosition, deltaTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            foreach (Enemy enemy in enemies) {
                enemy.Draw(spriteBatch);
            }
            foreach (Bullet bullet in enemyBullets) {
                bullet.draw(spriteBatch);
            }
        }
    }
}