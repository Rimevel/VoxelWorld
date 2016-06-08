using UnityEngine;
using System.Collections;
using VoxelWorld.Blocks;

/**
 * List of static single instances for every block.
 */
public class BlockList
{
	public static Block air = new BlockAir(0);
	public static Block stone = new Block(1);
	public static Block grass = new BlockGrass(2);
	public static Block dirt = new BlockDirt(3);

	public BlockList(){}
}
