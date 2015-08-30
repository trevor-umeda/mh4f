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
        Boolean isInSuper();
        void performSuper(int player);
        void endSuper();
        int playerPerformingSuper();
        void processSuper(); 
    }
}
