public bool shipwas = false;

public void Sensor_ObjectEnter(NeoAxis.Component_Sensor sender, NeoAxis.Component_ObjectInSpace obj)
{
	if(!shipwas && obj.Name == "Mesh in Space 25") {
		shipwas = true;
		var p = Component_Scene.All[0];
		var cam = p.GetComponent<Component_Camera>("Camera");
		p.CameraDefault = cam;
	}
}
