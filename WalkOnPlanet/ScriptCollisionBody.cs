public void CollisionBody_SimulationStep(NeoAxis.Component obj)
{
	var p = Component_Scene.All[0];
	Vector3 dir;
	if(p != null) {
		var cap = p.GetComponent<Component_MeshInSpace>("Box 4");
		var plnt = p.GetComponent<Component_MeshInSpace>("Sphere");
		if(cap != null && plnt != null) {
			var cb = cap.GetComponent<Component_RigidBody>("Collision Body");
			if(cb != null) {
				
			    dir = new Vector3(cb.TransformV.Position.X, cb.TransformV.Position.Y, cb.TransformV.Position.Z).GetNormalize();
			    cb.ApplyForce(dir * -100, plnt.TransformV.Position);
			    
			}
		}
	}
}
