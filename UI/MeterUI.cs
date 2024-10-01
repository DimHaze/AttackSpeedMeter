using AttackSpeedMeter.Helpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace AttackSpeedMeter.UI
{
    public class MeterUI : UIState
    {
        private AutoTextPanel mainPanel;
        public override void OnInitialize()
        {
            mainPanel = new AutoTextPanel();
            mainPanel.Width.Set(350, 0);
            mainPanel.Height.Set(150, 0);
            mainPanel.HAlign = 0.6f;
            mainPanel.Top.Set(20, 0);
            Append(mainPanel);

            mainPanel.AddText(LocalizationHelper.GetGreetings());
        }
        public override void Update(GameTime gameTime)
        {
            Player player = Main.player[Main.myPlayer];
            Item item = player.HeldItem;
            if (item.damage >= 0 && item.useTime > 0 && !(item.ammo > 0) && !item.accessory )
            {
                mainPanel.RemoveAllText();
                DamageClass damageClass = item.DamageType;
                var useTime = item.useTime;
                mainPanel.AddText(LocalizationHelper.GetHeader(damageClass));
                if (!item.attackSpeedOnlyAffectsWeaponAnimation)
                {
                    int totalUsetime = CombinedHooks.TotalUseTime(useTime, player, item);
                    float vanillaMult = ItemID.Sets.BonusAttackSpeedMultiplier[item.type];
                    float itemMult = ItemLoader.UseTimeMultiplier(item, player)
                                     / ItemLoader.UseSpeedMultiplier(item, player);
                    float playerMult = PlayerLoader.UseTimeMultiplier(player, item)
                                       / PlayerLoader.UseSpeedMultiplier(player, item);
                    var prevThreshold = ThresholdHelper.Threshold(useTime * itemMult * playerMult, totalUsetime + 1, vanillaMult);
                    var nextThreshold = ThresholdHelper.Threshold(useTime * itemMult * playerMult, totalUsetime, vanillaMult);
                    float attackSpeed = player.GetTotalAttackSpeed(damageClass);
                    mainPanel.AddText(LocalizationHelper.GetStatus(totalUsetime, attackSpeed));
                    if (totalUsetime == 1)
                    {
                        mainPanel.AddText(LocalizationHelper.GetMaxThresholds(prevThreshold));
                    }
                    else
                    {
                        mainPanel.AddText(LocalizationHelper.GetThresholds(prevThreshold, nextThreshold));
                    }
                    if (Math.Abs(itemMult - 1) >= 1e-6f)
                    {
                        mainPanel.AddText(LocalizationHelper.GetItemMultiplier(1/itemMult));
                    }
                    else if (Math.Abs(vanillaMult - 1) >= 1e-6f)
                    {
                        mainPanel.AddText(LocalizationHelper.GetItemMultiplier(vanillaMult));
                    }
                    if (Math.Abs(playerMult - 1) >= 1e-6f)
                    {
                        mainPanel.AddText(LocalizationHelper.GetPlayerMultiplier(1/playerMult));
                    }
                    if (useTime != item.useAnimation)
                    {
                        mainPanel.AddText(LocalizationHelper.GetWarn());
                    }
                }
                else
                {
                    mainPanel.AddText(LocalizationHelper.GetWarn());
                }
            }

            base.Update(gameTime);
        }
    }
}
