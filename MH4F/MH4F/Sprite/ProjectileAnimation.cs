using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MH4F
{
    public class ProjectileAnimation : Move
    {
        public Texture2D DummyTexture { get; set; }
        public Boolean Finished { get; set; }
        public Direction Direction { get; set; }
        public int NumOfHits { get; set; }
        // Currently can only shoot projectiles linearly... Is that a problem?
        //
        public int XSpeed { get; set; }
        public int YSpeed { get; set; }

        int timerLength { get; set; }
        public Rectangle Hitbox { get; set; }

        Vector2 v2Position = new Vector2(0, 0);

        int hitSlowdown = 0;

        public int PlayerNumber {get; set;}

        public Vector2 Position
        {
            get { return v2Position; }
            set
            {                
                v2Position = value;
            }
        }
        public int X
        {
            get { return (int)v2Position.X; }
            set
            {
                v2Position.X = value;
            }
        }

        ///
        /// The Y position of the sprite's upper left corner pixel.
        ///
        public int Y
        {
            get { return (int)v2Position.Y; }
            set
            {
                v2Position.Y = value;
            }
        }
        Vector2 v2Center;
        public ProjectileAnimation(Texture2D texture, int X, int Y, int Width, int Height, int Frames, int columns, float frameLength, CharacterState characterState, int timeLength, Direction direction)
            : base(texture, X, Y, Width, Height, Frames, columns, frameLength, characterState)
        {
            timerLength = timeLength;
            Direction = direction;
            v2Center = new Vector2(Width / 2, Height / 2);
            Finished = false;            
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            timerLength--;
            if (Direction == Direction.Right)
            {
                if (hitSlowdown > 0)
                {
                    X += XSpeed / 5;
                }
                else
                {
                    X += XSpeed;  
                }
                  
            }
            else 
            {
                X -= XSpeed;
            }
            Y += YSpeed;
            Hitbox = new Rectangle();
            Hitbox = CurrentHitboxInfo.getHitboxRectangle(Hitbox, Direction.Right, v2Position, FrameWidth);
            if (hitSlowdown > 0)
            {
                hitSlowdown--;
            }
            if (timerLength <= 0)
            {
                Finished = true;
            }

        }

        public void hitEnemy()
        {
            hitSlowdown = 20;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Direction == Direction.Right)
            {
                spriteBatch.Draw(Texture, (v2Position + v2Center),
                                 FrameRectangle, Color.White,
                                 0, v2Center, 1f, SpriteEffects.FlipHorizontally, 0);
            }
            else
            {
                spriteBatch.Draw(Texture, (v2Position + v2Center),
                                               FrameRectangle, Color.White,
                                               0, v2Center, 1f, SpriteEffects.None, 0);
            }
            Color translucentRed = Color.Red * 0.2f;
            
            spriteBatch.Draw(DummyTexture, Hitbox , translucentRed);  
        }


        public ProjectileAnimation Clone()
        {
            ProjectileAnimation projectileAnimation = new ProjectileAnimation(Texture, this.RectInitialFrame.X, this.RectInitialFrame.Y,
                                      this.RectInitialFrame.Width, this.RectInitialFrame.Height,
                                      FrameCount, this.Columns, this.FrameLength, CharacterState.NONE, timerLength, Direction.Right);
            projectileAnimation.HitBoxInfo = HitBoxInfo;
            projectileAnimation.DummyTexture = DummyTexture;
            projectileAnimation.HitInfo = HitInfo;
            projectileAnimation.XSpeed = XSpeed;
            projectileAnimation.YSpeed = YSpeed;
            projectileAnimation.NumOfHits = NumOfHits;
            return projectileAnimation; 
        }
    }
}
