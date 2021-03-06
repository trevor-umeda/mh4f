﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MH4F
{
    public interface ProrationStrategy
    {
        void startCombo();
        void registerHit(HitInfo hitInfo);
        int calculateProratedDamage(HitInfo hitInfo);
        int calculateProratedHitStun(HitInfo hitInfo);
    }
}
