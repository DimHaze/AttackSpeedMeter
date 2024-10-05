using AttackSpeedMeter.Helpers;
using AttackSpeedMeter.ModConfigs;
using AttackSpeedMeter.ModSystems;
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
            DamageClass damageClass = item.DamageType;
            // Get new infomation only if this is a weapon, not accessory or ammo
            if ((item.damage >= 0 || DamageClasses.Contains(damageClass)) && item.useTime > 0 && !(item.ammo > 0) && !item.accessory )
            {
                mainPanel.RemoveAllText();
                var useTime = item.useTime;
                bool needAnimationTime = true;
                bool needUseTime = false;
                float attackSpeed = player.GetTotalAttackSpeed(damageClass);
                int totalUseTime = CombinedHooks.TotalUseTime(useTime, player, item);
                int totalAnimationTime = CombinedHooks.TotalAnimationTime(item.useAnimation, player, item);
                float vanillaMult = ItemID.Sets.BonusAttackSpeedMultiplier[item.type];
                // Calculate Use Time Thresholds
                // nextUseTimeThreshold with a null means that Total Use Time cant be any smaller 
                float prevUseTimeThreshold;
                float? nextUseTimeThreshold = null;
                if (totalUseTime != 1)
                {
                    nextUseTimeThreshold = UseTimeHelper.BinarySearchThreshold(player, item, totalUseTime - 1);
                }
                prevUseTimeThreshold = UseTimeHelper.BinarySearchThreshold(player, item, totalUseTime);
                // Calculate Animation Thresholds
                // nextAnimationThreshold with a null means that Total Animation cant be any smaller 
                float prevAnimationThreshold;
                float? nextAnimationThreshold = null;
                var multipliedUseTime = Math.Max(1, (int)(item.useTime * (1 / CombinedHooks.TotalUseSpeedMultiplier(player, item))));
                if (totalAnimationTime != 1 && multipliedUseTime!=1 && (int)(multipliedUseTime * item.useAnimation / item.useTime)>1)
                {
                    nextAnimationThreshold = UseAnimationHelper.BinarySearchThreshold(player, item, totalAnimationTime - 1);
                }
                prevAnimationThreshold = UseAnimationHelper.BinarySearchThreshold(player, item, totalAnimationTime);
                // Decides which information to display
                // attackSpeedOnlyAffectsWeaponAnimation means that speed dont affact Use Time, so dont display thresholds
                // totalAnimationTime==totalUseTime or they have same thresholds lead to information redundancy
                if (!item.attackSpeedOnlyAffectsWeaponAnimation)
                {
                    needUseTime = true;
                    if(totalAnimationTime==totalUseTime ||(prevUseTimeThreshold==prevAnimationThreshold&&nextUseTimeThreshold==nextAnimationThreshold))
                    {
                        needAnimationTime = false;
                    }
                }
                mainPanel.AddText(LocalizationHelper.GetHeader(damageClass,attackSpeed));
                
                if (needUseTime)
                {
                    // extra multipliers
                    float itemMult = ItemLoader.UseTimeMultiplier(item, player)
                                     * (1 / ItemLoader.UseSpeedMultiplier(item, player));
                    float playerMult = PlayerLoader.UseTimeMultiplier(player, item)
                                       * (1 / PlayerLoader.UseSpeedMultiplier(player, item));
                    string? prevColor = null;
                    string? currentColor = null;
                    string? nextColor = null;
                    // Insert your code of coloring here
                    // Leave it null for white
                    // Change the color for infinity in LocalizationHelper.cs
                    float assumedUseTimeForColor = item.useTime * itemMult * playerMult;
                    float? p = prevUseTimeThreshold * (1f / 10000f);
                    float? n = nextUseTimeThreshold * (1f / 10000f);
                    prevColor = ColorHelper.GetColor(totalUseTime,assumedUseTimeForColor,p);
                    currentColor = ColorHelper.GetColor(totalUseTime,assumedUseTimeForColor,attackSpeed);
                    nextColor = ColorHelper.GetColor(totalUseTime-1,assumedUseTimeForColor,n);
                    // FOR DEBUG: Displays the color along side RGB value
                    // mainPanel.AddText(p.ToString());
                    //mainPanel.AddText("[c/" + FormatHelper.ColorToString(prevColor.Value) + ":"+
                    //                    FormatHelper.ColorToString(prevColor.Value)
                    //                    +"]");
                    mainPanel.AddText(LocalizationHelper.GetStatus(false, totalUseTime, prevUseTimeThreshold, nextUseTimeThreshold, prevColor, currentColor, nextColor));
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
                else
                {
                    // simply displays Total Use Time
                    mainPanel.AddText(LocalizationHelper.GetSimpleStatus(totalUseTime));
                }
                if (needAnimationTime)
                {
                    // extra multipliers
                    float itemMult = ItemLoader.UseAnimationMultiplier(item, player)
                                     * (1 / ItemLoader.UseSpeedMultiplier(item, player));
                    float playerMult = PlayerLoader.UseAnimationMultiplier(player, item)
                                       * (1 / PlayerLoader.UseSpeedMultiplier(player, item));
                    string? prevColor = null;
                    string? currentColor = null;
                    string? nextColor = null;
                    // Insert your code of coloring here
                    // Leave it null for white
                    // Change the color for infinity in LocalizationHelper.cs
                    float assumedUseAnimationTimeForColor = item.useAnimation * itemMult * playerMult;
                    // the assumed use animation time if there is absolutely no rounding and no attack speed
                    float? p = prevAnimationThreshold * (1f / 10000f);
                    float? n = nextAnimationThreshold * (1f / 10000f);
                    prevColor = ColorHelper.GetColor(totalAnimationTime,assumedUseAnimationTimeForColor,p);
                    currentColor = ColorHelper.GetColor(totalAnimationTime,assumedUseAnimationTimeForColor,attackSpeed);
                    nextColor = ColorHelper.GetColor(totalAnimationTime-1,assumedUseAnimationTimeForColor,n);
                    mainPanel.AddText(LocalizationHelper.GetStatus(true, totalAnimationTime, prevAnimationThreshold, nextAnimationThreshold, prevColor, currentColor, nextColor));
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
            mainPanel.FinalUpdate();
            base.Update(gameTime);
        }
    }
}
