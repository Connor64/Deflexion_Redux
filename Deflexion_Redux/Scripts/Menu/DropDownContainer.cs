using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Deflexion_Redux {
    class DropDownContainer {
        List<DropDownElement> elements;

        //private DropDownElement selectedElement;

        public Vector2 position;
        public Vector2 size;

        public string text = "";
        public bool dropped = false;

        private SpriteFont font;

        public Panel mainPanel;
        private Camera cam;
        private MouseState M_oldState;

        public bool changed = false;

        public DropDownContainer(List<string> elementText, Vector2 position, Vector2 size, SpriteFont font, ref GraphicsDeviceManager device) {
            elements = new List<DropDownElement>();
            this.position = position;
            this.size = size;
            this.font = font;

            for (int i = 0; i < elementText.Count; i++) {
                Debug.Print(elementText[i]);
                elements.Add(new DropDownElement(elementText[i], position + new Vector2(0, size.Y * (i + 1)), size, font, ref device));
            }
            mainPanel = new Panel(position, size, Color.LightGray, Sprite.Layers["UI"] - 0.01f, ref device);
            cam = Camera.Instance;
            M_oldState = Mouse.GetState();
        }

        public void Update() {
            mainPanel.position = position;
            if (mainPanel.isHovering()) {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed && M_oldState.LeftButton != ButtonState.Pressed) {
                    dropdown();
                }
            }
            if (dropped) {
                for (int i = 0; i < elements.Count; i++) {
                    elements[i].position = position + new Vector2(0, size.Y * (i + 1));
                    elements[i].action();
                    if (elements[i].selected && elements[i].backgroundPanel.isHovering()) {
                        text = elements[i].text;
                        changed = true;
                    } else {
                        elements[i].selected = false;
                    }
                }
            }
            M_oldState = Mouse.GetState();
        }

        public void dropdown() {
            if (!dropped) {
                foreach (DropDownElement element in elements) {
                    element.visible = true;
                }
            } else {
                foreach (DropDownElement element in elements) {
                    element.visible = false;
                }
            }
            dropped = !dropped;
        }

        public void Draw(SpriteBatch spriteBatch) {
            mainPanel.Draw(spriteBatch);
            spriteBatch.DrawString(font, text, position, Color.Black);
            if (dropped) {
                foreach (DropDownElement element in elements) {
                    element.Draw(spriteBatch);
                }
            }
        }
    }
}