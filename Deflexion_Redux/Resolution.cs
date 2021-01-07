using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Deflexion_Redux {
    class Resolution {

        public Matrix _scaleMatrix;
        public int ScreenWidth;
        public int ScreenHeight;
        public int VirtualWidth;
        public int VirtualHeight;

        public void setupMatrix() {
            Matrix.CreateScale((float)ScreenWidth / VirtualWidth, (float)ScreenWidth / VirtualWidth, 1f, out _scaleMatrix);
        }

        public Matrix getScaleMatrix() {
            return _scaleMatrix;
        }
        
    }
}
