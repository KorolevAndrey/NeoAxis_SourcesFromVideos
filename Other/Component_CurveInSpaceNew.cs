using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using NeoAxis;
using NeoAxis.Editor;

namespace NeoAxis
{
	public class Component_CurveInSpaceNew : NeoAxis.Component_CurveInSpace
	{
		[DefaultValue(0)]
		public double TimeLooped { get; set; } = 0;

        [DefaultValue(0)]
		public double TimeLoopedRev { get; set; } = 0;

		[DefaultValue(0)]
		public double TimeEnabled { get; set; } = 0;

		[DefaultValue(0)]
		public double EnabledTime { get; set; } = 0;

		[DefaultValue("0 0 0 0 0 0 1 1 1 1")]
		public Transform DisabledTransform { get; set; } = new Transform(new Vector3F(0, 0, 0), new QuaternionF(0, 0, 0, 1), new Vector3F(1, 1, 1));

		[DefaultValue("0 0 0 0 0 0 1 1 1 1")]
		public Transform DisabledTransformRev { get; set; } = new Transform(new Vector3F(0, 0, 0), new QuaternionF(0, 0, 0, 1), new Vector3F(1, 1, 1));


		[DefaultValue(false)]
		public bool Loop { get; } = false;

		[DefaultValue(false)]
		public bool Reverse { get; set; } = false;

		[DefaultValue(false)]
		public bool Pos1 { get; set; } = true;

		[DefaultValue(0)]
		public double DefScale { get; set; } = 3;


//		[DefaultValue(0)]
//		public double DefSize { get; set; } = 9;

		[DefaultValue(0)]
		public double Size { get; set; } = 0;

//		[DefaultValue(0)]
//		public double DefMinimum { get; set; } = 0;

		[DefaultValue(0)]
		public double Minimum { get; set; } = 0;

//		[DefaultValue(0)]
//		public int Mode { get; set; } = 0;

//		public double GetTimeLooped()
//		{
//			double res = 0;
//			Component_CurveInSpace.CachedData data = this.GetData();
//			if (data != null)
//			{
//				double size = data.timeRange.Size;
//				Size = size;
//				if (size != 0.0)
//				{
//					double num = this.ParentScene != null ? this.ParentScene.Time : 0.0;
//					res = data.timeRange.Minimum + num % size;
//				}
//			}
//			return res;
//		}

//		public double GetPSTime()
//		{
//			double res = 0;
//			double num = this.ParentScene != null ? this.ParentScene.Time : 0.0;
//			res = DefMinimum + num % DefSize;
//			return res;
//		}


		protected override void OnEnabledInSimulation()
		{
		}

		protected override void OnUpdate(float delta)
		{
//			TimeLooped = GetTimeLooped();
			if (Loop == false && TimeScale != 0)
			{
				if (TimeEnabled >= Size )
				{
					TimeScale = 0;
					Pos1 = Pos1 == true ? false : true;
					Reverse = Reverse == true ? false : true;
					TimeEnabled = 0;
				}
			}

			base.OnUpdate(delta);
		}

		protected override void OnSimulationStep()
		{
		}

        public void Go() 
        {
        	if(TimeScale == 0) {
				EnabledTime = this.ParentScene != null ? this.ParentScene.Time : 0.0;
				if (DefScale != 0)
				{
					TimeScale = DefScale;
				}
			}
        }

		protected override void OnEnabled()
		{
			//TimeScale = 0;
			//Mode = 4;
			//EnabledTime = this.ParentScene != null ? this.ParentScene.Time : 0.0;
			//if(TimeScale == 0 && DefScale != 0) {
			//	TimeScale = DefScale;
			//}
			Go();
			base.OnEnabled();
		}


		public Transform GetTransformBySceneTimeEnabledLooped()
		{
			Component_CurveInSpace.CachedData data = this.GetData();
			if (data != null)
			{
				double size = data.timeRange.Size;
				Size = size;
				if (size != 0.0)
				{
					double num = this.ParentScene != null ? (this.ParentScene.Time - EnabledTime) : 0.0;
					double numrev = this.ParentScene != null ? ((EnabledTime + Size) - this.ParentScene.Time) : 0.0;
					TimeEnabled = num;
					Minimum = data.timeRange.Minimum;
					TimeLooped = Minimum + num % Size;
					TimeLoopedRev = Minimum + numrev % Size;
					if(Reverse == false) {
						return this.GetTransformByTime(TimeLooped);
					} else {
						return this.GetTransformByTime(TimeLoopedRev);
					}
				} else {
					Minimum = 0;
					TimeLooped = 0;
					TimeLoopedRev = 0;														
				}
			} else {
					Minimum = 0;
					TimeLooped = 0;
					TimeLoopedRev = 0;				
			}
			if(Pos1 == false) {
				return (Transform)DisabledTransform;
			} else {
				return (Transform)DisabledTransformRev;
			}
		}


	}
}