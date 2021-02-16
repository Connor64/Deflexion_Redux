using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Deflexion_Redux {
    class Player : Physics {

        private float playerForce = 1500f;

        int maxBullets = 25;

        public Sprite playerSprite;
        private Sprite shieldSprite;
        private Sprite gunSprite;
        private Texture2D bulletTexture;

        private Camera cam;

        private KeyboardState kstate_old;
        private MouseState mstate_old;

        public bool isAlive = true;

        private Turret test_turret;

        public List<Bullet> playerBullets = new List<Bullet>();

        public Player(ContentManager Content, List<Sprite> tiles, Turret turret) {
            cam = Camera.Instance;
            position = new Vector2(cam._Width / 4, cam._Height / 4);
            mass = 1f;
            baseSpeedLimit = 1000f;
            collisionBoxSize = 32f;
            this.tiles = tiles;
            instantaneous = false;
            resistance = 1000f;

            test_turret = turret;

            collisionBoxSize = 4f;

            bodyType = BodyType.Player;

            playerSprite = new Sprite(Content.Load<Texture2D>("Sprites/test_ship_bottom"), position, 0, Vector2.One, Sprite.Layers["Player"], new Vector2(8, 8));
            shieldSprite = new Sprite(Content.Load<Texture2D>("Sprites/test_shield"), playerSprite.Position, 0, Vector2.One, Sprite.Layers["Player"], new Vector2(32, 32));
            gunSprite = new Sprite(Content.Load<Texture2D>("Sprites/test_ship_top"), position, 0, Vector2.One, Sprite.Layers["Player"] - 0.5f, new Vector2(16, 17));
            shieldSprite.Layer = 0.5f;
            bulletTexture = Content.Load<Texture2D>("Sprites/shotgunBlast");

            kstate_old = Keyboard.GetState();
            mstate_old = Mouse.GetState();
        }

        public void update(float deltaTime) {
            MouseState mstate = Mouse.GetState();
            KeyboardState kstate = Keyboard.GetState();

            movement(kstate);

            if (mstate.LeftButton == ButtonState.Pressed && mstate_old.LeftButton != ButtonState.Pressed && playerBullets.Count < maxBullets) {
                shoot();
            }

            PhysicsUpdate(deltaTime);
            
            if (collisionCheck(position, test_turret.bullets, out Bullet bullet)) {
                isAlive = false;
                bullet.isActive = false;
            }

            playerSprite.Position = position;
            gunSprite.Position = position;

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
            playerBullets.Add(new Bullet(bulletTexture, position, gunSprite.Rotation, BulletType.Player));
            addForce(new Vector2(MathF.Cos(gunSprite.Rotation + MathF.PI / 2), MathF.Sin(gunSprite.Rotation + MathF.PI / 2)), playerForce * 15, baseSpeedLimit * 2.3f);
        }

        public void mouseFollow() {
            MouseState mState = Mouse.GetState();
            shieldSprite.Position = playerSprite.Position;
            Vector2 mousePosition = new Vector2(mState.X, mState.Y);
            Vector2 virtualViewPort = new Vector2(cam.virtualViewportX, cam.virtualViewportY);
            mousePosition = Vector2.Transform(mousePosition - virtualViewPort, Matrix.Invert(cam.getTransformationMatrix()));
            float x = shieldSprite.Position.X - mousePosition.X + cam.position.X;
            float y = shieldSprite.Position.Y - mousePosition.Y + cam.position.Y;

            shieldSprite.Rotation = -(float)Math.Atan2(x, y);

            gunSprite.Rotation = shieldSprite.Rotation + MathF.PI;

        }

        public void Draw(SpriteBatch spriteBatch) {
            playerSprite.draw(spriteBatch);
            gunSprite.draw(spriteBatch);
            shieldSprite.draw(spriteBatch);
            foreach (Bullet bullet in playerBullets) {
                bullet.draw(spriteBatch);
            }
        }
    }
}