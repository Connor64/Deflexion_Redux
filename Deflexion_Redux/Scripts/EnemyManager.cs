using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Deflexion_Redux {
    class EnemyManager {
        public List<Bullet> enemyBullets;
        public Texture2D bullet;
        public Texture2D turret_base;
        public Texture2D turret_gun;

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
            
        }
    }
}
