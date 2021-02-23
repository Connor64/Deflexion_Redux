using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Deflexion_Redux {
    public class Panel {
        public Vector2 position;
        public Vector2 size;
        private float layer;

        public Color color;

        public Texture2D _background;

        private Camera cam;

        public bool visible = false;

        public Panel(Vector2 position, Vector2 size, Color color, float layer, ref GraphicsDeviceManager device) {
            this.position = position;
            this.size = size;
            this.color = color;
            this.layer = layer;
            _background = new Texture2D(device.GraphicsDevice, 1, 1);
            _background.SetData(new[] { color });

            cam = Camera.Instance;
        }

        public bool isHovering() {
            Vector2 mousePosition = cam.getMousePosition();

            return (mousePosition.X > position.X &&
                mousePosition.X < position.X + size.X &&
                mousePosition.Y > position.Y &&
                mousePosition.Y < position.Y + size.Y);
        }

        public void Draw(SpriteBatch spriteBatch) {
            //spriteBatch.Draw(_background, new Rectangle(xPos, yPos, Width, Height), color);
            spriteBatch.Draw(_background, position, null, color, 0, Vector2.Zero, size, SpriteEffects.None, layer);
        }
    }
}