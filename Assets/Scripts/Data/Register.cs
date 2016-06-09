using UnityEngine;
using System.Collections;
using VoxelWorld.Blocks;
using VoxelWorld.Rendering;

/**
 * Registry handling access and storage of persistent game data.
 **/
public class Register
{
	/** The global tileset containing all the terrain tiles used to render blocks. */
	public static Tileset tileset {get; private set;}

	private static Block[] blocks = new Block[256];

	/**
	 * Register the given block in the registry using the given id
	 * or 0 if none is given. If the given id is already in use then
	 * a search for the first available id will be done.
	 * 
	 * Id should be between 0 and 255.
	 **/
	public static void RegisterBlock(Block block, int id = 0)
	{
		if(blocks[id] != null)
		{
			id = 0;
			while(true)
			{
				id++;
				if(blocks[id] == null)
				{
					blocks[id] = block;
					return;
				}
				else if(id > blocks.Length - 1)
				{
					Debug.LogError("Block limit reached! No more slots!");
					return;
				}
			}
		}

		blocks[id] = block;
	}

	public static Block GetBlockById(int id)
	{
		if(blocks[id] == null)
		{
			return blocks[0];
		}

		return blocks[id];
	}
}
