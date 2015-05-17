using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MH4F
{
    class LongSwordPlayer : Player
    {
        
        public LongSwordPlayer(Texture2D texture, int xPosition) : base ( texture, xPosition)
        {
            
        }
        public override void Backstep()
        {
            int backStepVel = 8;
            if (Direction == Direction.Left)
            {
                Sprite.MoveBy(backStepVel, 0);
            }
            else
            {
                Sprite.MoveBy(-backStepVel, 0);
            }
        }

        public override void Dash()
        {
            int dashVel = 4;
            if (Direction == Direction.Left)
            {
                Sprite.MoveBy(-dashVel, 0);
            }
            else
            {
                Sprite.MoveBy(dashVel, 0);
            }

            GivePlayerMomentum(7, 3, true);      
        }
        public override void BackWalk()
        {
            base.BackWalk();
            int dashVel = 3;
            if (Direction == Direction.Left)
            {
                Sprite.MoveBy(dashVel, 0);
            }
            else
            {
                Sprite.MoveBy(-dashVel, 0);
            }
        }

        public override void ForwardWalk()
        {
            base.ForwardWalk();
            int dashVel = 3;
            if (Direction == Direction.Left)
            {
                Sprite.MoveBy(-dashVel, 0);
            }
            else
            {
                Sprite.MoveBy(dashVel, 0);
            }
        }
    }
}
