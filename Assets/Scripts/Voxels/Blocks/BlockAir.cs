using UnityEngine;
using System.Collections;
using System;
using VoxelWorld.Terrain;
using VoxelWorld.Rendering;

namespace VoxelWorld.Blocks
{

public class BlockAir : Block
{
	public BlockAir(int id) : base(id)
	{

	}

	public override MeshData GetBlockMesh (Chunk chunk, int x, int y, int z, MeshData meshData)
	{
		return meshData;
	}

	public override bool IsSolid (Direction direction)
	{
		return false;
	}
}

}