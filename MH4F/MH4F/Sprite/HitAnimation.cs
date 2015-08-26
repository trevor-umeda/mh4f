using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace MH4F
{
    class HitAnimation : Move
    {

        private int hitStunCounter = -1;

        private int totalTimePlaying = 0;

        public int HitStunCounter
        {
            get { return hitStunCounter; }
            set { this.hitStunCounter = value; }
        }

        public HitAnimation(Texture2D texture, int X, int Y, int Width, int Height, int Frames, int Columns, float FrameLength, CharacterState CharacterState)
            : base(texture, X, Y, Width, Height, Frames, Columns, FrameLength, CharacterState)
        {
        }

     

        public void reset()
        {
            totalTimePlaying = 0;
        }

        public override void Update(GameTime gameTime)
        {
            FrameTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (FrameTimer > FrameLength)
            {
                FrameTimer = 0.0f;
                CurrentFrame = (CurrentFrame + 1) % FrameCount;
                totalTimePlaying++;
              
                if (hitStunCounter > 0 && totalTimePlaying >= hitStunCounter)
                {
                    CurrentFrame = 0;
                    IsDone = true;
                    PlayCount++;
                    totalTimePlaying = 0;
                }

            }
            
        }
    }
}
