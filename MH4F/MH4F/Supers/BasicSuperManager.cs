using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MH4F
{
    class BasicSuperManager : SuperManager
    {
        int superFreezeDelayTime { get; set; }
        int superFreezeTimer { get; set; }
        int playerNumber;

        public BasicSuperManager()
        {
            superFreezeDelayTime = 300;
            superFreezeTimer = 0;
        }

        public void drawSuperEffects()
        {
            throw new NotImplementedException();
        }

        public void decrementTimer()
        {
            superFreezeTimer--;
        }

        public bool isInSuperFreeze()
        {
            return superFreezeTimer > 0;
        }

        public int playerPerformingSuper()
        {
            return playerNumber;
        }

        public void performSuper(int player)
        {
            playerNumber = player;
            superFreezeTimer = superFreezeDelayTime;
            Console.WriteLine("PERFORMING SUPER FOR PLAYER: " + player);
        }
       
    }
}
