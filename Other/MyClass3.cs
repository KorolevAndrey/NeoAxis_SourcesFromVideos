using System;
using System.ComponentModel;
using System.Collections.Generic;
using NeoAxis.Editor;

namespace NeoAxis
{
	public class MyClass3 : NeoAxis.Component_MeshModifier
	{



		[DefaultValue(1)]
		[Range(1, 3)]
		public int posN { get; set; } = 1;

		[DefaultValue(1)]
		[Range(-1, 1)]
		public int sigN { get; set; } = 1;



		[DefaultValue(0)]
		[Range(-2, 2)]
		public float off1 { get; set; } = 0;

		[DefaultValue(0)]
		[Range(-2, 2)]
		public float off2 { get; set; } = 0;

		protected override void OnEnabledInSimulation()
		{
		}

		protected override void OnUpdate(float delta)
		{
		}

		protected override void OnSimulationStep()
		{
		}



		[DefaultValue(0)]
		[Range(-20, 20, RangeAttribute.ConvenientDistributionEnum.Exponential)]
		public Reference<double> Multiplier3
		{
			get { if (_multiplier3.BeginGet()) Multiplier3 = _multiplier3.Get(this); return _multiplier3.value; }
			set
			{
				//if( value < 0 )
				//	value = new Reference<double>( 0, value.GetByReference );
				if (_multiplier3.BeginSet(ref value))
				{
					try
					{
						Multiplier3Changed?.Invoke(this);
						ShouldRecompileMesh();
					}
					finally { _multiplier3.EndSet(); }
				}
			}
		}
		/// <summary>Occurs when the <see cref="Multiplier"/> property value changes.</summary>
		public event Action<MyClass3> Multiplier3Changed;
		ReferenceField<double> _multiplier3 = 0;



		[DefaultValue(0)]
		[Range(-20, 20, RangeAttribute.ConvenientDistributionEnum.Exponential)]
		public Reference<double> Multiplier2
		{
			get { if (_multiplier2.BeginGet()) Multiplier2 = _multiplier2.Get(this); return _multiplier2.value; }
			set
			{
				//if( value < 0 )
				//	value = new Reference<double>( 0, value.GetByReference );
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
		/// <summary>Occurs when the <see cref="Multiplier"/> property value changes.</summary>
		public event Action<MyClass3> Multiplier2Changed;
		ReferenceField<double> _multiplier2 = 0;




		[DefaultValue(50)]
		[Range(-50, 50, RangeAttribute.ConvenientDistributionEnum.Exponential)]
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
		/// <summary>Occurs when the <see cref="Multiplier"/> property value changes.</summary>
		public event Action<MyClass3> Multiplier1Changed;
		ReferenceField<double> _multiplier1 = 50;









		[DefaultValue(30)]
		[Range(-180, 180, RangeAttribute.ConvenientDistributionEnum.Exponential)]
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
		/// <summary>Occurs when the <see cref="Multiplier"/> property value changes.</summary>
		public event Action<MyClass3> MultiplierChanged;
		ReferenceField<double> _multiplier = 30;

		/////////////////////////////////////////

		void ProcessVertex(float multiplier, float multiplier1, float invMultiplier, int n, ref Vector3F position, out Vector3F result)
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
			float mul = 0;
			result = position;
            //if(n % 2 == 0) {
			//	return;
			//}
            if(posN == 2) {
				if (position.Z > (float)Multiplier2 && position.Z < (float)Multiplier3 && multiplier1 != 0)
				{
					Vector2F xz = new Vector2F(position.X, position.Z - (float)Multiplier2);
					float a1 = NeoAxis.MathEx.Acos(xz.GetNormalize().X);
					mul = ((multiplier) * (MathEx.Sin(position.Z)*position.Z - (float)Multiplier2)) / multiplier1;
					nposx = NeoAxis.MathEx.Cos(a1 + (mul * NeoAxis.MathEx.PI) / 180) * xz.Length();
					nposz = (float)Multiplier2 + NeoAxis.MathEx.Sin(a1 + (mul * NeoAxis.MathEx.PI) / 180) * xz.Length();

					result = new Vector3F(
							   nposx,
							   position.Y,
							   nposz);
				} else if (position.Z >= (float)Multiplier3 && multiplier1 != 0)
				{
					Vector2F xz = new Vector2F(position.X, position.Z - (float)Multiplier2);
					float a1 = NeoAxis.MathEx.Acos(xz.GetNormalize().X);
					mul = ((multiplier) * (MathEx.Sin((float)Multiplier3)*(float)Multiplier3  - (float)Multiplier2)) / multiplier1;
					nposx = NeoAxis.MathEx.Cos(a1 + (mul * NeoAxis.MathEx.PI) / 180) * xz.Length();
					nposz = (float)Multiplier2 + NeoAxis.MathEx.Sin(a1 + (mul * NeoAxis.MathEx.PI) / 180) * xz.Length();

					result = new Vector3F(
							   nposx,
							   position.Y,
							   nposz);
				}
			} else if(posN == 1) {
				//z - y
				//x
				//y - z
				if (position.Y <= (float)Multiplier2 && multiplier1 != 0)
				{
					Vector2F xy = new Vector2F(position.X, position.Y + (float)Multiplier2);
					float a1 = NeoAxis.MathEx.Acos(xy.GetNormalize().X);
					mul = ((multiplier) * (position.Y - (float)Multiplier2)) / multiplier1;
					nposx = NeoAxis.MathEx.Cos(a1 + (mul * NeoAxis.MathEx.PI) / 180) * xy.Length();
					nposy = (float)Multiplier2 - NeoAxis.MathEx.Sin(a1 + (mul * NeoAxis.MathEx.PI) / 180) * xy.Length();

					result = new Vector3F(
							   nposx,
							   nposy,
							   position.Z);
				}				
			}
		}

		protected override void OnApplyToMeshData(Component_Mesh.CompiledData compiledData)
		{
			base.OnApplyToMeshData(compiledData);

			float multiplier = (float)Multiplier;
			float multiplier1 = (float)Multiplier1;
			float invMultiplier = 0; //-multiplier;
			int aa = 0;

			foreach (var oper in compiledData.MeshData.RenderOperations)
			{
				//aa = aa + 1;
				//ScreenMessages.Add(aa.ToString()+")"+oper.VertexCount.ToString());
				//if(aa == 2) {
				oper.VertexStructure.GetElementBySemantic(VertexElementSemantic.Position, out VertexElement positionElement);
				var vertexBuffer = oper.VertexBuffers[positionElement.Source];
				var positions = vertexBuffer.ExtractChannel<Vector3F>(positionElement.Offset);

				var newPositions = new Vector3F[positions.Length];
				for (int n = 0; n < positions.Length; n=n+1)
					ProcessVertex(multiplier, multiplier1, invMultiplier, n, ref positions[n], out newPositions[n]);

				vertexBuffer.MakeCopyOfData();
				vertexBuffer.WriteChannel(positionElement.Offset, newPositions);
				//}
			}
		}

		protected override void OnBakeIntoMesh(DocumentInstance document, UndoMultiAction undoMultiAction)
		{
			base.OnBakeIntoMesh(document, undoMultiAction);

			float multiplier = (float)Multiplier;
			float multiplier1 = (float)Multiplier1;
			float invMultiplier = 0;//multiplier != 0 ? ( 1.0f / multiplier ) : 0;

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
						ProcessVertex(multiplier, multiplier1, invMultiplier, n, ref positions[n], out newPositions[n]);

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