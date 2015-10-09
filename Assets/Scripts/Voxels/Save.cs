using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Save
{
	public byte[ , , ] blocks = new byte[ChunkData.chunkSize, ChunkData.chunkSize, ChunkData.chunkSize];
	public byte[ , , ] meta = new byte[ChunkData.chunkSize, ChunkData.chunkSize, ChunkData.chunkSize];
	[NonSerialized]
	public bool empty = false;

	public Save(ChunkData chunk)
	{
		if(chunk == null || chunk.empty == true){empty = true; return;}

		for(int x = 0; x < ChunkData.chunkSize; x++)
		{
			for(int y = 0; y < ChunkData.chunkSize; y++)
			{
				for(int z = 0; z < ChunkData.chunkSize; z++)
				{
					blocks[x, y, z] = chunk.blocks[x, y, z];
					meta[x, y, z] = chunk.meta[x, y, z];
				}
			}
		}
	}
}
