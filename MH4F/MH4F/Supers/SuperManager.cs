using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MH4F
{
    public interface SuperManager
    {
        
        void drawSuperEffects();
        Boolean isInSuperFreeze();
        void performSuper(int player);
        int playerPerformingSuper();
        void decrementTimer();
    }
}
