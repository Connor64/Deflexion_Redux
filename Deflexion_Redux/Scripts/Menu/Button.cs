using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Deflexion_Redux {
    public class Button {
        public Action action;

        public Panel panel;
        public string text;
        public Vector2 position;
        private SpriteFont font;

        public Button(Action action, string text, Vector2 position, Vector2 size, Color color, SpriteFont font, ref GraphicsDeviceManager device) {
            this.text = text;
            this.font = font;
            this.position = position;
            this.action = action;

            panel = new Panel(position, size, color, Sprite.Layers["UI"] - 0.01f, ref device);
        }

        public void Draw(SpriteBatch spriteBatch) {
            panel.Draw(spriteBatch);
            spriteBatch.DrawString(font, text, position, Color.Black);
        }

        //public T action<T>(Func<T> funcToRun) {
        //    return funcToRun();
        //}

    }
}