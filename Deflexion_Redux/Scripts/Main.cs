using System.Diagnostics;
using System.IO;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Deflexion_Redux {
    public class Main : Game {

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private GameStateManager gameStateManager;
        private Camera cam;

        public Main() {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize() {
            GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            //IsMouseVisible = false;

            cam = Camera.Instance;
            cam.Initialize(ref _graphics);
            cam.SetVirtualResolution(GraphicsDevice.DisplayMode.Width / 2, GraphicsDevice.DisplayMode.Height / 2);
            cam.SetResolution(GraphicsDevice.DisplayMode.Width, GraphicsDevice.DisplayMode.Height, true);

            gameStateManager = GameStateManager.Instance;
            AudioManager.Instance.setVolume(0.5f);

            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent() {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            AssetManager.LoadTextures(Content);

            gameStateManager.setContent(Content);
            gameStateManager.AddScreen(new TitleScreen(GraphicsDevice));
        }

        protected override void UnloadContent() {
            gameStateManager.UnloadContent();
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime) {
            if (gameStateManager.quit) {
                Exit();
            }
            gameStateManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            cam.BeginDraw();

            gameStateManager.Draw(_spriteBatch);

            base.Draw(gameTime);
        }
    }
}