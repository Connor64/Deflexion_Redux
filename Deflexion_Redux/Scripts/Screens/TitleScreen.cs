using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Deflexion_Redux {
    public class TitleScreen : GameState {

        private Sprite logo;
        private Sprite background;
        private Button playButton;
        private Button quitButton;

        private Panel copyrightInfo;
        private Panel versionInfo;

        public TitleScreen(GraphicsDevice graphicsDevice) : base(graphicsDevice) {}

        public override void Initialize() {
            AudioManager.Instance.stopAllSounds();
            cam.zoom = 1;
            cam.position = Vector2.Zero;

            logo = new Sprite(TextureType.deflexion_logo, new Vector2(cam.virtualWidth / 2, 130), 0, Sprite.Layers[LayerType.UI], new Vector2(2, 2), 
                new Vector2(AssetManager.textures[TextureType.deflexion_logo].Width / 2, AssetManager.textures[TextureType.deflexion_logo].Height / 2));
            background = new Sprite(TextureType.test_space_background, Vector2.Zero, 0, Sprite.Layers[LayerType.Background], Vector2.One);

            playButton = new Button(delegate { GameStateManager.Instance.AddScreen(new LevelSelectScreen(_graphicsDevice)); }, new Vector2(-50, 10), new Vector2(100, 50), ref _graphicsDevice);
            playButton.setScreenPosition(ScreenPosition.Center, new Vector2(0, 10), 1);
            playButton.setText("Start", Alignment.Center, 1, FontType.arial);
            playButton.setColor(ButtonStyle.Gray, 0.75f);

            quitButton = new Button(delegate { GameStateManager.Instance.quit = true; }, new Vector2(-50, 90), new Vector2(100, 50), ref _graphicsDevice);
            quitButton.setScreenPosition(ScreenPosition.Center, new Vector2(0, 80), 1 / cam.zoom);
            quitButton.setText("Quit", Alignment.Center, 1, FontType.arial);
            quitButton.setColor(ButtonStyle.Gray, 0.75f);

            copyrightInfo = new Panel(new Vector2(-cam.virtualWidth / 2, cam.virtualHeight / 2), Vector2.Zero,
                Color.Transparent, Sprite.Layers[LayerType.UI], ref _graphicsDevice, "Copyright 2021 Inverted Peas", FontType.arial, Alignment.Left);
            copyrightInfo.size = AssetManager.fonts[copyrightInfo.font].MeasureString(copyrightInfo.getText());
            copyrightInfo.setScreenPosition(ScreenPosition.BottomLeft, new Vector2(10, 0), 1);
            copyrightInfo.textColor = Color.White;

            versionInfo = new Panel(new Vector2(cam.virtualWidth / 2, cam.virtualHeight / 2), Vector2.Zero,
                Color.Transparent, Sprite.Layers[LayerType.UI], ref _graphicsDevice, "0.2.2 PreAplpha", FontType.arial, Alignment.Left);
            versionInfo.size = AssetManager.fonts[versionInfo.font].MeasureString(versionInfo.getText());
            versionInfo.setScreenPosition(ScreenPosition.BottomRight, new Vector2(-10, 0), 1);
            versionInfo.textColor = Color.White;

            AudioManager.Instance.playSong(SoundType.test_song_intro, SoundType.test_song_body, true, 0.75f);
        }

        public override void LoadContent(ContentManager content) {
            AssetManager.UnloadLevels();
            AssetManager.LoadLevels(content.RootDirectory);
        }
        public override void Update(GameTime gameTime) {
            AudioManager.Instance.UpdateSongs();
            playButton.Update();
            quitButton.Update();
        }

        public override void UnloadContent() {

        }

        public override void Draw(SpriteBatch spriteBatch) {
            _graphicsDevice.Clear(Color.CornflowerBlue);

            drawHandler.DrawUI(spriteBatch); // not scaling based on zoom as all elements are UI based

            logo.Draw(spriteBatch);
            background.Draw(spriteBatch);
            playButton.Draw(spriteBatch);
            quitButton.Draw(spriteBatch);
            copyrightInfo.Draw(spriteBatch);
            versionInfo.Draw(spriteBatch);

            spriteBatch.End();
        }
    }
}