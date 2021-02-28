using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deflexion_Redux {
    public class GameplayScreen : GameState {

        private LevelType level;
        private LevelManager levelManager = LevelManager.Instance;
        private LevelEditor levelEditor = LevelEditor.Instance;
        private EnemyManager enemyManager = EnemyManager.Instance;

        private Player player;
        private Background background;

        private Panel pausePanel;
        private Button return_Button, exit_Button;

        private float deltaTime = 0;
        private bool paused = false;

        private KeyboardState kState_OLD;

        public GameplayScreen(GraphicsDevice graphicsDevice, LevelType level) : base(graphicsDevice) {
            this.level = level;
        }

        public override void Initialize() {
            AudioManager.Instance.stopAllSounds();
            cam.zoom = 1;

            kState_OLD = Keyboard.GetState();

            pausePanel = new Panel(cam.position - new Vector2(-cam.virtualWidth / 2, -cam.virtualHeight / 2), new Vector2(cam.virtualWidth, cam.virtualHeight), Color.Black * 0.5f, Sprite.Layers[LayerType.UI], ref _graphicsDevice);

            return_Button = new Button(delegate { paused = false; }, new Vector2(-50, -50), new Vector2(100, 50), ref _graphicsDevice);
            return_Button.setText("Return", Alignment.Center, 1, FontType.arial);
            return_Button.setColor(ButtonStyle.Gray, 0.75f);

            exit_Button = new Button(delegate { GameStateManager.Instance.ChangeScreen(new TitleScreen(_graphicsDevice)); }, new Vector2(-50, 50), new Vector2(100, 50), ref _graphicsDevice, Color.LightGray * 0.75f, "Quit to Menu", FontType.arial);
            exit_Button.setText("Quit to Menu", Alignment.Center, 1, FontType.arial);
            exit_Button.setColor(ButtonStyle.Gray, 0.75f);

            AudioManager.Instance.playSong(SoundType.test_song_intro, SoundType.test_song_body, true, 0.75f);
        }

        public override void LoadContent(ContentManager content) {
            levelManager.Load(level, "");
            levelEditor.Load(content.RootDirectory, ref _graphicsDevice, "Tilesets");

            player = new Player();
            player.position = Vector2.Zero;

            background = new Background(player.position, TextureType.test_space_background);
            cam.position = player.position;
        }

        public override void Update(GameTime gameTime) {
            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            AudioManager.Instance.UpdateSongs();

            if (Keyboard.GetState().IsKeyDown(Keys.Escape) && !kState_OLD.IsKeyDown(Keys.Escape)) {
                paused = !paused;
                pausePanel.position = cam.position + new Vector2(-cam.virtualWidth / 2, -cam.virtualHeight / 2);

                return_Button.setPosition(cam.position + new Vector2(-50, -50));
                exit_Button.setPosition(cam.position + new Vector2(-50, 50));
            }

            if (!paused) {
                if (!levelEditor.visible) {

                    if (player.isAlive) {
                        player.Update(deltaTime);

                        cam.move(player.position, deltaTime);
                        background.loop(player.position);
                    }

                    bulletUpdate(player.playerBullets, out player.playerBullets);
                    enemyManager.Update(player.position, deltaTime);
                    bulletUpdate(enemyManager.enemyBullets, out enemyManager.enemyBullets);
                    cam.zoom = 1;
                } else {
                    levelEditor.Update();
                }
                levelEditor.Toggle();
            } else {
                return_Button.Update();
                exit_Button.Update();
            }

            kState_OLD = Keyboard.GetState();
        }

        public override void UnloadContent() {
        }
        public override void Draw(SpriteBatch spriteBatch) {
            _graphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend,
                               SamplerState.PointClamp, null, null, null, cam.getMatrix() * cam.getTransformationMatrix());

            levelManager.Draw(spriteBatch);
            player.Draw(spriteBatch);
            background.Draw(spriteBatch);
            levelEditor.Draw(spriteBatch);
            enemyManager.Draw(spriteBatch);
            if (paused) {
                pausePanel.Draw(spriteBatch);
                return_Button.Draw(spriteBatch);
                exit_Button.Draw(spriteBatch);
            }

            spriteBatch.End();
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