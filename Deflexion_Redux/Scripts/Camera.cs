// Uses David Amador's method of XNA 2D independent resolution rendering -> http://www.david-amador.com/2010/03/xna-2d-independent-resolution-rendering/
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Deflexion_Redux {
    class Camera {

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

        private int virtualWidth;
        private int virtualHeight;
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

        public void move(Vector2 deltaTranslate) {
            position += deltaTranslate; // should add interpolation (?)
        }

        public Matrix getMatrix() {
            var translationMatrix = Matrix.CreateTranslation(new Vector3(position.X, position.Y, 0));
            var scaleMatrix = Matrix.CreateScale(zoom);

            return translationMatrix * scaleMatrix;
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
            if (isFullscreen == false) {
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
            scaleMatrix = Matrix.CreateScale(device.GraphicsDevice.Viewport.Width / virtualWidth, device.GraphicsDevice.Viewport.Height / virtualHeight, 1f);
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