using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Deflexion_Redux {
    class DropDownElement {
        public string text;

        public Vector2 position;
        public Vector2 size;
        public Vector2 padding = new Vector2(8, 8);

        public bool selected = false;
        public bool visible = false;

        public Panel backgroundPanel;

        private SpriteFont font;

        public DropDownElement(string text, Vector2 position, Vector2 size, SpriteFont font, ref GraphicsDeviceManager device) {
            this.text = text;
            this.font = font;
            this.position = position;
            this.size = size;

            backgroundPanel = new Panel(position, size, Color.White, Sprite.Layers["UI"] - 0.01f, ref device);
        }

        public void action() {
            if (backgroundPanel.isHovering()) {
                backgroundPanel.color = Color.Azure;
                if (Mouse.GetState().LeftButton == ButtonState.Pressed) {
                    selected = true;
                }
            } else {
                backgroundPanel.color = Color.White;
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            backgroundPanel.position = position;

            backgroundPanel.Draw(spriteBatch);
            spriteBatch.DrawString(font, text, position, Color.Black);
        }
    }
}