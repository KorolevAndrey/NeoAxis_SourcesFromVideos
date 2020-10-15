using System;
using System.ComponentModel;
using System.Collections.Generic;
using NeoAxis.Editor;

namespace NeoAxis
{
	public class MyClass : NeoAxis.Component_MeshModifier
	{
		[DefaultValue( 0 )]
		[Range( 0, 2 )]
		public double Power { get; set; } = 0;
		
		[DefaultValue( "1 1 1" )]
		public ColorValue Color { get; set; } = new ColorValue( 1, 1, 1 );
		
		protected override void OnEnabledInSimulation()
		{
		}
		
		protected override void OnUpdate( float delta )
		{
		}
		
		protected override void OnSimulationStep()
		{
		}
		
		



		[DefaultValue( 0.1 )]
		[Range( 0, 2, RangeAttribute.ConvenientDistributionEnum.Exponential )]
		public Reference<double> Multiplier1
		{
			get { if( _multiplier1.BeginGet() ) Multiplier1 = _multiplier1.Get( this ); return _multiplier1.value; }
			set
			{
				//if( value < 0 )
				//	value = new Reference<double>( 0, value.GetByReference );
				if( _multiplier1.BeginSet( ref value ) )
				{
					try
					{
						Multiplier1Changed?.Invoke( this );
						ShouldRecompileMesh();
					}
					finally { _multiplier1.EndSet(); }
				}
			}
		}
		/// <summary>Occurs when the <see cref="Multiplier"/> property value changes.</summary>
		public event Action<MyClass> Multiplier1Changed;
		ReferenceField<double> _multiplier1 = 0.1;









		[DefaultValue( 0.1 )]
		[Range( 0, 2, RangeAttribute.ConvenientDistributionEnum.Exponential )]
		public Reference<double> Multiplier
		{
			get { if( _multiplier.BeginGet() ) Multiplier = _multiplier.Get( this ); return _multiplier.value; }
			set
			{
				//if( value < 0 )
				//	value = new Reference<double>( 0, value.GetByReference );
				if( _multiplier.BeginSet( ref value ) )
				{
					try
					{
						MultiplierChanged?.Invoke( this );
						ShouldRecompileMesh();
					}
					finally { _multiplier.EndSet(); }
				}
			}
		}
		/// <summary>Occurs when the <see cref="Multiplier"/> property value changes.</summary>
		public event Action<MyClass> MultiplierChanged;
		ReferenceField<double> _multiplier = 0.1;

		/////////////////////////////////////////

		void ProcessVertex( float multiplier, float multiplier1, float invMultiplier, ref Vector3F position, out Vector3F result )
		{
			var random = new Random( position.GetHashCode() );

            if(position.X < 0 && position.Z < 0 ) {
				result = new Vector3F(
				   position.X - Math.Sign(position.X) * random.Next(invMultiplier, multiplier / 10) - position.Z*multiplier1,
				   position.Y, //* random.Next( invMultiplier, multiplier ),
				   position.Z);// * random.Next( invMultiplier, multiplier ) );
			} else {
				result = position; 
			}
		}

		protected override void OnApplyToMeshData( Component_Mesh.CompiledData compiledData )
		{
			base.OnApplyToMeshData( compiledData );

			float multiplier = (float)Multiplier;
			float multiplier1 = (float)Multiplier1;
			float invMultiplier = 0; //-multiplier;

			foreach( var oper in compiledData.MeshData.RenderOperations )
			{
				oper.VertexStructure.GetElementBySemantic( VertexElementSemantic.Position, out VertexElement positionElement );
				var vertexBuffer = oper.VertexBuffers[ positionElement.Source ];
				var positions = vertexBuffer.ExtractChannel<Vector3F>( positionElement.Offset );

				var newPositions = new Vector3F[ positions.Length ];
				for( int n = 0; n < positions.Length; n++ )
					ProcessVertex( multiplier,multiplier1, invMultiplier, ref positions[ n ], out newPositions[ n ] );

				vertexBuffer.MakeCopyOfData();
				vertexBuffer.WriteChannel( positionElement.Offset, newPositions );
			}
		}

		protected override void OnBakeIntoMesh( DocumentInstance document, UndoMultiAction undoMultiAction )
		{
			base.OnBakeIntoMesh( document, undoMultiAction );

			float multiplier = (float)Multiplier;
			float multiplier1 = (float)Multiplier1;
			float invMultiplier = 0;//multiplier != 0 ? ( 1.0f / multiplier ) : 0;

			var mesh = (Component_Mesh)Parent;
			var geometries = mesh.GetComponents<Component_MeshGeometry>();

			foreach( var geometry in geometries )
			{
				var positions = geometry.VerticesExtractChannel<Vector3F>( VertexElementSemantic.Position );
				if( positions != null )
				{
					var vertexStructure = geometry.VertexStructure.Value;
					vertexStructure.GetInfo( out var vertexSize, out _ );

					var oldValue = geometry.Vertices;
					var vertices = geometry.Vertices.Value;
					var vertexCount = vertices.Length / vertexSize;

					var newPositions = new Vector3F[ positions.Length ];
					for( int n = 0; n < positions.Length; n++ )
						ProcessVertex( multiplier,multiplier1, invMultiplier, ref positions[ n ], out newPositions[ n ] );

					var newVertices = (byte[])vertices.Clone();
					if( geometry.VerticesWriteChannel( VertexElementSemantic.Position, newPositions, newVertices ) )
					{
						//update property
						geometry.Vertices = newVertices;

						//undo
						if( undoMultiAction != null )
						{
							var property = (Metadata.Property)geometry.MetadataGetMemberBySignature( "property:Vertices" );
							var undoAction = new UndoActionPropertiesChange( new UndoActionPropertiesChange.Item( geometry, property, oldValue ) );
							undoMultiAction.AddAction( undoAction );
						}
					}
				}
			}
		}








	}
}