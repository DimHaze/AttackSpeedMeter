﻿using AttackSpeedMeter.ModConfigs;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.ID;

namespace AttackSpeedMeter.UI
{
    public class AutoTextPanel: UIPanel
    {
        private static readonly float VPadding = 3;
        private static readonly float VLine = 23;
        private static readonly float _height = 200;
        private int childIndex;
        private bool _dragging;
        private float _xBias;
        private float _yBias;
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
        public override void LeftMouseDown(UIMouseEvent evt)
        {
            if (!_dragging && ModContent.GetInstance<ASMConfigs>().Draggable)
            {
                _dragging = true;
                _xBias = evt.MousePosition.X - HAlign*Main.screenWidth;
                _yBias = evt.MousePosition.Y - VAlign*Main.screenHeight;
                SoundEngine.PlaySound(SoundID.MenuTick);
            }
            base.LeftMouseDown(evt);
        }
        public override void LeftMouseUp(UIMouseEvent evt)
        {
            if (_dragging)
            {
                _dragging = false;
                SoundEngine.PlaySound(SoundID.MenuTick);
            }
            //Main.NewText("Mouse Up");
            base.LeftMouseUp(evt);
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
        public override void Update(GameTime gameTime)
        {
            if (_dragging && !IsMouseHovering)
            {
                _dragging = false;
                SoundEngine.PlaySound(SoundID.MenuTick);
            }
            if (_dragging && ModContent.GetInstance<ASMConfigs>().Draggable)
            {
                float _HAlign = ((float)Main.mouseX - _xBias) / Main.screenWidth;
                float _VAlign = ((float)Main.mouseY - _yBias) / Main.screenHeight;
                ModContent.GetInstance<ASMConfigs>().HPosition = _HAlign;
                ModContent.GetInstance<ASMConfigs>().VPosition = _VAlign;
            }
            HAlign = ModContent.GetInstance<ASMConfigs>().HPosition;
            VAlign = ModContent.GetInstance<ASMConfigs>().VPosition;
            base.Update(gameTime);
        }
        public void UpdateText()
        {
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
