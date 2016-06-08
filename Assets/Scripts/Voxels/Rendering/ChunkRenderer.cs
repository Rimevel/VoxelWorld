using UnityEngine;
using System.Collections;
using VoxelWorld.Terrain;

namespace VoxelWorld.Rendering
{

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class ChunkRenderer : MonoBehaviour
{
	public Chunk chunk;

	MeshFilter filter;
	MeshCollider coll;

	void Start()
	{
		filter = gameObject.GetComponent<MeshFilter>();
		coll = gameObject.GetComponent<MeshCollider>();
	}

	void Update()
	{
		if(chunk.update)
		{
			chunk.update = false;
			chunk.UpdateChunk();
		}
	}

	/**
	 * Sends the calculated mesh information in the
	 * chunk to the mesh and collision components.
	 **/
	public void RenderMesh(MeshData meshData)
	{
		filter.mesh.Clear();
		filter.mesh.vertices = meshData.vertices.ToArray();
		filter.mesh.triangles = meshData.triangles.ToArray();

		Mesh collMesh = new Mesh();
		coll.sharedMesh = null;
		collMesh.vertices = meshData.vertices.ToArray();
		collMesh.triangles = meshData.triangles.ToArray();
		collMesh.RecalculateNormals();
		coll.sharedMesh = collMesh;

		filter.mesh.uv = meshData.uv.ToArray();
		filter.mesh.RecalculateNormals();
	}
}

}