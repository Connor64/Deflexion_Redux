using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Deflexion_Redux {
    public class LevelSelectScreen : GameState {

        private Button back_Button;
        private Button newLevel_Button;
        private Sprite background;

        public LevelSelectScreen(GraphicsDevice graphicsDevice) : base(graphicsDevice) {}

        public override void Initialize() {
            cam.zoom = 1;
        }

        public override void LoadContent(ContentManager content) {
            background = new Sprite(TextureType.test_space_background, new Vector2(-cam.virtualWidth / 2, -cam.virtualHeight / 2), 0, Sprite.Layers[LayerType.Background], Vector2.One);
            
            back_Button = new Button(delegate { GameStateManager.Instance.RemoveTopScreen(); }, ScreenPosition.BottomLeft, new Vector2(25, -25), new Vector2(100, 50), ref _graphicsDevice);
            back_Button.setText("Return", Alignment.Center, 1, FontType.arial);
            back_Button.setColor(ButtonStyle.Gray, 0.75f);

            newLevel_Button = new Button(delegate { GameStateManager.Instance.ChangeScreen(new GameplayScreen(_graphicsDevice, LevelType.new_level)); },
                ScreenPosition.Center, Vector2.Zero, new Vector2(100, 50), ref _graphicsDevice);
            newLevel_Button.setText("New Level", Alignment.Center, 1, FontType.arial);
            newLevel_Button.setColor(ButtonStyle.Gray, 0.75f);
        }
        public override void Update(GameTime gameTime) {
            AudioManager.Instance.UpdateSongs();
            newLevel_Button.Update();
            back_Button.Update();
        }

        public override void UnloadContent() {
        }

        public override void Draw(SpriteBatch spriteBatch) {
            _graphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend,
                               SamplerState.PointClamp, null, null, null, cam.getMatrix() * cam.getTransformationMatrix());

            newLevel_Button.Draw(spriteBatch);
            back_Button.Draw(spriteBatch);
            background.Draw(spriteBatch);

            spriteBatch.End();
        }
    }
}