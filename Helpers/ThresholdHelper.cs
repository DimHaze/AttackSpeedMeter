using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttackSpeedMeter.Helpers
{
    internal class ThresholdHelper
    {
        public static int Threshold(float usetime, float tgtUsetime_plus_1, float vanillaMult) =>
            (int)(
                (
                    ((usetime / tgtUsetime_plus_1 -1)/vanillaMult) + 1
                )*100
            )+1
            ;
    }
}
