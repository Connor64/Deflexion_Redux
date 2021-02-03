using System.Collections.Generic;
using System.Diagnostics;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Deflexion_Redux {
    public class Main : Game {

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Player player;
        private TileManager tileManager;
        private BackgroundManager backgroundManager;
        private Camera cam;

        public KeyboardState kState;
        public KeyboardState kState_OLD;
        public MouseState mState;
        public MouseState mState_OLD;
        public float deltaTime = 0;

        public Main() {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize() {
            GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;

            cam = Camera.Instance;
            cam.zoom = 1;
            cam.Initialize(ref _graphics);
            cam.SetVirtualResolution(960, 540);
            //cam.SetVirtualResolution(16, 9);
            cam.SetResolution(GraphicsDevice.DisplayMode.Width, GraphicsDevice.DisplayMode.Height, true);

            _graphics.ToggleFullScreen();
            _graphics.ApplyChanges();
            kState = Keyboard.GetState();
            kState_OLD = kState;

            base.Initialize();
        }

        protected override void LoadContent() {
            player = new Player(Content, new Vector2(GraphicsDevice.DisplayMode.Width, GraphicsDevice.DisplayMode.Height));
            tileManager = new TileManager(Content);
            backgroundManager = new BackgroundManager(Content);
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) {
                Exit();
            }

            //cam.position = player.position;
            //cam.move(Vector2.Normalize(player.position));

            player.physicsMove((float)gameTime.ElapsedGameTime.TotalSeconds, tileManager.tileSprites);
            player.mouseFollow();
            cam.move(player.previousPosition - player.position);

            //kState_OLD = Keyboard.GetState();
            //mState_OLD = Mouse.GetState();

            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            //GraphicsDevice.Clear(Color.CornflowerBlue); // background color

            cam.BeginDraw();
            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied,
                           SamplerState.PointClamp, null, null, null, cam.getMatrix() * cam.getTransformationMatrix());

            //_spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            player.Draw(_spriteBatch);
            tileManager.Draw(_spriteBatch);
            backgroundManager.draw(_spriteBatch);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}