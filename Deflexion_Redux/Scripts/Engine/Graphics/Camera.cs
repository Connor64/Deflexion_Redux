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
        public GraphicsDeviceManager deviceManager;

        public int virtualViewportX;
        public int virtualViewportY;

        public int virtualWidth;
        public int virtualHeight;
        public int _Width;
        public int _Height;

        private Matrix scaleMatrix;
        private bool dirtyMatrix = true;

        public float scalar;

        public void Initialize(ref GraphicsDeviceManager deviceManager) {
            _Width = deviceManager.PreferredBackBufferWidth;
            _Height = deviceManager.PreferredBackBufferHeight;
            this.deviceManager = deviceManager;
            dirtyMatrix = true;
            ApplyResolutionChanges();
        }

        public void move(Vector2 targetPosition, float deltaTime) {
            //position = Vector2.Lerp(position, targetPosition, 0.5f);
            position = targetPosition;
        }

        public Vector2 getMousePosition(bool scaled) {
            Vector2 mousePosition;
            mousePosition.X = Mouse.GetState().X;
            mousePosition.Y = Mouse.GetState().Y;

            mousePosition.Y -= virtualViewportY;

            Vector2 screenPosition = Vector2.Transform(mousePosition, Matrix.Invert(getTransformationMatrix()));
            if (scaled) {
                Vector2 worldPosition = Vector2.Transform(screenPosition, Matrix.Invert(getTranslationMatrix()));
                return worldPosition / zoom;
            } else {
                return screenPosition;
            }

        }

        public Matrix getTranslationMatrix() {
            Matrix translationMatrix = Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0)); // Set position values to negative or else increasing will go left/up instead of right/down
            Matrix virtualTranslationMatrix = Matrix.CreateTranslation(new Vector3(virtualWidth * 0.5f, virtualHeight * 0.5f, 0)); // Centers camera

            return translationMatrix * virtualTranslationMatrix;
        }

        public Matrix getScaleMatrix() {
            return Matrix.CreateScale(zoom);
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

            scalar = (float)virtualHeight / 540f;
            //spriteScalar = 1;
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
                    deviceManager.PreferredBackBufferWidth = _Width;
                    deviceManager.PreferredBackBufferHeight = _Height;
                    deviceManager.IsFullScreen = false;
                    deviceManager.ApplyChanges();
                }
            } else {
                foreach(DisplayMode dm in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes) {
                    if ((dm.Width == _Width) && (dm.Height == _Height)) {
                        deviceManager.PreferredBackBufferWidth = _Width;
                        deviceManager.PreferredBackBufferHeight = _Height;
                        deviceManager.IsFullScreen = true;
                        deviceManager.ApplyChanges();
                    }
                }
            }

            dirtyMatrix = true;
            _Width = deviceManager.PreferredBackBufferWidth;
            _Height = deviceManager.PreferredBackBufferHeight;
         }

        public Matrix getTransformationMatrix() {
            if (dirtyMatrix) {
                RecreateScaleMatrix();
            }
            return scaleMatrix;
        }

        public void RecreateScaleMatrix() {
            dirtyMatrix = false;
            scaleMatrix = Matrix.CreateScale((float)deviceManager.GraphicsDevice.Viewport.Width / virtualWidth, (float)deviceManager.GraphicsDevice.Viewport.Height / virtualHeight, 1f);
        }

        public void FullViewport() {
            Viewport vp = new Viewport();
            vp.X = 0;
            vp.Y = 0;
            vp.Width = _Width;
            vp.Height = _Height;
            deviceManager.GraphicsDevice.Viewport = vp;
        }

        public void ResetViewPort() {
            float targetAspectRatio = (float)virtualWidth / (float)virtualHeight;

            int width = deviceManager.PreferredBackBufferWidth;
            int height = (int)(width / targetAspectRatio + 0.5f); // I'm not sure why there's a 0.5f
            bool changed = false;

            if (height != deviceManager.PreferredBackBufferHeight) {
                height = deviceManager.PreferredBackBufferHeight; // Pillarbox
                width = (int)(height * targetAspectRatio + 0.5f);
                changed = true;
            }

            Viewport viewport = new Viewport();
            viewport.X = (deviceManager.PreferredBackBufferWidth / 2) - (width / 2);
            viewport.Y = (deviceManager.PreferredBackBufferHeight / 2) - (height / 2);
            virtualViewportX = viewport.X;
            virtualViewportY = viewport.Y;
            viewport.Width = width;
            viewport.Height = height;

            viewport.MinDepth = 0;
            viewport.MaxDepth = 1;

            if (changed) {
                dirtyMatrix = true;
            }

            deviceManager.GraphicsDevice.Viewport = viewport;
        }

        public void BeginDraw() {
            FullViewport();

            deviceManager.GraphicsDevice.Clear(Color.Black);

            ResetViewPort();

            deviceManager.GraphicsDevice.Clear(Color.CornflowerBlue);
        }
    }
}