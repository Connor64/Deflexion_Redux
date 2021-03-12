using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Deflexion_Redux {
    public class Player : Physics {

        private float playerForce = 1500f;

        public Sprite playerSprite;
        private Sprite shieldSprite;
        private Sprite gunSprite;
        private Texture2D bulletTexture;

        private Camera cam = Camera.Instance;
        private EnemyManager enemyManager = EnemyManager.Instance;

        private KeyboardState kstate_old;
        private MouseState mstate_old;

        public bool isAlive = true;

        public List<Bullet> playerBullets = new List<Bullet>();
        int maxBullets = 25;

        public Player() {
            position = new Vector2(0.5f, 0.5f);
            mass = 1f;
            baseSpeedLimit = 500f;
            collisionBoxSize = 32f;
            instantaneous = false;
            resistance = 1000f;

            collisionBoxSize = 4f;

            bodyType = BodyType.Player;

            playerSprite = new Sprite(TextureType.player_bottom, position, 0, Sprite.Layers[LayerType.Player], Vector2.One, new Vector2(8, 8));
            shieldSprite = new Sprite(TextureType.player_shield, position, 0, Sprite.Layers[LayerType.Player], Vector2.One, new Vector2(32, 32));
            gunSprite = new Sprite(TextureType.player_gun, position, MathF.PI, Sprite.Layers[LayerType.Player] - 0.1f, Vector2.One, new Vector2(16, 17));
            bulletTexture = AssetManager.textures[TextureType.test_player_blast];

            kstate_old = Keyboard.GetState();
            mstate_old = Mouse.GetState();
        }

        public void Update(float deltaTime) {
            MouseState mstate = Mouse.GetState();
            KeyboardState kstate = Keyboard.GetState();

            movement(kstate);

            if (mstate.LeftButton == ButtonState.Pressed && mstate_old.LeftButton != ButtonState.Pressed && playerBullets.Count < maxBullets) {
                shoot();
            } else if (mstate.RightButton == ButtonState.Pressed && mstate_old.RightButton != ButtonState.Pressed && playerBullets.Count < maxBullets) {
                boost();
            }

            PhysicsUpdate(deltaTime);
            
            if (collisionCheck(position, enemyManager.enemyBullets, out Bullet bullet)) {
                isAlive = false;
                bullet.isActive = false;
            }

            playerSprite.Position = position;
            gunSprite.Position = position;

            mouseFollow();

            kstate_old = kstate;
            mstate_old = mstate;
        }

        void movement(KeyboardState kstate) {
            Vector2 direction = Vector2.Zero;

            if (kstate.IsKeyDown(Keys.W) && !kstate.IsKeyDown(Keys.S)) {
                direction += new Vector2(0, -1);
            } else if (kstate.IsKeyDown(Keys.S) && !kstate.IsKeyDown(Keys.W)) {
                direction += new Vector2(0, 1);
            }

            if (kstate.IsKeyDown(Keys.D) && !kstate.IsKeyDown(Keys.A)) {
                direction += new Vector2(1, 0);
            } else if (kstate.IsKeyDown(Keys.A) && !kstate.IsKeyDown(Keys.D)) {
                direction += new Vector2(-1, 0);
            }

            addForce(direction, playerForce, 0);
        }

        void shoot() {
            playerBullets.Add(new Bullet(TextureType.test_player_blast, position, 650, gunSprite.Rotation, 2));
            addForce(new Vector2(MathF.Cos(gunSprite.Rotation + MathF.PI / 2), MathF.Sin(gunSprite.Rotation + MathF.PI / 2)), playerForce * 7.5f, baseSpeedLimit * 1.9f);
        }

        void boost() {
            addForce(new Vector2(MathF.Cos(gunSprite.Rotation + MathF.PI / 2), MathF.Sin(gunSprite.Rotation + MathF.PI / 2)), playerForce * 20, baseSpeedLimit * 2.3f);
        }


        public void mouseFollow() {
            Vector2 mousePosition = cam.getMousePosition(true);
            float x = position.X - mousePosition.X;
            float y = position.Y - mousePosition.Y;

            shieldSprite.Position = position;

            gunSprite.Rotation = -(float)Math.Atan2(x, y);
            shieldSprite.Rotation = gunSprite.Rotation;
        }

        public void Draw(SpriteBatch spriteBatch) {
            if (isAlive) {
                playerSprite.Draw(spriteBatch);
                gunSprite.Draw(spriteBatch);
                shieldSprite.Draw(spriteBatch);
            }
            foreach (Bullet bullet in playerBullets) {
                bullet.draw(spriteBatch);
            }
        }
    }
}