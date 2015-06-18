using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MH4F
{
    class Camera2d
    {
        protected float          _zoom; // Camera Zoom
        public Matrix             _transform; // Matrix Transform
        public Vector2          _pos; // Camera Position
        protected float         _rotation; // Camera Rotation
        protected int leftSideLimit;
        protected int rightSideLimit;
        protected int width;

        public Camera2d(int gameWidth, int screenWidth)
        {
            _zoom = 1.0f;
            _rotation = 0.0f;
            _pos = Vector2.Zero;
            leftSideLimit = screenWidth / 2;
            rightSideLimit = gameWidth - (screenWidth / 2);
            width = screenWidth;
        }

        // Sets and gets zoom
        public float Zoom
        {
            get { return _zoom; }
            set { _zoom = value; if (_zoom < 0.1f) _zoom = 0.1f; } // Negative zoom will flip image
        }

        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        // Auxiliary function to move the camera
        public void Move(Vector2 amount)
        {
            _pos += amount;
        }
        // Get set position
        public Vector2 Pos
        {
            get { return _pos; }
            set { _pos = value; }
        }

        public int X
        {
            get { return (int)_pos.X; }
            set 
            { 
                _pos.X = value;
                if (value < leftSideLimit)
                {
                    _pos.X = leftSideLimit;
                }
                else if (value > rightSideLimit)
                {
                    _pos.X = rightSideLimit;
                }
            }
        }

        public int Y
        {
            get { return (int)_pos.Y; }
            set { _pos.Y = value; }
        }

        public int LeftEdge
        {
            get { return (int)_pos.X - (width / 2); }
        }

        public int RightEdge
        {
            get { return (int)_pos.X + (width / 2); }
        }

        public Matrix getTransformation(GraphicsDevice graphicsDevice)
        {
            _transform =       // Thanks to o KB o for this solution
              Matrix.CreateTranslation(new Vector3(-_pos.X, -_pos.Y, 0)) *
                                         Matrix.CreateRotationZ(Rotation) *
                                         Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                                         Matrix.CreateTranslation(new Vector3(1024 * 0.5f, 720 * 0.5f, 0));
            return _transform;
        }
    }
}
