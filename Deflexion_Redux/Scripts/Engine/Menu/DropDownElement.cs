using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Deflexion_Redux {
    class DropDownElement {
        public Vector2 position;
        private Vector2 size;
        public Vector2 padding = new Vector2(8, 8);

        public bool selected = false;
        public bool visible = false;

        Camera cam = Camera.Instance;

        public Panel panel;

        public DropDownElement(string text, Vector2 position, Vector2 size, FontType font, ref GraphicsDevice device) {
            this.position = position;
            this.size = size;

            panel = new Panel(position, size, Color.White, Sprite.Layers[LayerType.UI] - 0.01f, ref device, text, font, Alignment.Left);
        }

        public void action() {
            if (panel.isHovering()) {
                panel.changeColor(Color.Azure);
                if (Mouse.GetState().LeftButton == ButtonState.Pressed) {
                    selected = true;
                }
            } else {
                panel.changeColor(Color.White);
            }
        }

        public void setText(string newText, float scale) {
            panel.setText(newText, Alignment.Default, scale);
        }

        public string getText() {
            return panel.getText();
        }

        public void setSize(Vector2 size, float textScale) {
            panel.size = size;
            panel.setText(panel.getText(), Alignment.Default, textScale);
            this.size = size;
        }

        public void setPosition(Vector2 position) {
            this.position = position;
            panel.position = position;
        }

        public void Draw(SpriteBatch spriteBatch) {
            panel.Draw(spriteBatch);
        }
    }
}