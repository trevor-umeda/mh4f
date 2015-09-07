using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MH4F
{
    class Config
    {
        private static Config instance;

        private int screenWidth;
        private int screenHeight;

        private int gameWidth;
        private int gameHeight;

        public int ScreenWidth
        {
            get { return screenWidth; }
        }

        public int ScreenHeight
        {
            get { return screenHeight; }
        }

        public int GameWidth
        {
            get { return gameWidth; }
        }

        public int GameHeight
        {
            get { return gameHeight; }
        }
        private Config() 
        {
            gameWidth = 1512;
            gameHeight = 720;

            screenWidth = 1024;
            screenHeight = 720;
        }

        public static Config Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Config();
                }
                return instance;
            }
        }
    }
}
