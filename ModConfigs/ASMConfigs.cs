using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.Config;

namespace AttackSpeedMeter.ModConfigs
{
    public class ASMConfigs: ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;
        [DefaultValue(0.6f)]
        public float HPosition;
        [DefaultValue(0.05f)]
        public float VPosition;
        [Range(300, 600)]
        [DefaultValue(400)]
        public int Width;
    }
}
