using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Deflexion_Redux {
    public class EffectHandler {

        private static EffectHandler instance;
        public static EffectHandler Instance {
            get {
                if (instance == null) {
                    instance = new EffectHandler();
                }
                return instance;
            }
        }

        private Camera cam = Camera.Instance;

        public void BeginEffect(SpriteBatch spriteBatch, ref GraphicsDevice device, ref List<RenderTarget2D> targets, EffectType effect) {
            RenderTarget2D target = new RenderTarget2D(device, cam._Width, cam._Height);
            spriteBatch.GraphicsDevice.SetRenderTarget(target);
            spriteBatch.GraphicsDevice.Clear(new Color(0, 0, 0, 0));
            if (effect == EffectType.none) {
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, null, null, cam.getMatrix() * cam.getTransformationMatrix());
            } else {
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, null, AssetManager.effects[effect], cam.getMatrix() * cam.getTransformationMatrix());
            }

            targets.Add(target);
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
    }
}
