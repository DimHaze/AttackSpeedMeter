using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace AttackSpeedMeter.UI
{
    public class MeterUI : UIState
    {
        public int BackWidth = 300;
        public int BackHeight = 300;
        public override void OnInitialize()
        {
            UIPanel panel = new UIPanel();
            panel.Width.Set(300, 0);
            panel.Height.Set(300, 0);
            Append(panel);

            UIText text = new UIText("Hello world!");
            text.HAlign = 0.5f; // 1
            text.VAlign = 0.5f; // 1
            panel.Append(text);
        }
    }
}
