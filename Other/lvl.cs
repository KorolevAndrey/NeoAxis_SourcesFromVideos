using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using NeoAxis;
using NeoAxis.Editor;

namespace Project
{
	public class lvl : NeoAxis.Component
	{
		[DefaultValue( 3.5 )]		
		public double L { get; set; } = 3.5;
		
		[DefaultValue( "" )]
		public String lvlStr { get; set; } = "";
		
		protected override void OnEnabledInSimulation()
		{
		}
		
		protected override void OnUpdate( float delta )
		{
		}
		
		protected override void OnSimulationStep()
		{
		}
	}
}