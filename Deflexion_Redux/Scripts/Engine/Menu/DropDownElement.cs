using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Deflexion_Redux {
    class DropDownElement {
        public Vector2 padding = new Vector2(8, 8);

        public bool selected = false;
        public bool visible = false;

        Camera cam = Camera.Instance;

        public Panel panel;

        public DropDownElement(string text, Vector2 position, Vector2 size, FontType font, ref GraphicsDevice device) {
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

        public void scaleScreenPosition(ScreenPosition spot, Vector2 offset, float scalar) {
            panel.scaleScreenPosition(spot, offset, scalar);
        }

        public void scaleRelativePosition(Panel parentPanel, ScreenPosition pos, Vector2 offset, float scalar) {
            panel.scaleRelativePosition(parentPanel, pos, offset, scalar);
        }

        public void scaleSize(float scalar) {
            panel.scaleSize(scalar);
        }

        public void setPosition(Vector2 position) {
            panel.position = position;
        }

        public Vector2 getPosition() {
            return panel.position;
        }

        public void setSize(Vector2 size) {
            panel.size = size;
        }

        public Vector2 getSize() {
            return panel.size;
        }

        public void Draw(SpriteBatch spriteBatch) {
            panel.Draw(spriteBatch);
        }
    }
}