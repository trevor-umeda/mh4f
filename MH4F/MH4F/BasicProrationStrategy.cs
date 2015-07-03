using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MH4F
{
    class BasicProrationStrategy : ProrationStrategy
    {

        private double DamageProrationValue { get; set; }
        private double HitStunProrationValue { get; set; }

        private int comboLength { get; set; }

        public void startCombo()
        {
            DamageProrationValue = 1;
            HitStunProrationValue = 1;

        }

        public int calculateProratedDamage(HitInfo hitInfo)
        {            
            return (int)(hitInfo.Damage * DamageProrationValue);
        }

        public int calculateProratedHitStun(HitInfo hitInfo)
        {
            return (int)(hitInfo.Hitstun * HitStunProrationValue);

        }
    }
}
