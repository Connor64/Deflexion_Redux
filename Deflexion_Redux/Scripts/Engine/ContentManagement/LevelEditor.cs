using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Deflexion_Redux {

    public enum TileLayer {
        Foreground,
        Midground,
        Background,
    }

    public class LevelEditor {
        private Panel tilePanel;
        private Panel enemyPanel;
        private Panel canvas;
        private DropDownContainer tilesetDropdown;
        private string tilesetDropdownText = "fart"; // maybe change default value? idk
        private DropDownContainer tileLayerDropdown;
        private Button button_size_up, button_size_down;
        private Button[] enemyButtons;

        public bool visible = false;

        private readonly int tileSize = 16;
        private readonly float TILE_SCALE = 2;
        private float tileScale;
        private float scrollValue_OLD = 0;

        private List<AnimatedSprite> tile_pallette;
        private Dictionary<string, TextureType> tilesets = new Dictionary<string, TextureType>();

        private readonly TextureType[] keys = new TextureType[] {
            TextureType.test_tile,
            TextureType.editor_test_1,
            TextureType.editor_test_2,
            TextureType.editor_test_3,
            TextureType.alpha_tiles,
        };

        private readonly Dictionary<EnemyType, TextureType> enemyTypes = new Dictionary<EnemyType, TextureType>() {
            { EnemyType.Turret, TextureType.turret_bottom },
            { EnemyType.Drone, TextureType.test_drone }
        };


        private bool enemyIsSelected = false;
        private Sprite selectedEnemy;
        private TextureType enemyTexture;

        private bool tileIsSelected = false;
        private AnimatedSprite selectedTile;

        private Camera cam = Camera.Instance;
        private LevelManager levelManager = LevelManager.Instance;
        private KeyboardState kState_OLD;

        private string directory;

        private static LevelEditor instance = null;
        public static LevelEditor Instance {
            get {
                if (instance == null) {
                    instance = new LevelEditor();
                }
                return instance;
            }
        }

        public void Load(string directory, ref GraphicsDevice device, string folder) {
            kState_OLD = Keyboard.GetState();

            tile_pallette = new List<AnimatedSprite>();
            tilesets.Clear();
            this.directory = directory;

            tileScale = TILE_SCALE;

            foreach (TextureType type in keys) {
                tilesets.Add(type.ToString(), type);
            }

            canvas = new Panel(new Vector2(levelManager.foregroundTiles.GetLength(0), levelManager.foregroundTiles.GetLength(1)) * -tileSize / 2, new Vector2(levelManager.foregroundTiles.GetLength(0), levelManager.foregroundTiles.GetLength(1)) * tileSize, Color.White * 0.35f, Sprite.Layers[LayerType.Canvas], ref device);
            tilePanel = new Panel(ScreenPosition.TopRight, Vector2.Zero, new Vector2(240, 540), Color.Gray * 0.75f, Sprite.Layers[LayerType.UI], ref device);
            enemyPanel = new Panel(ScreenPosition.BottomLeft, Vector2.Zero, new Vector2(720, 100), Color.Gray * 0.75f, Sprite.Layers[LayerType.UI] + 0.01f, ref device);

            tilesetDropdown = new DropDownContainer(Vector2.Zero, new Vector2(100, 20), FontType.arial, new List<string>(tilesets.Keys), ref device);
            tilesetDropdown.setRelativePosition(tilePanel, ScreenPosition.BottomCenter, new Vector2(0, -100), 1);

            tileLayerDropdown = new DropDownContainer(Vector2.Zero, new Vector2(100, 20), FontType.arial, new List<string> {
                                "Foreground",
                                "Background"}, ref device);
            tileLayerDropdown.setRelativePosition(tilePanel, ScreenPosition.TopCenter, new Vector2(0, 10), 1);
            tileLayerDropdown.setText("Foreground", 1);

            button_size_up = new Button(delegate () { ChangeLevelSize(10, 10); }, tilesetDropdown.getPosition() + new Vector2(0, 200), new Vector2(50, 30), ref device, Color.White, "+10", FontType.arial);
            button_size_up.setRelativePosition(tilePanel, ScreenPosition.Center, new Vector2(35, 50), 1);

            button_size_down = new Button(delegate () { ChangeLevelSize(-10, -10); }, tilesetDropdown.getPosition() + new Vector2(0, 200), new Vector2(50, 30), ref device, Color.White, "-10", FontType.arial);
            button_size_down.setRelativePosition(tilePanel, ScreenPosition.Center, new Vector2(-35, 50), 1);

            // Sets up the buttons to add enemies
            enemyButtons = new Button[enemyTypes.Count];
            int i = 0;
            foreach (KeyValuePair<EnemyType, TextureType> type in enemyTypes) {
                TextureType texture = type.Value;
                enemyButtons[i] = new Button(delegate () {
                    enemyTexture = texture;
                    selectedEnemy = new Sprite(texture, Vector2.Zero, 0, Sprite.Layers[LayerType.UI] - 0.02f, Vector2.One);
                    tileIsSelected = false;
                    enemyIsSelected = true;

                }, ScreenPosition.BottomLeft, Vector2.Zero, new Vector2(50, 50), ref device, Color.White, texture);

                enemyButtons[i].setColor(Color.White * 0.75f, Color.Azure * 0.75f, Color.LightBlue * 0.75f, Color.Black);
                enemyButtons[i].setRelativePosition(enemyPanel, ScreenPosition.MiddleLeft, new Vector2(10 + 50 * i, 0), 1);
                i++;
            }
        }

        // Always called
        public void Toggle() {
            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) && !kState_OLD.IsKeyDown(Keys.LeftShift)) {
                visible = !visible;
                Debug.Print("editor visibility: " + visible);
                EnemyManager.Instance.appendEnemies(levelManager.enemies);
                GameplayScreen.player.isAlive = true;
                GameplayScreen.player.playerBullets.Clear();
            }
            kState_OLD = Keyboard.GetState();
        }

        // Only called when editor is visible/toggled
        public void Update() {
            MoveCamera();
            tilesetDropdown.Update();
            tileLayerDropdown.Update();
            button_size_up.Update();
            button_size_down.Update();

            if (Keyboard.GetState().IsKeyDown(Keys.P) && !kState_OLD.IsKeyDown(Keys.P))
                SaveLayout(); // Saves level layout to XML file

            foreach (AnimatedSprite tileSprite in tile_pallette) {
                if (tileSprite.isHovering(tileScale, false) && Mouse.GetState().LeftButton == ButtonState.Pressed) {
                    tileIsSelected = true;
                    enemyIsSelected = false;
                    selectedTile = tileSprite.animCopyOf();
                    selectedTile.Scale = Vector2.One;
                }
            }

            if (tileIsSelected || enemyIsSelected)
                CursorUpdate();


            if (tilesetDropdown.getText() != tilesetDropdownText) {
                tilesetDropdownText = tilesetDropdown.getText();
                LoadTileset();
                tileIsSelected = false;
                enemyIsSelected = false;
            }

            for (int i = 0; i < enemyButtons.Length; i++) {
                enemyButtons[i].Update();
            }
        }

        /// <summary>
        /// Moves and zooms the camera in the editor
        /// </summary>
        public void MoveCamera() {
            KeyboardState kState = Keyboard.GetState();
            float value = 5 / (cam.zoom * (2 - cam.zoom));

            if (kState.IsKeyDown(Keys.W)) {
                cam.position.Y -= value;
            } else if (kState.IsKeyDown(Keys.S)) {
                cam.position.Y += value;
            }
            if (kState.IsKeyDown(Keys.A)) {
                cam.position.X -= value;
            } else if (kState.IsKeyDown(Keys.D)) {
                cam.position.X += value;
            }


            if (Mouse.GetState().ScrollWheelValue != scrollValue_OLD) {
                if (Mouse.GetState().ScrollWheelValue > scrollValue_OLD) {
                    cam.zoom += 0.1f;
                } else if (cam.zoom > 0.1f) {
                    cam.zoom -= 0.1f;
                }
            }

            scrollValue_OLD = Mouse.GetState().ScrollWheelValue;
        }


        /// <summary>
        /// Saves the current level layout (exports to XML)
        /// </summary>
        public void SaveLayout() {
            string path = Path.Combine(directory, "testLevel2.xml");
            XmlManager.Save<Level>(@path, new Level(levelManager.foregroundTiles,
                                                    levelManager.backgroundTiles,
                                                    levelManager.enemies));
        }

        /// <summary>
        /// Updates the cursor's position and places a tile if it is clicked
        /// </summary>
        public void CursorUpdate() {
            Vector2 mousePosition = cam.getMousePosition(true);
            Vector2 cursorPosition = new Vector2(MathF.Round(((mousePosition.X / cam.scalar) - ((tileSize * cam.scalar) / 2)) / tileSize, MidpointRounding.AwayFromZero),
                                                    MathF.Round(((mousePosition.Y / cam.scalar) - ((tileSize * cam.scalar) / 2)) / tileSize, MidpointRounding.AwayFromZero)) * tileSize * cam.scalar;
            if (tileIsSelected)
                selectedTile.Position = cursorPosition;
            if (enemyIsSelected)
                selectedEnemy.Position = cursorPosition;

            int x = (int)(cursorPosition.X / (tileSize * cam.scalar) + levelManager.foregroundTiles.GetLength(0) / 2);
            int y = (int)(cursorPosition.Y / (tileSize * cam.scalar) + levelManager.foregroundTiles.GetLength(1) / 2);

            if (!tilePanel.isHovering(false) && !enemyPanel.isHovering(false) && x >= 0 && x < levelManager.foregroundTiles.GetLength(0) && y >= 0 && y < levelManager.foregroundTiles.GetLength(1)) {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed) {
                    if (tileIsSelected && !enemyIsSelected) {
                        placeTile(x, y, tileLayerDropdown.getText(), new Tile(tilesets[tilesetDropdown.getText()], selectedTile.Position, true, Sprite.Layers[LayerType.Tiles], tileSize, selectedTile.currentFrame)); ;
                        //levelManager.tiles[x, y] = new Tile(tilesets[tilesetDropdown.getText()], selectedTile.Position, true, Sprite.Layers[LayerType.Tiles], tileSize, selectedTile.currentFrame);
                    } else if (enemyIsSelected && !tileIsSelected) {
                        Vector2 enemyPosition = cursorPosition + (new Vector2(tileSize / 2, tileSize / 2)) * cam.scalar;
                        switch (enemyTexture) {
                            case TextureType.turret_bottom:
                                levelManager.enemies[x, y] = new EnemyTile(EnemyType.Turret, enemyPosition);
                                break;
                            case TextureType.test_drone:
                                levelManager.enemies[x, y] = new EnemyTile(EnemyType.Drone, enemyPosition);
                                break;
                            default:
                                Debug.Print("Uhhhh what that was not supposed to happen...");
                                break;
                        }
                    }
                }

                if (Mouse.GetState().RightButton == ButtonState.Pressed) {
                    if (!tilePanel.isHovering(false)) {
                        if (tileIsSelected && !enemyIsSelected) {
                            placeTile(x, y, tileLayerDropdown.getText(), new Tile());
                            //levelManager.foregroundTiles[x, y] = new Tile();
                        } else if (enemyIsSelected && !tileIsSelected) {
                            levelManager.enemies[x, y] = new EnemyTile(EnemyType.none, Vector2.Zero);
                        }
                    }
                }
            } else if (Mouse.GetState().RightButton == ButtonState.Pressed) {
                enemyIsSelected = false;
                tileIsSelected = false;
            }
        }

        public void LoadTileset() {
            tile_pallette.Clear();
            tileScale = TILE_SCALE;

            // Height/Width count set which tile it is in the tileset
            int heightCount = AssetManager.textures[tilesets[tilesetDropdown.getText()]].Height / tileSize;
            int widthCount = AssetManager.textures[tilesets[tilesetDropdown.getText()]].Width / tileSize;

            // Just creates the sprites, does not set them in correct position
            for (int _y = 0; _y < heightCount; _y++) {
                for (int _x = 0; _x < widthCount; _x++) {
                    tile_pallette.Add(new AnimatedSprite(tilesets[tilesetDropdown.getText()], Vector2.Zero, 0, tileSize, Sprite.Layers[LayerType.UI] - 0.01f, new Vector2(tileScale, tileScale), Vector2.Zero, _x + (_y * heightCount)));
                }
            }

            // If the tiles do not fit inside of the bounds of the panel, the scale will shrink until they are able to
            while (AssetManager.textures[tilesets[tilesetDropdown.getText()]].Width * tileScale > tilePanel.size.X && tileScale > 0.5f) {
                tileScale -= 0.1f;
            }

            // Positions tiles in correct position according to their original position inside tileset image
            float x = 0, y = 0;
            for (int i = 0; i < tile_pallette.Count; i++) {
                tile_pallette[i].Position = tilePanel.position + new Vector2(((tilePanel.size.X * cam.scalar - tile_pallette[i].sheetSize.X) / 2) + x, (tilePanel.size.Y / 5) * cam.scalar + y);

                x += tileSize * tileScale * cam.scalar;
                if (x >= tile_pallette[i].sheetSize.X) {
                    x = 0;
                    y += tileSize * tileScale * cam.scalar;
                }
            };
        }

        public void placeTile(int x, int y, string tileLayer, Tile tile) {
            switch (tileLayer) {
                case "Foreground":
                    levelManager.foregroundTiles[x, y] = tile;
                    break;
                case "Background":
                    levelManager.backgroundTiles[x, y] = tile;
                    break;
                default:
                    break;
            }
        }

        public void ChangeLevelSize(int deltaX, int deltaY) {
            Tile[,] tiles_OLD = levelManager.foregroundTiles;
            EnemyTile[,] enemyTiles_OLD = levelManager.enemies;

            int _width = tiles_OLD.GetLength(0);
            int _height = tiles_OLD.GetLength(1);

            Tile[,] tiles_NEW = new Tile[_width + deltaX, _height + deltaY];
            EnemyTile[,] enemyTiles_NEW = new EnemyTile[_width + deltaX, _height + deltaY];

            for (int x = 0; x < _width + deltaX; x++) {
                for (int y = 0; y < _height + deltaY; y++) {
                    if (x < deltaX / 2 || x >= _width + (deltaX - (deltaX / 2)) || y < deltaY / 2 || y >= _height + (deltaY - (deltaY / 2))) {
                        tiles_NEW[x, y] = new Tile();
                        enemyTiles_NEW[x, y] = new EnemyTile(EnemyType.none, Vector2.Zero);
                    } else {
                        tiles_NEW[x, y] = tiles_OLD[x - (deltaX / 2), y - (deltaY / 2)];
                        enemyTiles_NEW[x, y] = enemyTiles_OLD[x - (deltaX / 2), y - (deltaY / 2)];
                    }
                }
            }
            levelManager.foregroundTiles = tiles_NEW;
            levelManager.enemies = enemyTiles_NEW;

            canvas.position = new Vector2(levelManager.foregroundTiles.GetLength(0), levelManager.foregroundTiles.GetLength(1)) * -tileSize / (2 / cam.scalar);
            canvas.size = new Vector2(levelManager.foregroundTiles.GetLength(0), levelManager.foregroundTiles.GetLength(1)) * tileSize;
        }

        public void DrawUI(SpriteBatch spriteBatch) {
            if (visible) {
                tilePanel.Draw(spriteBatch);
                tilesetDropdown.Draw(spriteBatch);
                tileLayerDropdown.Draw(spriteBatch);
                button_size_up.Draw(spriteBatch);
                button_size_down.Draw(spriteBatch);
                enemyPanel.Draw(spriteBatch);

                foreach (AnimatedSprite tile in tile_pallette) {
                    tile.animDraw(spriteBatch);
                }

                foreach (Button button in enemyButtons) {
                    button.Draw(spriteBatch);
                }

            }
        }

        public void DrawCanvas(SpriteBatch spriteBatch) {
            if (visible) {
                canvas.Draw(spriteBatch);
                if (tileIsSelected)
                    selectedTile.animDraw(spriteBatch);
                if (enemyIsSelected) {
                    selectedEnemy.Draw(spriteBatch);
                }
                foreach (EnemyTile enemyTile in levelManager.enemies) {
                    if (enemyTile.enemyType != EnemyType.none) {
                        DrawHandler.Instance.DrawTexture(spriteBatch, enemyTypes[enemyTile.enemyType], enemyTile.position, Vector2.One, new Vector2(8, 8));
                    }
                }
            }
        }
    }
}