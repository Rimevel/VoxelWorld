using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Save
{
	public byte[ , , ] blocks = new byte[Chunk.chunkSize, Chunk.chunkSize, Chunk.chunkSize];
	public byte[ , , ] meta = new byte[Chunk.chunkSize, Chunk.chunkSize, Chunk.chunkSize];
	[NonSerialized]
	public bool empty = false;

	public Save(Chunk chunk)
	{
		if(chunk == null){empty = true; return;}

		for(int x = 0; x < Chunk.chunkSize; x++)
		{
			for(int y = 0; y < Chunk.chunkSize; y++)
			{
				for(int z = 0; z < Chunk.chunkSize; z++)
				{
					blocks[x, y, z] = chunk.blocks[x, y, z];
					meta[x, y, z] = chunk.meta[x, y, z];
				}
			}
		}
	}
}
