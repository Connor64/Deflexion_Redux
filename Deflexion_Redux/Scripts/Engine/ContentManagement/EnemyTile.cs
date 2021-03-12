using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Deflexion_Redux {
    public class EnemyTile {
        public EnemyType enemyType;
        public Vector2 position;

        public EnemyTile(EnemyType enemy, Vector2 startingPosition) {
            enemyType = enemy;
            position = startingPosition;
        }

        public EnemyTile() { } // To allow for serialization
    }
}