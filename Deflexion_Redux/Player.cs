using System;
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
        private float playerForce = 2000f;
        private float turn = 10f;
        float rotation = 0;
        int rotationCount = 1;

        public AnimatedSprite playerSprite;
        private AnimatedSprite shieldSprite;
        private Texture2D bulletTexture;

        private KeyboardState kstate_old;

        private List<Bullet> playerBullets = new List<Bullet>();

        public Player(ContentManager Content) {
            position = new Vector2(500, 500);
            mass = 1f;
            baseSpeedLimit = 500f;
            collisionBoxSize = 32f;
            player = true;
            instantaneous = false;

            playerSprite = new AnimatedSprite(Content.Load<Texture2D>("Sprites/deflector_SpriteSheet"), position, 0, new Vector2(2, 2), 1, 3, 16);
            shieldSprite = new AnimatedSprite(Content.Load<Texture2D>("Sprites/shields_SpriteSheet"), playerSprite.Position, 0, new Vector2(2, 2), 1, 6, 32);
            bulletTexture = Content.Load<Texture2D>("Sprites/shotgunBlast");

            playerSprite.Origin = new Vector2(8, 8);
            shieldSprite.Origin = new Vector2(16, 16);
            kstate_old = Keyboard.GetState();
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
                playerSprite.setFrame(0);
                playerSprite.Rotation -= turn * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (playerSprite.Rotation < 0) {
                    playerSprite.Rotation += 2 * MathF.PI;
                }
            } else if (kstate.IsKeyDown(Keys.D) && !kstate.IsKeyDown(Keys.A)) {
                //newPosition.X += movement;
                //playerSprite.Position = collision(playerSprite.Position, newPosition, tiles);
                playerSprite.setFrame(2);
                playerSprite.Rotation += turn * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (playerSprite.Rotation >= 2 * MathF.PI) {
                    playerSprite.Rotation -= 2 * MathF.PI;
                }
            } else {
                playerSprite.setFrame(1);
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
            var kstate = Keyboard.GetState();
            Vector2 direction = Vector2.Zero;

            playerSprite.setFrame(1);

            if (kstate.IsKeyDown(Keys.W) && !kstate.IsKeyDown(Keys.S)) {
                direction += new Vector2(0, -1);
                Debug.Print("W is pressed!");
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

            if (kstate.IsKeyDown(Keys.Space) && !kstate_old.IsKeyDown(Keys.Space) && direction != Vector2.Zero) {
                addForce(direction, playerForce * 3f, 750f);
            }

            playerSprite.Position = position;

            if (kstate.IsKeyDown(Keys.O) && !kstate_old.IsKeyDown(Keys.O)) {
                shoot();
            }

            kstate_old = kstate;

            PhysicsUpdate(deltaTime, tiles);
            foreach (Bullet bullet in playerBullets) {
                bullet.PhysicsUpdate(deltaTime, tiles);
                bullet.update();
            }
        }

        void shoot() {
            playerBullets.Add(new Bullet(bulletTexture, new Vector2(position.X - playerSprite.pixelWidth, position.Y - playerSprite.pixelWidth), shieldSprite.Rotation));
            addForce(new Vector2(MathF.Cos(shieldSprite.Rotation + MathF.PI / 2), MathF.Sin(shieldSprite.Rotation + MathF.PI / 2)), 750f, 100f);
        }

        public void shieldPowers() {
            MouseState mState = Mouse.GetState();
            //shieldSprite.Position = playerSprite.Position + new Vector2(playerSprite.textureSize.X / 2, playerSprite.textureSize.Y / 2);
            shieldSprite.Position = playerSprite.Position;
            float x = shieldSprite.Position.X - mState.X;
            float y = shieldSprite.Position.Y - mState.Y;
            shieldSprite.Rotation = -(float)Math.Atan2(x, y);

            //if (kstate.IsKeyDown(Keys.O) && !kstate_old.IsKeyDown(Keys.O)) {
            //    shoot();
            //}
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
            playerSprite.Draw(spriteBatch);
            shieldSprite.Draw(spriteBatch);
            foreach (Bullet bullet in playerBullets) {
                bullet.draw(spriteBatch);
            }
        }
    }
}