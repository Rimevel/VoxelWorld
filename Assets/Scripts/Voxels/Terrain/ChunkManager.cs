using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChunkManager : MonoBehaviour
{
	public World world;

	List<WorldPos> updateList = new List<WorldPos>();
	List<WorldPos> buildList = new List<WorldPos>();

	int timer = 0;

	static  WorldPos[] chunkPositions= {
		new WorldPos( 0, 0,  0), new WorldPos(-1, 0,  0), new WorldPos( 0, 0, -1), new WorldPos( 0, 0,  1), new WorldPos( 1, 0,  0),
		new WorldPos(-1, 0, -1), new WorldPos(-1, 0,  1), new WorldPos( 1, 0, -1), new WorldPos( 1, 0,  1), new WorldPos(-2, 0,  0),
		new WorldPos( 0, 0, -2), new WorldPos( 0, 0,  2), new WorldPos( 2, 0,  0), new WorldPos(-2, 0, -1), new WorldPos(-2, 0,  1),
		new WorldPos(-1, 0, -2), new WorldPos(-1, 0,  2), new WorldPos( 1, 0, -2), new WorldPos( 1, 0,  2), new WorldPos( 2, 0, -1),
		new WorldPos( 2, 0,  1), new WorldPos(-2, 0, -2), new WorldPos(-2, 0,  2), new WorldPos( 2, 0, -2), new WorldPos( 2, 0,  2),
		new WorldPos(-3, 0,  0), new WorldPos( 0, 0, -3), new WorldPos( 0, 0,  3), new WorldPos( 3, 0,  0), new WorldPos(-3, 0, -1),
		new WorldPos(-3, 0,  1), new WorldPos(-1, 0, -3), new WorldPos(-1, 0,  3), new WorldPos( 1, 0, -3), new WorldPos( 1, 0,  3),
		new WorldPos( 3, 0, -1), new WorldPos( 3, 0,  1), new WorldPos(-3, 0, -2), new WorldPos(-3, 0,  2), new WorldPos(-2, 0, -3),
		new WorldPos(-2, 0,  3), new WorldPos( 2, 0, -3), new WorldPos( 2, 0,  3), new WorldPos( 3, 0, -2), new WorldPos( 3, 0,  2),
		new WorldPos(-4, 0,  0), new WorldPos( 0, 0, -4), new WorldPos( 0, 0,  4), new WorldPos( 4, 0,  0), new WorldPos(-4, 0, -1),
		new WorldPos(-4, 0,  1), new WorldPos(-1, 0, -4), new WorldPos(-1, 0,  4), new WorldPos( 1, 0, -4), new WorldPos( 1, 0,  4),
		new WorldPos( 4, 0, -1), new WorldPos( 4, 0,  1), new WorldPos(-3, 0, -3), new WorldPos(-3, 0,  3), new WorldPos( 3, 0, -3),
		new WorldPos( 3, 0,  3), new WorldPos(-4, 0, -2), new WorldPos(-4, 0,  2), new WorldPos(-2, 0, -4), new WorldPos(-2, 0,  4),
		new WorldPos( 2, 0, -4), new WorldPos( 2, 0,  4), new WorldPos( 4, 0, -2), new WorldPos( 4, 0,  2), new WorldPos(-5, 0,  0),
		new WorldPos(-4, 0, -3), new WorldPos(-4, 0,  3), new WorldPos(-3, 0, -4), new WorldPos(-3, 0,  4), new WorldPos( 0, 0, -5),
		new WorldPos( 0, 0,  5), new WorldPos( 3, 0, -4), new WorldPos( 3, 0,  4), new WorldPos( 4, 0, -3), new WorldPos( 4, 0,  3),
		new WorldPos( 5, 0,  0), new WorldPos(-5, 0, -1), new WorldPos(-5, 0,  1), new WorldPos(-1, 0, -5), new WorldPos(-1, 0,  5),
		new WorldPos( 1, 0, -5), new WorldPos( 1, 0,  5), new WorldPos( 5, 0, -1), new WorldPos( 5, 0,  1), new WorldPos(-5, 0, -2),
		new WorldPos(-5, 0,  2), new WorldPos(-2, 0, -5), new WorldPos(-2, 0,  5), new WorldPos( 2, 0, -5), new WorldPos( 2, 0,  5),
		new WorldPos( 5, 0, -2), new WorldPos( 5, 0,  2), new WorldPos(-4, 0, -4), new WorldPos(-4, 0,  4), new WorldPos( 4, 0, -4),
		new WorldPos( 4, 0,  4), new WorldPos(-5, 0, -3), new WorldPos(-5, 0,  3), new WorldPos(-3, 0, -5), new WorldPos(-3, 0,  5),
		new WorldPos( 3, 0, -5), new WorldPos( 3, 0,  5), new WorldPos( 5, 0, -3), new WorldPos( 5, 0,  3), new WorldPos(-6, 0,  0),
		new WorldPos( 0, 0, -6), new WorldPos( 0, 0,  6), new WorldPos( 6, 0,  0), new WorldPos(-6, 0, -1), new WorldPos(-6, 0,  1),
		new WorldPos(-1, 0, -6), new WorldPos(-1, 0,  6), new WorldPos( 1, 0, -6), new WorldPos( 1, 0,  6), new WorldPos( 6, 0, -1),
		new WorldPos( 6, 0,  1), new WorldPos(-6, 0, -2), new WorldPos(-6, 0,  2), new WorldPos(-2, 0, -6), new WorldPos(-2, 0,  6),
		new WorldPos( 2, 0, -6), new WorldPos( 2, 0,  6), new WorldPos( 6, 0, -2), new WorldPos( 6, 0,  2), new WorldPos(-5, 0, -4),
		new WorldPos(-5, 0,  4), new WorldPos(-4, 0, -5), new WorldPos(-4, 0,  5), new WorldPos( 4, 0, -5), new WorldPos( 4, 0,  5),
		new WorldPos( 5, 0, -4), new WorldPos( 5, 0,  4), new WorldPos(-6, 0, -3), new WorldPos(-6, 0,  3), new WorldPos(-3, 0, -6),
		new WorldPos(-3, 0,  6), new WorldPos( 3, 0, -6), new WorldPos( 3, 0,  6), new WorldPos( 6, 0, -3), new WorldPos( 6, 0,  3),
		new WorldPos(-7, 0,  0), new WorldPos( 0, 0, -7), new WorldPos( 0, 0,  7), new WorldPos( 7, 0,  0), new WorldPos(-7, 0, -1),
		new WorldPos(-7, 0,  1), new WorldPos(-5, 0, -5), new WorldPos(-5, 0,  5), new WorldPos(-1, 0, -7), new WorldPos(-1, 0,  7),
		new WorldPos( 1, 0, -7), new WorldPos( 1, 0,  7), new WorldPos( 5, 0, -5), new WorldPos( 5, 0,  5), new WorldPos( 7, 0, -1),
		new WorldPos( 7, 0,  1), new WorldPos(-6, 0, -4), new WorldPos(-6, 0,  4), new WorldPos(-4, 0, -6), new WorldPos(-4, 0,  6),
		new WorldPos( 4, 0, -6), new WorldPos( 4, 0,  6), new WorldPos( 6, 0, -4), new WorldPos( 6, 0,  4), new WorldPos(-7, 0, -2),
		new WorldPos(-7, 0,  2), new WorldPos(-2, 0, -7), new WorldPos(-2, 0,  7), new WorldPos( 2, 0, -7), new WorldPos( 2, 0,  7),
		new WorldPos( 7, 0, -2), new WorldPos( 7, 0,  2), new WorldPos(-7, 0, -3), new WorldPos(-7, 0,  3), new WorldPos(-3, 0, -7),
		new WorldPos(-3, 0,  7), new WorldPos( 3, 0, -7), new WorldPos( 3, 0,  7), new WorldPos( 7, 0, -3), new WorldPos( 7, 0,  3),
		new WorldPos(-6, 0, -5), new WorldPos(-6, 0,  5), new WorldPos(-5, 0, -6), new WorldPos(-5, 0,  6), new WorldPos( 5, 0, -6),
		new WorldPos( 5, 0,  6), new WorldPos( 6, 0, -5), new WorldPos( 6, 0,  5)
	};

	void Update()
	{
		if(DeleteChunks())
		{
			return;
		}
		FindChunksToLoad();
		LoadAndRenderChunks();
	}

	void FindChunksToLoad()
	{
		WorldPos playerPos = new WorldPos();
		playerPos.x = Mathf.FloorToInt(transform.position.x / ChunkData.chunkSize) * ChunkData.chunkSize;
		playerPos.y = Mathf.FloorToInt(transform.position.y / ChunkData.chunkSize) * ChunkData.chunkSize;
		playerPos.z = Mathf.FloorToInt(transform.position.z / ChunkData.chunkSize) * ChunkData.chunkSize;

		if(updateList.Count == 0)
		{
			for(int i = 0; i < chunkPositions.Length; i++)
			{
				WorldPos newChunkPos = new WorldPos();
				newChunkPos.x = chunkPositions[i].x * ChunkData.chunkSize + playerPos.x;
				newChunkPos.y = 0;
				newChunkPos.z = chunkPositions[i].z * ChunkData.chunkSize + playerPos.z;

				ChunkData data = world.GetChunk(newChunkPos.x, newChunkPos.y, newChunkPos.z);

				if(data != null && (data.rendered || updateList.Contains(newChunkPos)))
				{
					continue;
				}

				for(int y = -4; y < 4; y++)
				{
					for (int x = newChunkPos.x - ChunkData.chunkSize; x <= newChunkPos.x + ChunkData.chunkSize; x += ChunkData.chunkSize)
					{
						for (int z = newChunkPos.z - ChunkData.chunkSize; z <= newChunkPos.z + ChunkData.chunkSize; z += ChunkData.chunkSize)
						{
							buildList.Add(new WorldPos(x, y * ChunkData.chunkSize, z));
						}
					}
					updateList.Add(new WorldPos(newChunkPos.x, y * ChunkData.chunkSize, newChunkPos.z));
				}

				return;
			}
		}
	}

	void BuildChunk(WorldPos pos)
	{
		if (world.GetChunk(pos.x,pos.y,pos.z) == null)
		{
			world.CreateChunk(pos.x,pos.y,pos.z);
		}
	}

	void LoadAndRenderChunks()
	{
		if (buildList.Count != 0)
		{
			for (int i = 0; i < buildList.Count && i < 8; i++)
			{
				BuildChunk(buildList[0]);
				buildList.RemoveAt(0);
			}

			return;
		}

		if(updateList.Count != 0)
		{
			ChunkData chunk = world.GetChunk(updateList[0].x, updateList[0].y, updateList[0].z);
			if (chunk != null)
			{
				chunk.update = true;
			}
			updateList.RemoveAt(0);
		}
	}

	bool DeleteChunks()
	{
		if(timer == 10)
		{
			var chunksToDelete = new List<WorldPos>();
			foreach(var chunk in world.chunks)
			{
				Vector3 v1 = new Vector3(chunk.Value.pos.x, 0, chunk.Value.pos.z);
				Vector3 v2 = new Vector3(transform.position.x, 0, transform.position.z);
				float distance = Vector3.Distance(v1, v2);

				if(distance > 256)
				{
					chunksToDelete.Add(chunk.Key);
				}
			}

			foreach(var chunk in chunksToDelete)
			{
				world.DestroyChunk(chunk.x, chunk.y, chunk.z);
			}

			timer = 0;
			return true;
		}

		timer++;
		return false;
	}
}
