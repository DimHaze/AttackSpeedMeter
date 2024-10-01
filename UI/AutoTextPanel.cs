using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI.Elements;

namespace AttackSpeedMeter.UI
{
    public class AutoTextPanel: UIPanel
    {
        private static readonly float VPadding = 7;
        private static readonly float VLine = 23;
        private int childIndex;
        private static float VPos(int index) => VPadding + VLine * index;
        public override void OnInitialize()
        {
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
        public void RemoveAllText()
        {
            childIndex = 0;
            base.RemoveAllChildren();
        }
    }
}
