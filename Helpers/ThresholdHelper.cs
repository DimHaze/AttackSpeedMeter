using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttackSpeedMeter.Helpers
{
    internal class ThresholdHelper
    {
        public static float Threshold(int usetime, int destUsetime, float speedMult, float usetimeMult) =>
            ((usetimeMult / speedMult) * (float)usetime) / (float)destUsetime;
    }
}
