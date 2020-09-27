using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using NeoAxis;
using NeoAxis.Editor;

namespace Project
{
	public class SimpleMeshInSpace : NeoAxis.Component_MeshInSpace
	{
		public Component_Mesh mesh = null;
		public Component_MeshGeometry geometry = null;
		public StandardVertex.StaticOneTexCoord[] vertices;

		private double time = 1;		
		private double deftime =0.1;
		
		private double maxdist =100.0;
		private int cnt = 0;

		unsafe byte[] ConvertVertices( StandardVertex.StaticOneTexCoord[] vertices)
		{
			var result = new byte[vertices.Length * sizeof(StandardVertex.StaticOneTexCoord)];
			fixed(byte* pResult = result)
				fixed(StandardVertex.StaticOneTexCoord* pVertices = vertices)
					NativeUtility.CopyMemory(pResult, pVertices, result.Length);
			return result;
		}
		

		protected override void OnEnabledInSimulation()
		{
			//create a mesh		
			mesh = CreateComponent<Component_Mesh>(enabled: false);
			mesh.Name = "Mesh 1";
				
			//generate vertices. use StandardVertex to make it easier 
			vertices = new StandardVertex.StaticOneTexCoord[4];

			var v = new StandardVertex.StaticOneTexCoord();
			v.Position = new Vector3F(-0.4f, -0.4f, 0f);
			v.Normal = new Vector3F(0, 0, 1);
			v.Tangent = new Vector4F(1, 0, 0, 0);
			v.Color = new ColorValue(1, 1, 1);
			v.TexCoord0 = new Vector2F(1, 1);
				
			vertices[0] = v;

            v.TexCoord0 = new Vector2F(0, 1);
            v.Position = new Vector3F(0.4f, -0.4f, 0);
			vertices[1] = v;

            v.TexCoord0 = new Vector2F(0, 0);
			v.Position = new Vector3F(0.4f, 0.4f, 0);
			vertices[2] = v;

            v.TexCoord0 = new Vector2F(1, 0);
			v.Position = new Vector3F(-0.4f, 0.4f, 0);
			vertices[3] = v;

			//generate indices
			var indices = new int[] { 0, 1, 2, 2, 3, 0 };

			//create geometry of the mesh
			geometry = mesh.CreateComponent<Component_MeshGeometry>();
			geometry.VertexStructure = StandardVertex.MakeStructure(StandardVertex.Components.StaticOneTexCoord, true, out int vertexSize);
			geometry.Vertices = ConvertVertices(vertices);
			geometry.Indices = indices;
			geometry.Material = this.ReplaceMaterial;
			
			//mesh has been created, now we can enable it
			mesh.Enabled = true;
			
			//make reference to the mesh. 'Root' reference - global path from scene root.
			this.Mesh = ReferenceUtility.MakeRootReference(mesh);

		}













		protected override void OnUpdate( float delta )
		{
			if(vertices != null && geometry != null && mesh != null) {
				var dir = SimulationApp.MainViewport.CameraSettings.Position - this.TransformV.Position;
				var l = dir.Length();
				
				if(l < maxdist) {

					time = time - delta;
					if (time <= 0)
					{
						cnt = cnt + 1;
						if (cnt % 2 == 0)
						{
							vertices[0].TexCoord0 = new Vector2F(0.7f, 0.7f);
							vertices[1].TexCoord0 = new Vector2F(0.3f, 0.7f);
							vertices[2].TexCoord0 = new Vector2F(0.3f, 0.3f);
							vertices[3].TexCoord0 = new Vector2F(0.7f, 0.3f);
						}
						else
						{
							vertices[0].TexCoord0 = new Vector2F(1f, 1f);
							vertices[1].TexCoord0 = new Vector2F(0f, 1f);
							vertices[2].TexCoord0 = new Vector2F(0f, 0f);
							vertices[3].TexCoord0 = new Vector2F(1f, 0f);
						}
						geometry.Vertices = ConvertVertices(vertices);
						//mesh.ShouldRecompile = true;

						time = deftime;
					}
					
				}
			}
		}
		
		protected override void OnSimulationStep()
		{
		}
	}
}