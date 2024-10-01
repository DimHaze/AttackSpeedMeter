using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameInput;
using Terraria.ModLoader;
using AttackSpeedMeter.ModSystems;

namespace AttackSpeedMeter
{
    public class ASMPlayer:ModPlayer
    {
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (AttackSpeedMeter.MeterKey.JustPressed)
            {
                ModContent.GetInstance<UISystem>().ToggleMeter();
            }
            base.ProcessTriggers(triggersSet);
        }
    }
}
