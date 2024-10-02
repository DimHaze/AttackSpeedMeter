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
            Append(mainPanel);

            mainPanel.AddText(LocalizationHelper.GetGreetings());
        }
        public override void Update(GameTime gameTime)
        {
            if ((long)Main.GameUpdateCount % 2 != 0)
            {
                return;
            }
            Player player = Main.player[Main.myPlayer];
            Item item = player.HeldItem;
            if (item.damage >= 0 && item.useTime > 0 && !(item.ammo > 0) && !item.accessory )
            {
                mainPanel.RemoveAllText();
                DamageClass damageClass = item.DamageType;
                var useTime = item.useTime;
                bool needAnimationTime = true;
                bool needUseTime = false;
                float attackSpeed = player.GetTotalAttackSpeed(damageClass);
                int totalUseTime = CombinedHooks.TotalUseTime(useTime, player, item);
                int totalAnimationTime = CombinedHooks.TotalAnimationTime(useTime, player, item);
                float vanillaMult = ItemID.Sets.BonusAttackSpeedMultiplier[item.type];

                float prevUseTimeThreshold;
                float? nextUseTimeThreshold = null;
                if (totalUseTime != 1)
                {
                    nextUseTimeThreshold = UseTimeHelper.BinarySearchThreshold(player, item, totalUseTime - 1);
                }
                prevUseTimeThreshold = UseTimeHelper.BinarySearchThreshold(player, item, totalUseTime);

                float prevAnimationThreshold;
                float? nextAnimationThreshold = null;
                if (totalAnimationTime != 1)
                {
                    nextAnimationThreshold = UseAnimationHelper.BinarySearchThreshold(player, item, totalAnimationTime - 1);
                }
                prevAnimationThreshold = UseAnimationHelper.BinarySearchThreshold(player, item, totalAnimationTime);

                if (!item.attackSpeedOnlyAffectsWeaponAnimation)
                {
                    needUseTime = true;
                    if(totalAnimationTime==totalUseTime ||(prevUseTimeThreshold==prevAnimationThreshold&&nextUseTimeThreshold==nextAnimationThreshold))
                    {
                        needAnimationTime = false;
                    }
                }
                mainPanel.AddText(LocalizationHelper.GetHeader(damageClass,attackSpeed));
                

                mainPanel.AddText(LocalizationHelper.GetStatus(false, totalUseTime, prevUseTimeThreshold, nextUseTimeThreshold));
                if (needUseTime)
                {
                    float itemMult = ItemLoader.UseTimeMultiplier(item, player)
                                     * (1 / ItemLoader.UseSpeedMultiplier(item, player));
                    float playerMult = PlayerLoader.UseTimeMultiplier(player, item)
                                       * (1 / PlayerLoader.UseSpeedMultiplier(player, item));
                    if (Math.Abs(itemMult - 1) >= 1e-4f)
                    {
                        mainPanel.AddText(LocalizationHelper.GetMultiplier(false,1/playerMult,1 / itemMult));
                    }
                    else if (Math.Abs(vanillaMult - 1) >= 1e-4f)
                    {
                        mainPanel.AddText(LocalizationHelper.GetMultiplier(false, 1 / playerMult, vanillaMult));
                    }
                    else if (Math.Abs(playerMult - 1) >= 1e-4f)
                    {
                        mainPanel.AddText(LocalizationHelper.GetMultiplier(false, 1 / playerMult, 1 / itemMult));
                    }
                }
                if (needAnimationTime)
                {
                    mainPanel.AddText(LocalizationHelper.GetStatus(true, totalAnimationTime, prevAnimationThreshold, nextAnimationThreshold));
                    float itemMult = ItemLoader.UseAnimationMultiplier(item, player)
                                     * (1 / ItemLoader.UseSpeedMultiplier(item, player));
                    float playerMult = PlayerLoader.UseAnimationMultiplier(player, item)
                                       * (1 / PlayerLoader.UseSpeedMultiplier(player, item));
                    if (Math.Abs(itemMult - 1) >= 1e-4f)
                    {
                        mainPanel.AddText(LocalizationHelper.GetMultiplier(true, 1 / playerMult, 1 / itemMult));
                    }
                    else if (Math.Abs(vanillaMult - 1) >= 1e-4f)
                    {
                        mainPanel.AddText(LocalizationHelper.GetMultiplier(true, 1 / playerMult, vanillaMult));
                    }
                    else if (Math.Abs(playerMult - 1) >= 1e-4f)
                    {
                        mainPanel.AddText(LocalizationHelper.GetMultiplier(true, 1 / playerMult, 1 / itemMult));
                    }
                }
            }
            mainPanel.UpdateHeight();
            base.Update(gameTime);
        }
    }
}
