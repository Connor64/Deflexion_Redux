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
        private DropDownContainer dropdown;
        private Button sizeButton_UP;
        public bool visible = false;

        public int deltaWidth = 0, deltaHeight = 0;

        private int tileSize = 16;
        private float tileScale = 1;

        private List<AnimatedSprite> tile_pallette;
        private Dictionary<string, Texture2D> tilesets = new Dictionary<string, Texture2D>();
        private Dictionary<string, TextureType> tilesetTypes = new Dictionary<string, TextureType>();

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
        private KeyboardState oldState;

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

        public void Load(string directory, ref GraphicsDeviceManager device, string folder) {
            this.directory = directory;
            oldState = Keyboard.GetState();

            foreach (TextureType type in keys) {
                tilesets.Add(type.ToString(), AssetManager.textures[type]);
                tilesetTypes.Add(type.ToString(), type);
            }

            tile_pallette = new List<AnimatedSprite>();

            panel = new Panel(cam.position + new Vector2(cam.virtualWidth / 2 - 130, -cam.virtualHeight / 2), new Vector2(cam.virtualWidth / 4, cam.virtualHeight), Color.Gray, Sprite.Layers["UI"], ref device);
            dropdown = new DropDownContainer(new List<string>(tilesets.Keys), panel.position, new Vector2(100, 20), AssetManager.fonts[FontType.arial], ref device);
            sizeButton_UP = new Button(delegate() { deltaWidth++; deltaHeight++; }, "+10", dropdown.position + new Vector2(0, 200), new Vector2(100, 50), Color.White, AssetManager.fonts[FontType.arial], ref device);
            //sizeButton_UP.action = this.ChangeLevelSize;
        }

        public void Toggle() {
            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) && !oldState.IsKeyDown(Keys.LeftShift)) {
                visible = !visible;
                Debug.Print("editor visibility: " + visible);
            }
            oldState = Keyboard.GetState();
        }

        // Only called in Main when visible
        public void Update() {
            MoveUI();
            
            if (Keyboard.GetState().IsKeyDown(Keys.P) && !oldState.IsKeyDown(Keys.P))
                SaveLayout(); // Saves level layout to .lvl XML file

            if (selected)
                tileCursorUpdate();

            if (dropdown.changed)
                loadTileset();

            if (Mouse.GetState().LeftButton == ButtonState.Pressed && !dropdown.mainPanel.isHovering())
                dropdown.dropped = false;

            dropdown.changed = false;
        }

        public void ScaleUI() {
            panel.size.X = (cam.virtualWidth / cam.zoom) / 4;
            panel.size.Y = cam.virtualHeight / cam.zoom;

            tileScale = tileScale / cam.zoom;
        }

        public void MoveUI() {
            panel.position = cam.position + new Vector2((cam.virtualWidth / cam.zoom) / 4, (-cam.virtualHeight / cam.zoom) / 2);
            dropdown.position = panel.position + new Vector2((panel.size.X - dropdown.size.X) / 2, 400);

            dropdown.Update();

            float x = 0, y = 0;
            for (int i = 0; i < tile_pallette.Count; i++) {
                tile_pallette[i].Position = panel.position + new Vector2((panel.size.X - tile_pallette[i].sheetSize.X) / 2 + x, panel.size.Y / 5 + y);

                if (tile_pallette[i].isHovering() && Mouse.GetState().LeftButton == ButtonState.Pressed) {
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
            //string path = Path.Combine($"{System.AppDomain.CurrentDomain.BaseDirectory}/Content", "testLevel2.xml");
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "testLevel.xml");
            XmlManager.Save<Level>(@path, new Level(levelManager.getTiles()));
        }

        public void tileCursorUpdate() {
            Vector2 mousePosition = cam.getMousePosition();
            selectedTile.Position = new Vector2(MathF.Round((mousePosition.X - (tileSize / 2)) / tileSize, MidpointRounding.AwayFromZero),
                                                MathF.Round((mousePosition.Y - (tileSize / 2)) / tileSize, MidpointRounding.AwayFromZero)) * tileSize;
            int x = (int)selectedTile.Position.X / tileSize + levelManager.getTiles().GetLength(0) / 2;
            int y = (int)selectedTile.Position.Y / tileSize + levelManager.getTiles().GetLength(1) / 2;

            if (Mouse.GetState().LeftButton == ButtonState.Pressed && !panel.isHovering() && x >= 0 && x < levelManager.getTiles().GetLength(0) && y >= 0 && y < levelManager.getTiles().GetLength(1)) {
                levelManager.getTiles()[x, y] = new Tile(tilesetTypes[dropdown.text], selectedTile.Position, true, Sprite.Layers["Tiles"], tileSize, selectedTile.currentFrame);
            }

            if (Mouse.GetState().RightButton == ButtonState.Pressed && x >= 0 && x < levelManager.getTiles().GetLength(0) && y >= 0 && y < levelManager.getTiles().GetLength(1)) {
                if (!panel.isHovering()) {
                    levelManager.getTiles()[x, y] = new Tile();
                }
            }
        }

        public void loadTileset() {
            tile_pallette.Clear();
            tileScale = 2;
            int heightCount = tilesets[dropdown.text].Height / tileSize;
            int widthCount = tilesets[dropdown.text].Width / tileSize;

            for (int y = 0; y < heightCount; y++) {
                for (int x = 0; x < widthCount; x++) {
                    tile_pallette.Add(new AnimatedSprite(tilesetTypes[dropdown.text], dropdown.position + new Vector2(dropdown.size.X / 2, -300), 0, Vector2.Zero, new Vector2(tileScale, tileScale), heightCount, widthCount, tileSize, x + (y * heightCount), Sprite.Layers["UI"] - 0.01f));
                }
            }

            while (tilesets[dropdown.text].Width * tileScale > panel.size.X && tileScale > 0.5f) {
                tileScale -= 0.5f;
            }
        }

        public void ChangeLevelSize(int deltaX, int deltaY) {
            Tile[,] oldArray = levelManager.tileArray;
            int _width = oldArray.GetLength(0);
            int _height = oldArray.GetLength(1);
            Tile[,] newArray = new Tile[_width + deltaX, _height + deltaY];
            for (int x = 0; x < _width + deltaX; x++) {
                for (int y = 0; y < _height + deltaY; y++) {
                    if (x < _width && y < _height) {
                        newArray[x, y] = oldArray[x, y];
                    }  else {
                        newArray[x, y] = new Tile();
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            if (visible) {
                panel.Draw(spriteBatch);
                dropdown.Draw(spriteBatch);

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