﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MH4F
{
    class HuntingHornPlayer : Player
    {

        public HuntingHornPlayer(int playerNumber, int xPosition, int yHeight, ComboManager comboManager, ThrowManager throwManager, Gauge healthBar) 
            : base ( playerNumber, xPosition, yHeight, comboManager, throwManager, healthBar)
        {
            CurrentHealth = 1000;
            MaxHealth = 1000;
           
            Sprite.BoundingBoxHeight = 288;
            Sprite.BoundingBoxWidth = 90;

            // TODO make these have to be set for every character.
            //
            ThrowRange = 200;
            BackAirDashVel = 8;
            AirDashVel = 13;
            BackStepVel = 8;
            DashVel = 8;
            BackWalkVel = 3;
            WalkVel = 4;
//            projectile = new ProjectileAnimation(texture, X, Y, Width, Height, Frames, columns, frameLength, characterState, timeLength, direction);

        }

        public override void checkValidityAndChangeMove(string moveName)
        {
            // If we have a designated cost for our move. Make sure we can perform it. 
          
        }
    }
}
