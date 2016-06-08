using UnityEngine;
using System.Collections;
using VoxelWorld.Terrain;

namespace VoxelWorld.Terrain.Generation
{

public class TerrainGen
{
	protected int stoneBaseHeight = -20;
	protected float stoneBaseNoise = 0.03f;
	protected int stoneBaseNoiseHeight = 10;

	protected int stoneMountainHeight = 10;
	protected float stoneMountainFrequency = 0.008f;
	protected int stoneMinHeight = 0;

	protected int dirtBaseHeight = 1;
	protected float dirtNoise = 0.04f;
	protected int dirtNoiseHeight = 2;

	public TerrainGen(Noise noise)
	{
		noiseGen = noise;
		//treeStructure = new StructureTree();
	}

	Noise noiseGen;
	TerrainGen terrainGen;

	/**
	 * Generate data for the given Chunk object.
	 **/
	public virtual Chunk ChunkGen(Chunk chunk)
	{
		chunk.empty = true;

		for (int x = 0; x < Chunk.chunkSize; x++)
		{
			for (int z = 0; z < Chunk.chunkSize; z++)
			{
				chunk = GenerateTerrain(chunk, x, z);
			}
		}

		for (int x = -3; x < Chunk.chunkSize + 3; x++)
		{
			for (int z = -3; z < Chunk.chunkSize + 3; z++)
			{
				//CreateTreeIfValid(x, z, chunk);
			}
		}

		return chunk;
	}

	/**
	 * Get the thickness of the base stone layer.
	 **/
	protected virtual int LayerStoneBase(int x, int z)
	{
		int stoneHeight = stoneBaseHeight;
		stoneHeight += GetNoise(x, 0, z, stoneMountainFrequency, stoneMountainHeight, 1.6f);
		stoneHeight += GetNoise(x, 1000, z, 0.03f, 8, 1) * 2;

		if (stoneHeight < stoneMinHeight)
			return stoneMinHeight;

		return stoneHeight;
	}

	/**
	 * Get the thickness of the top stone layer.
	 **/
	protected virtual int LayerStoneNoise(int x, int z)
	{
		return GetNoise(x, 0, z, stoneBaseNoise, stoneBaseNoiseHeight, 1);
	}

	/**
	 * Get the thickness of the dirt layer.
	 **/
	protected virtual int LayerDirt(int x, int z)
	{
		int dirtHeight = dirtBaseHeight;
		dirtHeight += GetNoise(x, 100, z, dirtNoise, dirtNoiseHeight, 1);

		return dirtHeight;
	}

	/**
	 * Generate a column of blocks in the given 2d coords inside the given Chunk.
	 **/
	protected virtual Chunk GenerateTerrain(Chunk chunk, int x, int z)
	{
		int stoneHeight = LayerStoneBase(chunk.pos.x + x, chunk.pos.z + z);
		stoneHeight += LayerStoneNoise(chunk.pos.x + x, chunk.pos.z + z);

		int dirtHeight = stoneHeight + LayerDirt(chunk.pos.x + x, chunk.pos.z + z);
		//CreateTreeIfValid(x, z, chunk, dirtHeight);

		for (int y = 0; y < Chunk.chunkSize; y++)
		{
			if (y + chunk.pos.y <= stoneHeight)
			{
				SetBlock(chunk, 1, new WorldPos(x, y, z));
				if(chunk.empty){chunk.empty = false;}
			}
			else if (y + chunk.pos.y < dirtHeight)
			{
				SetBlock(chunk, 3, new WorldPos(x, y, z));
				if(chunk.empty){chunk.empty = false;}
			}
			else if (y + chunk.pos.y == dirtHeight)
			{
				SetBlock(chunk, 2, new WorldPos(x, y, z));
				if(chunk.empty){chunk.empty = false;}
			}
			else
			{
				SetBlock(chunk, 0, new WorldPos(x, y, z));
			}
		}

		return chunk;
	}

	void SetBlock(Chunk chunk, int blockId, WorldPos pos, bool replaceBlocks = false)
	{
		if (Chunk.InRange(pos.x) && Chunk.InRange(pos.y) && Chunk.InRange(pos.z))
		{
			if (replaceBlocks || chunk.GetBlock(pos.x, pos.y, pos.z) == 0)
			{
				chunk.SetBlock(pos.x, pos.y, pos.z, blockId);
			}
		}
	}

	public int GetNoise(int x, int y, int z, float scale, int max, float power)
	{
		float noise = (noiseGen.Generate(x * scale, y * scale, z * scale) + 1f) * (max / 2f);

		noise = Mathf.Pow(noise, power);

		return Mathf.FloorToInt(noise);
	}

	//	void CreateTreeIfValid(int x, int z, Chunk chunk)
	//	{
	//		if (GetNoise(x + chunk.pos.x, -10000, z + chunk.pos.z, 100, 100, 1) < 10)
	//		{
	//			if (GetNoise(x + chunk.pos.x, 10000, z + chunk.pos.z, 100, 100, 1) < 15)
	//			{
	//				int terrainHeight = LayerStoneBase(x + chunk.pos.x, z + chunk.pos.z);
	//				terrainHeight += LayerStoneNoise(x + chunk.pos.x, z + chunk.pos.z);
	//				terrainHeight += LayerDirt(x + chunk.pos.x, z + chunk.pos.z);
	//				
	//				treeStructure.OldBuild(chunk.world, chunk.pos, new BlockPos(x, terrainHeight - chunk.pos.y, z), this);
	//			}
	//		}
	//	}

}

}
