using System;
using System.ComponentModel;
using System.Collections.Generic;
using NeoAxis.Editor;

namespace NeoAxis
{
	public class MyClass6 : NeoAxis.Component_MeshModifier
	{

		[DefaultValue(1)]
		[Range(0, 100)]
		public int Mode { get; set; } = 1;

		[DefaultValue(0)]
		[Range(0, 100)]
		public int Mode1 { get; set; } = 0;

		[DefaultValue(1)]
		[Range(1, 100)]
		public double R1 { get; set; } = 1;

		[DefaultValue(1)]
		[Range(1, 100)]
		public double R2 { get; set; } = 1;

		[DefaultValue(1)]
		[Range(1, 100)]
		public double R3 { get; set; } = 1;

		[DefaultValue(0)]
		[Range(0, 100)]
		public int DataN { get; set; } = 0;


		[DefaultValue("0 0 0")]
		public Vector3F Pos1 { get; set; } = new Vector3F(0, 0, 0);



		[DefaultValue(0)]
		[Range(-100, 100)]
		public Reference<double> Multiplier0
		{
			get { if (_multiplier0.BeginGet()) Multiplier0 = _multiplier0.Get(this); return _multiplier0.value; }
			set
			{
				//if( value < 0 )
				//	value = new Reference<double>( 0, value.GetByReference );
				if (_multiplier0.BeginSet(ref value))
				{
					try
					{
						Multiplier0Changed?.Invoke(this);
						ShouldRecompileMesh();
					}
					finally { _multiplier0.EndSet(); }
				}
			}
		}
		public event Action<MyClass6> Multiplier0Changed;
		ReferenceField<double> _multiplier0 = 0;



		[DefaultValue(0)]
		[Range(-100, 100)]
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
		public event Action<MyClass6> MultiplierChanged;
		ReferenceField<double> _multiplier = 0;

		[DefaultValue(0)]
		[Range(-100, 100)]
		public Reference<double> Multiplier1
		{
			get { if (_multiplier1.BeginGet()) Multiplier1 = _multiplier1.Get(this); return _multiplier1.value; }
			set
			{
				//if( value < 0 )
				//	value = new Reference<double>( 0, value.GetByReference );
				if (_multiplier1.BeginSet(ref value))
				{
					try
					{
						Multiplier1Changed?.Invoke(this);
						ShouldRecompileMesh();
					}
					finally { _multiplier1.EndSet(); }
				}
			}
		}
		public event Action<MyClass6> Multiplier1Changed;
		ReferenceField<double> _multiplier1 = 0;




		[DefaultValue(10)]
		[Range(1, 10000)]
		public Reference<double> Multiplier2
		{
			get { if (_multiplier2.BeginGet()) Multiplier2 = _multiplier2.Get(this); return _multiplier2.value; }
			set
			{
				if (value < 1)
					value = new Reference<double>(1, value.GetByReference);
				if (_multiplier2.BeginSet(ref value))
				{
					try
					{
						Multiplier2Changed?.Invoke(this);
						ShouldRecompileMesh();
					}
					finally { _multiplier2.EndSet(); }
				}
			}
		}
		public event Action<MyClass6> Multiplier2Changed;
		ReferenceField<double> _multiplier2 = 10;


		[DefaultValue("0 0 0 0 0 0 1 1 1 1")]
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
		public event Action<MyClass6> pos1nChanged;
		ReferenceField<Transform> _pos1n = new Transform(new Vector3F(0, 0, 0), new QuaternionF(0, 0, 0, 1), new Vector3F(1, 1, 1));


		protected override void OnEnabledInSimulation()
		{
		}

		protected override void OnUpdate(float delta)
		{
		}

		protected override void OnSimulationStep()
		{
		}





		void ProcessVertex(int dataN, ref Vector3F position, out Vector3F result)
		{

			result = position;
			var l_r = (float)Pos1n.Value.Scale.X / 2;
			var l_d = (position - Pos1n.Value.Position.ToVector3F());
			var mulr = ((float)R2 * (float)Multiplier2) / (float)R3;
			var offx = (float)Multiplier / ((float)R1 * 100);
			var offy = (float)Multiplier1 / ((float)R1 * 100);
			var offz = (float)Multiplier0 / ((float)R1 * 100);
			var offs = new Vector3F(offx, offy, offz);
			float l_l = l_d.Length();

			if (Mode == 1)
			{
				if (l_l <= l_r)
				{
					result = Pos1n.Value.Position.ToVector3F() + l_d * (0.5f + 0.5f * ((l_l * l_l) / (l_r * l_r)));
				}
			}
			else if (Mode == 2)
			{
				if (l_l <= l_r)
				{
					result = Pos1n.Value.Position.ToVector3F() + l_d * (1.5f - 0.5f * ((l_l * l_l) / (l_r * l_r)));
				}
			}
			else if (Mode == 3)
			{
				if (l_l <= l_r)
				{
					result = Pos1n.Value.Position.ToVector3F() + l_d.GetNormalize() * l_r * (0.8f + 0.2f * ((l_l * l_l) / (l_r * l_r)));
				}
			}
			else if (Mode == 4)
			{

				if (Mode1 == 0)
				{
					if (l_l <= l_r)
					{
						result = position + offs * (1 / (MathEx.Exp((mulr * l_r) * l_l * l_l)));
					}
				}
				else if (Mode1 == 1)
				{
					if (position.Y < Pos1.Y && l_l <= l_r)
					{
						result = position + offs * (1 / (MathEx.Exp((mulr * l_r) * l_l * l_l)));
					}
				}
				else if (Mode1 == 2)
				{
					if (position.Y > Pos1.Y && l_l <= l_r)
					{
						result = position + offs * (1 / (MathEx.Exp((mulr * l_r) * l_l * l_l)));
					}
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
				if (DataN == 0 || DataN == aa)
				{

					oper.VertexStructure.GetElementBySemantic(VertexElementSemantic.Position, out VertexElement positionElement);
					var vertexBuffer = oper.VertexBuffers[positionElement.Source];
					var positions = vertexBuffer.ExtractChannel<Vector3F>(positionElement.Offset);

					var newPositions = new Vector3F[positions.Length];
					for (int n = 0; n < positions.Length; n = n + 1)
						ProcessVertex(aa, ref positions[n], out newPositions[n]);

					vertexBuffer.MakeCopyOfData();
					vertexBuffer.WriteChannel(positionElement.Offset, newPositions);
				}
			}
		}

		protected override void OnBakeIntoMesh(DocumentInstance document, UndoMultiAction undoMultiAction)
		{
			base.OnBakeIntoMesh(document, undoMultiAction);

			var mesh = (Component_Mesh)Parent;
			var geometries = mesh.GetComponents<Component_MeshGeometry>();
			int aa = 0;

			foreach (var geometry in geometries)
			{
				var positions = geometry.VerticesExtractChannel<Vector3F>(VertexElementSemantic.Position);
				if (positions != null)
				{
					aa = aa + 1;
					if (DataN == 0 || DataN == aa)
					{
						var vertexStructure = geometry.VertexStructure.Value;
						vertexStructure.GetInfo(out var vertexSize, out _);

						var oldValue = geometry.Vertices;
						var vertices = geometry.Vertices.Value;
						var vertexCount = vertices.Length / vertexSize;

						var newPositions = new Vector3F[positions.Length];
						for (int n = 0; n < positions.Length; n++)
							ProcessVertex(aa, ref positions[n], out newPositions[n]);

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
}