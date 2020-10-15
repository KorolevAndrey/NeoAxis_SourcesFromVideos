using System;
using System.ComponentModel;
using System.Collections.Generic;
using NeoAxis.Editor;

namespace NeoAxis
{
	public class Timer : NeoAxis.Component
	{

		[DefaultValue( false )]
		public bool Running { get; set; } = false;

		[DefaultValue( true )]
		public bool Deactivate { get; set; } = true;

		[DefaultValue( false )]
		public bool Disable { get; set; } = false;

		[DefaultValue( 2 )]
		[Range( 0, 1000000 )]
		public double TimeToDeactivate { get; set; } = 2;


		[DefaultValue( 0 )]
		public double enableEngineTime { get; set; } = 0;

        [DefaultValue( 0 )]
		public double curTime { get; set; } = 0;

		[DefaultValue( "" )]
		public String ComponentNamePath { get; set; } = "";
		
		[DefaultValue( "" )]
		public String LastErr { get; set; } = "";


        public void Activate() {
        	enableEngineTime = EngineApp.EngineTime;
			curTime = 0;
			Running = true;
			LastErr = "";
		}

		public void Diactivate() {
			Running = false;
			var p = this.Parent;
			var comp = p?.GetComponentByPath(ComponentNamePath);
			if (comp == null)
			{
				LastErr = LastErr + "|comp = null";
				return;
			}

			if (Deactivate) {
				((Component_ParticleSystemInSpace)comp).Activated = false;
			}
			if (Disable) {
				comp.Enabled = false;
			}
		}
		
		protected override void OnEnabledInSimulation()
		{
		}
		
		protected override void OnUpdate( float delta )
		{
			if(Running == true) {
				curTime = EngineApp.EngineTime - enableEngineTime;
				if (curTime >= TimeToDeactivate) {					
					Diactivate();
				}
			}
		}
		
		protected override void OnSimulationStep()
		{
		}

        protected override void OnEnabled()
        {
			Activate();            
            base.OnEnabled();
        }
    }
}