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
        private Panel panel;
        private MouseState M_oldState;

        public bool dropped = false;

        public DropDownContainer(Vector2 position, Vector2 size, FontType font, List<string> elementText, ref GraphicsDevice device) {

            elements = new List<DropDownElement>();
            for (int i = 0; i < elementText.Count; i++) {
                Debug.Print(elementText[i]);
                elements.Add(new DropDownElement(elementText[i], position + new Vector2(0, size.Y * (i + 1)), size, font, ref device));
            }

            panel = new Panel(position, size, Color.LightGray, Sprite.Layers[LayerType.UI] - 0.01f, ref device, "", font, Alignment.Left);
            M_oldState = Mouse.GetState();
        }

        public void Update() {
            if (panel.isHovering(false) && Mouse.GetState().LeftButton == ButtonState.Pressed && M_oldState.LeftButton != ButtonState.Pressed) {
                drop();
            }

            if (dropped) {
                for (int i = 0; i < elements.Count; i++) {
                    elements[i].action();
                    if (elements[i].selected) {
                        setText(elements[i].getText(), panel.textScale);
                        dropped = false;
                        elements[i].selected = false;
                    }
                }
            }
            if (dropped && Mouse.GetState().LeftButton == ButtonState.Pressed && !panel.isHovering(false)) {
                dropped = false;
            }
            M_oldState = Mouse.GetState();
        }

        public void drop() {
            dropped = !dropped;

            foreach (DropDownElement element in elements) {
                element.visible = dropped;
            }
        }

        public void setText(string newText, float scale) {
            panel.textScale = scale;
            panel.setText(newText, Alignment.Center, scale);
            foreach (DropDownElement element in elements) {
                element.setText(element.getText(), scale);
            }
        }
        public string getText() {
            return panel.getText();
        }

        public void setScreenPosition(ScreenPosition pos, Vector2 offset, float scalar) {
            panel.setScreenPosition(pos, offset, scalar);
            for (int i = 0; i < elements.Count; i++) {
                elements[i].scaleScreenPosition(pos, offset + new Vector2(0, panel.size.Y * (i + 1)), scalar);
            }
        }

        public void setRelativePosition(Panel parentPanel, ScreenPosition pos, Vector2 offset, float scalar) {
            panel.setRelativePosition(parentPanel, pos, offset, scalar);
            for (int i = 0; i < elements.Count; i++) {
                elements[i].scaleRelativePosition(panel, ScreenPosition.BottomCenter, new Vector2(0, panel.size.Y * (i + 1)), 1);
            }
        }

        public void scaleSize(float scalar) {
            panel.scaleSize(scalar);
            panel.setText(panel.getText(), Alignment.Center, scalar);
            foreach (DropDownElement element in elements) {
                element.scaleSize(scalar);
                element.setText(element.getText(), scalar);
            }
        }

        public void setSize(Vector2 size) {
            panel.size = size;
            foreach(DropDownElement element in elements) {
                element.setSize(size);
            }
        }

        public Vector2 getSize() {
            return panel.size;
        }

        public void setPosition(Vector2 position) {
            panel.position = position;
            for (int i = 0; i < elements.Count; i++) {
                elements[i].setPosition(position + new Vector2(0, panel.size.Y * (i + 1)));
            }
        }

        public Vector2 getPosition() {
            return panel.position;
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