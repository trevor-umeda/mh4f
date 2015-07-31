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
        bool DisplayShadow { get;  set;}
        int SwordLevel { get; set; }

        public LongSwordPlayer(Texture2D texture, int playerNumber, int xPosition, int yHeight, ComboManager comboManager, ThrowManager throwManager) 
            : base ( texture, playerNumber, xPosition, yHeight, comboManager, throwManager)
        {
            CurrentHealth = 1000;
            MaxHealth = 1000;
            DisplayShadow = false;
            SwordLevel = 1;
            // TODO make these have to be set for every character.
            //
            ThrowRange = 200;
            BackAirDashVel = 8;
            AirDashVel = 8;
            BackStepVel = 8;
            DashVel = 8;
            BackWalkVel = 3;
            WalkVel = 4;
        }

        public override void cleanUp()
        {
            base.cleanUp();
            if (!Sprite.CurrentMoveAnimation.IsAttack)
            {
                DisplayShadow = false;
            }
        }

        public override void checkMoveChangeValidity(string moveName)
        {
            base.checkMoveChangeValidity(moveName);
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
            DisplayShadow = true;
            Dash();          
        }

        public void BackFireball()
        {
            DisplayShadow = true;
            Backstep();
        }

        public override void Backstep()
        {
            base.Backstep();
        }

        public override void Dash()
        {
            base.Dash();

            GivePlayerMomentum(7, 3, true);      
        }
        public override void BackWalk()
        {
            base.BackWalk();
            
        }

        public override void ForwardWalk()
        {
            base.ForwardWalk();
            
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch, 0, 0, Direction);
            if (DisplayShadow)
            {
                Sprite.shadowDraw(spriteBatch, -9, 0, Direction);
            }
           
        }
    }
}
