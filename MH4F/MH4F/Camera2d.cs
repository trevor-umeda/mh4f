using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MH4F
{
    public class Camera2d
    {
        protected float zoom; // Camera Zoom
        public Matrix transform; // Matrix Transform
        public Vector2 position; // Camera Position
        protected float rotation; // Camera Rotation
        protected int leftSideLimit;
        protected int rightSideLimit;
        protected int width;

        public Camera2d(int gameWidth, int screenWidth)
        {
            zoom = 1.0f;
            rotation = 0.0f;
            position = Vector2.Zero;
            leftSideLimit = screenWidth / 2;
            rightSideLimit = gameWidth - (screenWidth / 2);
            width = screenWidth;
        }

        // Sets and gets zoom
        public float Zoom
        {
            get { return zoom; }
            set { zoom = value; if (zoom < 0.1f) zoom = 0.1f; } // Negative zoom will flip image
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        // Auxiliary function to move the camera
        public void Move(Vector2 amount)
        {
            position += amount;
        }

        public void ZoomIn(float amount)
        {
            zoom += amount;

        }
        // Get set position
        public Vector2 Pos
        {
            get { return position; }
            set { position = value; }
        }

        public int X
        {
            get { return (int)position.X; }
            set 
            { 
                position.X = value;
                if (value < leftSideLimit)
                {
                    position.X = leftSideLimit;
                }
                else if (value > rightSideLimit)
                {
                    position.X = rightSideLimit;
                }
            }
        }

        public int Y
        {
            get { return (int)position.Y; }
            set { position.Y = value; }
        }

        public int LeftEdge
        {
            get { return (int)position.X - (width / 2); }
        }

        public int RightEdge
        {
            get { return (int)position.X + (width / 2); }
        }

        public Matrix getTransformation(GraphicsDevice graphicsDevice)
        {
            transform =       // Thanks to o KB o for this solution
              Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0)) *
                                         Matrix.CreateRotationZ(Rotation) *
                                         Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                                         Matrix.CreateTranslation(new Vector3(1024 * 0.5f, 720 * 0.5f, 0));
            return transform;
        }
    }
}
