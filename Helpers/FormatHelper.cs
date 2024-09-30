using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttackSpeedMeter.Helpers
{
    internal class FormatHelper
    {
        public static float SafeFloor(float input)
        {
            if (Math.Ceiling(input) - input < 0.00001f)
                return (float)Math.Ceiling(input);
            return (float)Math.Floor(input);
        }
        public static float SafeCeiling(float input)
        {
            if (input - Math.Floor(input) < 0.00001f)
                return (float)Math.Floor(input);
            return (float)Math.Ceiling(input);
        }
        public static string PercentageFloor(float percent) => 
            ((int)SafeFloor(percent * 100f)).ToString() + "%";
        public static string PercentageCeil(float percent) =>
            ((int)SafeCeiling(percent * 100f)).ToString() + "%";
    }
}
