using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using NeoAxis;
using NeoAxis.Editor;

namespace Project
{
	public class LifeChecker : NeoAxis.Component
	{
		private Reference<Component_Scene> cursc;
		private Reference<Component_InputProcessing> inppr;
		private double lifeValue = 0;
		
		public event LifeChecker.OnSignalDelegate OnSignal;
		public delegate void OnSignalDelegate(double lifeValue);
		
		protected override void OnEnabledInSimulation()
		{
			cursc = Component_Scene.First;
			inppr = cursc.Value.GetComponent<Component_InputProcessing>(true);
			if (cursc.Value == null || inppr.Value == null) return;

			inppr.Value.InputMessageEvent += InputMessageEvent;			
		}
		
		protected override void OnUpdate( float delta )
		{
		}
		
		public void InputMessageEvent(NeoAxis.Component_InputProcessing sender, NeoAxis.UIControl playScreen, NeoAxis.Component_GameMode gameMode, NeoAxis.InputMessage message) 
		{
			if(sender.IsKeyPressed(EKeys.Space) && message.GetType().ToString() == "NeoAxis.InputMessageKeyPress") {
				lifeValue = lifeValue + 1;
				OnSignal?.Invoke(lifeValue);                
				//ScreenMessages.Add("Space");
			}
		}
		
		protected override void OnSimulationStep()
		{
		}
	}
}