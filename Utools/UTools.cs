using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using NeoAxis;
using NeoAxis.Editor;

namespace Project
{
	public static class UTools 
	{


		public static void DrawSphere(Viewport viewport, Vector3 pos, double sp_size) 
		{
			viewport.Simple3DRenderer.SetColor(new ColorValue(1, 0, 0));
			viewport.Simple3DRenderer.AddSphere(pos, sp_size, 32, false, 0);
		}

		public static void ShowBox(Viewport viewport) 
		{
			// Find object by the cursor. 
			var obj = GetObjectByCursor(viewport);

			// Draw selection border.
			if (obj != null)
			{
				viewport.Simple3DRenderer.SetColor(new ColorValue(1, 1, 0));
				viewport.Simple3DRenderer.AddBounds(obj.SpaceBounds.CalculatedBoundingBox);
			}
		}

		public static void ShowBoxAroundObject(Component_ObjectInSpace obj, Viewport viewport) 
		{
			// Find object by the cursor. 
			//var obj = GetObjectByCursor(viewport);

			// Draw selection border.
			if (obj != null)
			{
				viewport.Simple3DRenderer.SetColor(new ColorValue(1, 1, 0));
				viewport.Simple3DRenderer.AddBounds(obj.SpaceBounds.CalculatedBoundingBox);
			}
		}


        public static List<Component_ObjectInSpace> GetObjectsThanCanBeSelected(bool skipGround)
		{
			var result = new List<Component_ObjectInSpace>();
			foreach (var obj in Component_Scene.First.GetComponents<Component_MeshInSpace>())
			{
				// Skip ground if set skipGround.
				if (skipGround == false) 
				{
					result.Add(obj);
				} 
				else if(skipGround == true && obj.Name != "Ground")
				{
					result.Add(obj);
				}
			}
			return result;
		}

		public static Component_ObjectInSpace GetObjectByCursor(Viewport viewport)
		{
			// Get scene object.
			var scene = Component_Scene.First;

			// Get world ray by cursor position.
			var ray = viewport.CameraSettings.GetRayByScreenCoordinates(viewport.MousePosition);
			var epve3 = ray.GetEndPoint();
			
			// Get objects by the ray.
			var item = new Component_Scene.GetObjectsInSpaceItem(Component_Scene.GetObjectsInSpaceItem.CastTypeEnum.All, null, true, ray);
			scene.GetObjectsInSpace(item);

			// To test by physical objects:
			//scene.PhysicsRayTest()
			//scene.PhysicsContactTest()
			//scene.PhysicsConvexSweepTest()

			var objectsThanCanBeSelected = GetObjectsThanCanBeSelected(true);

			// Process objects.
			foreach (var resultItem in item.Result)
			{
				if (objectsThanCanBeSelected.Contains(resultItem.Object))
				{
					// Found.
					return resultItem.Object;
				}
			}

			return null;
		}


		public static Component_ObjectInSpace GetObjectByCursor(Viewport viewport, out Vector3 epve3)
		{
			// Get scene object.
			var scene = Component_Scene.First;

			// Get world ray by cursor position.
			var ray = viewport.CameraSettings.GetRayByScreenCoordinates(viewport.MousePosition);
			epve3 = ray.GetEndPoint();
			
			// Get objects by the ray.
			var item = new Component_Scene.GetObjectsInSpaceItem(Component_Scene.GetObjectsInSpaceItem.CastTypeEnum.All, null, true, ray);
			scene.GetObjectsInSpace(item);

			// To test by physical objects:
			//scene.PhysicsRayTest()
			//scene.PhysicsContactTest()
			//scene.PhysicsConvexSweepTest()

			var objectsThanCanBeSelected = GetObjectsThanCanBeSelected(true);

			// Process objects.
			foreach (var resultItem in item.Result)
			{
				if (objectsThanCanBeSelected.Contains(resultItem.Object))
				{
					// Found.
					return resultItem.Object;
				}
			}

			return null;
		}


		public static Component_ObjectInSpace GetObjectByCursor(Viewport viewport, out Ray ray, out Vector3? pos)
		{
			// Get scene object.
			var scene = Component_Scene.First;

			// Get world ray by cursor position.
			ray = viewport.CameraSettings.GetRayByScreenCoordinates(viewport.MousePosition);
			pos = null;
			
			// Get objects by the ray.
			var item = new Component_Scene.GetObjectsInSpaceItem(Component_Scene.GetObjectsInSpaceItem.CastTypeEnum.All, null, true, ray);
			scene.GetObjectsInSpace(item);

			// To test by physical objects:
			//scene.PhysicsRayTest()
			//scene.PhysicsContactTest()
			//scene.PhysicsConvexSweepTest()

			var objectsThanCanBeSelected = GetObjectsThanCanBeSelected(true);

			// Process objects.
			foreach (var resultItem in item.Result)
			{
				if (objectsThanCanBeSelected.Contains(resultItem.Object))
				{
					pos = resultItem.Position;
					// Found.
					return resultItem.Object;
				}
			}

			return null;
		}



	}
}