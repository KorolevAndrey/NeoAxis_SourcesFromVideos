using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Linq;
using NeoAxis;
using NeoAxis.Editor;

namespace NeoAxis
{

    [AddToResourcesWindow( @"Base\Game framework\Simple Item", -8000 )]
	[NewObjectDefaultName( "Simple Item" )]
	[EditorSettingsCell( typeof( Component_SimpleItemInSpace_SettingsCell ) )]
	public class Component_SimpleItemInSpace : NeoAxis.Component_MeshInSpace, IComponent_InteractiveObject
	{

		[DefaultValue( true )]
		public Reference<bool> AllowInteract
		{
			get { if( _allowInteract.BeginGet() ) AllowInteract = _allowInteract.Get( this ); return _allowInteract.value; }
			set { if( _allowInteract.BeginSet( ref value ) ) { try { AllowInteractChanged?.Invoke( this ); } finally { _allowInteract.EndSet(); } } }
		}
		/// <summary>Occurs when the <see cref="AllowInteract"/> property value changes.</summary>
		public event Action<Component_SimpleItemInSpace> AllowInteractChanged;
		ReferenceField<bool> _allowInteract = true;



        protected override void OnEnabledInSimulation()
        {
			log("Init Ok");
        	
            base.OnEnabledInSimulation();
        }

		public override void NewObjectSetDefaultConfiguration( bool createdFromNewObjectWindow = false )
		{
			if (Components.Count == 0)
			{
				//Mesh
				{
					var mesh = CreateComponent<Component_Mesh>();
					mesh.Name = "Mesh";
					Mesh = ReferenceUtility.MakeThisReference(this, mesh);

					var geometry = mesh.CreateComponent<Component_MeshGeometry_Box>();
					geometry.Name = "Mesh Geometry";
					geometry.Dimensions = new Vector3(0.3, 0.3, 0.3);
					geometry.Material = ReferenceUtility.MakeReference("Base\\Materials\\White.material");
				}

			}
		}

		
		protected override void OnUpdate( float delta )
		{
			base.OnUpdate( delta );

            if( EngineApp.ApplicationType == EngineApp.ApplicationTypeEnum.Simulation ) {
				//...
			}

			//if( EngineApp.ApplicationType == EngineApp.ApplicationTypeEnum.Editor )
		}

		protected override void OnSimulationStep()
		{
			base.OnSimulationStep();
		}
		
		
		public virtual bool ObjectInteractionInputMessage( UIControl playScreen, Component_GameMode gameMode, InputMessage message )
		{
			var mouseDown = message as InputMessageMouseButtonDown;
			if( mouseDown != null )
			{
				if( mouseDown.Button == EMouseButtons.Left || mouseDown.Button == EMouseButtons.Right )
				{
					//ClickingBegin();
					
					log("InteractionInputMessage");
					return true;
				}
			}

			return false;
		}

		public void ObjectInteractionGetInfo( UIControl playScreen, Component_GameMode gameMode, ref IComponent_InteractiveObject_ObjectInfo info )
		{
			info = new IComponent_InteractiveObject_ObjectInfo();
			info.AllowInteract = AllowInteract;
			//info.DisplaySelectionRectangle = true;
			info.SelectionTextInfo.Add( "Test message = " + Name );
		}

		public void ObjectInteractionEnter( Component_GameMode.ObjectInteractionContextClass context )
		{
		}

		public void ObjectInteractionExit( Component_GameMode.ObjectInteractionContextClass context )
		{
		}

		public void ObjectInteractionUpdate( Component_GameMode.ObjectInteractionContextClass context )
		{
		}

		protected override bool OnSpaceBoundsUpdateIncludeChildren()
		{
			return true;
		}


    }
}