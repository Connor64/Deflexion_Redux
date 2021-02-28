// Uses David Amador's method of XNA 2D independent resolution rendering -> http://www.david-amador.com/2010/03/xna-2d-independent-resolution-rendering/
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Deflexion_Redux {
    public class Camera {

        private static Camera instance = null;
        public static Camera Instance {
            get {
                if (instance == null) {
                    instance = new Camera();
                }
                return instance;
            }
        }

        public bool isFullscreen;
        public Vector2 position = Vector2.Zero;           
        public float zoom = 1;
        public GraphicsDeviceManager device;

        public int virtualViewportX;
        public int virtualViewportY;

        public int virtualWidth;
        public int virtualHeight;
        public int _Width;
        public int _Height;

        private Matrix scaleMatrix;
        private bool dirtyMatrix = true;

        public void Initialize(ref GraphicsDeviceManager device) {
            _Width = device.PreferredBackBufferWidth;
            _Height = device.PreferredBackBufferHeight;
            this.device = device;
            dirtyMatrix = true;
            ApplyResolutionChanges();
        }

        public void move(Vector2 targetPosition, float deltaTime) {
            //position = Vector2.Lerp(position, targetPosition, 0.5f);
            position = targetPosition;
        }

        public Vector2 getMousePosition() {
            Vector2 mousePosition;
            mousePosition.X = Mouse.GetState().X;
            mousePosition.Y = Mouse.GetState().Y;

            mousePosition.Y -= virtualViewportY;

            Vector2 screenPosition = Vector2.Transform(mousePosition, Matrix.Invert(getTransformationMatrix()));
            Vector2 worldPosition = Vector2.Transform(screenPosition, Matrix.Invert(getMatrix()));

            return worldPosition;
        }

        public Matrix getMatrix() {
            Matrix translationMatrix = Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0)); // Set position values to negative or else increasing will go left/up instead of right/down
            Matrix scaleMatrix = Matrix.CreateScale(zoom);
            Matrix virtualTranslationMatrix = Matrix.CreateTranslation(new Vector3(virtualWidth * 0.5f, virtualHeight * 0.5f, 0)); // Centers camera

            return translationMatrix * scaleMatrix * virtualTranslationMatrix;
        }

        public bool contains(Vector2 objPosition, Vector2 tolerance) {
            return !(objPosition.Y < position.Y - (virtualHeight / 2) - tolerance.Y || // Above camera
                     objPosition.Y > position.Y + (virtualHeight / 2) + tolerance.Y || // Below camera
                     objPosition.X < position.X - (virtualWidth / 2) - tolerance.X ||  // Left of camera
                     objPosition.X > position.X + (virtualWidth / 2) + tolerance.X);   // Right of camera
        }

        public void SetVirtualResolution(int Width, int Height) {
            virtualWidth = Width;
            virtualHeight = Height;
            dirtyMatrix = true;
        }

        public void SetResolution(int Width, int Height, bool fullScreen) {
            _Width = Width;
            _Height = Height;

            isFullscreen = fullScreen;

            ApplyResolutionChanges();
        }


        private void ApplyResolutionChanges() {
            if (!isFullscreen) {
                if ((_Width <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width) && (_Height <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height)) {
                    device.PreferredBackBufferWidth = _Width;
                    device.PreferredBackBufferHeight = _Height;
                    device.IsFullScreen = false;
                    device.ApplyChanges();
                }
            } else {
                foreach(DisplayMode dm in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes) {
                    if ((dm.Width == _Width) && (dm.Height == _Height)) {
                        device.PreferredBackBufferWidth = _Width;
                        device.PreferredBackBufferHeight = _Height;
                        device.IsFullScreen = true;
                        device.ApplyChanges();
                    }
                }
            }

            dirtyMatrix = true;
            _Width = device.PreferredBackBufferWidth;
            _Height = device.PreferredBackBufferHeight;
         }

        public Matrix getTransformationMatrix() {
            if (dirtyMatrix) {
                RecreateScaleMatrix();
            }
            return scaleMatrix;
        }

        public void RecreateScaleMatrix() {
            dirtyMatrix = false;
            scaleMatrix = Matrix.CreateScale((float)device.GraphicsDevice.Viewport.Width / virtualWidth, (float)device.GraphicsDevice.Viewport.Height / virtualHeight, 1f);
        }

        public void FullViewport() {
            Viewport vp = new Viewport();
            vp.X = 0;
            vp.Y = 0;
            vp.Width = _Width;
            vp.Height = _Height;
            device.GraphicsDevice.Viewport = vp;
        }

        public void ResetViewPort() {
            float targetAspectRatio = (float)virtualWidth / (float)virtualHeight;

            int width = device.PreferredBackBufferWidth;
            int height = (int)(width / targetAspectRatio + 0.5f); // I'm not sure why there's a 0.5f
            bool changed = false;

            if (height != device.PreferredBackBufferHeight) {
                height = device.PreferredBackBufferHeight; // Pillarbox
                width = (int)(height * targetAspectRatio + 0.5f);
                changed = true;
            }

            Viewport viewport = new Viewport();
            viewport.X = (device.PreferredBackBufferWidth / 2) - (width / 2);
            viewport.Y = (device.PreferredBackBufferHeight / 2) - (height / 2);
            virtualViewportX = viewport.X;
            virtualViewportY = viewport.Y;
            viewport.Width = width;
            viewport.Height = height;

            viewport.MinDepth = 0;
            viewport.MaxDepth = 1;

            if (changed) {
                dirtyMatrix = true;
            }

            device.GraphicsDevice.Viewport = viewport;
        }

        public void BeginDraw() {
            FullViewport();

            device.GraphicsDevice.Clear(Color.Black);

            ResetViewPort();

            device.GraphicsDevice.Clear(Color.CornflowerBlue);
        }
    }
}