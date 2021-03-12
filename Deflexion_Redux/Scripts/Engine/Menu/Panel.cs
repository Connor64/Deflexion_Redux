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

        // Default panel constructor
        public Panel(Vector2 position, Vector2 size, Color color, float layer, ref GraphicsDevice device) {
            this.position = position;
            //this.defaultPosition = position;
            this.size = size;
            defaultSize = size;
            this.color = color;
            this.layer = layer;
            textColor = Color.Black;
            _background = new Texture2D(device, 1, 1);
            _background.SetData(new[] { color });

            cam = Camera.Instance;
        }

        // Used for Panels that are ONLY sprites w/o a background
        public Panel(Vector2 position, TextureType texture, Vector2 scale, float layer) {
            this.position = position;
            this.texture = texture;
            sprite = new Sprite(texture, position, 0, layer, scale, Vector2.Zero);
            size = sprite.Size;
            defaultSize = size;
        }

        // Used for Panels that are normal but also have a sprite on them
        public Panel(ScreenPosition screenPos, Vector2 offset, Vector2 size, Color color, float layer, TextureType texture, ref GraphicsDevice device) : this(new Vector2(), size, color, layer, ref device) {
            setScreenPosition(screenPos, offset, 1);
            sprite = new Sprite(texture, position, 0, Sprite.Layers[LayerType.UI] - 0.02f, Vector2.One, OriginType.Center);
        }

        public Panel(ScreenPosition screenPos, Vector2 offset, Vector2 size, Color color, float layer, ref GraphicsDevice device) : this(new Vector2(), size, color, layer, ref device) {
            setScreenPosition(screenPos, offset, 1);
        }

        public Panel(Vector2 position, Vector2 size, Color color, float layer, ref GraphicsDevice device, string text, FontType font, Alignment alignment) : this(position, size, color, layer, ref device) {
            this.font = font;
            setText(text, alignment, 1);
        }

        public bool isHovering(bool scaled) {
            Vector2 mousePosition = cam.getMousePosition(scaled);

            return (mousePosition.X > position.X &&
                    mousePosition.X < position.X + size.X * cam.scalar &&
                    mousePosition.Y > position.Y &&
                    mousePosition.Y < position.Y + size.Y * cam.scalar);
        }

        public void changeColor(Color color) {
            this.color = color;
            _background.SetData(new[] { color });
        }

        public void setText(string newText, Alignment alignment, float scale) {
            textScale = scale;
            text = newText;
            textAlignment = alignment;

            textSize = AssetManager.fonts[font].MeasureString(text) * cam.scalar;
            Vector2 scaledSize = size * cam.scalar;

            switch (alignment) {
                case Alignment.Left:
                    textAdjustment = new Vector2(0, (scaledSize.Y - textSize.Y) / 2);
                    break;
                case Alignment.Right:
                    textAdjustment = new Vector2(scaledSize.X - textSize.X, (scaledSize.Y - textSize.Y) / 2);
                    break;
                case Alignment.Center:
                    textAdjustment = (scaledSize - textSize) / 2;
                    break;
                case Alignment.Top:
                    textAdjustment = new Vector2((scaledSize.X - textSize.X) / 2, 0);
                    break;
                case Alignment.Bottom:
                    textAdjustment = new Vector2((scaledSize.X - textSize.X) / 2, scaledSize.Y - textSize.Y);
                    break;
                case Alignment.Default:
                    textAdjustment = Vector2.Zero;
                    break;
            }
        }

        public void setScreenPosition(ScreenPosition pos, Vector2 offset, float scalar) {
            scalePosition(pos, cam.position, cam.virtualWidth * scalar, cam.virtualHeight * scalar, offset, scalar);
        }

        public void setRelativePosition(Panel parentPanel, ScreenPosition pos, Vector2 offset, float scalar) {
            float parentWidth = parentPanel.size.X * cam.scalar;
            float parentHeight = parentPanel.size.Y * cam.scalar;
            scalePosition(pos, parentPanel.position, parentWidth, parentHeight, offset, scalar);
        }

        private void scalePosition(ScreenPosition pos, Vector2 parentPos, float parentWidth, float parentHeight, Vector2 offset, float scalar) {
            scalar *= cam.scalar;
            Vector2 scaledSize = size * cam.scalar;
            switch (pos) {
                case ScreenPosition.Center:
                    position = parentPos + new Vector2((parentWidth - scaledSize.X) / 2, (parentHeight - scaledSize.Y) / 2) + offset * scalar;
                    break;
                case ScreenPosition.TopCenter:
                    position = parentPos + new Vector2(parentWidth - (scaledSize.X / 2), 0) + offset * scalar;
                    break;
                case ScreenPosition.BottomCenter:
                    position = parentPos + new Vector2((parentWidth - scaledSize.X ) / 2, parentHeight - scaledSize.Y) + offset * scalar;
                    break;
                case ScreenPosition.BottomLeft:
                    position = parentPos + new Vector2(0, parentHeight - scaledSize.Y) + offset * scalar;
                    break;
                case ScreenPosition.MiddleLeft:
                    position = parentPos + new Vector2(0, (parentHeight / 2) - (scaledSize.Y / 2)) + offset * scalar;
                    break;
                case ScreenPosition.TopLeft:
                    position = parentPos + offset * scalar;
                    break;
                case ScreenPosition.BottomRight:
                    position = parentPos + new Vector2(parentWidth - scaledSize.X, parentHeight - scaledSize.Y) + offset * scalar;
                    break;
                case ScreenPosition.MiddleRight:
                    position = parentPos + new Vector2(parentWidth - scaledSize.X, (parentHeight / 2) -scaledSize.Y / 2) + offset * scalar;
                    break;
                case ScreenPosition.TopRight:
                    position = parentPos + new Vector2(parentWidth - scaledSize.X, 0) + offset * scalar;
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
            spriteBatch.Draw(_background, position, null, color, 0, Vector2.Zero, size * cam.scalar, SpriteEffects.None, layer);
            if (text != "") {
                spriteBatch.DrawString(AssetManager.fonts[font], text, position + textAdjustment, textColor, 0, Vector2.Zero, textScale * cam.scalar, SpriteEffects.None, layer - 0.01f);
            }
            if (sprite != null) {
                sprite.Position = position + size/2;
                sprite.Draw(spriteBatch);
            }
        }
    }
}