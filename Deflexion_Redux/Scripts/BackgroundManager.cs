using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;

namespace Deflexion_Redux {
    class BackgroundManager {
        public Sprite[] backgrounds = new Sprite[4];
        private Vector2 textureSize;
        private Camera cam;

        private Vector2[][] positions;
        
        public BackgroundManager(ContentManager content) {
            Texture2D backgroundTexture = content.Load<Texture2D>("Sprites/spaceBackground_test");
            textureSize = new Vector2(backgroundTexture.Width, backgroundTexture.Height);
            positions = generatePositions();

            for (int i = 0; i < backgrounds.Length; i++) {
                backgrounds[i] = new Sprite(backgroundTexture, new Vector2(textureSize.X * i, 0), 0, Vector2.One, 1);
            }

            cam = Camera.Instance;
        }

        private Vector2[][] generatePositions() {
            Vector2[][] positions = new Vector2[][] {
                new Vector2[] {
                    new Vector2(-textureSize.X, 0),
                    new Vector2(-textureSize.X, textureSize.Y),
                    new Vector2(0, textureSize.Y)
                },
                new Vector2[] {
                    new Vector2(0, textureSize.Y),
                    new Vector2(textureSize.X, textureSize.Y),
                    new Vector2(textureSize.X, 0)
                },
                new Vector2[] {
                    new Vector2(-textureSize.X, 0),
                    new Vector2(-textureSize.X, -textureSize.Y),
                    new Vector2(0, -textureSize.Y)
                },
                new Vector2[] {
                    new Vector2(textureSize.X, 0),
                    new Vector2(textureSize.X, -textureSize.Y),
                    new Vector2(0, -textureSize.Y)
                }
            };
            return positions;
        }

        public void loop(Vector2 playerPosition) {
            Sprite sprite = currentSprite(playerPosition);
            if (sprite != null) {
                int positionVal = 0;
                if (playerPosition.X > sprite.textureSize.X / 2 + sprite.Position.X) {
                    positionVal++;
                }
                if (playerPosition.Y < sprite.textureSize.Y / 2 + sprite.Position.Y) {
                    positionVal += 2;
                }

                int index = 0;
                for (int i = 0; i < backgrounds.Length; i++) {
                    if (backgrounds[i] != sprite) {
                        backgrounds[i].Position = sprite.Position + positions[positionVal][index];
                        index++;
                    }
                }
            }
        }

        public Sprite currentSprite(Vector2 playerPosition) {
            Sprite sprite = null;
            foreach (Sprite backSprite in backgrounds) {
                if (playerPosition.X > backSprite.Position.X &&
                    playerPosition.X < backSprite.Position.X + backSprite.textureSize.X &&
                    playerPosition.Y > backSprite.Position.Y &&
                    playerPosition.Y < backSprite.Position.Y + backSprite.textureSize.Y) {
                    // If the player is within the sprite's bounds
                    sprite = backSprite;
                    break;
                }
            }
            return sprite;
        }

        public void draw(SpriteBatch spriteBatch) {
            //backgroundSprite.draw(spriteBatch);
            foreach (Sprite backSprite in backgrounds) {
                backSprite.draw(spriteBatch);
            }
        }
    }
}