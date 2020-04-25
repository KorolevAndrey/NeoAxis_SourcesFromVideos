using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using NeoAxis;
using NeoAxis.Editor;

namespace Project
{
	public class MyClass2 : NeoAxis.Component_MeshInSpaceAnimationController
	{
		protected override void OnEnabledInSimulation()
		{
		}

		protected override void OnUpdate(float delta)
		{
		}

		protected override void OnSimulationStep()
		{
		}

		protected override void CalculateCPU(Component_Skeleton skeleton, Component_Mesh originalMesh, Component_Mesh modifiableMesh)
		{
			if (EngineApp.ApplicationType == EngineApp.ApplicationTypeEnum.Simulation)
			{

				if (Component_Scene.All != null && Component_Scene.All.Length > 0)
				{
//					Component_MeshInSpace[] sphs = new Component_MeshInSpace[2];
//					Component_MeshInSpace sph = Component_Scene.All[0]?.GetComponent<Component_MeshInSpace>("chest");
//					sphs[0] = sph?.GetComponent<Component_MeshInSpace>("upper_arm.R");
//					sphs[1] = sphs[0]?.GetComponent<Component_MeshInSpace>("forearm.R");

//					var bones = skeleton?.GetBones(false);
//					if (bones != null)
//					{
//						var sR = Array.Find(bones, item => item.Name == "upper_arm.R");
//						sR.Transform = new Transform(sphs[0].TransformV.Position, sphs[0].TransformV.Rotation, sR.Transform.Value.Scale);
//						var sR1 = Array.Find(bones, item => item.Name == "forearm.R");
//						sR1.Transform = new Transform(sphs[1].TransformV.Position, sphs[1].TransformV.Rotation, sR1.Transform.Value.Scale);
						
//					}

                    Component_MeshInSpace sph = Component_Scene.All[0]?.GetComponent<Component_MeshInSpace>("chest");
                    var sphch = sph.GetComponents<Component_MeshInSpace>(false, true, false);
                    
                    var bones = skeleton?.GetBones(false);                    
					if (bones != null)
					{
						var sR = Array.Find(bones, item => item.Name == "chest");
						sR.Transform = new Transform(sph.TransformV.Position, sph.TransformV.Rotation, sR.Transform.Value.Scale);
						var chB = sR.GetComponents<Component_SkeletonBone>(false, true, false);
						
						for (int i = 0; i < chB.Length;i = i + 1) 
						{
							var cursph = Array.Find(sphch, item => item.Name == chB[i].Name);
							if(cursph != null) {
								chB[i].Transform = new Transform(cursph.TransformV.Position, cursph.TransformV.Rotation, chB[i].Transform.Value.Scale);
							}
						}
						
					}

				}

				MainViewport_UpdateBeforeOutput(Project.SimulationApp.MainViewport, skeleton);
				base.CalculateCPU(skeleton, originalMesh, modifiableMesh);
			}
		}


		private static void MainViewport_UpdateBeforeOutput(Viewport viewport, Component_Skeleton skeleton)
		{
			if (Component_Scene.All != null && Component_Scene.All.Length > 0)
			{				
				var sk = skeleton;

				if (sk != null)
				{
					var bones = sk?.GetBones(false);
					if (bones != null)
					{

						var renderer = viewport.Simple3DRenderer;
						renderer.SetColor(new ColorValue(0, 0, 1));

						var startPosition = new Vector3(0, 0, 0);//msh.TransformV.Position;//+ new Vector3(3,0,0);
						var rootbone = Array.Find(bones, item => item.Name == "Root");

						var comps = rootbone?.GetComponents<Component_SkeletonBone>();
						drawBones(comps, renderer, startPosition, new Vector3(0, 0, 0));
					}
				}
			}

		}


		private static void drawBones(Component_SkeletonBone[] bones, Simple3DRenderer renderer, Vector3 iniPosition, Vector3 startPosition)
		{
			Vector3 lendPosition;
			Component_SkeletonBone comp;

			for (var i = 0; i < bones?.Length; i = i + 1)
			{
				comp = bones[i];

				lendPosition = comp.Transform.Value.Position;
				drawAr(renderer, iniPosition + startPosition, iniPosition + lendPosition);
				if (comp.GetComponents().Length > 0)
				{
					drawBones(comp.GetComponents<Component_SkeletonBone>(), renderer, iniPosition, lendPosition);
				}
			}
		}

		private static void drawAr(Simple3DRenderer renderer, Vector3 startPosition, Vector3 endPosition)
		{
			renderer.AddArrow(startPosition, endPosition, 0, 0, true);
		}

	}
}