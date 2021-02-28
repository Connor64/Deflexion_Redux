// From tutorial by cmason at Rare Element Games: https://rareelementgames.wordpress.com/2017/04/21/game-state-management/
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Deflexion_Redux {
    public class GameStateManager {

        private ContentManager content;
        private Stack<GameState> screens = new Stack<GameState>();

        public bool quit = false;

        private static GameStateManager instance;
        public static GameStateManager Instance {
            get {
                if (instance == null) {
                    instance = new GameStateManager();
                }
                return instance;
            }
        }

        public void setContent(ContentManager content) {
            this.content = content;
        }

        // Adds a screen to the top of the stack
        public void AddScreen(GameState screen) {
            try {
                screens.Push(screen);
                screens.Peek().Initialize();
                if (content != null) {
                    screens.Peek().LoadContent(content);
                }
            } catch (Exception ex) {
                Debug.Print(ex.Message);
            }
        }

        // Removes the top screen
        public void RemoveTopScreen() {
            if (screens.Count > 0) {
                try {
                    var screen = screens.Peek();
                    screens.Pop();
                } catch (Exception ex) {
                    Debug.Print(ex.Message);
                }
            }
        }

        // Removes all screens
        public void ClearScreens() {
            while (screens.Count > 0) {
                screens.Pop();
            }
        }

        // Removes all screens from the stack and adds a new one 
        public void ChangeScreen(GameState screen) {
            try {
                ClearScreens();
                AddScreen(screen);
            } catch (Exception ex) {
                Debug.Print(ex.Message);
            }
        }

        // Updates the top screen.
        public void Update(GameTime gameTime) {
            try {
                if (screens.Count > 0) {
                    screens.Peek().Update(gameTime);
                }
            } catch (Exception ex) {
                Debug.Print(ex.Message);
            }
        }

        // Draws the top screen
        public void Draw(SpriteBatch spriteBatch) {
            try {
                if (screens.Count > 0) {
                    screens.Peek().Draw(spriteBatch);
                }
            } catch (Exception ex) {
                Debug.Print(ex.Message);
            }
        }

        // Unloads all of the content in every screen
        public void UnloadContent() {
            foreach (GameState state in screens) {
                state.UnloadContent();
            }
        }
    }
}