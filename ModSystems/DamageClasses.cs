using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using AttackSpeedMeter.Helpers;

namespace AttackSpeedMeter.ModSystems
{
    public class DamageClasses: ModSystem
    {
        private static Dictionary<DamageClass, string> _supportedClasses = new()
        {
            {DamageClass.Melee , "Melee"},
            {DamageClass.Ranged , "Ranged"},
            {DamageClass.MeleeNoSpeed, "MeleeNoSpeed"},
            {DamageClass.Magic, "Magic"},
            {DamageClass.SummonMeleeSpeed, "SummonMeleeSpeed"},
            {DamageClass.Throwing, "Throwing"},
            {DamageClass.Summon, "Summon"},
            {DamageClass.Generic, "Generic"}
        };
        private static readonly string _defaultClass = "Generic";
        public string this[DamageClass damageClass]
        {
            get
            {
                if (_supportedClasses.TryGetValue(damageClass, out string value)){
                    return value;
                }
                return _defaultClass;
            }
        }
        public override void OnWorldLoad()
        {
            if (ModLoader.TryGetMod("CalamityMod", out var cal))
            {
                // Rogue
                if (cal.TryFind<DamageClass>("RogueDamageClass", out var RogueDamageClass))
                {
                    _supportedClasses[RogueDamageClass] = "Rogue";
                }

                // True Melee
                if (cal.TryFind<DamageClass>("TrueMeleeDamageClass", out var TrueMeleeDamageClass))
                {
                    _supportedClasses[TrueMeleeDamageClass] = "TrueMelee";
                }

                // True Melee no Speed
                if (cal.TryFind<DamageClass>("TrueMeleeNoSpeedDamageClass", out var TrueMeleeNoSpeedDamageClass))
                {
                    _supportedClasses[TrueMeleeNoSpeedDamageClass] = "TrueMeleeNoSpeed";
                }

                // Melee Ranged Hybrid
                if (cal.TryFind<DamageClass>("MeleeRangedHybridDamageClass", out var MeleeRangedHybridDamageClass))
                {
                    _supportedClasses[MeleeRangedHybridDamageClass] = "MeleeRangedHybrid";
                }

                // Average
                if (cal.TryFind<DamageClass>("AverageDamageClass", out var AverageDamageClass))
                {
                    _supportedClasses[AverageDamageClass] = "Average";
                }
            }
            if (ModLoader.TryGetMod("ThoriumMod", out var thorium))
            {
                // Healer
                if (thorium.TryFind<DamageClass>("HealerDamage", out var HealerDamage))
                {
                    _supportedClasses[HealerDamage] = "Healer";
                }

                // Healer Healing
                if (thorium.TryFind<DamageClass>("HealerTool", out var HealerTool))
                {
                    _supportedClasses[HealerTool] = "HealerTool";
                }

                // Bard
                if (thorium.TryFind<DamageClass>("BardDamage", out var BardDamage))
                {
                    _supportedClasses[BardDamage] = "Bard";
                }
            }
            base.OnWorldLoad();
        }
        
    }
}
