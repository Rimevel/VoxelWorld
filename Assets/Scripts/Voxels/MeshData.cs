using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * General class acting as a container for raw mesh data.
 **/
public class MeshData
{
	public List<Vector3> vertices = new List<Vector3>();
	public List<int> triangles = new List<int>();
	public List<Vector2> uv = new List<Vector2>();

	public List<Vector3> colVertices = new List<Vector3>();
	public List<int> colTriangles = new List<int>();

	public bool useRenderDataForCol;

	public MeshData(){}

	/**
	 * Create a quad from the latest vertice data and add it to the mesh.
	 * Also handles quad generation for collision if enabled.
	 **/
	public void AddQuadTriangles()
	{
		triangles.Add(vertices.Count - 4);
		triangles.Add(vertices.Count - 3);
		triangles.Add(vertices.Count - 2);

		triangles.Add(vertices.Count - 4);
		triangles.Add(vertices.Count - 2);
		triangles.Add(vertices.Count - 1);

		if(useRenderDataForCol)
		{
			colTriangles.Add(colVertices.Count - 4);
			colTriangles.Add(colVertices.Count - 3);
			colTriangles.Add(colVertices.Count - 2);
			
			colTriangles.Add(colVertices.Count - 4);
			colTriangles.Add(colVertices.Count - 2);
			colTriangles.Add(colVertices.Count - 1);
		}
	}

	/**
	 * Add a vertex to the mesh.
	 * Also adds it to the collision if enabled.
	 **/
	public void AddVertex(Vector3 vertex)
	{
		vertices.Add(vertex);

		if(useRenderDataForCol)
		{
			colVertices.Add(vertex);
		}
	}

	/**
	 * Add a triangle to the mesh.
	 * Also adds it to the collision if enabled.
	 **/
	public void AddTriangle(int tri)
	{
		triangles.Add(tri);

		if(useRenderDataForCol)
		{
			colTriangles.Add(tri - (vertices.Count - colVertices.Count));
		}
	}

	/**
	 * Get UVs matching the face in the given direction on the given block.
	 **/
	public static Vector2[] GetFaceUVs(Block block, Direction direction)
	{
		float tileSize = 1f / 32f;
		float antiBleed = 0.0001f;

		Vector2[] UVs = new Vector2[4];
		Tile tile = block.GetTile(direction);

		UVs[0] = new Vector2(tileSize * tile.x + tileSize - antiBleed, tileSize * tile.y + antiBleed);
		UVs[1] = new Vector2(tileSize * tile.x + tileSize - antiBleed, tileSize * tile.y + tileSize - antiBleed);
		UVs[2] = new Vector2(tileSize * tile.x + antiBleed, tileSize * tile.y + tileSize - antiBleed);
		UVs[3] = new Vector2(tileSize * tile.x + antiBleed, tileSize * tile.y + antiBleed);
		
		return UVs;
	}

	/**
	 * Assemble an upwards facing face.
	 **/
	public static MeshData CreateFaceUp(Chunk chunk, int x, int y, int z, Block block, MeshData meshData, float size)
	{
		meshData.AddVertex(new Vector3(x - size, y + size, z + size));
		meshData.AddVertex(new Vector3(x + size, y + size, z + size));
		meshData.AddVertex(new Vector3(x + size, y + size, z - size));
		meshData.AddVertex(new Vector3(x - size, y + size, z - size));

		meshData.AddQuadTriangles();

		meshData.uv.AddRange(MeshData.GetFaceUVs(block, Direction.UP));

		return meshData;
	}

	/**
	 * Assemble a downwards facing face.
	 **/
	public static MeshData CreateFaceDown(Chunk chunk, int x, int y, int z, Block block, MeshData meshData, float size)
	{
		meshData.AddVertex(new Vector3(x - size, y - size, z - size));
		meshData.AddVertex(new Vector3(x + size, y - size, z - size));
		meshData.AddVertex(new Vector3(x + size, y - size, z + size));
		meshData.AddVertex(new Vector3(x - size, y - size, z + size));
		
		meshData.AddQuadTriangles();

		meshData.uv.AddRange(MeshData.GetFaceUVs(block, Direction.DOWN));
		
		return meshData;
	}

	/**
	 * Assemble a north facing face.
	 **/
	public static MeshData CreateFaceNorth(Chunk chunk, int x, int y, int z, Block block, MeshData meshData, float size)
	{
		meshData.AddVertex(new Vector3(x + size, y - size, z + size));
		meshData.AddVertex(new Vector3(x + size, y + size, z + size));
		meshData.AddVertex(new Vector3(x - size, y + size, z + size));
		meshData.AddVertex(new Vector3(x - size, y - size, z + size));
		
		meshData.AddQuadTriangles();

		meshData.uv.AddRange(MeshData.GetFaceUVs(block, Direction.NORTH));
		
		return meshData;
	}

	/**
	 * Assemble a south facing face.
	 **/
	public static MeshData CreateFaceSouth(Chunk chunk, int x, int y, int z, Block block, MeshData meshData, float size)
	{
		meshData.AddVertex(new Vector3(x - size, y - size, z - size));
		meshData.AddVertex(new Vector3(x - size, y + size, z - size));
		meshData.AddVertex(new Vector3(x + size, y + size, z - size));
		meshData.AddVertex(new Vector3(x + size, y - size, z - size));
		
		meshData.AddQuadTriangles();

		meshData.uv.AddRange(MeshData.GetFaceUVs(block, Direction.SOUTH));

		return meshData;
	}

	/**
	 * Assemble an east facing face.
	 **/
	public static MeshData CreateFaceEast(Chunk chunk, int x, int y, int z, Block block, MeshData meshData, float size)
	{
		meshData.AddVertex(new Vector3(x + size, y - size, z - size));
		meshData.AddVertex(new Vector3(x + size, y + size, z - size));
		meshData.AddVertex(new Vector3(x + size, y + size, z + size));
		meshData.AddVertex(new Vector3(x + size, y - size, z + size));
		
		meshData.AddQuadTriangles();

		meshData.uv.AddRange(MeshData.GetFaceUVs(block, Direction.EAST));

		return meshData;
	}

	/**
	 * Assemble a west facing face.
	 **/
	public static MeshData CreateFaceWest(Chunk chunk, int x, int y, int z, Block block, MeshData meshData, float size)
	{
		meshData.AddVertex(new Vector3(x - size, y - size, z + size));
		meshData.AddVertex(new Vector3(x - size, y + size, z + size));
		meshData.AddVertex(new Vector3(x - size, y + size, z - size));
		meshData.AddVertex(new Vector3(x - size, y - size, z - size));
		
		meshData.AddQuadTriangles();

		meshData.uv.AddRange(MeshData.GetFaceUVs(block, Direction.WEST));

		return meshData;
	}
}
