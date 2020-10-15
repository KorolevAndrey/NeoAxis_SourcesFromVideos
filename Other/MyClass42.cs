using System;
using System.ComponentModel;
using System.Collections.Generic;
using NeoAxis.Editor;

namespace NeoAxis
{
	public class MyClass4 : NeoAxis.Component_MeshModifier
	{

		[DefaultValue( 1 )]
		[Range( 0, 100 )]
		public int Mode { get; set; } = 1;

		[DefaultValue( 1 )]
		[Range( 0, 100 )]
		public double R1 { get; set; } = 1;
		
		[DefaultValue( "0 0 0" )]
		public Vector3F Pos1 { get; set; } = new Vector3F( 0, 0, 0 );




        [DefaultValue( 1 )]
        [Range( -100, 100 )]
        public Reference<double> Multiplier
		{
			get { if (_multiplier.BeginGet()) Multiplier = _multiplier.Get(this); return _multiplier.value; }
			set
			{
				//if( value < 0 )
				//	value = new Reference<double>( 0, value.GetByReference );
				if (_multiplier.BeginSet(ref value))
				{
					try
					{
						MultiplierChanged?.Invoke(this);
						ShouldRecompileMesh();
					}
					finally { _multiplier.EndSet(); }
				}
			}
		}
		public event Action<MyClass4> MultiplierChanged;
		ReferenceField<double> _multiplier = 1;
		

		[DefaultValue( "0 0 0 0 0 0 1 1 1 1" )]
		public Reference<Transform> Pos1n
		{
			get { if (_pos1n.BeginGet()) Pos1n = _pos1n.Get(this); return _pos1n.value; }
			set
			{
				//if( value < 0 )
				//	value = new Reference<double>( 0, value.GetByReference );
				if (_pos1n.BeginSet(ref value))
				{
					try
					{
						pos1nChanged?.Invoke(this);
						ShouldRecompileMesh();
					}
					finally { _pos1n.EndSet(); }
				}
			}
		}		
		public event Action<MyClass4> pos1nChanged;
		ReferenceField<Transform> _pos1n = new Transform(new Vector3F(0,0,0), new QuaternionF(0,0,0,1), new Vector3F(1,1,1));
		

		protected override void OnEnabledInSimulation()
		{
		}

		protected override void OnUpdate(float delta)
		{
		}

		protected override void OnSimulationStep()
		{
		}





		void ProcessVertex(ref Vector3F position, out Vector3F result)
		{

            result = position;
			var l_r = (float)Pos1n.Value.Scale.X;
            var l_d = (position - Pos1n.Value.Position.ToVector3F());
			float l_l = l_d.Length();
			
			if(Mode == 1) {
				if (l_l <= l_r)
				{
					result = Pos1n.Value.Position.ToVector3F() + l_d * (0.5f + 0.5f * ((l_l * l_l) / (l_r * l_r)));
				}
			} else if(Mode == 2) {
				if (l_l <= l_r)
				{
					result = Pos1n.Value.Position.ToVector3F() + l_d * (1.5f - 0.5f * ((l_l * l_l) / (l_r * l_r)));
				}
			} else if(Mode == 3) {
				if (l_l <= l_r)
				{
					result = Pos1n.Value.Position.ToVector3F() + l_d.GetNormalize() * l_r * ( 0.8f + 0.2f * ((l_l * l_l) / (l_r * l_r)) );
				}
			}



		}



		protected override void OnApplyToMeshData(Component_Mesh.CompiledData compiledData)
		{
			base.OnApplyToMeshData(compiledData);

			int aa = 0;

			foreach (var oper in compiledData.MeshData.RenderOperations)
			{
				aa = aa + 1;

				oper.VertexStructure.GetElementBySemantic(VertexElementSemantic.Position, out VertexElement positionElement);
				var vertexBuffer = oper.VertexBuffers[positionElement.Source];
				var positions = vertexBuffer.ExtractChannel<Vector3F>(positionElement.Offset);

				var newPositions = new Vector3F[positions.Length];
				for (int n = 0; n < positions.Length; n = n + 1)
					ProcessVertex(ref positions[n], out newPositions[n]);

				vertexBuffer.MakeCopyOfData();
				vertexBuffer.WriteChannel(positionElement.Offset, newPositions);

			}
		}

		protected override void OnBakeIntoMesh(DocumentInstance document, UndoMultiAction undoMultiAction)
		{
			base.OnBakeIntoMesh(document, undoMultiAction);

			var mesh = (Component_Mesh)Parent;
			var geometries = mesh.GetComponents<Component_MeshGeometry>();

			foreach (var geometry in geometries)
			{
				var positions = geometry.VerticesExtractChannel<Vector3F>(VertexElementSemantic.Position);
				if (positions != null)
				{
					var vertexStructure = geometry.VertexStructure.Value;
					vertexStructure.GetInfo(out var vertexSize, out _);

					var oldValue = geometry.Vertices;
					var vertices = geometry.Vertices.Value;
					var vertexCount = vertices.Length / vertexSize;

					var newPositions = new Vector3F[positions.Length];
					for (int n = 0; n < positions.Length; n++)
						ProcessVertex(ref positions[n], out newPositions[n]);

					var newVertices = (byte[])vertices.Clone();
					if (geometry.VerticesWriteChannel(VertexElementSemantic.Position, newPositions, newVertices))
					{
						//update property
						geometry.Vertices = newVertices;

						//undo
						if (undoMultiAction != null)
						{
							var property = (Metadata.Property)geometry.MetadataGetMemberBySignature("property:Vertices");
							var undoAction = new UndoActionPropertiesChange(new UndoActionPropertiesChange.Item(geometry, property, oldValue));
							undoMultiAction.AddAction(undoAction);
						}
					}
				}
			}
		}




	}
}