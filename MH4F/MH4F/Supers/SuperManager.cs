using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MH4F
{
    public interface SuperManager
    {
        
        void drawSuperEffects();
        Boolean isInSuperFreeze();
        Boolean isInSuper();
        void performSuper(int player, Vector2 position);
        void endSuper();
        int playerPerformingSuper();
        void processSuperFreeze(); 
    }
}
