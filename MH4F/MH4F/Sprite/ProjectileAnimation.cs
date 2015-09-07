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
        public Texture2D dummyTexture;
        public Boolean Finished { get; set; }
        public Direction Direction { get; set; }
        int timerLength { get; set; }
        Vector2 v2Position = new Vector2(0, 0);
        Vector2 v2Center;
        public ProjectileAnimation(Texture2D texture, int X, int Y, int Width, int Height, int Frames, int columns, float frameLength, CharacterState characterState, int timeLength, Direction direction)
            : base(texture, X, Y, Width, Height, Frames, columns, frameLength, characterState)
        {
            timerLength = timeLength;
            Direction = direction;
            v2Center = new Vector2(Width / 2, Height / 2);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Direction == Direction.Right)
            {
                spriteBatch.Draw(Texture, (v2Position + v2Center),
                                                FrameRectangle, Color.White,
                                                0, v2Center, 1f, SpriteEffects.None, 0);

            }
            else
            {
                spriteBatch.Draw(Texture, (v2Position + v2Center),
                                                FrameRectangle, Color.White,
                                                0, v2Center, 1f, SpriteEffects.FlipHorizontally, 0);
            }
            Color translucentRed = Color.Red * 0.5f;
            Rectangle hitbox = new Rectangle();
            CurrentHitboxInfo.getHitboxRectangle(hitbox, Direction.Right, v2Position, 100);

            //spriteBatch.Draw(null, hitbox , translucentRed);  
        }

        public ProjectileAnimation Clone()
        {
            ProjectileAnimation projectileAnimation = new ProjectileAnimation(Texture, this.RectInitialFrame.X, this.RectInitialFrame.Y,
                                      this.RectInitialFrame.Width, this.RectInitialFrame.Height,
                                      FrameCount, this.Columns, this.FrameLength, CharacterState.NONE, timerLength, Direction.Right);
            projectileAnimation.HitBoxInfo = HitBoxInfo;
            return projectileAnimation; 
        }
    }
}
