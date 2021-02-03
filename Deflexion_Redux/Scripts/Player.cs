﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Deflexion_Redux {
    class Player : Physics {

        private float playerSpeed = 400f;
        private float playerForce = 1500f;
        private float turn = 10f;
        float rotation = 0;
        int rotationCount = 1;

        int maxBullets = 25;

        public Sprite playerSprite;
        //private AnimatedSprite shieldSprite;
        private Sprite shieldSprite;
        private Sprite gunSprite;
        private Texture2D bulletTexture;

        private Vector2 bounds;
        private Camera cam;

        private KeyboardState kstate_old;
        private MouseState mstate_old;

        private List<Bullet> playerBullets = new List<Bullet>();

        public Player(ContentManager Content, Vector2 bounds) {
            cam = Camera.Instance;
            this.bounds = bounds;
            boundary = bounds;
            position = new Vector2(1920/4, 1080/4);
            mass = 1f;
            baseSpeedLimit = 500f;
            collisionBoxSize = 32f;
            player = true;
            instantaneous = false;
            resistance = 1000f;

            //playerSprite = new AnimatedSprite(Content.Load<Texture2D>("Sprites/deflector_SpriteSheet"), position, 0, Vector2.One, 1, 3, 16);
            playerSprite = new Sprite(Content.Load<Texture2D>("Sprites/test_ship_bottom"), position, 0, Vector2.One, 0.5f, new Vector2(8, 8));
            //shieldSprite = new AnimatedSprite(Content.Load<Texture2D>("Sprites/shields_SpriteSheet"), playerSprite.Position, 0, Vector2.One, 1, 6, 32);
            shieldSprite = new Sprite(Content.Load<Texture2D>("Sprites/test_shield"), playerSprite.Position, 0, Vector2.One, 0.5f, new Vector2(32, 32));
            gunSprite = new Sprite(Content.Load<Texture2D>("Sprites/test_ship_top"), position, 0, Vector2.One, 0.45f, new Vector2(16, 17));
            shieldSprite.Layer = 0.5f;
            //shieldSprite.Origin = new Vector2(16, 16);
            bulletTexture = Content.Load<Texture2D>("Sprites/shotgunBlast");

            kstate_old = Keyboard.GetState();
            mstate_old = Mouse.GetState();
        }

        public void move(GameTime gameTime, List<Sprite> tiles) {
            Vector2 newPosition = playerSprite.Position;

            var kstate = Keyboard.GetState();
            Vector2 direction = new Vector2(MathF.Cos(playerSprite.Rotation - MathF.PI / 2), MathF.Sin(playerSprite.Rotation - MathF.PI / 2));
            //float movement = playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            //if (kstate.IsKeyDown(Keys.W) && !kstate.IsKeyDown(Keys.S)) {
            //    newPosition.Y -= movement;
            //    // height is relative to the top of the screen (less is higher, more is lower)

            //    playerSprite.Position = collision(playerSprite.Position, newPosition, tiles);
            //    // assign player's position according to where it is able to move

            //    newPosition = playerSprite.Position;
            //    // reset newPosition to current position (incase the player's position doesn't end up updating)
            //} else if (kstate.IsKeyDown(Keys.S) && !kstate.IsKeyDown(Keys.W)) {
            //    newPosition.Y += movement;
            //    playerSprite.Position = collision(playerSprite.Position, newPosition, tiles);
            //    newPosition = playerSprite.Position;
            //}

            if (kstate.IsKeyDown(Keys.W)) {
                newPosition += direction * playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                //playerSprite.Position = collision(playerSprite.Position, newPosition, tiles);
                turn = 3f;
            } else if (kstate.IsKeyDown(Keys.S)) {
                newPosition -= direction * (playerSpeed / 2) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                //playerSprite.Position = collision(playerSprite.Position, newPosition, tiles);
                turn = 6f;
            } else {
                turn = 6f;
            }

            if (kstate.IsKeyDown(Keys.A) && !kstate.IsKeyDown(Keys.D)) {
                //newPosition.X -= movement;
                //playerSprite.Position = collision(playerSprite.Position, newPosition, tiles);
                //playerSprite.setFrame(0);
                playerSprite.Rotation -= turn * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (playerSprite.Rotation < 0) {
                    playerSprite.Rotation += 2 * MathF.PI;
                }
            } else if (kstate.IsKeyDown(Keys.D) && !kstate.IsKeyDown(Keys.A)) {
                //newPosition.X += movement;
                //playerSprite.Position = collision(playerSprite.Position, newPosition, tiles);
                //playerSprite.setFrame(2);
                playerSprite.Rotation += turn * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (playerSprite.Rotation >= 2 * MathF.PI) {
                    playerSprite.Rotation -= 2 * MathF.PI;
                }
            } else {
                //playerSprite.setFrame(1);
            }
        }

        public void move2(GameTime gameTime, List<Sprite> tiles) {
            Vector2 oldPosition = playerSprite.Position;

            var kstate = Keyboard.GetState();
            float movement = playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            rotationCount = 0;

            if (kstate.IsKeyDown(Keys.A) && !kstate.IsKeyDown(Keys.D)) {
                oldPosition.X -= movement;
                //playerSprite.Position = collision(playerSprite.Position, oldPosition, tiles);
                rotation += (3 * MathF.PI) / 2;
                rotationCount++;
            } else if (kstate.IsKeyDown(Keys.D) && !kstate.IsKeyDown(Keys.A)) {
                oldPosition.X += movement;
                //playerSprite.Position = collision(playerSprite.Position, oldPosition, tiles);
                rotation += MathF.PI / 2;
                rotationCount++;
            }

            if (kstate.IsKeyDown(Keys.W) && !kstate.IsKeyDown(Keys.S)) {
                oldPosition.Y -= movement;                                                      // height is relative to the top of the screen (less is higher, more is lower)
                //playerSprite.Position = collision(playerSprite.Position, oldPosition, tiles);   // assign player's position according to where it is able to move
                if (rotation == (3 * MathF.PI) / 2) {
                    rotation += 2 * MathF.PI;
                } else {
                    rotation += 0;
                }
                rotationCount++;
            } else if (kstate.IsKeyDown(Keys.S) && !kstate.IsKeyDown(Keys.W)) {
                oldPosition.Y += movement;
                //playerSprite.Position = collision(playerSprite.Position, oldPosition, tiles);
                rotation += MathF.PI;
                rotationCount++;
            }

            if (rotationCount > 0) {
                playerSprite.Rotation = rotation / rotationCount;
                rotation = 0;
            } else {
                playerSprite.Rotation = rotation;
            }
        }

        public void physicsMove(float deltaTime, List<Sprite> tiles) {
            MouseState mstate = Mouse.GetState();
            KeyboardState kstate = Keyboard.GetState();

            Vector2 direction = Vector2.Zero;

            //playerSprite.setFrame(1);

            if (kstate.IsKeyDown(Keys.W) && !kstate.IsKeyDown(Keys.S)) {
                direction += new Vector2(0, -1);
                //addForce(new Vector2(0, -1), playerForce, false);
            } else if (kstate.IsKeyDown(Keys.S) && !kstate.IsKeyDown(Keys.W)) {
                direction += new Vector2(0, 1);
                //addForce(new Vector2(0, 1), playerForce, false);
            }

            if (kstate.IsKeyDown(Keys.D) && !kstate.IsKeyDown(Keys.A)) {
                direction += new Vector2(1, 0);
                //addForce(new Vector2(1, 0), playerForce, false);
            } else if (kstate.IsKeyDown(Keys.A) && !kstate.IsKeyDown(Keys.D)) {
                direction += new Vector2(-1, 0);
                //addForce(new Vector2(-1, 0), playerForce, false);
            }

            addForce(direction, playerForce, 0);

            //if (kstate.IsKeyDown(Keys.Space) && !kstate_old.IsKeyDown(Keys.Space) && direction != Vector2.Zero) {
            //    addForce(direction, playerForce * 3f, 750f);
            //}

            if (mstate.LeftButton == ButtonState.Pressed && mstate_old.LeftButton != ButtonState.Pressed && playerBullets.Count < maxBullets) {
                shoot();
            }

            PhysicsUpdate(deltaTime, tiles);
            List<Bullet> bulletsToRemove = playerBullets;
            for (int i = 0; i < playerBullets.Count; i++) {
                Bullet bullet = playerBullets[i];
                if (bullet.isActive) {
                    bullet.PhysicsUpdate(deltaTime, tiles);
                    bullet.update();
                } else {
                    bulletsToRemove.RemoveAt(i);
                    //bulletsToRemove.Add(i);
                }
            }
            playerBullets = bulletsToRemove;

            //foreach (int index in bulletsToRemove) {
            //    playerBullets.RemoveAt(index);
            //}

            playerSprite.Position = position;
            gunSprite.Position = position;

            kstate_old = kstate;
            mstate_old = mstate;
        }

        void shoot() {
            playerBullets.Add(new Bullet(bulletTexture, new Vector2(position.X - playerSprite.textureSize.X, position.Y - playerSprite.textureSize.Y), gunSprite.Rotation, bounds));
            addForce(new Vector2(MathF.Cos(gunSprite.Rotation + MathF.PI / 2), MathF.Sin(gunSprite.Rotation + MathF.PI / 2)), playerForce * 15, 1150f);
        }

        public void mouseFollow() {
            MouseState mState = Mouse.GetState();
            //shieldSprite.Position = playerSprite.Position + new Vector2(playerSprite.textureSize.X / 2, playerSprite.textureSize.Y / 2);
            shieldSprite.Position = playerSprite.Position;
            Vector2 mousePosition = new Vector2(mState.X, mState.Y);
            Vector2 virtualViewPort = new Vector2(cam.virtualViewportX, cam.virtualViewportY);
            mousePosition = Vector2.Transform(mousePosition - virtualViewPort, Matrix.Invert(cam.getTransformationMatrix()));
            float x = shieldSprite.Position.X - mousePosition.X + cam.position.X;
            float y = shieldSprite.Position.Y - mousePosition.Y + cam.position.Y;

            shieldSprite.Rotation = -(float)Math.Atan2(x, y);

            gunSprite.Rotation = shieldSprite.Rotation + MathF.PI;

        }

        //public Vector2 collision(Vector2 currentPosition, Vector2 newPosition, List<Sprite> tiles) {
        //    Vector2 nextPosition = newPosition;
        //    foreach (Sprite tile in tiles) {
        //        if (newPosition.Y + (playerSprite.pixelWidth * playerSprite.ColliderScale) < tile.Position.Y + tile.Texture.Height * 2 &&
        //            newPosition.Y + 2 * (playerSprite.pixelWidth * playerSprite.ColliderScale) > tile.Position.Y &&
        //            newPosition.X + (playerSprite.pixelWidth * playerSprite.ColliderScale) < tile.Position.X + tile.Texture.Width * 2 &&
        //            newPosition.X + 2 * (playerSprite.pixelWidth * playerSprite.ColliderScale) > tile.Position.X) {
        //            nextPosition = currentPosition;
        //            break;
        //        }
        //    }
        //    return nextPosition;
        //}

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