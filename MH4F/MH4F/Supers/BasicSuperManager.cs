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
        bool isInSuperState;
        Camera2d camera;
        public BasicSuperManager(Camera2d camera)
        {
            superFreezeDelayTime = 25;
            superFreezeTimer = 0;
            this.camera = camera;
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

        public bool isInSuper()
        {
            return isInSuperState;
        }

        public int playerPerformingSuper()
        {
            return playerNumber;
        }

        public void endSuper()
        {
            isInSuperState = false;
            camera.Zoom = 1;
        }
        public void processSuper()
        {
            if (camera.Zoom < 1.2)
            {
                camera.ZoomIn(0.1f);
            }
            
            decrementTimer();

        }
        public void performSuper(int player)
        {
            playerNumber = player;
            isInSuperState = true;
            superFreezeTimer = superFreezeDelayTime;
            Console.WriteLine("PERFORMING SUPER FOR PLAYER: " + player);
        }
       
    }
}
