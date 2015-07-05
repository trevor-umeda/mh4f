using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MH4F
{
    public class LongSwordPlayer : Player
    {
        bool displayShadow {get; set;}

        public LongSwordPlayer(Texture2D texture, int playerNumber, int xPosition, int yHeight, ComboManager comboManager, ThrowManager throwManager) 
            : base ( texture, playerNumber, xPosition, yHeight, comboManager, throwManager)
        {
            CurrentHealth = 1000;
            MaxHealth = 1000;
            displayShadow = false;
        }

        public override void cleanUp()
        {
            base.cleanUp();
            if (!Sprite.CurrentMoveAnimation.IsAttack)
            {
                displayShadow = false;
            }
        }

        public override void performGroundSpecialMove(KeyboardState ks, String moveName)
        {
            if (moveName == "fireball")
            {
                Fireball();
            }
            else if (moveName == "backfireball")
            {
                BackFireball();
            }
            else
            {
                base.performGroundSpecialMove(ks, moveName);
            }
        }

        public void Fireball()
        {
           
            displayShadow = true;
            Dash();
            
        }

        public void BackFireball()
        {
            displayShadow = true;
            Backstep();
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

        public override void AirBackdash()
        {
            base.AirBackdash();
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
        public override void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch, 0, 0, Direction);
            if (displayShadow)
            {
                Sprite.shadowDraw(spriteBatch, -9, 0, Direction);
            }
           
        }
    }
}
