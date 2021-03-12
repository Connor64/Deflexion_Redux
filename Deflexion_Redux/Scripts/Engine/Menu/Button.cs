using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Deflexion_Redux {

    public enum ButtonStyle {
        Default,
        Gray,
    }

    public class Button {
        public Action action;

        private Color backgroundColor = Color.White;
        private Color hoverColor = Color.Azure;
        private Color clickColor = Color.LightCyan;

        private Panel panel;
        private MouseState mState_OLD;

        public Button(Action action, Vector2 position, Vector2 size, ref GraphicsDevice device) {
            this.action = action;

            panel = new Panel(position, size, backgroundColor, Sprite.Layers[LayerType.UI] - 0.01f, ref device);
            mState_OLD = Mouse.GetState();
        }

        public Button(Action action, ScreenPosition pos, Vector2 offset, Vector2 size, ref GraphicsDevice device) : this(action, new Vector2(), size, ref device) {
            panel = new Panel(pos, offset, size, backgroundColor, Sprite.Layers[LayerType.UI] - 0.01f, ref device);
        }

        public Button(Action action, Vector2 position, Vector2 size, ref GraphicsDevice device, Color color, string text, FontType font) : this(action, position, size, ref device) {
            backgroundColor = color;
            panel = new Panel(position, size, color, Sprite.Layers[LayerType.UI] - 0.01f, ref device, text, font, Alignment.Center);
        }

        public Button(Action action, ScreenPosition pos, Vector2 offset, Vector2 size, ref GraphicsDevice device, Color color, string text, FontType font) : this(action, new Vector2(), size, ref device, color, text, font) {
            panel = new Panel(pos, offset, size, color, Sprite.Layers[LayerType.UI] - 0.01f, ref device);
        }

        public Button(Action action, ScreenPosition pos, Vector2 offset, Vector2 size, ref GraphicsDevice device, Color color, TextureType texture) {
            this.action = action;
            panel = new Panel(pos, offset, size, color, Sprite.Layers[LayerType.UI] - 0.01f, texture, ref device);
            mState_OLD = Mouse.GetState();
        }

        public void Update() {
            if (panel.isHovering(false)){
                panel.changeColor(hoverColor);
                if (Mouse.GetState().LeftButton == ButtonState.Pressed) {
                    if (mState_OLD.LeftButton != ButtonState.Pressed) {
                        action.Invoke();
                    }
                    panel.changeColor(clickColor);
                }
            } else {
                panel.changeColor(backgroundColor);
            }
            mState_OLD = Mouse.GetState();
        }

        public void setColor(Color backgroundColor, Color hoverColor, Color clickColor, Color textColor) {
            this.backgroundColor = backgroundColor;
            panel.changeColor(backgroundColor);
            panel.textColor = textColor;
            this.hoverColor = hoverColor;
            this.clickColor = clickColor;
        }

        public void setColor(ButtonStyle buttonStyle, float opacity) {
            switch (buttonStyle) {
                case ButtonStyle.Gray:
                    backgroundColor = Color.Gray * opacity;
                    hoverColor = Color.LightGray * opacity;
                    clickColor = Color.Black * opacity;

                    panel.changeColor(backgroundColor);
                    panel.textColor = Color.White;
                    break;
                case ButtonStyle.Default:
                    break;
            }
        }

        public void setText(string text, Alignment alignment) {
            panel.setText(text, alignment, panel.textScale);
        }

        public void setText(string text, Alignment alignment, float textScale, FontType font) {
            panel.font = font;
            panel.setText(text, alignment, textScale);
        }

        public void setScreenPosition(ScreenPosition pos, Vector2 offset, float scalar) {
            panel.setScreenPosition(pos, offset, scalar);
        }

        public void setRelativePosition(Panel parentPanel, ScreenPosition pos, Vector2 offset, float scalar) {
            panel.setRelativePosition(parentPanel, pos, offset, scalar);
        }

        public void scaleSize(float scalar) {
            panel.scaleSize(scalar);
            panel.setText(panel.getText(), panel.textAlignment, scalar);
        }

        public void setSize(Vector2 size) {
            panel.size = size;
        }

        public void setSize(Vector2 size, float textScale) {
            panel.size = size;
            panel.setText(panel.getText(), panel.textAlignment, textScale);
        }

        public Vector2 getSize() {
            return panel.size;
        }

        public void setPosition(Vector2 position) {
            panel.position = position;
        }

        public Vector2 getPosition() {
            return panel.position;
        }


        public void Draw(SpriteBatch spriteBatch) {
            panel.Draw(spriteBatch);
            //spriteBatch.DrawString(font, text, position, Color.Black);
        }
    }
}