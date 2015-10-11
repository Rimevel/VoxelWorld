using UnityEngine;
using System.Collections;

public class BlockDirt : Block
{
	public BlockDirt(int id) : base(id)
	{
		
	}
	
	public override Tile GetTile (Direction direction)
	{
		Tile tile = new Tile();
		tile.x = 1;
		tile.y = 0;
		return tile;
	}
}
