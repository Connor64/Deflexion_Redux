﻿using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Deflexion_Redux {
    public class Main : Game {

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Player player;
        private TileManager tileManager;
        private Background background;
        private Camera cam;
        private Turret testTurret;

        public KeyboardState kState;
        public KeyboardState kState_OLD;
        public MouseState mState;
        public MouseState mState_OLD;

        float deltaTime = 0;

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
            tileManager = new TileManager(Content);
            testTurret = new Turret(Content, new Vector2(1920 / 3, 1080 / 3));
            player = new Player(Content, tileManager.tileSprites, testTurret);
            background = new Background(Content);
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime) {
            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) {
                Exit();
            }

            if (player.isAlive) {
                player.update(deltaTime);
                player.mouseFollow();
                bulletCleanup(player.playerBullets, out player.playerBullets);

                cam.move(player.previousPosition - player.position);
                background.loop(player.position);
            }

            testTurret.update(player.position, deltaTime);
            bulletCleanup(testTurret.bullets, out testTurret.bullets);

            //kState_OLD = Keyboard.GetState();
            //mState_OLD = Mouse.GetState();

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
            background.draw(_spriteBatch);
            testTurret.Draw(_spriteBatch);
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        void bulletCleanup(List<Bullet> bullets, out List<Bullet> outBullets) {
            List<Bullet> bulletsToRemove = bullets;
            for (int i = 0; i < bullets.Count; i++) {
                Bullet bullet = bullets[i];
                if (bullet.isActive) {
                    bullet.PhysicsUpdate(deltaTime);
                    bullet.update(deltaTime);
                } else {
                    bulletsToRemove.RemoveAt(i);
                }
            }
            outBullets = bulletsToRemove;
        }

    }
}