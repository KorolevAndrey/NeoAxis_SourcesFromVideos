// Copyright (C) NeoAxis Group Ltd. 8 Copthall, Roseau Valley, 00152 Commonwealth of Dominica.
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using NeoAxis;

namespace Project
{
	/// <summary>
	/// The class for general management of the project application.
	/// </summary>
	public static class SimulationApp
	{
		static UIControl currentUIScreen;
		static bool firstTick = true;

		public static Dictionary<int, Dictionary<int, Vector3>> arrows = new Dictionary<int, Dictionary<int, Vector3>>();

		//music
		static SoundChannelGroup musicChannelGroup;

		/////////////////////////////////////////

		static double soundVolume = 0.8;
		[EngineConfig]
		public static double SoundVolume
		{
			get { return soundVolume; }
			set
			{
				soundVolume = value;
				if (EngineApp.DefaultSoundChannelGroup != null)
					EngineApp.DefaultSoundChannelGroup.Volume = soundVolume;
			}
		}

		static double musicVolume = 0.8;
		[EngineConfig]
		public static double MusicVolume
		{
			get { return musicVolume; }
			set
			{
				musicVolume = value;
				if (musicChannelGroup != null)
					musicChannelGroup.Volume = musicVolume;
			}
		}

		static string antialiasing = "";
		[EngineConfig]
		public static string Antialiasing
		{
			get { return antialiasing; }
			set { antialiasing = value; }
		}

		[EngineConfig]
		public static bool DisplayViewportStatistics { get; set; }
		[EngineConfig]
		public static Vector2I VideoMode { get; set; }
		[EngineConfig]
		public static bool Fullscreen { get; set; } = true;
		[EngineConfig]
		public static bool VerticalSync { get; set; } = true;
		[EngineConfig]
		public static bool DisplayBackgroundScene { get; set; } = true;

		/////////////////////////////////////////

		public static Viewport MainViewport
		{
			get { return RenderingSystem.ApplicationRenderTarget.Viewports[0]; }
		}

		public static UIControl CurrentUIScreen
		{
			get { return currentUIScreen; }
		}

		public static bool ChangeUIScreen(string fileName)
		{
			//load
			UIControl newScreen = null;
			if (!string.IsNullOrEmpty(fileName))
			{
				newScreen = ResourceManager.LoadSeparateInstance<UIControl>(fileName, false, true);
				if (newScreen == null)
					return false;
			}

			//remove previous
			if (currentUIScreen != null)
			{
				currentUIScreen.RemoveFromParent(false);
				currentUIScreen = null;
			}

			//enable
			if (newScreen != null)
			{
				currentUIScreen = newScreen;
				MainViewport.UIContainer.AddComponent(currentUIScreen);

				currentUIScreen.ResetCreateTime();

				//reset mouse state
				MainViewport.MouseRelativeMode = false;
			}

			return true;
		}

		/////////////////////////////////////////

		public static void EngineApp_AppCreateBefore()
		{
			//register [EngineConfig] fields, properties
			EngineConfig.RegisterClassParameters(typeof(SimulationApp));

			//creation settings
			if (!Fullscreen)
				EngineApp.InitSettings.CreateWindowFullscreen = false;
			if (VideoMode != Vector2I.Zero && (SystemSettings.VideoModeExists(VideoMode) || !Fullscreen))
			{
				EngineApp.InitSettings.CreateWindowSize = VideoMode;
				if (!Fullscreen)
					EngineApp.InitSettings.CreateWindowState = EngineApp.WindowStateEnum.Normal;
			}
			if (!VerticalSync)
				EngineSettings.Init.SimulationVSync = false;
		}

		public static void EngineApp_AppCreateAfter()
		{
			Log.Handlers.InvisibleInfoHandler += InvisibleLog_Handlers_InfoHandler;
			Log.Handlers.InfoHandler += Log_Handlers_InfoHandler;
			Log.Handlers.WarningHandler += Log_Handlers_WarningHandler;
			Log.Handlers.ErrorHandler += Log_Handlers_ErrorHandler;
			Log.Handlers.FatalHandler += Log_Handlers_FatalHandler;

			EngineApp.RegisterConfigParameter += EngineApp_RegisterConfigParameter;

			EngineConsole.Init();
			//!!!!
			//GameControlsManager.Init();

			//UIControl engineLoadingWindow = ResourceManager.LoadSeparateInstance<UIControl>( "Base\\UI\\Windows\\EngineLoadingWindow.ui", false, null );
			//if( engineLoadingWindow != null )
			//	MainViewport.UIContainer.AddComponent( engineLoadingWindow );

			////Subcribe to callbacks during engine loading. We will render scene from callback.
			//LongOperationCallbackManager.Subscribe( LongOperationCallbackManager_LoadingCallback, programLoadingWindow );

			EngineApp.Tick += EngineApp_Tick;

			////finish initialization of materials and hide loading window.
			////!!!!!!
			////LongOperationCallbackManager.Unsubscribe();
			//if( engineLoadingWindow != null )
			//	engineLoadingWindow.RemoveFromParent( true );

			//subscribe to main viewport events
			{
				MainViewport.KeyDown += MainViewport_KeyDown;
				MainViewport.KeyPress += MainViewport_KeyPress;
				MainViewport.KeyUp += MainViewport_KeyUp;
				MainViewport.MouseDown += MainViewport_MouseDown;
				MainViewport.MouseUp += MainViewport_MouseUp;
				MainViewport.MouseDoubleClick += MainViewport_MouseDoubleClick;
				MainViewport.MouseMove += MainViewport_MouseMove;
				MainViewport.MouseWheel += MainViewport_MouseWheel;
				MainViewport.JoystickEvent += MainViewport_JoystickEvent;
				MainViewport.SpecialInputDeviceEvent += MainViewport_SpecialInputDeviceEvent;

				//!!!!!Tick +=

				MainViewport.UpdateBegin += MainViewport_UpdateBegin;
				MainViewport.UpdateBeforeOutput += MainViewport_UpdateBeforeOutput;
				MainViewport.UpdateEnd += MainViewport_UpdateEnd;
			}

			//change application title
			if (EngineApp.CreatedInsideEngineWindow != null)
				EngineApp.CreatedInsideEngineWindow.Title = ProjectSettings.Get.ProjectName;

			//update sound volume
			if (EngineApp.DefaultSoundChannelGroup != null)
				EngineApp.DefaultSoundChannelGroup.Volume = soundVolume;

			//create music channel group
			musicChannelGroup = SoundWorld.CreateChannelGroup("Music");
			if (musicChannelGroup != null)
			{
				SoundWorld.MasterChannelGroup.AddGroup(musicChannelGroup);
				musicChannelGroup.Volume = musicVolume;
			}
		}

		public static void EngineApp_AppDestroy()
		{
		}

		private static void EngineApp_RegisterConfigParameter(EngineConfig.Parameter parameter)
		{
			EngineConsole.RegisterConfigParameter(parameter);
		}

		static bool IsNeedDisableKeyboardAndMouseInput()
		{
			//!!!!
			return false;

			//!!!!
			//return IsScreenFadingOut();
		}

		private static void MainViewport_KeyDown(Viewport viewport, KeyEvent e, ref bool handled)
		{
			//Engine console
			if (EngineConsole.PerformKeyDown(e))
			{
				handled = true;
				return;
			}

			//disable input processing
			if (IsNeedDisableKeyboardAndMouseInput())
			{
				handled = true;
				return;
			}


		}

		private static void MainViewport_KeyPress(Viewport viewport, KeyPressEvent e, ref bool handled)
		{
			//Engine console
			if (EngineConsole.PerformKeyPress(e))
			{
				handled = true;
				return;
			}

			//disable input processing
			if (IsNeedDisableKeyboardAndMouseInput())
			{
				handled = true;
				return;
			}

		}

		private static void MainViewport_KeyUp(Viewport viewport, KeyEvent e, ref bool handled)
		{
			//disable input processing
			if (IsNeedDisableKeyboardAndMouseInput())
			{
				handled = true;
				return;
			}

		}

		private static void MainViewport_MouseDown(Viewport viewport, EMouseButtons button, ref bool handled)
		{
			//disable input processing
			if (IsNeedDisableKeyboardAndMouseInput())
			{
				handled = true;
				return;
			}

		}

		private static void MainViewport_MouseUp(Viewport viewport, EMouseButtons button, ref bool handled)
		{
			//disable input processing
			if (IsNeedDisableKeyboardAndMouseInput())
			{
				handled = true;
				return;
			}

		}

		private static void MainViewport_MouseDoubleClick(Viewport viewport, EMouseButtons button, ref bool handled)
		{
			//disable input processing
			if (IsNeedDisableKeyboardAndMouseInput())
			{
				handled = true;
				return;
			}

		}

		private static void MainViewport_MouseMove(Viewport viewport, Vector2 mouse)//, ref bool handled )
		{
			//disable input processing
			if (IsNeedDisableKeyboardAndMouseInput())
			{
				//!!!!handled

				//handled = true;
				return;
			}

		}

		private static void MainViewport_MouseWheel(Viewport viewport, int delta, ref bool handled)
		{
			//Engine console
			if (EngineConsole.PerformMouseWheel(delta))
			{
				handled = true;
				return;
			}

			//disable input processing
			if (IsNeedDisableKeyboardAndMouseInput())
			{
				handled = true;
				return;
			}

		}

		private static void MainViewport_JoystickEvent(Viewport viewport, NeoAxis.Input.JoystickInputEvent e, ref bool handled)
		{
			//disable input processing
			if (IsNeedDisableKeyboardAndMouseInput())
			{
				handled = true;
				return;
			}

		}

		private static void MainViewport_SpecialInputDeviceEvent(Viewport viewport, NeoAxis.Input.InputEvent e, ref bool handled)
		{
			//disable input processing
			if (IsNeedDisableKeyboardAndMouseInput())
			{
				handled = true;
				return;
			}

		}

		private static void EngineApp_Tick(float delta)
		{
			// Perform update viewport, attached scene, UI container.
			MainViewport.PerformTick(delta);

			// Process screen messages.
			ScreenMessages.PerformTick(delta);

			//engine console
			EngineConsole.PerformTick(delta);

			if (firstTick)
				FirstTickActions();

			firstTick = false;
		}

		private static void MainViewport_UpdateBegin(Viewport viewport)
		{
		}

		static void MainViewport_RenderUI()
		{
			//configure cursor file name
			EngineApp.SystemCursorFileName = "Base\\UI\\Cursors\\DefaultSystem.cur";

			//!!!!
			//Draw UI controls
			MainViewport.UIContainer.PerformRenderUI(MainViewport.CanvasRenderer);

			// Process screen messages.
			ScreenMessages.PerformRenderUI(MainViewport);

			//viewport statistics
			if (DisplayViewportStatistics)
			{

				var statistics = MainViewport.RenderingContext?.UpdateStatisticsPrevious;
				if (statistics != null)
				{
					var lines = new List<string>();
					lines.Add("FPS: " + statistics.FPS.ToString("F1"));
					lines.Add("Triangles: " + statistics.Triangles.ToString());
					lines.Add("Lines: " + statistics.Lines.ToString());
					lines.Add("Draw calls: " + statistics.DrawCalls.ToString());
					lines.Add("Render targets: " + statistics.RenderTargets.ToString());
					lines.Add("Dynamic textures: " + statistics.DynamicTextures.ToString());
					lines.Add("Lights: " + statistics.Lights.ToString());
					lines.Add("Reflection probes: " + statistics.ReflectionProbes.ToString());

					var renderer = MainViewport.CanvasRenderer;
					var fontSize = renderer.DefaultFontSize;
					var offset = new Vector2(fontSize * renderer.AspectRatioInv * 0.8, fontSize * 0.6);

					CanvasRendererUtility.AddTextLinesWithShadow(MainViewport, null, fontSize, lines, new Rectangle(offset.X, offset.Y, 1, 1), EHorizontalAlignment.Left, EVerticalAlignment.Top, new ColorValue(1, 1, 1));
				}
			}

			//Engine console
			EngineConsole.PerformRenderUI();
		}

		private static void MainViewport_UpdateBeforeOutput(Viewport viewport)
		{

			if (EngineApp.ApplicationType == EngineApp.ApplicationTypeEnum.Simulation)
			{
				var renderer = viewport.Simple3DRenderer;
				drawAllArs(renderer);
			}
			//			if(Component_Scene.All != null && Component_Scene.All.Length > 0) {
			//				var msh = Component_Scene.All[0]?.GetComponent<Component_Character>("Character");
			//				var sk = Component_Scene.All[0]?.GetComponent<Component_Skeleton>("Skeleton");

			//				if (msh != null && sk != null)
			//				{
			//					var bones = sk?.GetBones(true);
			//					if (bones != null)
			//					{

			//						var renderer = viewport.Simple3DRenderer;
			//					    renderer.SetColor(new ColorValue(1, 0, 0));

			//						var startPosition = msh.TransformV.Position;//+ new Vector3(3,0,0.1);
			//						var rootbone = Array.Find(bones, item => item.Name == "Root");

			//        	            var comps = rootbone?.GetComponents<Component_SkeletonBone>();
			//						drawBones(comps, renderer, startPosition, new Vector3(0,0,0));
			//					}
			//				}
			//			}

			MainViewport_RenderUI();
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


		private static void drawAllArs(Simple3DRenderer renderer)
		{
			if(arrows.Count > 0) {
			for (int i = 0; i < arrows.Count; i = i + 1)
			{
				renderer.SetColor(new ColorValue(arrows[i][0].X, arrows[i][0].Y, arrows[i][0].Z));
				drawAr(renderer, arrows[i][1], arrows[i][2]);
			}
			}
		}


		private static void drawAr(Simple3DRenderer renderer, Vector3 startPosition, Vector3 endPosition)
		{
			renderer.AddArrow(startPosition, endPosition, 0, 0, true);
		}


		private static void MainViewport_UpdateEnd(Viewport viewport)
		{
		}

		//!!!!!
		//protected override void OnSystemPause( bool pause )
		//{
		//	base.OnSystemPause( pause );

		//	if( EntitySystemWorld.Instance != null )
		//		EntitySystemWorld.Instance.SystemPauseOfSimulation = pause;
		//}

		////!!!!!
		//bool IsScreenFadingOut()
		//{
		//	//!!!!!
		//	//if( needMapLoadName != null || needRunExampleOfProceduralMapCreation || needWorldLoadName != null )
		//	//	return true;
		//	//if( needFadingOutAndExit )
		//	//	return true;
		//	return false;
		//}

		static void InvisibleLog_Handlers_InfoHandler(string text, ref bool dumpToLogFile)
		{
			//EngineConsole.Print( text );
		}

		static void Log_Handlers_InfoHandler(string text, ref bool dumpToLogFile)
		{
			EngineConsole.Print(text);
		}

		static void Log_Handlers_WarningHandler(string text, ref bool handled, ref bool dumpToLogFile)
		{
			handled = true;
			EngineConsole.Print("Warning: " + text, new ColorValue(1, 0, 0));
			if (EngineConsole.AutoOpening)
				EngineConsole.Active = true;
		}

		static void Log_Handlers_ErrorHandler(string text, ref bool handled, ref bool dumpToLogFile)
		{
			handled = true;
			EngineConsole.Print("Error: " + text, new ColorValue(1, 0, 0));
			if (EngineConsole.AutoOpening)
				EngineConsole.Active = true;

			//if( MainViewport != null && MainViewport.UIContainer != null )
			//{
			//	handled = true;

			//	//!!!!!
			//	//find already created MessageBoxWindow
			//	foreach( UIControl control in MainViewport.UIContainer.GetComponents<UIControl>( false ) )
			//	{
			//		if( control is MessageBoxWindow && !control.RemoveFromParentQueued )
			//			return;
			//	}

			//	//!!!!!
			//	bool insideTheGame = false;
			//	//bool insideTheGame = GameWindow.Instance != null;

			//	//!!!!!
			//	//if( insideTheGame )
			//	//{
			//	//	if( Map.Instance != null )
			//	//	{
			//	//		if( EntitySystemWorld.Instance.IsServer() || EntitySystemWorld.Instance.IsSingle() )
			//	//			EntitySystemWorld.Instance.Simulation = false;
			//	//	}

			//	//	EngineApp.Instance.MouseRelativeMode = false;

			//	//	DeleteAllGameWindows();

			//	//	MapSystemWorld.MapDestroy();
			//	//	if( EntitySystemWorld.Instance != null )
			//	//		EntitySystemWorld.Instance.WorldDestroy();
			//	//}

			//	//!!!!!
			//	//GameEngineApp.Instance.Server_DestroyServer( "Error on the server" );
			//	//GameEngineApp.Instance.Client_DisconnectFromServer();

			//	//show message box

			//	MessageBoxWindow messageBoxWindow = new MessageBoxWindow( text, "Error", delegate ( UIButton sender )
			//	{
			//		if( insideTheGame )
			//		{
			//			//close all windows
			//			foreach( UIControl control in MainViewport.UIContainer.GetComponents<UIControl>( false ) )
			//				control.RemoveFromParent( true );
			//		}
			//		else
			//		{
			//			//!!!!!
			//			////destroy Lobby Window
			//			//foreach( UIControl control in MainViewport.ControlManager.Controls )
			//			//{
			//			//	if( control is MultiplayerLobbyWindow )
			//			//	{
			//			//		control.SetShouldDetach();
			//			//		break;
			//			//	}
			//			//}
			//		}

			//		//!!!!!
			//		//if( EntitySystemWorld.Instance == null )
			//		//{
			//		//	EngineApp.Instance.NeedExit = true;
			//		//	return;
			//		//}

			//		//!!!!
			//		////create main menu
			//		//if( MainMenuWindow.Instance == null )
			//		//	MainViewport.UIContainer.AddComponent( new MainMenuWindow() );

			//	} );

			//	MainViewport.UIContainer.AddComponent( messageBoxWindow );
			//}
		}

		static void Log_Handlers_FatalHandler(string text, string createdLogFilePath, ref bool handled)
		{
			//if( MainViewport != null && MainViewport.UIContainer != null )
			//{
			//	//find already created MessageBoxWindow
			//	foreach( UIControl control in MainViewport.UIContainer.GetComponents<UIControl>( false ) )
			//	{
			//		if( control is MessageBoxWindow && !control.RemoveFromParentQueued )
			//		{
			//			handled = true;
			//			return;
			//		}
			//	}
			//}
		}

		static void FirstTickActions()
		{
			string playFile = "";
			if (SystemSettings.CommandLineParameters.TryGetValue("-play", out playFile))
			{
				try
				{
					if (Path.IsPathRooted(playFile))
						playFile = VirtualPathUtility.GetVirtualPathByReal(playFile);
				}
				catch { }
			}

			if (!string.IsNullOrEmpty(playFile) && VirtualFile.Exists(playFile))
			{
				//load Play screen
				if (ChangeUIScreen(@"Base\UI\Screens\PlayScreen.ui"))
				{
					var playScreen = CurrentUIScreen as PlayScreen;
					if (playScreen != null)
					{
						playScreen.Load(playFile, true);
						playScreen.ResetCreateTime();
					}
				}
			}
			else
			{
				//default start screen
				if (!string.IsNullOrEmpty(ProjectSettings.Get.InitialUIScreen.GetByReference))
					ChangeUIScreen(ProjectSettings.Get.InitialUIScreen.GetByReference);
			}
		}



















		//
		//https://gist.github.com/aeroson/043001ca12fe29ee911e
		//
		public static double sqrMagnitude3(Vector3 vec)
		{
			return vec.X * vec.X + vec.Y * vec.Y + vec.Z * vec.Z;
		}

		public static double sqrMagnitude4(Quaternion vec)
		{
			return vec.X * vec.X + vec.Y * vec.Y + vec.Z * vec.Z + vec.W * vec.W;
		}


		public static Quaternion Inverse(Quaternion rotation)
		{
			var lengthSq = sqrMagnitude4(rotation);
			if (lengthSq != 0.0)
			{
				double i = 1.0 / lengthSq;
				return new Quaternion(rotation.X * -i, rotation.Y * -i, rotation.Z * -i, rotation.W * i);
			}
			return rotation;
		}

		public static Quaternion AngleAxis(float degress, Vector3 axis)
		{
			if (sqrMagnitude3(axis) == 0.0)
				return Quaternion.Identity;

			Quaternion result = Quaternion.Identity;
			var radians = degress * (Math.PI / 180);
			radians *= 0.5f;
			axis.Normalize();
			axis = axis * System.Math.Sin(radians);
			result.X = axis.X;
			result.Y = axis.Y;
			result.Z = axis.Z;
			result.W = System.Math.Cos(radians);

			return result.GetNormalize();
		}


		//https://answers.unity.com/questions/489350/rotatearound-without-transform.html
		// function RotateAround(center: Vector3, axis: Vector3, angle: float){
		//   var pos: Vector3 = transform.position;
		//   var rot: Quaternion = Quaternion.AngleAxis(angle, axis); // get the desired rotation
		//   var dir: Vector3 = pos - center; // find current direction relative to center
		//   dir = rot * dir; // rotate the direction
		//   transform.position = center + dir; // define new position
		//   // rotate object to keep looking at the center:
		//   var myRot: Quaternion = transform.rotation;
		//   transform.rotation *= Quaternion.Inverse(myRot) * rot * myRot;
		// }

		public static Transform RotateAround(Transform transform, Vector3 center, Vector3 axis, float angle, bool lookCenter = true)
		{
			Vector3 pos = transform.Position;
			Quaternion rot = AngleAxis(angle, axis); // get the desired rotation
			Vector3 dir = pos - center; // find current direction relative to center
			dir = rot * dir; // rotate the direction

			var respos = center + dir; // define new position

			var myRot = transform.Rotation;
			var resrot = transform.Rotation * Inverse(myRot) * rot * myRot; // rotate object to keep looking at the center

			Transform res = new Transform();
			if (lookCenter)
			{
				res = new Transform(respos, resrot, transform.Scale);
			}
			else
			{
				res = new Transform(respos, transform.Rotation, transform.Scale);
			}
			return res;
		}

























	}
}