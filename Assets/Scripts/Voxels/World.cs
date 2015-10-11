using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour
{
	public Dictionary<WorldPos, ChunkData> chunks = new Dictionary<WorldPos, ChunkData>();
	public GameObject chunkPrefab;

	public SimplexNoise.Noise noise;
	public TerrainGen terrainGen;

	public string worldName = "world";

	void Start()
	{
		//Change application settings.
		Application.targetFrameRate = 60;
		Cursor.visible = false;
		//Load all blocks.
		new Blocks();
		//Start the terrain generator.
		noise = new SimplexNoise.Noise();
		terrainGen = new TerrainGen(noise);
	}

	/**
	 * Creates a new chunk at the given coords.
	 **/
	public void CreateChunk(int x, int y, int z)
	{
		WorldPos worldPos = new WorldPos(x, y, z);

		ChunkData data = new ChunkData();

		data.pos = worldPos;
		data.world = this;

		chunks.Add(worldPos, data);

		if(!UtilSerialization.LoadChunk(data))
		{
			data = terrainGen.ChunkGen(data);
			MakePhysical(data);
			return;
		}

		data.empty = IsChunkEmpty(data);
		MakePhysical(data);
	}

	/**
	 * Create a new GameObject with a chunk component
	 * and link it to the given chunk data.
	 * Returns false if the chunk is empty. In that case
	 * no visual chunk will be generated.
	 **/
	bool MakePhysical(ChunkData data)
	{
		if(!data.empty)
		{
			Vector3 newPos = new Vector3(data.pos.x, data.pos.y, data.pos.z);
			GameObject newChunkObject = Instantiate(chunkPrefab, newPos, Quaternion.Euler(Vector3.zero)) as GameObject;
			Chunk newChunk = newChunkObject.GetComponent<Chunk>();
			data.chunk = newChunk;
			newChunk.data = data;
			return true;
		}
		return false;
	}

	/**
	 * Get the chunk holding data at the given coords.
	 **/
	public ChunkData GetChunk(int x, int y, int z)
	{
		WorldPos pos = new WorldPos();
		float multiple = ChunkData.chunkSize;
		pos.x = Mathf.FloorToInt(x / multiple) * ChunkData.chunkSize;
		pos.y = Mathf.FloorToInt(y / multiple) * ChunkData.chunkSize;
		pos.z = Mathf.FloorToInt(z / multiple) * ChunkData.chunkSize;

		ChunkData containerChunk = null;

		chunks.TryGetValue(pos, out containerChunk);

		return containerChunk;
	}

	/**
	 * Destroy the chunk at the given coords.
	 **/
	public void DestroyChunk(int x, int y, int z)
	{
		ChunkData chunk = null;
		if(chunks.TryGetValue(new WorldPos(x, y, z), out chunk))
		{
			UtilSerialization.SaveChunk(chunk);
			if(chunk.chunk != null)
			{
				Object.Destroy(chunk.chunk.gameObject);
			}
			chunks.Remove(new WorldPos(x, y, z));
		}
	}

	/**
	 * Get the block at the given coords. Returns BlockAir if null.
	 **/
	public int GetBlock(int x, int y, int z)
	{
		ChunkData chunk = GetChunk(x, y, z);

		if(chunk != null)
		{
			int block = chunk.GetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z);

			return block;
		}
		else
		{
			return 0;
		}
	}

	/**
	 * Set the block at the given coords.
	 * Also updates neighbor chunks if an edge block is changed.
	 **/
	public void SetBlock(int x, int y, int z, int id)
	{
		ChunkData chunk = GetChunk(x, y, z);

		if(chunk != null)
		{
			chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, id);

			if(chunk.empty && !IsChunkEmpty(chunk))
			{
				chunk.empty = false;
				if(MakePhysical(chunk))
				{
					chunk.update = true;
				}
			}
			else
			{
				chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, id);
				chunk.update = true;
			}

			UpdateIfEqual(x - chunk.pos.x, 0, new WorldPos(x - 1, y, z));
			UpdateIfEqual(x - chunk.pos.x, ChunkData.chunkSize - 1, new WorldPos(x + 1, y, z));
			UpdateIfEqual(y - chunk.pos.y, 0, new WorldPos(x, y - 1, z));
			UpdateIfEqual(y - chunk.pos.y, ChunkData.chunkSize - 1, new WorldPos(x, y + 1, z));
			UpdateIfEqual(z - chunk.pos.z, 0, new WorldPos(x, y, z - 1));
			UpdateIfEqual(z - chunk.pos.z, ChunkData.chunkSize - 1, new WorldPos(x, y, z + 1));
		}
	}

	public bool IsChunkEmpty(ChunkData chunk)
	{
		foreach(byte block in chunk.blocks)
		{
			if(block > 0)
			{
				return false;
			}
		}

		return true;
	}

	/**
	 * Rounds a given Vector3 to the coords of a potential block.
	 **/
	public static WorldPos GetBlockPos(Vector3 pos)
	{
		WorldPos blockPos = new WorldPos(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z));
		
		return blockPos;
	}
	
	/**
	 * Gets the block postion of the block hit by the raycast.
	 * If adjecent is true then it returns the block that is adjecent to the surface hit.
	 **/
	public static WorldPos GetBlockPos(RaycastHit hit, bool adjecent = false)
	{
		Vector3 pos = new Vector3();
		pos.x = MoveWithinBlock(hit.point.x, hit.normal.x, adjecent);
		pos.y = MoveWithinBlock(hit.point.y, hit.normal.y, adjecent);
		pos.z = MoveWithinBlock(hit.point.z, hit.normal.z, adjecent);
		
		return GetBlockPos(pos);
	}
	
	static float MoveWithinBlock(float pos, float norm, bool adjecent = false)
	{
		if(pos - (int)pos == 0.5f || pos - (int)pos == -0.5f)
		{
			if(adjecent)
			{
				pos += (norm / 2);
			}
			else
			{
				pos -= (norm / 2);
			}
		}
		
		return (float)pos;
	}
	
	/**
	 * Sets the block where the raycast hits and returns true.
	 * Returns false if no block was found to be set.
	 **/
	public static bool SetBlock(RaycastHit hit, int id, bool adjacent = false)
	{
		Chunk chunk = hit.collider.GetComponent<Chunk>();
		if(chunk == null){return false;}
		
		WorldPos pos = GetBlockPos(hit, adjacent);
		
		chunk.data.world.SetBlock(pos.x, pos.y, pos.z, id);
		
		return true;
	}
	
	/**
	 * Returns the block where the raycast hits.
	 * Returns 0(air) if no block was found.
	 **/
	public static int GetBlock(RaycastHit hit, bool adjacent = false)
	{
		Chunk chunk = hit.collider.GetComponent<Chunk>();
		if(chunk == null){return 0;}
		
		WorldPos pos = GetBlockPos(hit, adjacent);
		
		int block = chunk.data.world.GetBlock(pos.x, pos.y, pos.z);
		
		return block;
	}

	/**
	 * Set a chunk at the given WorldPos to update if the two given values match.
	 **/
	void UpdateIfEqual(int valueOne, int valueTwo, WorldPos pos)
	{
		if(valueOne == valueTwo)
		{
			ChunkData chunk = GetChunk(pos.x, pos.y, pos.z);
			if(chunk != null)
			{
				chunk.update = true;
			}
		}
	}

	void OnApplicationQuit()
	{
		foreach(KeyValuePair<WorldPos, ChunkData> chunk in chunks)
		{
			if(!chunk.Value.needsSaving){continue;}

			UtilSerialization.SaveChunk(chunk.Value);
		}
	}
}