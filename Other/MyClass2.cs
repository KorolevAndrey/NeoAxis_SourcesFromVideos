using System;
using System.ComponentModel;
using System.Collections.Generic;
using NeoAxis.Editor;

namespace NeoAxis
{
	public class MyClass2 : NeoAxis.Component_MeshModifier
	{
		
		
		
		[DefaultValue( 1 )]
		[Range( 1, 3 )]
		public int posN { get; set; } = 1;

		[DefaultValue( 1 )]
		[Range( -1, 1 )]
		public int sigN { get; set; } = 1;



		[DefaultValue( 0 )]
		[Range( -2, 2 )]
		public float off1 { get; set; } = 0;

        [DefaultValue( 0 )]
		[Range( -2, 2 )]
		public float off2 { get; set; } = 0;

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
		[Range( -2, 2, RangeAttribute.ConvenientDistributionEnum.Exponential )]
		public Reference<double> Multiplier2
		{
			get { if( _multiplier2.BeginGet() ) Multiplier2 = _multiplier2.Get( this ); return _multiplier2.value; }
			set
			{
				//if( value < 0 )
				//	value = new Reference<double>( 0, value.GetByReference );
				if( _multiplier2.BeginSet( ref value ) )
				{
					try
					{
						Multiplier2Changed?.Invoke( this );
						ShouldRecompileMesh();
					}
					finally { _multiplier2.EndSet(); }
				}
			}
		}
		/// <summary>Occurs when the <see cref="Multiplier"/> property value changes.</summary>
		public event Action<MyClass2> Multiplier2Changed;
		ReferenceField<double> _multiplier2 = 0.1;




		[DefaultValue( 0.1 )]
		[Range( -5, 10, RangeAttribute.ConvenientDistributionEnum.Exponential )]
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
		public event Action<MyClass2> Multiplier1Changed;
		ReferenceField<double> _multiplier1 = 0.1;









		[DefaultValue( 0.1 )]
		[Range( -2, 2, RangeAttribute.ConvenientDistributionEnum.Exponential )]
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
		public event Action<MyClass2> MultiplierChanged;
		ReferenceField<double> _multiplier = 0.1;

		/////////////////////////////////////////

		void ProcessVertex( float multiplier, float multiplier1, float invMultiplier, ref Vector3F position, out Vector3F result )
		{
			var random = new Random(position.GetHashCode());
			float nposx = 0;
			float Ax = 0;
			float nposy = 0;
			float Ay = 0;			
			float nposz = 0;
			float Az = 0;
			float ofs1 = off1;
			float ofs2 = off2;
            result = position;
            
            
			if (posN == 1) {
				if (sigN == 1 && position.X <= (float)Multiplier2)
				{

					Ax = multiplier * (float)Math.Exp(-position.Z * position.Z * multiplier1);
					nposx = position.X + Math.Sign(position.X) * (Ax * ((float)Math.Exp(-position.Y * position.Y * multiplier1)));
					if (nposx > (float)Multiplier2)
					{
						nposx = (float)Multiplier2;
					}

					result = new Vector3F(
					   nposx,
					   position.Y,
					   position.Z);
				}
				if (sigN == -1 && position.X > (float)Multiplier2)
				{

					Ax = multiplier * (float)Math.Exp(-position.Z * position.Z * multiplier1);
					nposx = position.X + Math.Sign(position.X) * (Ax * ((float)Math.Exp(-position.Y * position.Y * multiplier1)));
					if (nposx < (float)Multiplier2)
					{
						nposx = (float)Multiplier2;
					}

					result = new Vector3F(
					   nposx,
					   position.Y,
					   position.Z);
				}				
			} else if (posN == 2) {
				if (sigN == 1 && position.Y <= (float)Multiplier2)
				{

					Ay = multiplier * (float)Math.Exp(-position.Z * position.Z * multiplier1);
					nposy = position.Y + Math.Sign(position.Y) * (Ay * ((float)Math.Exp(-position.X * position.X * multiplier1)));
					if (nposy > (float)Multiplier2)
					{
						nposy = (float)Multiplier2;
					}

					result = new Vector3F(
					   position.X,
					   nposy,
					   position.Z);
				}				
				if (sigN == -1 && position.Y > (float)Multiplier2)
				{

					Ay = multiplier * (float)Math.Exp(-position.Z * position.Z * multiplier1);
					nposy = position.Y + Math.Sign(position.Y) * (Ay * ((float)Math.Exp(-position.X * position.X * multiplier1)));
					if (nposy < (float)Multiplier2)
					{
						nposy = (float)Multiplier2;
					}

					result = new Vector3F(
					   position.X,
					   nposy,
					   position.Z);
				}						
			} else if (posN == 3) {
				if (sigN == 1 && position.Z <= (float)Multiplier2)
				{

					Az = multiplier * (float)Math.Exp(-(position.X - ofs2) * (position.X - ofs2) * multiplier1);
					nposz = position.Z + Math.Sign(position.Z) * (Az * ((float)Math.Exp(-(position.Y - ofs1) * (position.Y-ofs1) * multiplier1)));
					if (nposz > (float)Multiplier2)
					{
						nposz = (float)Multiplier2;
					}

					result = new Vector3F(
					   position.X,
					   position.Y,
					   nposz);
				}
				if (sigN == -1 && position.Z > (float)Multiplier2)
				{

					Az = multiplier * (float)Math.Exp(-(position.X - ofs2) * (position.X - ofs2) * multiplier1);
					nposz = position.Z + Math.Sign(position.Z) * (Az * ((float)Math.Exp(-(position.Y - ofs1) * (position.Y - ofs1) * multiplier1)));
					if (nposz < (float)Multiplier2)
					{
						nposz = (float)Multiplier2;
					}

					result = new Vector3F(
					   position.X,
					   position.Y,
					   nposz);
				}				
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