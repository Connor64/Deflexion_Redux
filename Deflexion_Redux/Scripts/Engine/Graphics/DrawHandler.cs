using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Deflexion_Redux {
    public class DrawHandler {

        private static DrawHandler instance;
        public static DrawHandler Instance {
            get {
                if (instance == null) {
                    instance = new DrawHandler();
                }
                return instance;
            }
        }

        private Camera cam = Camera.Instance;

        public void BeginEffect(SpriteBatch spriteBatch, ref GraphicsDevice device, ref List<RenderTarget2D> targets, Effect effect) {
            RenderTarget2D target = new RenderTarget2D(device, cam._Width, cam._Height);
            spriteBatch.GraphicsDevice.SetRenderTarget(target);
            spriteBatch.GraphicsDevice.Clear(new Color(0, 0, 0, 0));
            if (effect == null) {
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, null, null, cam.getScaleMatrix() * cam.getTranslationMatrix() * cam.getTransformationMatrix());
            } else {
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, null, effect, cam.getScaleMatrix() * cam.getTranslationMatrix() * cam.getTransformationMatrix());
            }

            targets.Add(target);
        }

        public void DrawUI(SpriteBatch spriteBatch) {
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, cam.getTransformationMatrix());
        }

        public void DrawTargets(SpriteBatch spriteBatch, ref List<RenderTarget2D> targets) {
            spriteBatch.GraphicsDevice.SetRenderTarget(null);
            spriteBatch.GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, null, null, null);
            foreach (RenderTarget2D target in targets) {
                spriteBatch.Draw(target, Vector2.Zero, new Rectangle(0, 0, target.Width, target.Height), Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 0);
            }
            spriteBatch.End();
            foreach(RenderTarget2D target in targets) {
                target.Dispose();
            }
            targets.Clear();
        }

        public void DrawTexture(SpriteBatch spriteBatch, TextureType texture, Vector2 position, Vector2 scale, Vector2 origin) {
            spriteBatch.Draw(AssetManager.textures[texture], position, new Rectangle(0, 0, AssetManager.textures[texture].Width, AssetManager.textures[texture].Height), Color.White, 0, origin, scale * cam.scalar, SpriteEffects.None, 0);
        }
    }
}
