using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using NeoAxis;
using NeoAxis.Editor;

namespace Project
{
	public class SimpleMesh : NeoAxis.Component
	{
		public Component_MeshInSpace meshInSpace = null;
		public Component_Mesh mesh = null;
		public Component_MeshGeometry geometry = null;

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
			StandardVertex.StaticOneTexCoord[] vertices = new StandardVertex.StaticOneTexCoord[4];

			var v = new StandardVertex.StaticOneTexCoord();
			v.Position = new Vector3F(-0.4f, -0.4f, 0f);
			v.Normal = new Vector3F(0, 0, 1);
			v.Tangent = new Vector4F(1, 0, 0, 0);
			v.Color = new ColorValue(1, 1, 1);
			//v.TexCoord0 = new Vector2F(-1, -1);
				
			vertices[0] = v;

            v.Position = new Vector3F(0.4f, -0.4f, 0);
			vertices[1] = v;

			v.Position = new Vector3F(0.4f, 0.4f, 0);
			vertices[2] = v;

			v.Position = new Vector3F(-0.4f, 0.4f, 0);
			vertices[3] = v;

			//generate indices
			var indices = new int[] { 0, 1, 2, 2, 3, 0 };

			//create geometry of the mesh
			geometry = mesh.CreateComponent<Component_MeshGeometry>();
			geometry.VertexStructure = StandardVertex.MakeStructure(StandardVertex.Components.StaticOneTexCoord, true, out int vertexSize);
			geometry.Vertices = ConvertVertices(vertices);
			geometry.Indices = indices;

			
			//mesh has been created, now we can enable it
			mesh.Enabled = true;
			
            meshInSpace = CreateComponent<Component_MeshInSpace>(enabled: false);			
			meshInSpace.Transform = new Transform(new Vector3(1, 1, 1));

			//make reference to the mesh. 'Root' reference - global path from scene root.
			meshInSpace.Mesh = ReferenceUtility.MakeRootReference(mesh);

			meshInSpace.Color = new ColorValue(1, 0, 0);
			meshInSpace.Enabled = true;			
		}
		
		protected override void OnUpdate( float delta )
		{
		}
		
		protected override void OnSimulationStep()
		{
		}
	}
}