using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class Chunk : MonoBehaviour
{
	public byte[ , , ] blocks = new byte[chunkSize, chunkSize, chunkSize];
	public byte[ , , ] meta = new byte[chunkSize, chunkSize, chunkSize];

	//How many cubic blocks is a chunk?
	public static int chunkSize = 16;
	//If set to true the chunk will update.
	public bool update = false;

	//The world this chunk belong to.
	public World world;
	//World position of this chunk.
	public WorldPos pos;

	//Has the chunk been rendered yet?
	public bool rendered;
	//Have this chunk been modified and needs to be saved?
	public bool needsSaving;

	MeshFilter filter;
	MeshCollider coll;

	void Start()
	{
		filter = gameObject.GetComponent<MeshFilter>();
		coll = gameObject.GetComponent<MeshCollider>();
	}

	void Update()
	{
		if(update)
		{
			update = false;
			UpdateChunk();
		}
	}

	/**
	 * Get block at the specified coords inside the chunk.
	 * Use World.GetBlock() in most cases!
	 **/
	public int GetBlock(int x, int y, int z)
	{
		if(InRange(x) && InRange(y) && InRange(z))
		{
			return (int)blocks[x, y, z];
		}

		return world.GetBlock(pos.x + x, pos.y + y, pos.z + z);
	}

	/**
	 * Set the block at the specified coords inside the chunk.
	 * Use World.SetBlock() instead in most cases!
	 **/
	public void SetBlock(int x, int y, int z, int block)
	{
		if(InRange(x) && InRange(y) && InRange(z))
		{
			blocks[x, y, z] = (byte)block;
			needsSaving = true;
		}
		else
		{
			world.SetBlock(pos.x + x, pos.y + y, pos.z + z, block);
			needsSaving = true;
		}
	}

	/**
	 * Check if the index value is within the min and max values of a chunk.
	 **/
	public static bool InRange(int index)
	{
		if(index < 0 || index >= chunkSize)
		{
			return false;
		}

		return true;
	}

	/**
	 * Update the chunk based on the contents inside.
	 **/
	void UpdateChunk()
	{
		rendered = true;

		MeshData meshData = new MeshData();

		for(int x = 0; x < chunkSize; x++)
		{
			for(int y = 0; y < chunkSize; y++)
			{
				for(int z = 0; z < chunkSize; z++)
				{
					meshData = Register.GetBlockById(blocks[x, y, z]).GetBlockMesh(this, x, y, z, meshData);
				}
			}
		}

		RenderMesh(meshData);
	}

	/**
	 * Sends the calculated mesh information in the
	 * chunk to the mesh and collision components.
	 **/
	void RenderMesh(MeshData meshData)
	{
		filter.mesh.Clear();
		filter.mesh.vertices = meshData.vertices.ToArray();
		filter.mesh.triangles = meshData.triangles.ToArray();

		filter.mesh.uv = meshData.uv.ToArray();
		filter.mesh.RecalculateNormals();

		coll.sharedMesh = null;
		Mesh mesh = new Mesh();
		mesh.vertices = meshData.colVertices.ToArray();
		mesh.triangles = meshData.colTriangles.ToArray();
		mesh.RecalculateNormals();

		coll.sharedMesh = mesh;
	}
}
