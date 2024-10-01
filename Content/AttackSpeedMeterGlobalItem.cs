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
using AttackSpeedMeter.ModSystems;
using static System.Net.Mime.MediaTypeNames;

namespace AttackSpeedMeter.Content
{
    public class AttackSpeedMeterGlobalItem : GlobalItem
    {
# nullable enable
        AttackSpeedMeterGlobalItem()
        {
            
        }
        private TooltipLine GetTooltipHeader(DamageClass damageClass) =>
            new TooltipLine(base.Mod, "AttackSpeedMeter.Header",
                Language.GetTextValue("Mods.AttackSpeedMeter.ExpandedTooltips."
                    + ModContent.GetInstance<DamageClasses>()[damageClass] +"SpeedHeader")
                );
        private TooltipLine GetStatus(int usetime, float buff) => 
            new TooltipLine(base.Mod, "AttackSpeedMeter.Status",
                Language.GetTextValue("Mods.AttackSpeedMeter.ExpandedTooltips.StatusTemplate")
                    .Replace("[usetime]",usetime.ToString())
                    .Replace("[buff]", FormatHelper.PercentageFloor(buff-1))
                );
        private TooltipLine GetThresholds(float prev, float next) =>
            new TooltipLine(base.Mod, "AttackSpeedMeter.Thresholds",
                Language.GetTextValue("Mods.AttackSpeedMeter.ExpandedTooltips.ThresholdTemplate")
                    .Replace("[prev]", (prev - 100).ToString() + "%")
                    .Replace("[next]", (next - 100).ToString() + "%")
                );
        private TooltipLine GetMaxThresholds(float prev) =>
            new TooltipLine(base.Mod, "AttackSpeedMeter.Thresholds",
                Language.GetTextValue("Mods.AttackSpeedMeter.ExpandedTooltips.MaxThresholdTemplate")
                    .Replace("[prev]", (prev - 100).ToString() + "%")
                );
        private TooltipLine GetItemMultiplier(float mult) =>
            new TooltipLine(base.Mod, "AttackSpeedMeter.ItemMultiplier",
                Language.GetTextValue("Mods.AttackSpeedMeter.ExpandedTooltips.ItemMultiplierTemplate")
                    .Replace("[mult]", FormatHelper.PercentageFloor(mult))
                );
        private TooltipLine GetPlayerMultiplier(float mult) =>
            new TooltipLine(base.Mod, "AttackSpeedMeter.ItemMultiplier",
                Language.GetTextValue("Mods.AttackSpeedMeter.ExpandedTooltips.PlayerMultiplierTemplate")
                    .Replace("[mult]", FormatHelper.PercentageFloor(mult))
                );
        private TooltipLine GetWarn() =>
            new TooltipLine(base.Mod, "AttackSpeedMeter.ItemMultiplier",
                Language.GetTextValue("Mods.AttackSpeedMeter.ExpandedTooltips.UseAnimationWarn")
                );
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.damage >= 0 && item.useTime > 0 && !(item.ammo>0))
            {
                DamageClass damageClass = item.DamageType;
                var useTime = item.useTime;
                Player player = Main.player[Main.myPlayer];
                // Stats
                if (item.attackSpeedOnlyAffectsWeaponAnimation)
                {
                    tooltips.Add(GetWarn());
                    return;
                }
                int totalUsetime = CombinedHooks.TotalUseTime(useTime, player, item);
                float vanillaMult = ItemID.Sets.BonusAttackSpeedMultiplier[item.type];
                float itemMult = ItemLoader.UseTimeMultiplier(item, player)
                                 / ItemLoader.UseSpeedMultiplier(item, player);
                float playerMult = PlayerLoader.UseTimeMultiplier(player, item)
                                   / PlayerLoader.UseSpeedMultiplier(player, item);
                var prevThreshold = ThresholdHelper.Threshold(useTime * itemMult * playerMult, totalUsetime + 1,vanillaMult);
                var nextThreshold = ThresholdHelper.Threshold(useTime * itemMult * playerMult, totalUsetime,vanillaMult);
                float attackSpeed = player.GetTotalAttackSpeed(damageClass);
                // add tooltips
                tooltips.Add(GetTooltipHeader(damageClass));
                tooltips.Add(GetStatus(totalUsetime, attackSpeed));
                if(totalUsetime == 1)
                {
                    tooltips.Add(GetMaxThresholds(prevThreshold));
                }
                else
                {
                    tooltips.Add(GetThresholds(prevThreshold, nextThreshold));
                }
                if(Math.Abs(itemMult - 1)>=1e-6f)
                {
                    tooltips.Add(GetItemMultiplier(itemMult));
                }
                else if(Math.Abs(vanillaMult - 1) >= 1e-6f)
                {
                    tooltips.Add(GetItemMultiplier(vanillaMult));
                }
                if (Math.Abs(playerMult - 1) >= 1e-6f)
                {
                    tooltips.Add(GetPlayerMultiplier(playerMult));
                }
                if (useTime != item.useAnimation )
                {
                    tooltips.Add(GetWarn());
                }

            }
            base.ModifyTooltips(item, tooltips);
        }
    }
}
