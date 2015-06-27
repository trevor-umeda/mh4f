using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MH4F
{
    class ParticleAnimation : SpriteAnimation
    {
        public ParticleAnimation(Texture2D texture, int X, int Y, int Width, int Height, int Frames, float frameLength) 
        {
            Texture = texture;
            RectInitialFrame = new Rectangle(X, Y, Width, Height);
            FrameCount = Frames;
            FrameLength = frameLength;
        }        
    }
}
