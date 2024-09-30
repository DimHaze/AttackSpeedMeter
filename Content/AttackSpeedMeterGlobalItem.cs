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
using static System.Net.Mime.MediaTypeNames;

namespace AttackSpeedMeter.Content
{
    public class AttackSpeedMeterGlobalItem : GlobalItem
    {
# nullable enable
        private static DamageClass? RogueDamageClass = null;
        private static DamageClass? TrueMeleeDamageClass = null;
        private static DamageClass? TrueMeleeNoSpeedDamageClass = null;
        private static DamageClass? MeleeRangedHybridDamageClass = null;
        private static DamageClass? AverageDamageClass = null;
        private static DamageClass? HealerDamage = null;
        private static DamageClass? HealerTool = null;
        private static DamageClass? BardDamage = null;
        AttackSpeedMeterGlobalItem()
        {
            if (Terraria.ModLoader.ModLoader.TryGetMod("CalamityMod", out var cal))
            {
                // Rogue
                if (cal.TryFind<DamageClass>("RogueDamageClass", out RogueDamageClass))
                {
                    supportedDamageClasses[RogueDamageClass] = "Rogue";
                }

                // True Melee
                if (cal.TryFind<DamageClass>("TrueMeleeDamageClass", out TrueMeleeDamageClass))
                {
                    supportedDamageClasses[TrueMeleeDamageClass] = "TrueMelee";
                }

                // True Melee no Speed
                if (cal.TryFind<DamageClass>("TrueMeleeNoSpeedDamageClass", out TrueMeleeNoSpeedDamageClass))
                {
                    supportedDamageClasses[TrueMeleeNoSpeedDamageClass] = "TrueMeleeNoSpeed";
                }

                // Melee Ranged Hybrid
                if (cal.TryFind<DamageClass>("MeleeRangedHybridDamageClass", out MeleeRangedHybridDamageClass))
                {
                    supportedDamageClasses[MeleeRangedHybridDamageClass] = "MeleeRangedHybrid";
                }

                // Average
                if (cal.TryFind<DamageClass>("AverageDamageClass", out AverageDamageClass))
                {
                    supportedDamageClasses[AverageDamageClass] = "Average";
                }
            }
            if (Terraria.ModLoader.ModLoader.TryGetMod("ThoriumMod", out var thorium))
            {
                // Healer
                if (thorium.TryFind<DamageClass>("HealerDamage", out HealerDamage))
                {
                    supportedDamageClasses[HealerDamage] = "Healer";
                }

                // Healer Healing
                if (thorium.TryFind<DamageClass>("HealerTool", out HealerTool))
                {
                    supportedDamageClasses[HealerTool] = "HealerTool";
                }

                // Bard
                if (thorium.TryFind<DamageClass>("BardDamage", out BardDamage))
                {
                    supportedDamageClasses[BardDamage] = "Bard";
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
                    .Replace("[buff]", FormatHelper.PercentageFloor(buff-1))
                );
        private TooltipLine GetThresholds(float prev, float next) =>
            new TooltipLine(base.Mod, "AttackSpeedMeter.Thresholds",
                Language.GetTextValue("Mods.AttackSpeedMeter.ExpandedTooltips.ThresholdTemplate")
                    .Replace("[prev]", (prev - 100).ToString() + "%")
                    .Replace("[next]", (next - 100).ToString() + "%")
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
            if (item.damage >= 0 && item.useTime > 0)
            {
                DamageClass damageClass = item.DamageType;
                var useTime = item.useTime;
                Player player = Main.player[Main.myPlayer];

                int totalUsetime = CombinedHooks.TotalUseTime(useTime, player, item);
                float usetimeBuff = CombinedHooks.TotalUseTimeMultiplier(player, item);
                var prevThreshold = ThresholdHelper.Threshold(useTime, totalUsetime + 1);
                var nextThreshold = ThresholdHelper.Threshold(useTime, totalUsetime);
                float itemMult = ItemLoader.UseTimeMultiplier(item, player) * 
                                 ItemID.Sets.BonusAttackSpeedMultiplier[item.type];
                float playerMult = PlayerLoader.UseTimeMultiplier(player, item);
                if (!item.attackSpeedOnlyAffectsWeaponAnimation)
                {
                    itemMult /= ItemLoader.UseSpeedMultiplier(item, player);
                    playerMult /= PlayerLoader.UseSpeedMultiplier(player, item);
                }
                if (!supportedDamageClasses.ContainsKey(damageClass)){
                    damageClass = DamageClass.Generic;
                }
                tooltips.Add(GetTooltipHeader(damageClass));
                tooltips.Add(GetStatus(totalUsetime, 1f/usetimeBuff));
                tooltips.Add(GetThresholds(prevThreshold, nextThreshold));
                if(Math.Abs(itemMult - 1)>=1e-6f)
                {
                    tooltips.Add(GetItemMultiplier(itemMult));
                }
                if (Math.Abs(playerMult - 1) >= 1e-6f)
                {
                    tooltips.Add(GetPlayerMultiplier(playerMult));
                }
                if (useTime != item.useAnimation)
                {
                    tooltips.Add(GetWarn());
                }

            }
            base.ModifyTooltips(item, tooltips);
        }
    }
}
