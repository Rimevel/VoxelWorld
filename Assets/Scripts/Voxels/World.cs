using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour
{
	public Dictionary<WorldPos, Chunk> chunks = new Dictionary<WorldPos, Chunk>();
	public GameObject chunkPrefab;

	public string worldName = "world";

	void Start()
	{
		//Change application settings.
		Application.targetFrameRate = 60;
		Cursor.visible = false;
		//Load all blocks.
		new Blocks();
	}

	/**
	 * Creates a new chunk at the given coords.
	 **/
	public void CreateChunk(int x, int y, int z)
	{
		WorldPos worldPos = new WorldPos(x, y, z);

		GameObject newChunkObject = Instantiate(chunkPrefab, new Vector3(x, y, z), Quaternion.Euler(Vector3.zero)) as GameObject;

		Chunk newChunk = newChunkObject.GetComponent<Chunk>();

		newChunk.pos = worldPos;
		newChunk.world = this;

		chunks.Add(worldPos, newChunk);

		if(!UtilSerialization.LoadChunk(newChunk))
		{
			var terrainGen = new TerrainGen();
			newChunk = terrainGen.ChunkGen(newChunk);
		}
	}

	/**
	 * Get the chunk holding data at the given coords.
	 **/
	public Chunk GetChunk(int x, int y, int z)
	{
		WorldPos pos = new WorldPos();
		float multiple = Chunk.chunkSize;
		pos.x = Mathf.FloorToInt(x / multiple) * Chunk.chunkSize;
		pos.y = Mathf.FloorToInt(y / multiple) * Chunk.chunkSize;
		pos.z = Mathf.FloorToInt(z / multiple) * Chunk.chunkSize;

		Chunk containerChunk = null;

		chunks.TryGetValue(pos, out containerChunk);

		return containerChunk;
	}

	/**
	 * Destroy the chunk at the given coords.
	 **/
	public void DestroyChunk(int x, int y, int z)
	{
		Chunk chunk = null;
		if(chunks.TryGetValue(new WorldPos(x, y, z), out chunk))
		{
			UtilSerialization.SaveChunk(chunk);
			Object.Destroy(chunk.gameObject);
			chunks.Remove(new WorldPos(x, y, z));
		}
	}

	/**
	 * Get the block at the given coords. Returns BlockAir if null.
	 **/
	public int GetBlock(int x, int y, int z)
	{
		Chunk containerChunk = GetChunk(x, y, z);

		if(containerChunk != null)
		{
			int block = containerChunk.GetBlock(x - containerChunk.pos.x, y - containerChunk.pos.y, z - containerChunk.pos.z);

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
		Chunk chunk = GetChunk(x, y, z);

		if(chunk != null)
		{
			chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, id);
			chunk.update = true;

			UpdateIfEqual(x - chunk.pos.x, 0, new WorldPos(x - 1, y, z));
			UpdateIfEqual(x - chunk.pos.x, Chunk.chunkSize - 1, new WorldPos(x + 1, y, z));
			UpdateIfEqual(y - chunk.pos.y, 0, new WorldPos(x, y - 1, z));
			UpdateIfEqual(y - chunk.pos.y, Chunk.chunkSize - 1, new WorldPos(x, y + 1, z));
			UpdateIfEqual(z - chunk.pos.z, 0, new WorldPos(x, y, z - 1));
			UpdateIfEqual(z - chunk.pos.z, Chunk.chunkSize - 1, new WorldPos(x, y, z + 1));
		}
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
		
		chunk.world.SetBlock(pos.x, pos.y, pos.z, id);
		
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
		
		int block = chunk.world.GetBlock(pos.x, pos.y, pos.z);
		
		return block;
	}

	/**
	 * Set a chunk at the given WorldPos to update if the two given values match.
	 **/
	void UpdateIfEqual(int valueOne, int valueTwo, WorldPos pos)
	{
		if(valueOne == valueTwo)
		{
			Chunk chunk = GetChunk(pos.x, pos.y, pos.z);
			if(chunk != null)
			{
				chunk.update = true;
			}
		}
	}

	void OnApplicationQuit()
	{
		foreach(KeyValuePair<WorldPos, Chunk> chunk in chunks)
		{
			if(!chunk.Value.needsSaving){continue;}

			UtilSerialization.SaveChunk(chunk.Value);
		}
	}
}
