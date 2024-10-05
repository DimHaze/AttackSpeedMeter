using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AttackSpeedMeter.Helpers
{
    public class ColorHelper
    {
        public static float AttackSpeedBoostFromRounding(float totalTime, float assumedTime, float? attackSpeed)
        {
            // assumedTime = item.usetime * all factors except for attack speed
            // assume there is no attack speed and no rounding
            float EffectiveAttackSpeed = assumedTime / totalTime;
            if (attackSpeed == null)
            {
                return 1f;
            }
            if (attackSpeed == 1)
            {
                return 1f;
            }
            float Boost = (EffectiveAttackSpeed - 1f) / ((float)attackSpeed - 1f);
            // even if this could happen, it should be seen as no positive boost and no negative boost
            if (Boost < 0f)
            {
                return 1f;
            }
            // intentionally treat negative attack speed differently
            if (attackSpeed < 1f)
            {
                return 0f - Boost;
            }
            return Boost;
        }
        public static float Scaling = 0.4f;
        public static float ScaledBoost(float Boost)
        {
            if (Boost > 1f)
            {
                // 0 at boost = 1, 1 at boost -> infinity, concave down
                float a = (1f - Boost) / Scaling;
                float b = (float)Math.Exp((double)a);
                return 2f / (1f + b) - 1f;
            }
            else if (Boost > -1f && Boost < 0f)
            {
                // 0 at boost = -1 (boost = 1 but negative attack speed)
                // 1 at boost -> 0
                // concave down, smooth near boost -> 0
                float a = Boost * Boost;
                return 1f - a;
            }
            // 0 <= boost <=1, positive attack speed but less effective after rounding, this shouldn't happen.
            // boost <= -1, negative attack speed but even more negative after rounding, this shouldn't happen.
            // if these happen it is rubbish attack speed anyway
            return 0f;
        }
        public static string ColorHex(float boost)
        {
            // boost == 0 -> color = (197,59,59)
            // boost == 0.5 -> color = (246,236,234)
            // boost == 1 -> color = (182,255,0)
            int r = 246;
            int g = 236;
            int b = 234;
            if (boost >= 0.5f)
            {
                float bs = boost * 2f - 1f;
                r = (int)MathHelper.Lerp(197, 246, bs);
                g = (int)MathHelper.Lerp(59, 236, bs);
                b = (int)MathHelper.Lerp(59, 234, bs);
            }
            else
            {
                float bs = boost * 2f;
                r = (int)MathHelper.Lerp(246, 182, bs);
                g = (int)MathHelper.Lerp(236, 255, bs);
                b = (int)MathHelper.Lerp(234, 0, bs);
            }
            string R = r.ToString("x");
            string G = g.ToString("x");
            string B = b.ToString("x");
            return R + G + B;
        }
        public static string GetColor(float totalTime, float assumedTime, float? attackSpeed)
        {
            return ColorHex(ScaledBoost(AttackSpeedBoostFromRounding(totalTime, assumedTime, attackSpeed)/*, MeanBoost(assumedTime,attackSpeed) */));
        }

    }
}