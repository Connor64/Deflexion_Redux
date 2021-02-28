using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Deflexion_Redux {
    class DropDownContainer {
        private List<DropDownElement> elements;

        public Vector2 position;
        public Vector2 size;

        public bool dropped = false;

        private Panel panel;
        private MouseState M_oldState;
        private Camera cam;


        public DropDownContainer(Vector2 position, Vector2 size, FontType font, List<string> elementText, ref GraphicsDevice device) {
            this.position = position;
            this.size = size;
            cam = Camera.Instance;

            elements = new List<DropDownElement>();
            for (int i = 0; i < elementText.Count; i++) {
                Debug.Print(elementText[i]);
                elements.Add(new DropDownElement(elementText[i], position + new Vector2(0, size.Y * (i + 1)), size, font, ref device));
            }

            panel = new Panel(position, size, Color.LightGray, Sprite.Layers[LayerType.UI] - 0.01f, ref device, "", font, Alignment.Left);
            M_oldState = Mouse.GetState();
        }

        public void Update() {
            if (panel.isHovering() && Mouse.GetState().LeftButton == ButtonState.Pressed && M_oldState.LeftButton != ButtonState.Pressed) {
                dropdown();
            }

            if (dropped) {
                for (int i = 0; i < elements.Count; i++) {
                    //elements[i].position = position + new Vector2(0, size.Y * (i + 1));
                    //elements[i].setSize(size);
                    elements[i].action();
                    if (elements[i].selected) {
                        setText(elements[i].getText(), panel.textScale);
                        dropped = false;
                        elements[i].selected = false;
                    }
                }
            }
            if (dropped && Mouse.GetState().LeftButton == ButtonState.Pressed && !panel.isHovering()) {
                dropped = false;
            }
            M_oldState = Mouse.GetState();
        }

        public void dropdown() {
            dropped = !dropped;

            foreach (DropDownElement element in elements) {
                element.visible = dropped;
            }
        }

        public string getText() {
            return panel.getText();
        }

        public void setText(string newText, float scale) {
            panel.textScale = scale;
            panel.setText(newText, Alignment.Center, scale);
            foreach (DropDownElement element in elements) {
                element.setText(element.getText(), scale);
            }
        }

        public void setSize(Vector2 size, float textScale) {
            panel.size = size;
            panel.setText(panel.getText(), Alignment.Center, textScale);
            this.size = size;
            foreach(DropDownElement element in elements) {
                element.setSize(size, textScale);
            }
        }

        public void setPosition(Vector2 position) {
            this.position = position;
            panel.position = position;
            for (int i = 0; i < elements.Count; i++) {
                elements[i].setPosition(position + new Vector2(0, size.Y * (i + 1)));
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            panel.Draw(spriteBatch);
            if (dropped) {
                foreach (DropDownElement element in elements) {
                    element.Draw(spriteBatch);
                }
            }
        }
    }
}