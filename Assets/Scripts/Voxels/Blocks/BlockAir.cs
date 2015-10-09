using UnityEngine;
using System.Collections;
using System;

public class BlockAir : Block
{
	public BlockAir(int id) : base(id)
	{

	}

	public override MeshData GetBlockMesh (ChunkData chunk, int x, int y, int z, MeshData meshData)
	{
		return meshData;
	}

	public override bool IsSolid (Direction direction)
	{
		return false;
	}
}
