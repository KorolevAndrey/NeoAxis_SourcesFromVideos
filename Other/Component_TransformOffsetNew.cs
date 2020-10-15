using System;
using System.ComponentModel;
using System.Collections.Generic;
using NeoAxis.Editor;

namespace NeoAxis
{
	public class Component_TransformOffsetNew : NeoAxis.Component_TransformOffset
	{
		[DefaultValue( false )]
		public bool SkipRotation { get; set; } = false;
		

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