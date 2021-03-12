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
        private Stack<Button> levelButtons;
        private Sprite background;

        public LevelSelectScreen(GraphicsDevice graphicsDevice) : base(graphicsDevice) {}

        public override void Initialize() {
            cam.zoom = 1;
        }

        public override void LoadContent(ContentManager content) {
            levelButtons = new Stack<Button>();
            background = new Sprite(TextureType.test_space_background, Vector2.Zero, 0, Sprite.Layers[LayerType.Background], Vector2.One);
            
            back_Button = new Button(delegate { GameStateManager.Instance.RemoveTopScreen(); }, ScreenPosition.BottomLeft, new Vector2(10, -10), new Vector2(100, 50), ref _graphicsDevice);
            back_Button.setText("Return", Alignment.Center, 1, FontType.arial);
            back_Button.setColor(ButtonStyle.Gray, 0.75f);

            newLevel_Button = new Button(delegate { GameStateManager.Instance.ChangeScreen(new GameplayScreen(_graphicsDevice, LevelType.new_level)); },
                ScreenPosition.BottomRight, new Vector2(-10, -10), new Vector2(100, 50), ref _graphicsDevice);
            newLevel_Button.setText("New Level", Alignment.Center, 1, FontType.arial);
            newLevel_Button.setColor(ButtonStyle.Gray, 0.75f);

            float offset_X = (AssetManager.levels.Count - 1) * -62.5f;

            int i = 0;
            foreach (KeyValuePair<LevelType, Level> levels in AssetManager.levels) {
                levelButtons.Push(new Button(delegate { GameStateManager.Instance.ChangeScreen(new GameplayScreen(_graphicsDevice, levels.Key)); },
                                    ScreenPosition.Center, new Vector2(offset_X + (62.5f * i), 0), new Vector2(100, 50), ref _graphicsDevice));
                if (levels.Value.name == null) {
                    levelButtons.Peek().setText(levels.Key.ToString(), Alignment.Center, 1, FontType.arial);
                } else {
                    levelButtons.Peek().setText(levels.Value.name, Alignment.Center, 1, FontType.arial);
                }
                levelButtons.Peek().setColor(ButtonStyle.Gray, 0.75f);
                i++;
            }
        }
        public override void Update(GameTime gameTime) {
            AudioManager.Instance.UpdateSongs();
            newLevel_Button.Update();
            back_Button.Update();
            foreach(Button button in levelButtons) {
                button.Update();
            }
        }

        public override void UnloadContent() {
        }

        public override void Draw(SpriteBatch spriteBatch) {
            _graphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend,
                               SamplerState.PointClamp, null, null, null, cam.getTransformationMatrix());

            newLevel_Button.Draw(spriteBatch);
            back_Button.Draw(spriteBatch);
            background.Draw(spriteBatch);

            foreach(Button button in levelButtons) {
                button.Draw(spriteBatch);
            }

            spriteBatch.End();
        }
    }
}