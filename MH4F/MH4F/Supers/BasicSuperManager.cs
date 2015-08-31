using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MH4F
{
    class BasicSuperManager : SuperManager
    {
        int superFreezeDelayTime { get; set; }
        int superFreezeTimer { get; set; }

        int timeAllowedToZoom;

        Vector2 cameraPositionMovement;

        Vector2 startingCamPosition;
        Vector2 playerZoomInPosition;

        int playerNumber;
        bool isInSuperState;
        Camera2d camera;
        public BasicSuperManager(Camera2d camera)
        {
            superFreezeDelayTime = 25;
            superFreezeTimer = 0;
            timeAllowedToZoom = 3;
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
        public void processSuperFreeze()
        {
            if( superFreezeDelayTime - superFreezeTimer <= timeAllowedToZoom)
            {
                   
                camera.Move(cameraPositionMovement);
                camera.ZoomIn(0.1f);
            }
            if (superFreezeTimer < timeAllowedToZoom)
            {
                camera.Move(-cameraPositionMovement);
                camera.Y = 360;
                camera.ZoomIn(-0.1f);   
            }

            decrementTimer();

        }
        public void performSuper(int player, Vector2 position)
        {
            playerNumber = player;
            isInSuperState = true;
            playerZoomInPosition = position;
            startingCamPosition = camera.Pos;

            cameraPositionMovement = (playerZoomInPosition - startingCamPosition) / timeAllowedToZoom;
            //camera.X = (int)playerZoomInPosition;
            superFreezeTimer = superFreezeDelayTime;
            Console.WriteLine("PERFORMING SUPER FOR PLAYER: " + player);
        }

    }
}
