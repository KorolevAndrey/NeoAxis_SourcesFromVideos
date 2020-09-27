using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using NeoAxis;
using NeoAxis.Editor;

namespace Project
{
	public class MouseMove : NeoAxis.Component
	{


		public event Action<NeoAxis.Component_ObjectInSpace> OnObjectClick;
		public Component_InputProcessing parent_inputproc = null;
	    public NeoAxis.Component parent_comp = null;	

		public NeoAxis.Component_ObjectInSpace last_selected_obj = null;
		public Vector3? pos;
		public Ray ray;
		
		
		protected override void OnEnabledInSimulation()
		{
			parent_comp = this.Parent;
			parent_inputproc = parent_comp?.GetComponent<Component_InputProcessing>();
			if (parent_inputproc == null) return;
			
			parent_inputproc.InputMessageEvent += inputProcess;
			Component_Scene.First.RenderEvent += SceneRenderEvent;
		}
		
		protected override void OnUpdate( float delta )
		{
		}
		
		protected override void OnSimulationStep()
		{
		}
		
		private void SceneRenderEvent(Component_Scene scene, Viewport viewport)
		{
			// Find object by the cursor. 
			//var obj = GetObjectByCursor(viewport);

			// Draw selection border.
			if (last_selected_obj != null)
			{
				//	viewport.Simple3DRenderer.SetColor(new ColorValue(0.2, 0.1, 1));
				//	viewport.Simple3DRenderer.AddBounds(last_selected_obj.SpaceBounds.CalculatedBoundingBox, false,  4);
				UTools.ShowBoxAroundObject(last_selected_obj, viewport);
				if(pos != null) {
					UTools.DrawSphere(viewport, (Vector3)pos, 0.01);					
				}
				
			}
		}
		
		
		private void inputProcess(Component_InputProcessing sender, UIControl playScreen, Component_GameMode gameMode, InputMessage message) 
		{
			//mouse down
			var mouseDown = message as InputMessageMouseButtonDown;
			if( mouseDown != null && mouseDown.Button == EMouseButtons.Left )
			{
				last_selected_obj = UTools.GetObjectByCursor(SimulationApp.MainViewport, out ray, out pos);
				if(last_selected_obj != null && pos != null) {
					OnObjectClick?.Invoke(last_selected_obj);
					//ScreenMessages.Add("pos = " + pos.Value.ToString());
					//ScreenMessages.Add("Obj = " + (last_selected_obj.TransformV.Position - pos.Value).ToString());
				}
			}

		}
	}
}