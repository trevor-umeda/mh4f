using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MH4F
{
    public class RoundManager
    {
        int MaxRoundWins { get; set; }
        int Player1RoundWins { get; set; }
        int Player2RoundWins { get; set; }

        int Timer { get; set; }
        int TimeLimit { get; set; }

        Player player1;
        Player player2;
        float currentTime;
        public RoundManager(Player player1, Player player2)
        {
            TimeLimit = 99;
            MaxRoundWins = 2;
            Timer = TimeLimit;
            this.player1 = player1;
            this.player2 = player2;
            currentTime = 0f;
        }

        public void roundEnd(int playerNumber)
        {
            if (playerNumber == 1)
            {
                Player1RoundWins++;
            }
            else
            {
                Player2RoundWins++;
            }
            player1.resetRound();
            player2.resetRound();
            Timer = TimeLimit;
        }

        public void decrementTimer(GameTime gameTime)
        {

            float countDuration = 2f; //every  2s.
            

            currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds; //Time passed since last Update() 

            if (currentTime >= countDuration)
            {
                Timer--;
                currentTime -= countDuration; // "use up" the time
                //any actions to perform
            }    
        }

        public String displayTime()
        {
            return string.Format("{0}", Timer); 
        }

        public Boolean isTimeOut()
        {
            if (Timer <= 0)
            {
                return true;
            }
            else return false;
        }

        public void timeOut()
        {
            if (player1.CurrentHealth < player2.CurrentHealth)
            {
                roundEnd(1);
            }
            else
            {
                roundEnd(2);
            }
        }

    }
}
