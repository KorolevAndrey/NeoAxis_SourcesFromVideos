using System;
using System.ComponentModel;
using System.Collections.Generic;
using NeoAxis.Editor;

namespace NeoAxis
{
	public class Attach : NeoAxis.Component_MeshModifier
	{

//		[DefaultValue( 1 )]
//		[Range( 0, 100 )]
//		public int Mode { get; set; } = 1;

//		[DefaultValue( 1 )]
//		[Range( 0, 100 )]
//		public double R1 { get; set; } = 1;
		
//		[DefaultValue( "0 0 0" )]
//		public Vector3F Pos1 { get; set; } = new Vector3F( 0, 0, 0 );




//        [DefaultValue( 1 )]
//        [Range( -100, 100 )]
//        public Reference<double> Multiplier
//		{
//			get { if (_multiplier.BeginGet()) Multiplier = _multiplier.Get(this); return _multiplier.value; }
//			set
//			{
//				//if( value < 0 )
//				//	value = new Reference<double>( 0, value.GetByReference );
//				if (_multiplier.BeginSet(ref value))
//				{
//					try
//					{
//						MultiplierChanged?.Invoke(this);
//						ShouldRecompileMesh();
//					}
//					finally { _multiplier.EndSet(); }
//				}
//			}
//		}
//		public event Action<Attach> MultiplierChanged;
//		ReferenceField<double> _multiplier = 1;


		[DefaultValue( false )]
		public bool MatrixInv { get; set; } = false;


		[DefaultValue( "0 0 0 0 0 0 1 1 1 1" )]
		public Reference<Transform> PosCenter
		{
			get { if (_posCenter.BeginGet()) PosCenter = _posCenter.Get(this); return _posCenter.value; }
			set
			{
				//if( value < 0 )
				//	value = new Reference<double>( 0, value.GetByReference );
				if (_posCenter.BeginSet(ref value))
				{
					try
					{
						posCenterChanged?.Invoke(this);
						//ShouldRecompileMesh();
					}
					finally { _posCenter.EndSet(); }
				}
			}
		}		
		public event Action<Attach> posCenterChanged;
		ReferenceField<Transform> _posCenter = new Transform(new Vector3F(0,0,0), new QuaternionF(0,0,0,1), new Vector3F(1,1,1));
		



		[DefaultValue( "0 0 0 0 0 0 1 1 1 1" )]
		public Reference<Transform> Pos01n
		{
			get { if (_pos01n.BeginGet()) Pos01n = _pos01n.Get(this); return _pos01n.value; }
			set
			{
				//if( value < 0 )
				//	value = new Reference<double>( 0, value.GetByReference );
				if (_pos01n.BeginSet(ref value))
				{
					try
					{
						pos01nChanged?.Invoke(this);
						//ShouldRecompileMesh();
					}
					finally { _pos01n.EndSet(); }
				}
			}
		}		
		public event Action<Attach> pos01nChanged;
		ReferenceField<Transform> _pos01n = new Transform(new Vector3F(0,0,0), new QuaternionF(0,0,0,1), new Vector3F(1,1,1));
				


		[DefaultValue( "0 0 0 0 0 0 1 1 1 1" )]
		public Reference<Transform> Pos02n
		{
			get { if (_pos02n.BeginGet()) Pos02n = _pos02n.Get(this); return _pos02n.value; }
			set
			{
				//if( value < 0 )
				//	value = new Reference<double>( 0, value.GetByReference );
				if (_pos02n.BeginSet(ref value))
				{
					try
					{
						pos02nChanged?.Invoke(this);
						//ShouldRecompileMesh();
					}
					finally { _pos02n.EndSet(); }
				}
			}
		}		
		public event Action<Attach> pos02nChanged;
		ReferenceField<Transform> _pos02n = new Transform(new Vector3F(0,0,0), new QuaternionF(0,0,0,1), new Vector3F(1,1,1));
				

		[DefaultValue( "0 0 0 0 0 0 1 1 1 1" )]
		public Reference<Transform> Pos03n
		{
			get { if (_pos03n.BeginGet()) Pos03n = _pos03n.Get(this); return _pos03n.value; }
			set
			{
				//if( value < 0 )
				//	value = new Reference<double>( 0, value.GetByReference );
				if (_pos03n.BeginSet(ref value))
				{
					try
					{
						pos03nChanged?.Invoke(this);
						//ShouldRecompileMesh();
					}
					finally { _pos03n.EndSet(); }
				}
			}
		}		
		public event Action<Attach> pos03nChanged;
		ReferenceField<Transform> _pos03n = new Transform(new Vector3F(0,0,0), new QuaternionF(0,0,0,1), new Vector3F(1,1,1));
				


		[DefaultValue( "0 0 0 0 0 0 1 1 1 1" )]
		public Reference<Transform> Pos04n
		{
			get { if (_pos04n.BeginGet()) Pos04n = _pos04n.Get(this); return _pos04n.value; }
			set
			{
				//if( value < 0 )
				//	value = new Reference<double>( 0, value.GetByReference );
				if (_pos04n.BeginSet(ref value))
				{
					try
					{
						pos04nChanged?.Invoke(this);
						//ShouldRecompileMesh();
					}
					finally { _pos04n.EndSet(); }
				}
			}
		}		
		public event Action<Attach> pos04nChanged;
		ReferenceField<Transform> _pos04n = new Transform(new Vector3F(0,0,0), new QuaternionF(0,0,0,1), new Vector3F(1,1,1));
				





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
						//ShouldRecompileMesh();
					}
					finally { _pos1n.EndSet(); }
				}
			}
		}		
		public event Action<Attach> pos1nChanged;
		ReferenceField<Transform> _pos1n = new Transform(new Vector3F(0,0,0), new QuaternionF(0,0,0,1), new Vector3F(1,1,1));
		


		[DefaultValue( "0 0 0 0 0 0 1 1 1 1" )]
		public Reference<Transform> Pos2n
		{
			get { if (_pos2n.BeginGet()) Pos2n = _pos2n.Get(this); return _pos2n.value; }
			set
			{
				//if( value < 0 )
				//	value = new Reference<double>( 0, value.GetByReference );
				if (_pos2n.BeginSet(ref value))
				{
					try
					{
						pos2nChanged?.Invoke(this);
						//ShouldRecompileMesh();
					}
					finally { _pos2n.EndSet(); }
				}
			}
		}		
		public event Action<Attach> pos2nChanged;
		ReferenceField<Transform> _pos2n = new Transform(new Vector3F(0,0,0), new QuaternionF(0,0,0,1), new Vector3F(1,1,1));
		
		[DefaultValue( "0 0 0 0 0 0 1 1 1 1" )]
		public Reference<Transform> Pos3n
		{
			get { if (_pos3n.BeginGet()) Pos3n = _pos3n.Get(this); return _pos3n.value; }
			set
			{
				//if( value < 0 )
				//	value = new Reference<double>( 0, value.GetByReference );
				if (_pos3n.BeginSet(ref value))
				{
					try
					{
						pos3nChanged?.Invoke(this);
						//ShouldRecompileMesh();
					}
					finally { _pos3n.EndSet(); }
				}
			}
		}		
		public event Action<Attach> pos3nChanged;
		ReferenceField<Transform> _pos3n = new Transform(new Vector3F(0,0,0), new QuaternionF(0,0,0,1), new Vector3F(1,1,1));
		
		
		[DefaultValue( "0 0 0 0 0 0 1 1 1 1" )]
		public Reference<Transform> Pos4n
		{
			get { if (_pos4n.BeginGet()) Pos4n = _pos4n.Get(this); return _pos4n.value; }
			set
			{
				//if( value < 0 )
				//	value = new Reference<double>( 0, value.GetByReference );
				if (_pos4n.BeginSet(ref value))
				{
					try
					{
						pos4nChanged?.Invoke(this);
						//ShouldRecompileMesh();
					}
					finally { _pos4n.EndSet(); }
				}
			}
		}		
		public event Action<Attach> pos4nChanged;
		ReferenceField<Transform> _pos4n = new Transform(new Vector3F(0,0,0), new QuaternionF(0,0,0,1), new Vector3F(1,1,1));
				




		[DefaultValue( "0 0 0 0 0 0 1 1 1 1" )]
		public Reference<Transform> ResPos1
		{
			get { if (_resPos1.BeginGet()) ResPos1 = _resPos1.Get(this); return _resPos1.value; }
			set
			{
				//if( value < 0 )
				//	value = new Reference<double>( 0, value.GetByReference );
				if (_resPos1.BeginSet(ref value))
				{
					try
					{
						resPos1Changed?.Invoke(this);
						//ShouldRecompileMesh();
					}
					finally { _resPos1.EndSet(); }
				}
			}
		}		
		public event Action<Attach> resPos1Changed;
		ReferenceField<Transform> _resPos1 = new Transform(new Vector3F(0,0,0), new QuaternionF(0,0,0,1), new Vector3F(1,1,1));
		


		[DefaultValue( "0 0 0" )]
		public Reference<Vector3F> ResPosVec1
		{
			get { if (_resPosVec1.BeginGet()) ResPosVec1 = _resPosVec1.Get(this); return _resPosVec1.value; }
			set
			{
				//if( value < 0 )
				//	value = new Reference<double>( 0, value.GetByReference );
				if (_resPosVec1.BeginSet(ref value))
				{
					try
					{
						resPosVec1Changed?.Invoke(this);
						//ShouldRecompileMesh();
					}
					finally { _resPosVec1.EndSet(); }
				}
			}
		}		
		public event Action<Attach> resPosVec1Changed;
		ReferenceField<Vector3F> _resPosVec1 = new Vector3F(0,0,0);
		




		protected override void OnEnabledInSimulation()
		{
		}

		protected override void OnUpdate(float delta)
		{
			
		}

		protected override void OnSimulationStep()
		{
			ShouldRecompileMesh();
			base.OnSimulationStep();
		}





		void ProcessVertex(ref Vector3F position, out Vector3F result)
		{
			
            Matrix4? nullable = new Matrix4?(PosCenter.Value.ToMatrix4());
            Matrix4 matrix4 = nullable ?? Matrix4.Identity;
            Matrix4F matrix4f = matrix4.ToMatrix4F();
    		bool inv = matrix4f.Inverse();
			MatrixInv = inv;

            ResPosVec1 = matrix4f * Pos1n.Value.Position.ToVector3F();
            
            result = position;
			var sc = new Vector3F(0.2f, 0.2f, 0.2f);
			var rposition = position * sc;
                        
			var l_r01 = (float)Pos01n.Value.Scale.X;			
			var relp01 = (Pos01n.Value.Position).ToVector3F();
            var l_dist1 = (rposition - relp01);
			float l_l01 = l_dist1.Length();
			
			var l_r02 = (float)Pos02n.Value.Scale.X;
			var relp02 = (Pos02n.Value.Position).ToVector3F();
            var l_dist2 = (rposition - relp02);
			float l_l02 = l_dist2.Length();

			var l_r03 = (float)Pos03n.Value.Scale.X;
			var relp03 = (Pos03n.Value.Position).ToVector3F();
            var l_dist3 = (rposition - relp03);
			float l_l03 = l_dist3.Length();

			var l_r04 = (float)Pos04n.Value.Scale.X;
			var relp04 = (Pos04n.Value.Position).ToVector3F();
            var l_dist4 = (rposition - relp04);
			float l_l04 = l_dist4.Length();

            var f = ((ResPos1.Value.Position - PosCenter.Value.Position) / sc).ToVector3F().GetNormalize();
			SphericalDirectionF sd = new SphericalDirectionF(f.X, f.Y);
			float ax = MathEx.Acos(f.X);
			float ay = MathEx.Asin(f.X);			
			QuaternionF q = new AnglesF().ToQuaternion();
            
            
			if (l_l01 <= l_r01)
			{
				var relp1 =  matrix4f * Pos1n.Value.Position.ToVector3F();
				result = relp1;
			}
			if (l_l02 <= l_r02)
			{
				var relp2 =  matrix4f * Pos2n.Value.Position.ToVector3F();;
				result = relp2;
			}
			if (l_l03 <= l_r03)
			{
				var relp3 =  matrix4f * Pos3n.Value.Position.ToVector3F();;
				result = relp3;
			}
			if (l_l04 <= l_r04)
			{
				var relp4 =  matrix4f * Pos4n.Value.Position.ToVector3F();;
				result = relp4;
			}



		}



		protected override void OnApplyToMeshData(Component_Mesh.CompiledData compiledData)
		{
			base.OnApplyToMeshData(compiledData);

//			int aa = 0;

			foreach (var oper in compiledData.MeshData.RenderOperations)
			{
//				aa = aa + 1;

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