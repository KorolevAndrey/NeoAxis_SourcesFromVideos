using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using NeoAxis;
using NeoAxis.Editor;

namespace NeoAxis.Editor
{
	public class Component_SimpleItemInSpace_SettingsCell : NeoAxis.Editor.SettingsCellProcedureUI
	{

		ProcedureUI.Button buttonClick;

		//

		protected override void OnInitUI()
		{
			buttonClick = ProcedureForm.CreateButton( "Click" );
			buttonClick.Click += ButtonClick_Click;

			ProcedureForm.AddRow( new ProcedureUI.Control[] { buttonClick } );
		}

		private void ButtonClick_Click( ProcedureUI.Button sender )
		{
			//foreach( var button in GetObjects<Component_ButtonInSpace>() )
			//	button.ClickingBegin();
			Log.Info("ButtonClick_Click");
		}


	}
}