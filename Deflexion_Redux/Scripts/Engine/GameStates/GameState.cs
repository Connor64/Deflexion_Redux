using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Deflexion_Redux {
    public abstract class GameState : IGameState {
        protected GraphicsDevice _graphicsDevice;
        protected Camera cam;
        protected DrawHandler drawHandler;
        public GameState(GraphicsDevice graphicsDevice) {
            _graphicsDevice = graphicsDevice;
            cam = Camera.Instance;
            drawHandler = DrawHandler.Instance;
        }
        public abstract void Initialize();
        public abstract void LoadContent(ContentManager content);
        public abstract void UnloadContent();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}