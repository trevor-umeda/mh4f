using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MH4F
{
    class ComboManager
    {
        private int player1ComboNumber = 0;
        private int player2ComboNumber = 0;

        private int TIME_TO_DISPLAY_COMBO = 50;
        private int comboTimer = 0;

        public ComboManager()
        {

        }

        public int Player1ComboNumber
        {
            get { return player1ComboNumber; }
        }

        public int Player2ComboNumber
        {
            get { return player2ComboNumber; }
        }

        public void player1LandedHit(CharacterState hitPlayersState)
        {
            playerLandedHit(hitPlayersState, 1);
        }
        public void player2LandedHit(CharacterState hitPlayersState)
        {
            playerLandedHit(hitPlayersState, 2);
        }
        public Boolean displayCombo()
        {
            return (comboTimer > 0);
        }

        public void decrementComboTimer()
        {
            if (comboTimer > 0)
            {
                comboTimer--;
            }
        }

        public void playerLandedHit(CharacterState hitPlayersState, int playerNumber)
        {
            // No object reference for integers and I don't wanna anything goofy so have repetition;
            //
            if (playerNumber == 1)
            {
                if (hitPlayersState != CharacterState.HIT)
                {
                    player1ComboNumber = 0;
                }
                if (player1ComboNumber == 0 || hitPlayersState == CharacterState.HIT)
                {
                    player1ComboNumber++;
                }
            }
            else
            {
                if (hitPlayersState != CharacterState.HIT)
                {
                    player2ComboNumber = 0;
                }
                if (player2ComboNumber == 0 || hitPlayersState == CharacterState.HIT)
                {
                    player2ComboNumber++;
                }
            }
            comboTimer = TIME_TO_DISPLAY_COMBO;
        }       
    }  
}
