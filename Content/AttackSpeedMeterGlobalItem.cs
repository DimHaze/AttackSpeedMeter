using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using AttackSpeedMeter.Helpers;

namespace AttackSpeedMeter.Content
{
    public class AttackSpeedMeterGlobalItem : GlobalItem
    {
# nullable enable
        private static DamageClass? rogueDamageClass = null;
        AttackSpeedMeterGlobalItem()
        {
            if (Terraria.ModLoader.ModLoader.TryGetMod("CalamityMod", out var cal))
            {
                if (cal.TryFind<DamageClass>("RogueDamageClass", out rogueDamageClass))
                {
                    supportedDamageClasses[rogueDamageClass] = "Rogue";
                }
            }
        }
        private static Dictionary<DamageClass, String> supportedDamageClasses = new Dictionary<DamageClass, string> {
            {DamageClass.Melee , "Melee"},
            {DamageClass.Ranged , "Ranged"},
            {DamageClass.MeleeNoSpeed, "MeleeNoSpeed"},
            {DamageClass.Magic, "Magic"},
            {DamageClass.SummonMeleeSpeed, "SummonMeleeSpeed"},
            {DamageClass.Throwing, "Throwing"},
            {DamageClass.Summon, "Summon"},
            {DamageClass.Generic, "Generic"}
        };
        private TooltipLine GetTooltipHeader(DamageClass damageClass) =>
            new TooltipLine(base.Mod, "AttackSpeedMeter.Header",
                Language.GetTextValue("Mods.AttackSpeedMeter.ExpandedTooltips."
                    + supportedDamageClasses[damageClass] +"SpeedHeader")
                );
        private TooltipLine GetStatus(int usetime, float buff) => 
            new TooltipLine(base.Mod, "AttackSpeedMeter.Status",
                Language.GetTextValue("Mods.AttackSpeedMeter.ExpandedTooltips.StatusTemplate")
                    .Replace("[usetime]",usetime.ToString())
                    .Replace("[buff]", FormatHelper.PercentageFloor(buff))
                );
        private TooltipLine GetThresholds(float prev, float next) =>
            new TooltipLine(base.Mod, "AttackSpeedMeter.Thresholds",
                Language.GetTextValue("Mods.AttackSpeedMeter.ExpandedTooltips.ThresholdTemplate")
                    .Replace("[prev]", prev.ToString()+"%")
                    .Replace("[next]", next.ToString() + "%")
                );
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.damage >= 0 && item.useTime>0)
            {
                DamageClass damageClass = item.DamageType;
                var useTime = item.useTime;
                Player player = Main.player[Main.myPlayer];

                float speedMult = CombinedHooks.TotalUseSpeedMultiplier(player, item);
                float usetimeMult = PlayerLoader.UseTimeMultiplier(player, item) * ItemLoader.UseTimeMultiplier(item, player);
                int totalUsetime = CombinedHooks.TotalUseTime(useTime, player, item);
                float usetimeBuff = CombinedHooks.TotalUseTimeMultiplier(player, item);

                var prevThreshold = ThresholdHelper.Threshold(useTime, totalUsetime+1);
                var nextThreshold = ThresholdHelper.Threshold(useTime, totalUsetime);

                if (!supportedDamageClasses.ContainsKey(damageClass)){
                    damageClass = DamageClass.Generic;
                }
                tooltips.Add(GetTooltipHeader(damageClass));
                tooltips.Add(GetStatus(totalUsetime, 1f/usetimeBuff));
                tooltips.Add(GetThresholds(prevThreshold, nextThreshold));
            }
            base.ModifyTooltips(item, tooltips);
        }
    }
}
