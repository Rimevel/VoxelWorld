using UnityEngine;
using System.Collections;

public class TerrainGen
{
	float stoneBaseHeight = -24;
	float stoneBaseNoise = 0.015f;
	float stoneBaseNoiseHeight = 4;

	float stoneMountainHeight = 48;
	float stoneMountainFrequency = 0.08f;
	float stoneMinHeight = -12;

	float dirtBaseHeight = 1;
	float dirtNoise = 0.04f;
	float dirtNoiseHeight = 3;

	public ChunkData ChunkGen(ChunkData chunk)
	{
		for(int x = chunk.pos.x; x < chunk.pos.x + ChunkData.chunkSize; x++)
		{
			for(int z = chunk.pos.z; z < chunk.pos.z + ChunkData.chunkSize; z++)
			{
				chunk = ChunkColumnGen(chunk, x, z);
			}
		}

		return chunk;
	}

	public static int GetNoise(int x, int y, int z, float scale, int max)
	{
		return Mathf.FloorToInt((Noise.Generate(x * scale, y * scale, z * scale) + 1f) + (max / 2f));
	}

	public ChunkData ChunkColumnGen(ChunkData chunk, int x, int z)
	{
		bool empty = true;
		int stoneHeight = Mathf.FloorToInt(stoneBaseHeight);
		stoneHeight += GetNoise(x, 0, z, stoneMountainFrequency, Mathf.FloorToInt(stoneMountainHeight));

		if(stoneHeight < stoneMinHeight)
		{
			stoneHeight = Mathf.FloorToInt(stoneMinHeight);
		}

		stoneHeight += GetNoise(x, 0, z, stoneBaseNoise, Mathf.FloorToInt(stoneBaseNoiseHeight));

		int dirtHeight = stoneHeight + Mathf.FloorToInt(dirtBaseHeight);
		dirtHeight += GetNoise(x, 100, z, dirtNoise, Mathf.FloorToInt(dirtNoiseHeight));

		for(int y = chunk.pos.y; y < chunk.pos.y + ChunkData.chunkSize; y++)
		{
			if(y <= stoneHeight)
			{
				chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, 1);
				if(empty){empty = false;}
			}
			else if(y <= dirtHeight)
			{
				chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, 2);
				if(empty){empty = false;}
			}
			else
			{
				chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, 0);
			}
		}

		chunk.empty = empty;

		return chunk;
	}
}
