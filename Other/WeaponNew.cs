using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using NeoAxis;
using NeoAxis.Editor;

namespace Project
{
	public class WeaponNew : NeoAxis.Component_Weapon
	{
		protected override void OnEnabledInSimulation()
		{
		}
		
		protected override void OnUpdate( float delta )
		{
		}
		
		protected override void OnSimulationStep()
		{
		}

        protected override bool OnEnabledSelectionByCursor()
        {
            return base.OnEnabledSelectionByCursor();
        }

        public override bool ObjectInteractionInputMessage(UIControl playScreen, Component_GameMode gameMode, InputMessage message)
        {
            return base.ObjectInteractionInputMessage(playScreen, gameMode, message);
        }
    }
}