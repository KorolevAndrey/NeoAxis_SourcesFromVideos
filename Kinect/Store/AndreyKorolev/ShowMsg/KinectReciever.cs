using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using NeoAxis;
using NeoAxis.Editor;
using Microsoft.Kinect;

namespace Project
{
	public class KinectReciever : NeoAxis.Component
	{
		public KinectSensor myKinect;
		public bool isOK = false;
		public event Action<int,int, Skeleton> OnKinect1;
		public event Action<int,int, Skeleton> OnKinect2;
		public event Action<int,int, Skeleton> OnKinect3;
		public event Action<int,int, Skeleton> OnKinect4;
		public event Action OnKinectInitOK;
		public event Action<string> OnKinectInitError;
		
		
		protected override void OnEnabledInSimulation()
		{
			if(KinectSensor.KinectSensors.Count > 0) {
				myKinect = KinectSensor.KinectSensors[0];
				
				try {
					myKinect.SkeletonStream.Enable();
					myKinect.SkeletonFrameReady += MyKinect_SkeletonFrameReady;
					myKinect.Start();
					isOK = true;
					OnKinectInitOK?.Invoke();
				} catch(Exception e) {
					OnKinectInitError?.Invoke("Error " + e.Message);
				}
			} else {
				OnKinectInitError?.Invoke("KinectSensors.Count = 0");
			}
		}
		
        private void MyKinect_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
        	try {
				/*canvSkel.Children.Clear();*/
				//string msg = "";
				//string msgC = "";
				Skeleton[] skels = null;

				using (SkeletonFrame frameSkeleton = e.OpenSkeletonFrame())
				{
					if (frameSkeleton != null)
					{
						skels = new Skeleton[frameSkeleton.SkeletonArrayLength];
						frameSkeleton.CopySkeletonDataTo(skels);
					}
				}

				if (skels == null) return;
				/*
				Color[] skelcol = new Color[16];
				skelcol[0] = Colors.Blue;
				skelcol[1] = Colors.Green;
				skelcol[2] = Colors.Red;
				skelcol[3] = Colors.Yellow;
				skelcol[4] = Colors.Pink;
				skelcol[5] = Colors.Blue;
				skelcol[6] = Colors.Green;
				skelcol[7] = Colors.Red;
				skelcol[8] = Colors.Yellow;
				skelcol[9] = Colors.Pink;
				skelcol[10] = Colors.Blue;
				skelcol[11] = Colors.Green;
				skelcol[12] = Colors.Red;
				skelcol[13] = Colors.Yellow;
				skelcol[14] = Colors.Pink;
				skelcol[15] = Colors.Blue;
				*/
				int sk_indx = 0;
				int sk_indx1 = 0;
				int sk_cl = 0;

				foreach (Skeleton skel in skels)
				{
					sk_indx = sk_indx + 1;
					if (skel.TrackingState == SkeletonTrackingState.Tracked)
					{
						sk_indx1 = sk_indx1 + 1;
						if (skel.ClippedEdges == 0)
						{
							sk_cl = 0;
						}
						else
						{
							sk_cl = 1;
						}

						//Joint[] jsp = new Joint[4];
						//jsp[0] = skel.Joints[JointType.Head];
						if (sk_indx1 == 1)
						{
							OnKinect1?.Invoke(sk_indx, sk_cl, skel);
						}
						else if (sk_indx1 == 2)
						{
							OnKinect2?.Invoke(sk_indx, sk_cl, skel);
						}
						else if (sk_indx1 == 3)
						{
							OnKinect3?.Invoke(sk_indx, sk_cl, skel);
						}
						else if (sk_indx1 == 4)
						{
							OnKinect4?.Invoke(sk_indx, sk_cl, skel);
						}
					}
				}
			} catch(Exception ex) {
				OnKinectInitError("Error " + ex.Message);
			}
		}
		
		protected override void OnUpdate( float delta )
		{
		}
		
		protected override void OnSimulationStep()
		{
		}

        public void DisposeKinect() {
        	if(myKinect != null) {
				isOK = false;								
				myKinect.Dispose();
			}
		}


        protected override void OnDispose()
        {
        	DisposeKinect();
            base.OnDispose();
        }
    }
}