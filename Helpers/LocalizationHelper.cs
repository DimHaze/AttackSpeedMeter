using AttackSpeedMeter.ModSystems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AttackSpeedMeter.Helpers
{
    public class LocalizationHelper
    {
        public static String GetGreetings() =>
            Language.GetTextValue("Mods.AttackSpeedMeter.UITips.Greetings");
        public static String GetHeader(DamageClass damageClass) =>
            Language.GetTextValue("Mods.AttackSpeedMeter.UITips."
            + ModContent.GetInstance<DamageClasses>()[damageClass] + "SpeedHeader");
        public static String GetStatus(int usetime, float buff) =>
            Language.GetTextValue("Mods.AttackSpeedMeter.UITips.StatusTemplate")
                    .Replace("[usetime]", usetime.ToString())
                    .Replace("[buff]", FormatHelper.PercentageFloor(buff - 1));
        public static String GetThresholds(float prev, float next) =>
            Language.GetTextValue("Mods.AttackSpeedMeter.UITips.ThresholdTemplate")
                    .Replace("[prev]", (prev - 100).ToString() + "%")
                    .Replace("[next]", (next - 100).ToString() + "%");
        public static String GetMaxThresholds(float prev) =>
            Language.GetTextValue("Mods.AttackSpeedMeter.UITips.MaxThresholdTemplate")
                    .Replace("[prev]", (prev - 100).ToString() + "%");
        public static String GetItemMultiplier(float mult) =>
            Language.GetTextValue("Mods.AttackSpeedMeter.UITips.ItemMultiplierTemplate")
                    .Replace("[mult]", FormatHelper.PercentageFloor(mult));
        public static String GetPlayerMultiplier(float mult) =>
            Language.GetTextValue("Mods.AttackSpeedMeter.UITips.PlayerMultiplierTemplate")
                    .Replace("[mult]", FormatHelper.PercentageFloor(mult));
        public static String GetWarn() =>
            Language.GetTextValue("Mods.AttackSpeedMeter.UITips.UseAnimationWarn");
        public static String GetEnterWorldText() =>
            Language.GetTextValue("Mods.AttackSpeedMeter.OtherTips.OnEnterWorld");
    }
}
