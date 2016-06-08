using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VoxelWorld.Terrain;
using VoxelWorld.Blocks;

namespace VoxelWorld.Rendering
{

/**
 * General class acting as a container for raw mesh data.
 **/
public class MeshData
{
	/** List of all vertices in this mesh. */
	public List<Vector3> vertices {get; private set;}
	/** List of all triangles in this mesh. */
	public List<int> triangles {get; private set;}
	/** List of all uv points in this mesh. */
	public List<Vector2> uv {get; private set;}
	/** List of all vertice colors in this mesh. */
	public List<Color32> colors {get; private set;}

	/** Generate vertice colors for all vertices? */
	public bool useVerticeColors;

	public MeshData()
	{
		/** List of all vertices in this mesh. */
		vertices = new List<Vector3>();
		/** List of all triangles in this mesh. */
		triangles = new List<int>();
		/** List of all uv points in this mesh. */
		uv = new List<Vector2>();
		/** List of all vertice colors in this mesh. */
		colors = new List<Color32>();
	}

	/**
	 * Create a quad from the latest vertice data and add it to the mesh.
	 * Also handles quad generation for collision if enabled.
	 */
	public void AddQuadTriangles()
	{
		triangles.Add(vertices.Count - 4);
		triangles.Add(vertices.Count - 3);
		triangles.Add(vertices.Count - 2);

		triangles.Add(vertices.Count - 4);
		triangles.Add(vertices.Count - 2);
		triangles.Add(vertices.Count - 1);
	}

	/**
	 * Add a vertex to the mesh.
	 * Also adds it to the collision if enabled.
	 **/
	public void AddVertex(Vector3 vertex)
	{
		vertices.Add(vertex);
	}

	/**
	 * Add a triangle to the mesh.
	 * Also adds it to the collision if enabled.
	 **/
	public void AddTriangle(int tri)
	{
		triangles.Add(tri);
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

}