using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public enum Alignment {
    Left,
    Center,
    Right,
    Top,
    Bottom,
    Default
}

public enum ScreenPosition {
    Center,
    TopCenter,
    BottomCenter,

    BottomLeft,
    MiddleLeft,
    TopLeft,

    BottomRight,
    MiddleRight,
    TopRight,
}

namespace Deflexion_Redux {
    public class Panel {
        public Color color;
        public Texture2D _background;
        public float textScale = 1;
        public Color textColor;
        public Alignment textAlignment = Alignment.Left;
        public FontType font;

        public Vector2 position;
        //private Vector2 defaultPosition;
        public Vector2 size;
        private Vector2 defaultSize;
        private TextureType texture = TextureType.none;
        private Sprite sprite;

        private string text = "";
        private float layer;
        private Vector2 textAdjustment;
        private Vector2 textSize;
        private Camera cam;

        public bool visible = false;

        public Panel(Vector2 position, Vector2 size, Color color, float layer, ref GraphicsDevice device) {
            this.position = position;
            //this.defaultPosition = position;
            this.size = size;
            this.defaultSize = size;
            this.color = color;
            this.layer = layer;
            textColor = Color.Black;
            _background = new Texture2D(device, 1, 1);
            _background.SetData(new[] { color });

            cam = Camera.Instance;
        }

        public Panel(Vector2 position, TextureType texture, Vector2 scale, float layer) {
            this.position = position;
            this.texture = texture;
            sprite = new Sprite(texture, position, 0, layer, scale, Vector2.Zero);
        } 

        public Panel(ScreenPosition relativeScreenPos, Vector2 offset, Vector2 size, Color color, float layer, ref GraphicsDevice device) : this(new Vector2(), size, color, layer, ref device) {
            scaleScreenPosition(relativeScreenPos, offset, 1);
        }

        public Panel(Vector2 position, Vector2 size, Color color, float layer, ref GraphicsDevice device, string text, FontType font, Alignment alignment) : this(position, size, color, layer, ref device) {
            this.font = font;
            setText(text, alignment, 1);
        }

        public bool isHovering() {
            Vector2 mousePosition = cam.getMousePosition();

            return (mousePosition.X > position.X &&
                mousePosition.X < position.X + size.X &&
                mousePosition.Y > position.Y &&
                mousePosition.Y < position.Y + size.Y);
        }

        public void changeColor(Color color) {
            this.color = color;
            _background.SetData(new[] { color });
        }

        public void setText(string newText, Alignment alignment, float scale) {
            textScale = scale;
            text = newText;
            textSize = AssetManager.fonts[font].MeasureString(text) / cam.zoom;

            textAlignment = alignment;

            switch (alignment) {
                case Alignment.Left:
                    textAdjustment = new Vector2(0, (size.Y - textSize.Y) / 2);
                    break;
                case Alignment.Right:
                    textAdjustment = new Vector2(size.X - textSize.X, (size.Y - textSize.Y) / 2);
                    break;
                case Alignment.Center:
                    textAdjustment = (size - textSize) / 2;
                    break;
                case Alignment.Top:
                    textAdjustment = new Vector2((size.X - textSize.X) / 2, 0);
                    break;
                case Alignment.Bottom:
                    textAdjustment = new Vector2((size.X - textSize.X) / 2, size.Y - textSize.Y);
                    break;
                case Alignment.Default:
                    textAdjustment = Vector2.Zero;
                    break;
            }
        }

        public void scaleScreenPosition(ScreenPosition pos, Vector2 offset, float scalar) {
            scalePosition(pos, cam.position, cam.virtualWidth * scalar, cam.virtualHeight * scalar, offset, scalar);
        }

        public void scaleRelativePosition(Panel parentPanel, ScreenPosition pos, Vector2 offset, float scalar) {
            Vector2 parentCenter = parentPanel.position + (parentPanel.size / 2);
            float parentWidth = parentPanel.size.X;
            float parentHeight = parentPanel.size.Y;
            scalePosition(pos, parentCenter, parentWidth, parentHeight, offset, scalar);
        }

        private void scalePosition(ScreenPosition pos, Vector2 parentCenter, float parentWidth, float parentHeight, Vector2 offset, float scalar) {
            switch (pos) {
                case ScreenPosition.Center:
                    position = parentCenter - (size / 2) + offset * scalar;
                    break;
                case ScreenPosition.TopCenter:
                    position = parentCenter - new Vector2(size.X / 2, parentHeight / 2) + offset * scalar;
                    break;
                case ScreenPosition.BottomCenter:
                    position = parentCenter - new Vector2(size.X / 2, -((parentHeight) / 2) + size.Y) + offset * scalar;
                    break;
                case ScreenPosition.BottomLeft:
                    position = parentCenter - new Vector2(parentWidth / 2, (-parentHeight / 2) + size.Y) + offset * scalar;
                    break;
                case ScreenPosition.MiddleLeft:
                    position = parentCenter - new Vector2(parentWidth / 2, size.Y / 2) + offset * scalar;
                    break;
                case ScreenPosition.TopLeft:
                    position = parentCenter - new Vector2(parentWidth / 2, parentHeight / 2) + offset * scalar;
                    break;
                case ScreenPosition.BottomRight:
                    position = parentCenter + new Vector2((parentWidth / 2) - size.X, (parentHeight / 2) - size.Y) + offset * scalar;
                    break;
                case ScreenPosition.MiddleRight:
                    position = parentCenter + new Vector2((parentHeight / 2) - size.X, -size.Y / 2) + offset * scalar;
                    break;
                case ScreenPosition.TopRight:
                    position = parentCenter + new Vector2((parentWidth / 2) - size.X, -parentHeight / 2) + offset * scalar;
                    break;
                default:
                    Debug.Print("That Screen Position (" + pos.ToString() + ") does not exist");
                    break;
            }
        }

        public void scaleSize(float scalar) {
            size = defaultSize * scalar;
        }

        public string getText() {
            return text;
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(_background, position, null, color, 0, Vector2.Zero, size, SpriteEffects.None, layer);
            if (text != "") {
                spriteBatch.DrawString(AssetManager.fonts[font], text, position + textAdjustment, textColor, 0, Vector2.Zero, textScale, SpriteEffects.None, layer - 0.01f);
            }
        }
    }
}