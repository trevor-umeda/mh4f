using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MH4F
{
    public class Hitbox
    {
        int xPos;
        int yPos;
        int width;
        int height;

        public int XPos
        {
            get { return xPos; }
        }

        public int YPos
        {
            get { return yPos; }
        }

        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get { return height; }
        }
        public Hitbox(String X, String Y, String Width, String Height)
        {
            xPos = Convert.ToInt32(X);
            yPos = Convert.ToInt32(Y);
            width = Convert.ToInt32(Width);
            height = Convert.ToInt32(Height);
        }
    }
}
