using SteelSeries.GameSense;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace AttackSpeedMeter.Helpers
{
    internal class FormatHelper
    {
        public static string ColorToString(Color color) =>
            color.R.ToString("X").ToLower() + color.G.ToString("X").ToLower() + color.B.ToString("X").ToLower();
        public static float SafeFloor(float input)
        {
            if (Math.Ceiling(input) - input < 0.01f)
                return (float)Math.Ceiling(input);
            return (float)Math.Floor(input);
        }
        public static float SafeCeiling(float input)
        {
            if (input - Math.Floor(input) < 0.01f)
                return (float)Math.Floor(input);
            return (float)Math.Ceiling(input);
        }
        public static string PercentageFloorSigned(float percent) => 
            (SafeFloor(percent - 10000f)/100f).ToString("+#0.00;-#0.00;0") + "%";
        public static string PercentageFloor(float percent) =>
            (SafeFloor(percent * 10000f) / 100f).ToString() + "%";
        public static string PercentageCeil(float percent) =>
            (SafeCeiling(percent - 10000f)/100f).ToString("+#0.00;-#0.00;0") + "%";
    }
}
