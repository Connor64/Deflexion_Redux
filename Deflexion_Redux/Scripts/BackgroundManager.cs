using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Deflexion_Redux {
    class BackgroundManager {
        //public Sprite[] backgrounds = new Sprite[4];
        public Sprite backgroundSprite;
        private Camera cam;

        public BackgroundManager(ContentManager content) {
            Texture2D backgroundTexture = content.Load<Texture2D>("Sprites/spaceBackground_test");
            //for (int i = 0; i < backgrounds.Length; i++) {
            //    backgrounds[i] = new Sprite(backgroundTexture, Vector2.Zero, 0, Vector2.One, 1);
            //}
            backgroundSprite = new Sprite(backgroundTexture, Vector2.Zero, 0, Vector2.One, 1);
            cam = Camera.Instance;
        }

        public void loopUpdate(Vector2 playerPosition) {

        }

        public void draw(SpriteBatch spriteBatch) {
            backgroundSprite.draw(spriteBatch);
        }
    }
}