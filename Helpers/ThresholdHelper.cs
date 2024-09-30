using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttackSpeedMeter.Helpers
{
    internal class ThresholdHelper
    {
        public static int Threshold(int usetime, int tgtUsetime_plus_1) =>
            (int)((float)usetime / (float)tgtUsetime_plus_1 * 100) + 1;
    }
}
