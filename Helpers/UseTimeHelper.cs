using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AttackSpeedMeter.Helpers
{
    public class UseTimeHelper
    {
        private static float _left = -0.99f;
        private static float _right = 10.0f;
        private static float _epsilon = 0.01f;
        public static int TotalUseTimeSim(float attackSpeed, float vanillaSpeedMult,
            float playerSpeedMult, float itemSpeedMult,
            float playerTimeMult, float itemTimeMult,
            int useTime)
        {
            attackSpeed = 1 + ((attackSpeed - 1) * vanillaSpeedMult);
            float totalUseSpeedMult = playerSpeedMult * itemSpeedMult * attackSpeed;
            float totalUseTimeMult = playerTimeMult * itemTimeMult;
            totalUseTimeMult/=totalUseSpeedMult;
            return Math.Max(1, (int)(useTime * totalUseTimeMult));
        }
        public static int BinarySearchThreshold(Player player,Item item,int tgtUseTime)
        {
            var vanillaSpeedMult = ItemID.Sets.BonusAttackSpeedMultiplier[item.type];
            var playerSpeedMult = PlayerLoader.UseSpeedMultiplier(player, item);
            var itemSpeedMult = ItemLoader.UseSpeedMultiplier(item, player);
            var playerTimeMult = PlayerLoader.UseTimeMultiplier(player, item);
            var itemTimeMult = ItemLoader.UseTimeMultiplier(item, player);
            var useTime = item.useTime;

            float left = _left;
            float right = Math.Max(_right, player.GetTotalAttackSpeed(item.DamageType) * 3);
            while (right - left > _epsilon)
            {
                float mid = (left + right) / 2;
                float f_mid = TotalUseTimeSim(mid, vanillaSpeedMult, playerSpeedMult, itemSpeedMult, playerTimeMult, itemTimeMult, useTime);
       
                if (f_mid > tgtUseTime)
                {
                    left = mid; 
                }
                else
                {
                    right = mid; 
                }
            }
            var right_high = Math.Ceiling(right * 100)/100;
            var right_low = Math.Floor(right * 100) / 100;
            var f_right_low = TotalUseTimeSim((float)right_low, vanillaSpeedMult, playerSpeedMult, itemSpeedMult, playerTimeMult, itemTimeMult, useTime);
            if (f_right_low<=tgtUseTime)
            {
                return (int)FormatHelper.SafeFloor((float)(right_low*100));
            }
            else
            {
                return (int)FormatHelper.SafeFloor((float)(right_high*100));
            }
        }
    }
}
