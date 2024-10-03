using AttackSpeedMeter.ModConfigs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;

namespace AttackSpeedMeter.UI
{
    public class AutoTextPanel: UIPanel
    {
        private static readonly float VPadding = 3;
        private static readonly float VLine = 23;
        private static readonly float _height = 200;
        private int childIndex;
        private static float VPos(int index) => VPadding + VLine * index;
        public override void OnInitialize()
        {
            Width.Set(ModContent.GetInstance<ASMConfigs>().Width, 0);
            Height.Set(_height, 0);
            HAlign = ModContent.GetInstance<ASMConfigs>().HPosition;
            VAlign = ModContent.GetInstance<ASMConfigs>().VPosition;
            childIndex = 0;
            base.OnInitialize();
        }
        public void AddText(string text)
        {
            UIText temp = new(text)
            {
                HAlign = 0.5f
            };
            temp.Top.Set(VPos(childIndex),0);
            childIndex++;
            base.Append(temp);
        }
        public void FinalUpdate()
        {
            HAlign = ModContent.GetInstance<ASMConfigs>().HPosition;
            VAlign = ModContent.GetInstance<ASMConfigs>().VPosition;
            Width.Set(ModContent.GetInstance<ASMConfigs>().Width, 0);
            Height.Set((childIndex+1)*VLine+VPadding, 0);
        }
        public void RemoveAllText()
        {
            childIndex = 0;
            base.RemoveAllChildren();
        }
    }
}
