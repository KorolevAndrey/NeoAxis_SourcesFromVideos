using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using NeoAxis;
using NeoAxis.Editor;

namespace Project
{
	public static class MyClass
	{
		public static void ShowMsg( string msg )
		{
			var sc = Component_Scene.First;
			var txtcomp = (Component_Text2D)sc?.GetComponentByPath("Text 2D");
			if (txtcomp == null) { ScreenMessages.Add("No Text 2D"); return; }

			txtcomp.Text = msg;
			
		}	
	}
}