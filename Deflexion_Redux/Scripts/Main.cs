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
        private Player player;
        private LevelManager levelManager;
        private LevelEditor levelEditor;
        private EnemyManager enemyManager;
        private Background background;
        private Camera cam;

        public KeyboardState kState;
        public KeyboardState kState_OLD;
        public MouseState mState;
        public MouseState mState_OLD;
        private float scrollValue_OLD = 0;

        float deltaTime = 0;

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

            levelManager = LevelManager.Instance;
            levelEditor = LevelEditor.Instance;
            enemyManager = EnemyManager.Instance;

            _graphics.ApplyChanges();
            kState = Keyboard.GetState();
            kState_OLD = kState;

            base.Initialize();
        }

        protected override void LoadContent() {
            AssetManager.LoadTextures(Content);

            levelManager.Load(LevelType.test_level, "Content/entityLayout_1.bmp");

            player = new Player();

            enemyManager.Load(player);

            levelEditor.Load(Content.RootDirectory, ref _graphics, "Tilesets");

            player.position = Vector2.Zero;
            cam.position = player.position;

            background = new Background(player.position);
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime) {
            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) {
                //IsMouseVisible = true;
                Exit();
            }

            if (!levelEditor.visible) {
                if (player.isAlive) {
                    player.update(deltaTime);

                    cam.move(player.position, deltaTime);
                    background.loop(player.position);
                }

                bulletUpdate(player.playerBullets, out player.playerBullets);
                enemyManager.Update(player.position, deltaTime);
                bulletUpdate(enemyManager.enemyBullets, out enemyManager.enemyBullets);
                cam.zoom = 1;
            } else {
                KeyboardState kState = Keyboard.GetState();

                if (kState.IsKeyDown(Keys.W)) {
                    cam.position.Y -= 5;
                } else if (kState.IsKeyDown(Keys.S)) {
                    cam.position.Y += 5;
                }
                if (kState.IsKeyDown(Keys.A)) {
                    cam.position.X -= 5;
                } else if (kState.IsKeyDown(Keys.D)) {
                    cam.position.X += 5;
                }


                if (Mouse.GetState().ScrollWheelValue != scrollValue_OLD) {
                    if (Mouse.GetState().ScrollWheelValue > scrollValue_OLD) {
                        cam.zoom += 0.1f;
                    } else {
                        cam.zoom -= 0.1f;
                    }
                    levelEditor.ScaleUI();
                    //cam.zoom += (Mouse.GetState().ScrollWheelValue - scrollValue_OLD) / 1080f;
                }
                levelEditor.Update();
                scrollValue_OLD = Mouse.GetState().ScrollWheelValue;
            }

            levelEditor.Toggle();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            cam.BeginDraw();
            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied,
                               SamplerState.PointClamp, null, null, null, cam.getMatrix() * cam.getTransformationMatrix());

            //_spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            player.Draw(_spriteBatch);
            levelManager.Draw(_spriteBatch);
            background.draw(_spriteBatch);
            enemyManager.Draw(_spriteBatch);

            levelEditor.Draw(_spriteBatch);
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        void bulletUpdate(List<Bullet> bullets, out List<Bullet> outBullets) {
            List<Bullet> toRemove = bullets;
            for (int i = 0; i < bullets.Count; i++) {
                Bullet bullet = bullets[i];
                if (bullet.isActive) {
                    bullet.update(deltaTime);
                } else {
                    toRemove.RemoveAt(i);
                }
            }
            outBullets = toRemove;
        }
    }
}