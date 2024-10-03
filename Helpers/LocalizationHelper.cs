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
        public static String GetHeader(DamageClass damageClass, float buff) =>
            Language.GetTextValue("Mods.AttackSpeedMeter.UITips.Headers."
            + ModContent.GetInstance<DamageClasses>()[damageClass] + "Speed")
            .Replace("[BUFF]", FormatHelper.PercentageFloor(buff - 1));
        public static String GetSimpleStatus(int useTime)
        {
            return Language.GetTextValue("Mods.AttackSpeedMeter.UITips.UseTime.SimpleStatusTemplate")
                            .Replace("[TIME]", useTime.ToString());
        }
        public static String GetStatus(bool IsUseAnimation, int time, float prev, float? next)
        {
            string loc = IsUseAnimation ? "UseAnimation" : "UseTime";
            return Language.GetTextValue("Mods.AttackSpeedMeter.UITips." + loc + ".StatusTemplate")
                            .Replace("[TIME]", time.ToString())
                            .Replace("[PREV]", ((prev - 10000)/100).ToString("+#0.00;-#0.00;0") + "%")
                            .Replace("[NEXT]", next==null? "∞" : (((next - 10000)/100).Value.ToString("+#0.00;-#0.00;0") + "%"));
        }
        public static String GetMultiplier(bool IsUseAnimation, float playerMult, float itemMult)
        {
            string loc = IsUseAnimation ? "UseAnimation" : "UseTime";
            return Language.GetTextValue("Mods.AttackSpeedMeter.UITips." + loc + ".ExtraMultiplierTemplate")
                            .Replace("[PLAYER]", FormatHelper.PercentageFloor(playerMult))
                            .Replace("[ITEM]", FormatHelper.PercentageFloor(itemMult));
        }
        public static String GetEnterWorldText() =>
            Language.GetTextValue("Mods.AttackSpeedMeter.OtherTips.OnEnterWorld");
    }
}
