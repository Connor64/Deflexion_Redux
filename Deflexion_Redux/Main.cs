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
        private TileManager tiles;
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
            _graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;

            cam = Camera.Instance;
            cam.zoom = 1;
            cam.Initialize(ref _graphics);
            cam.SetVirtualResolution(768, 432);
            cam.SetResolution(GraphicsDevice.DisplayMode.Width, GraphicsDevice.DisplayMode.Height, true);

            _graphics.ToggleFullScreen();
            _graphics.ApplyChanges();
            kState = Keyboard.GetState();
            kState_OLD = kState;

            base.Initialize();
        }

        protected override void LoadContent() {
            player = new Player(Content, new Vector2(GraphicsDevice.DisplayMode.Width, GraphicsDevice.DisplayMode.Height));
            tiles = new TileManager(Content);
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) {
                Exit();
            }

            //cam.position = player.position;
            //cam.move(Vector2.Normalize(player.position));

            player.physicsMove((float)gameTime.ElapsedGameTime.TotalSeconds, tiles.tileSprites);
            player.shieldPowers(player.position.X - Mouse.GetState().X + cam.position.X, player.position.Y - Mouse.GetState().Y + cam.position.Y);

            //kState_OLD = Keyboard.GetState();
            //mState_OLD = Mouse.GetState();

            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            //GraphicsDevice.Clear(Color.CornflowerBlue); // background color

            cam.BeginDraw();

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied,
                           SamplerState.PointClamp, null, null, null, cam.getMatrix() * cam.getTransformationMatrix());

            //_spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            player.Draw(_spriteBatch);
            tiles.Draw(_spriteBatch);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}