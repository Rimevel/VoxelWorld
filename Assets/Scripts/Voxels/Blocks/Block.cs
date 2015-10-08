using UnityEngine;
using System.Collections;
using System;

public class Block
{
	private static float vertCorners = 0.5f;
	public const float TILE_SIZE = 0.5f;

	public bool changed = true;

	public Block(int id)
	{
		Register.RegisterBlock(this, id);
	}

	/**
	 * Get block mesh as a MeshData object.
	 **/
	public virtual MeshData GetBlockMesh(Chunk chunk, int x, int y, int z, MeshData meshData)
	{
		meshData.useRenderDataForCol = true;

		if(!Register.GetBlockById(chunk.GetBlock(x, y + 1, z)).IsSolid(Direction.UP))
		{
			meshData = MeshData.CreateFaceUp(chunk, x, y, z, this, meshData, 0.5f);
		}

		if(!Register.GetBlockById(chunk.GetBlock(x, y - 1, z)).IsSolid(Direction.DOWN))
		{
			meshData = MeshData.CreateFaceDown(chunk, x, y, z, this, meshData, 0.5f);
		}

		if(!Register.GetBlockById(chunk.GetBlock(x, y, z + 1)).IsSolid(Direction.NORTH))
		{
			meshData = MeshData.CreateFaceNorth(chunk, x, y, z, this, meshData, 0.5f);
		}

		if(!Register.GetBlockById(chunk.GetBlock(x, y, z - 1)).IsSolid(Direction.SOUTH))
		{
			meshData = MeshData.CreateFaceSouth(chunk, x, y, z, this, meshData, 0.5f);
		}

		if(!Register.GetBlockById(chunk.GetBlock(x + 1, y, z)).IsSolid(Direction.EAST))
		{
			meshData = MeshData.CreateFaceEast(chunk, x, y, z, this, meshData, 0.5f);
		}

		if(!Register.GetBlockById(chunk.GetBlock(x - 1, y, z)).IsSolid(Direction.WEST))
		{
			meshData = MeshData.CreateFaceWest(chunk, x, y, z, this, meshData, 0.5f);
		}

		return meshData;
	}

	/**
	 * Is the block solid on the given side?
	 **/
	public virtual bool IsSolid(Direction direction)
	{
		return true;
	}

	/**
	 * Get texture tile for the given direction.
	 **/
	public virtual Tile GetTile(Direction direction)
	{
		Tile tile = new Tile();
		tile.x = 0;
		tile.y = 0;

		return tile;
	}
}
