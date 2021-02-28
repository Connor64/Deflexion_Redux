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
    public class LevelEditor {
        private Panel panel;
        private Panel canvas;
        private DropDownContainer dropdown;
        private Button button_size_up, button_size_down;
        private string dropdownText = "";

        public bool visible = false;

        private int tileSize = 16;
        private readonly float TILE_SCALE = 2;
        private float scrollValue_OLD = 0;
        private float tileScale;

        private List<AnimatedSprite> tile_pallette;
        private Dictionary<string, TextureType> tilesets = new Dictionary<string, TextureType>();

        private TextureType[] keys = new TextureType[] {
            TextureType.test_tile,
            TextureType.editor_test_1,
            TextureType.editor_test_2,
            TextureType.editor_test_3,
        };

        private bool selected = false;
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
            tilesets.Clear();

            tileScale = TILE_SCALE;
            this.directory = directory;
            kState_OLD = Keyboard.GetState();

            foreach (TextureType type in keys) {
                tilesets.Add(type.ToString(), type);
            }

            tile_pallette = new List<AnimatedSprite>();

            canvas = new Panel(new Vector2(levelManager.tiles.GetLength(0), levelManager.tiles.GetLength(1)) * -tileSize/2, new Vector2(levelManager.tiles.GetLength(0), levelManager.tiles.GetLength(1)) * tileSize, Color.Red * 0.5f, Sprite.Layers[LayerType.Canvas], ref device);
            panel = new Panel(cam.position + new Vector2(cam.virtualWidth / 2 - 130, -cam.virtualHeight / 2), new Vector2(cam.virtualWidth / 4, cam.virtualHeight), Color.Gray, Sprite.Layers[LayerType.UI], ref device);
            dropdown = new DropDownContainer(panel.position, new Vector2(100, 20), FontType.arial, new List<string>(tilesets.Keys), ref device);
            button_size_up = new Button(delegate() { ChangeLevelSize(10, 10); }, dropdown.position + new Vector2(0, 200), new Vector2(50, 30), ref device, Color.White, "+10", FontType.arial);
            button_size_down = new Button(delegate () { ChangeLevelSize(-10, -10); }, dropdown.position + new Vector2(0, 200), new Vector2(50, 30), ref device, Color.White, "-10", FontType.arial);
        }

        // Always called
        public void Toggle() {
            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) && !kState_OLD.IsKeyDown(Keys.LeftShift)) {
                visible = !visible;
                Debug.Print("editor visibility: " + visible);
                ScaleUI();
                MoveUI();
            }
            kState_OLD = Keyboard.GetState();
        }

        // Only called in Main when visible
        public void Update() {
            MoveCamera();
            MoveUI();
            dropdown.Update();
            button_size_up.Update();
            button_size_down.Update();

            if (Keyboard.GetState().IsKeyDown(Keys.P) && !kState_OLD.IsKeyDown(Keys.P))
                SaveLayout(); // Saves level layout to .lvl XML file

            if (selected)
                tileCursorUpdate();

            if (dropdown.getText() != dropdownText) {
                dropdownText = dropdown.getText();
                loadTileset();
                ScaleUI();
                selected = false;
            }
        }

        public void MoveCamera() {
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
                } else if (cam.zoom > 0.1f) {
                    cam.zoom -= 0.1f;
                }
                ScaleUI();
            }

            scrollValue_OLD = Mouse.GetState().ScrollWheelValue;
        }

        public void ScaleUI() {
            panel.size.X = (cam.virtualWidth / cam.zoom) / 4;
            panel.size.Y = cam.virtualHeight / cam.zoom;
            dropdown.setSize(new Vector2(100, 20) / cam.zoom, 1 / cam.zoom);
            button_size_up.setSize(new Vector2(50, 30) / cam.zoom, 1 / cam.zoom);
            button_size_down.setSize(button_size_up.size, 1 / cam.zoom);

            tileScale = TILE_SCALE / cam.zoom;

            foreach(AnimatedSprite tile in tile_pallette) {
                tile.setScale(new Vector2(tileScale, tileScale));
            }
        }

        public void MoveUI() {
            panel.position = cam.position + new Vector2((cam.virtualWidth / cam.zoom) / 4, (-cam.virtualHeight / cam.zoom) / 2);
            dropdown.setPosition(panel.position + new Vector2((panel.size.X - dropdown.size.X) / 2, 400 / cam.zoom));
            button_size_up.setPosition(panel.position + new Vector2((panel.size.X - button_size_up.size.X) / 1.5f, 300 / cam.zoom));
            button_size_down.setPosition(panel.position + new Vector2((panel.size.X - button_size_up.size.X) / 3, 300 / cam.zoom));

            canvas.position = new Vector2(levelManager.tiles.GetLength(0), levelManager.tiles.GetLength(1)) * -tileSize / 2;
            canvas.size = new Vector2(levelManager.tiles.GetLength(0), levelManager.tiles.GetLength(1)) * tileSize;

            float x = 0, y = 0;
            for (int i = 0; i < tile_pallette.Count; i++) {
                tile_pallette[i].Position = panel.position + new Vector2((panel.size.X - tile_pallette[i].sheetSize.X) / 2 + x, panel.size.Y / 5 + y);

                if (tile_pallette[i].isHovering(tileScale) && Mouse.GetState().LeftButton == ButtonState.Pressed) {
                    selected = true;
                    selectedTile = tile_pallette[i].animCopyOf();
                    selectedTile.Scale = Vector2.One;
                }

                x += tileSize * tileScale;
                if (x >= tile_pallette[i].sheetSize.X) {
                    x = 0;
                    y += tileSize * tileScale;
                }
            }
        }

        public void SaveLayout() {
            //string path = Path.Combine("Content", "testLevel.lvl");
            string path = Path.Combine(directory, "testLevel2.xml");
            //string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "testLevel.xml");
            XmlManager.Save<Level>(@path, new Level(levelManager.tiles));
        }

        public void tileCursorUpdate() {
            Vector2 mousePosition = cam.getMousePosition();
            selectedTile.Position = new Vector2(MathF.Round((mousePosition.X - (tileSize / 2)) / tileSize, MidpointRounding.AwayFromZero),
                                                MathF.Round((mousePosition.Y - (tileSize / 2)) / tileSize, MidpointRounding.AwayFromZero)) * tileSize;
            int x = (int)selectedTile.Position.X / tileSize + levelManager.tiles.GetLength(0) / 2;
            int y = (int)selectedTile.Position.Y / tileSize + levelManager.tiles.GetLength(1) / 2;

            if (Mouse.GetState().LeftButton == ButtonState.Pressed && !panel.isHovering() && x >= 0 && x < levelManager.tiles.GetLength(0) && y >= 0 && y < levelManager.tiles.GetLength(1)) {
                levelManager.tiles[x, y] = new Tile(tilesets[dropdown.getText()], selectedTile.Position, true, Sprite.Layers[LayerType.Tiles], tileSize, selectedTile.currentFrame);
            }

            if (Mouse.GetState().RightButton == ButtonState.Pressed && x >= 0 && x < levelManager.tiles.GetLength(0) && y >= 0 && y < levelManager.tiles.GetLength(1)) {
                if (!panel.isHovering()) {
                    levelManager.tiles[x, y] = new Tile();
                }
            }
        }

        public void loadTileset() {
            tile_pallette.Clear();
            tileScale = TILE_SCALE;
            int heightCount = AssetManager.textures[tilesets[dropdown.getText()]].Height / tileSize;
            int widthCount = AssetManager.textures[tilesets[dropdown.getText()]].Width / tileSize;

            for (int y = 0; y < heightCount; y++) {
                for (int x = 0; x < widthCount; x++) {
                    tile_pallette.Add(new AnimatedSprite(tilesets[dropdown.getText()], dropdown.position + new Vector2(dropdown.size.X / 2, -300), 0, tileSize, Sprite.Layers[LayerType.UI] - 0.01f, new Vector2(tileScale, tileScale), Vector2.Zero, x + (y * heightCount)));
                }
            }

            while (AssetManager.textures[tilesets[dropdown.getText()]].Width * tileScale > panel.size.X && tileScale > 0.5f) {
                tileScale -= 0.5f;
            }
        }

        public void ChangeLevelSize(int deltaX, int deltaY) {
            Tile[,] oldArray = levelManager.tiles;
            int _width = oldArray.GetLength(0);
            int _height = oldArray.GetLength(1);
            Tile[,] newArray = new Tile[_width + deltaX, _height + deltaY];
            for (int x = 0; x < _width + deltaX; x++) {
                for (int y = 0; y < _height + deltaY; y++) {
                    if (x < deltaX / 2 || x >= _width + (deltaX - (deltaX / 2))|| y < deltaY / 2 || y >= _height + (deltaY - (deltaY / 2))) {
                        newArray[x, y] = new Tile();
                    }  else {
                        newArray[x, y] = oldArray[x - (deltaX / 2), y - (deltaY / 2)];
                    }
                }
            }
            levelManager.tiles = newArray;
        }

        public void Draw(SpriteBatch spriteBatch) {
            if (visible) {
                canvas.Draw(spriteBatch);
                panel.Draw(spriteBatch);
                dropdown.Draw(spriteBatch);
                button_size_up.Draw(spriteBatch);
                button_size_down.Draw(spriteBatch);

                foreach (AnimatedSprite tile in tile_pallette) {
                    tile.animDraw(spriteBatch);
                }

                if (selected) {
                    selectedTile.animDraw(spriteBatch);
                }
            }
        }

    }
}