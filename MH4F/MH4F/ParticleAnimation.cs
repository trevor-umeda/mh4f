using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MH4F
{
    class ParticleAnimation : Move
    {
        public ParticleAnimation(Texture2D texture, int X, int Y, int Width, int Height, int Frames, float FrameLength, CharacterState CharacterState)
            : base(texture, X, Y, Width, Height, Frames, FrameLength, CharacterState)
        {
        }

        public ParticleAnimation(Texture2D texture, int X, int Y, int Width, int Height, int Frames, float FrameLength, CharacterState CharacterState, String strNextAnimation)
            : base(texture, X, Y, Width, Height, Frames, FrameLength, CharacterState, strNextAnimation)
        {
        } 
    }
}
