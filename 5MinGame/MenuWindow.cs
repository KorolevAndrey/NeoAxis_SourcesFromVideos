using System;
using System.Collections.Generic;
using NeoAxis;

namespace Project
{
	public class MenuWindow : UIWindow
	{

		private int CurOp = 0; 
		
		
		protected override void OnEnabledInSimulation()
		{
			if( Components[ "Button Close" ] != null )
				( (UIButton)Components[ "Button Close" ] ).Click += ButtonClose_Click;

			if( Components[ "Button Scenes" ] != null )
				( (UIButton)Components[ "Button Scenes" ] ).Click += ButtonScenes_Click;

			if( Components[ "Button Options" ] != null )
				( (UIButton)Components[ "Button Options" ] ).Click += ButtonOptions_Click;

			if( Components[ "Button Main Menu" ] != null )
				( (UIButton)Components[ "Button Main Menu" ] ).Click += ButtonMainMenu_Click;

			if( Components[ "Button Exit" ] != null )
				( (UIButton)Components[ "Button Exit" ] ).Click += ButtonExit_Click;
				
				
			if( Components[ "Button Save" ] != null )
				( (UIButton)Components[ "Button Save" ] ).Click += ButtonSave_Click;
				
			if( Components[ "Button Load" ] != null )
				( (UIButton)Components[ "Button Load" ] ).Click += ButtonLoad_Click;

            if( ((UIWindow)Components["Window"]).Components[ "OKButton" ] != null )
				( (UIButton)(((UIWindow)Components["Window"]).Components[ "OKButton" ]) ).Click += OKButton_Click;
				
				
		}

		void ButtonClose_Click( UIButton sender )
		{
			Dispose();
		}

		void ButtonScenes_Click( UIButton sender )
		{
			var scenesWindow = ResourceManager.LoadSeparateInstance<UIWindow>( @"Base\UI\Screens\ScenesWindow.ui", false, true );
			if( scenesWindow != null )
			{
				Parent.AddComponent( scenesWindow );

				Dispose();
			}
		}

		void ButtonOptions_Click( UIButton sender )
		{
			var optionsWindow = ResourceManager.LoadSeparateInstance<UIWindow>( @"Base\UI\Screens\OptionsWindow.ui", false, true );
			if( optionsWindow != null )
			{
				Parent.AddComponent( optionsWindow );

				Dispose();
			}
		}

		void ButtonMainMenu_Click( UIButton sender )
		{
			SimulationApp.ChangeUIScreen( @"Base\UI\Screens\MainMenuScreen.ui" );
		}

		void ButtonExit_Click( UIButton sender )
		{
			EngineApp.NeedExit = true;
		}
		
		
		
		
		
		void ButtonSave_Click( UIButton sender )
		{
			ScreenMessages.Add("Save");
			((UIWindow)Components["Window"]).Visible = true;
			CurOp = 2;
		}
		
		void ButtonLoad_Click( UIButton sender )
		{
			ScreenMessages.Add("Load");
			((UIWindow)Components["Window"]).Visible = true;
			CurOp = 1;
		}
		
		
		
		void OKButton_Click( UIButton sender )
		{
			
			((UIWindow)Components["Window"]).Visible = false;
			var filename = ((UIEdit)(((UIWindow)Components["Window"]).Components["FileNameEdit"])).Text;			
			
			var uips = SimulationApp.CurrentUIScreen as Project.PlayScreen;
		    var ui = uips.UIControl as Project.File;
		    
		    if(CurOp == 1) {
				ui.LoadGame(filename);
			} else if(CurOp == 2) {
				ui.SaveGame(filename);
			}
			
			ScreenMessages.Add("OK, filename = " + filename);
			
			Dispose();
		}
		
		
		
		
	}
}