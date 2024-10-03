using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AttackSpeedMeter.Helpers
{
    public class UseAnimationHelper
    {
        private static float _left = -0.9900f;
        private static float _right = 20.000f;
        private static float _epsilon = 0.0100f; 
        public static int TotalAnimationTimeSim(float attackSpeed, float vanillaSpeedMult,
            float playerSpeedMult, float itemSpeedMult,
            float playerAnimationMult, float itemAnimationMult,
            int useTime, int useAnimation)
        {
            attackSpeed = 1 + ((attackSpeed - 1) * vanillaSpeedMult);
            float totalUseSpeedMult = playerSpeedMult * itemSpeedMult * attackSpeed;
            float totalUseAnimationMult = playerAnimationMult * itemAnimationMult;
            int multipliedUseTime = Math.Max(1, (int)(useTime * (1 / totalUseSpeedMult)));
            int relativeUseAnimation = Math.Max(1, (int)(multipliedUseTime * useAnimation / useTime));
            totalUseAnimationMult *= relativeUseAnimation / (float)useAnimation;
            return Math.Max(1, (int)(useAnimation * totalUseAnimationMult));
        }

        public static float BinarySearchThreshold(Player player, Item item, int tgtAnimation)
        {
            var vanillaSpeedMult = ItemID.Sets.BonusAttackSpeedMultiplier[item.type];
            var playerSpeedMult = PlayerLoader.UseSpeedMultiplier(player, item);
            var itemSpeedMult = ItemLoader.UseSpeedMultiplier(item, player);
            var playerAnimationMult = PlayerLoader.UseAnimationMultiplier(player, item);
            var itemAnimationMult = ItemLoader.UseAnimationMultiplier(item, player);
            var useTime = item.useTime;
            var useAnimation = item.useAnimation;


            float left = _left;
            float right = Math.Max(_right, player.GetTotalAttackSpeed(item.DamageType) * 3);
            while (right - left > _epsilon)
            {
                float mid = (left + right) / 2;
                float f_mid = TotalAnimationTimeSim(mid,vanillaSpeedMult,playerSpeedMult,itemSpeedMult,playerAnimationMult,itemAnimationMult,useTime,useAnimation);

                if (f_mid > tgtAnimation)
                {
                    left = mid;
                }
                else
                {
                    right = mid;
                }
            }
            var right_high = Math.Ceiling(right * 10000) / 10000;
            var right_low = Math.Floor(right * 10000) / 10000;
            var f_right_low = TotalAnimationTimeSim((float)right_low, vanillaSpeedMult, playerSpeedMult, itemSpeedMult, playerAnimationMult, itemAnimationMult, useTime,useAnimation);
            if (f_right_low <= tgtAnimation)
            {
                return (int)FormatHelper.SafeFloor((float)(right_low * 10000));
            }
            else
            {
                return (int)FormatHelper.SafeFloor((float)(right_high * 10000));
            }
        }
    }
}
