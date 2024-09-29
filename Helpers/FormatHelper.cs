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
        public static string PercentageFloor(float percent) => 
            ((int)Math.Floor(percent * 100f)).ToString() + "%";
        public static string PercentageCeil(float percent) =>
            ((int)Math.Ceiling(percent * 100f)).ToString() + "%";
    }
}
