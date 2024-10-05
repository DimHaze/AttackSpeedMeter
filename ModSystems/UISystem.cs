using AttackSpeedMeter.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace AttackSpeedMeter.ModSystems
{
    public class UISystem : ModSystem
    {
        internal UserInterface _MeterInterface;
        internal MeterUI _MeterUI;
        private GameTime _lastUpdateUiGameTime;
        internal bool IsMeterClosed() => _MeterInterface?.CurrentState == null;
        public override void Load()
        {
            if (!Main.dedServ)
            {
                _MeterInterface = new UserInterface();
                _MeterUI = new MeterUI();
                _MeterUI.Activate();
            }
            base.Load();
        }
        public override void Unload()
        {
            _MeterInterface = null;
            _MeterUI = null;
            base.Unload();
        }
        public override void UpdateUI(GameTime gameTime)
        {
            _lastUpdateUiGameTime = gameTime;
            if (_MeterInterface?.CurrentState!=null)
            {
                _MeterInterface.Update(gameTime);
            }
            base.UpdateUI(gameTime);
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "AttackSpeedMeter: MeterInterface",
                    delegate
                    {
                        if (_lastUpdateUiGameTime != null && _MeterInterface?.CurrentState != null)
                        {
                            _MeterInterface.Draw(Main.spriteBatch, _lastUpdateUiGameTime);
                        }
                        return true;
                    },
                    InterfaceScaleType.UI));
            }
            base.ModifyInterfaceLayers(layers);
        }
        internal void CloseMeter()
            => _MeterInterface?.SetState((UIState)null);
        //{
        //    _MeterInterface?.SetState((UIState)null);
        //    Main.NewText("2");
        //}
        internal void OpenMeter() 
            => _MeterInterface?.SetState((UIState)_MeterUI);
        public void ToggleMeter()
        {
            if (IsMeterClosed())
            {
                SoundEngine.PlaySound(SoundID.MenuOpen);
                OpenMeter();
            }
            else
            {
                SoundEngine.PlaySound(SoundID.MenuClose);
                CloseMeter();
            }
        }
    }
}
