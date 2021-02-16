using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Deflexion_Redux {
    class Enemy {
        public Camera cam = Camera.Instance;
        public EnemyManager enemyManager = EnemyManager.Instance;

        public Vector2 position;


        public virtual void Update(Vector2 playerPosition, float deltaTime) {}
        public virtual void Draw(SpriteBatch spriteBatch) {}
    }
}
