public double angl = 0;

public void InputProcessing_SimulationStep(NeoAxis.Component obj)
{
	
	var sender = (NeoAxis.Component_InputProcessing)obj;
	var dir = new Vector3(0,0,0);
	
	var p = Component_Scene.All[0];
	if(p != null) {
		var cap = p.GetComponent<Component_MeshInSpace>("Capsule");
		var plnt = p.GetComponent<Component_MeshInSpace>("Sphere");
		if (cap != null && plnt != null)
		{

            var cb = cap.GetComponent<Component_RigidBody>("Collision Body");
            var trl = cap.GetComponent<Component_TransformOffset>("Transform Offset left");
            var trr = cap.GetComponent<Component_TransformOffset>("Transform Offset right");
			if(cb != null && trl != null && trr != null) {
				
				//fly front
				if (sender.IsKeyPressed(EKeys.Space))
				{
					dir = new Vector3(cb.TransformV.Position.X, cb.TransformV.Position.Y, cb.TransformV.Position.Z).GetNormalize();
					cb.ApplyForce(dir * 120.0, plnt.TransformV.Position);
					ScreenMessages.Add("W");
				}
				
				
				//fly front
				if (sender.IsKeyPressed(EKeys.W) || sender.IsKeyPressed(EKeys.Up) || sender.IsKeyPressed(EKeys.NumPad8))
				{					
			        dir = cb.TransformV.Rotation.GetForward().GetNormalize();
			        cb.ApplyForce(dir * 55.0, plnt.TransformV.Position);

					ScreenMessages.Add("W");
				}
		        //fly back
         		if (sender.IsKeyPressed(EKeys.S) || sender.IsKeyPressed(EKeys.Down) || sender.IsKeyPressed(EKeys.NumPad2))
        		{
			        dir = cb.TransformV.Rotation.GetForward().GetNormalize();
			        cb.ApplyForce(dir * -55.0, plnt.TransformV.Position);

			        ScreenMessages.Add("S");
        		}

		        //turn left
		        if (sender.IsKeyPressed(EKeys.A) || sender.IsKeyPressed(EKeys.Left) || sender.IsKeyPressed(EKeys.NumPad4)) {

					var tr = cb.TransformV;
					cb.TransformV = tr.UpdateRotation(trr.Result.Rotation);		        	
		        	
			    	ScreenMessages.Add("A");
                }
				

		        //turn right
		        if (sender.IsKeyPressed(EKeys.D) || sender.IsKeyPressed(EKeys.Right) || sender.IsKeyPressed(EKeys.NumPad6)) {

                    
					var tr = cb.TransformV;
					cb.TransformV = tr.UpdateRotation(trl.Result.Rotation);		        	
		        	
					ScreenMessages.Add("D");        
		        }

			}
			

		}
	}
   
}
