using System;
using System.ComponentModel;
using System.Collections.Generic;
using NeoAxis.Editor;

namespace NeoAxis
{
	public class MyClass5 : NeoAxis.Component_MeshInSpaceAnimationController
	{

		public double currentAnimationTime;
		public double currentEngineTime;
		public string PlayAnimationName = "";

		public void ResetTime()
		{
			if (this.PlayAnimation.Value == null)
			{
				return;
			}
			this.PlayAnimationName = this.PlayAnimation.Value.Name;
			this.currentEngineTime = EngineApp.EngineTime;
			this.currentAnimationTime = (double)(this.PlayAnimation.Value is Component_SkeletonAnimation skeletonAnimation ? skeletonAnimation.TrackStartTime : (Reference<double>)0.0);
		}


		private void UpdateAnimationTime()
		{	
			if (this.PlayAnimation.Value == null)
			{
				return;
			}
			if(this.PlayAnimationName != this.PlayAnimation.Value.Name) {
				ResetTime();
			}
			double engineTime = EngineApp.EngineTime;
			double num = engineTime - this.currentEngineTime;
			this.currentEngineTime = engineTime;
			this.currentAnimationTime += num * (double)this.Speed;
			if (this.PlayAnimation.Value is Component_SkeletonAnimation skeletonAnimation)
			{
				double trackStartTime = (double)skeletonAnimation.TrackStartTime;
				double length = (double)skeletonAnimation.Length;
				if (length > 0.0)
				{
					if ((bool)this.AutoRewind)
					{
						while (this.currentAnimationTime > trackStartTime + length)
							this.currentAnimationTime -= length;
						while (this.currentAnimationTime < trackStartTime)
							this.currentAnimationTime += length;
					}
					else
						MathEx.Clamp(ref this.currentAnimationTime, trackStartTime, trackStartTime + length);
				}
				else
					this.currentAnimationTime = trackStartTime;
			}
			else
				this.currentAnimationTime = 0.0;
		}


		protected override void OnEnabledInSimulation()
		{
			ResetTime();

			base.OnEnabledInSimulation();
		}

		protected override void OnUpdate(float delta)
		{
			UpdateAnimationTime();

			base.OnUpdate(delta);
		}


	}
}