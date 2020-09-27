using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using NeoAxis;
using NeoAxis.Editor;

namespace Project
{
	public class DroppableObject : Component_MeshInSpace ,IDroppable
	{
		public bool CanDrop() {
			return true;
		}
	}
}