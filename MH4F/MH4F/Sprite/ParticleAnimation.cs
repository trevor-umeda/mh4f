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
        public Boolean Finished { get; set; }
        Vector2 v2Position = new Vector2(0, 0);
        Vector2 v2Center;
        float rotation;
        Random random = new Random();
        public float Rotation
        {
            get { return rotation; }
            set
            {
               rotation = value;   
            }
        }
        public Vector2 Position
        {
            get { return v2Position; }
            set
            {
                v2Position = value;
                v2Position.X += this.FrameWidth / 2;
                v2Position.Y += this.FrameHeight / 2;
                v2Center = new Vector2(this.FrameWidth / 2, this.FrameHeight / 2);
            }
        }

        public ParticleAnimation(Texture2D texture, int X, int Y, int Width, int Height, int Frames, int columns, float frameLength) 
        {
            Texture = texture;
            RectInitialFrame = new Rectangle(X, Y, Width, Height);
            FrameCount = Frames;
            FrameLength = frameLength;
            Columns = columns;
        }
        public ParticleAnimation NewInstance(int XPos, int YPos)
        {
           ParticleAnimation clonedParticle = new ParticleAnimation(
               Texture,
                RectInitialFrame.X,
                RectInitialFrame.Y,
                RectInitialFrame.Width,
                RectInitialFrame.Height,
                FrameCount,
                Columns,
                FrameLength
               );
            clonedParticle.Position = new Vector2(XPos, YPos);
            
            double mantissa = (random.NextDouble() * 2.0) - 1.0;
            double exponent = Math.Pow(2.0, random.Next(0, 6));
            float RotationAngle = (float)(mantissa * exponent);
            float circle = MathHelper.Pi * 2;
            clonedParticle.Rotation = RotationAngle % circle;
            return clonedParticle;
        }
        public void Update(GameTime gameTime)
        {
            base.Update(gameTime);
          
            if (PlayCount > 0)
            {
                // If there is, see if the currently playing animation has
                // completed a full animation loop
                //
              Finished = true;
            }
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            
            spriteBatch.Draw(Texture, (v2Position),
                                  FrameRectangle, Color.White,
                                  Rotation, v2Center, 1f, SpriteEffects.FlipHorizontally, 0);
        }
    }
}
