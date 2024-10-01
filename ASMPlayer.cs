using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameInput;
using Terraria.ModLoader;
using AttackSpeedMeter.ModSystems;
using AttackSpeedMeter.Helpers;
using Terraria;

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
        public override void OnEnterWorld()
        {
            if (AttackSpeedMeter.MeterKey.GetAssignedKeys().Count == 0)
            {
                Main.NewText(LocalizationHelper.GetEnterWorldText());
            }
            base.OnEnterWorld();
        }
    }
}
