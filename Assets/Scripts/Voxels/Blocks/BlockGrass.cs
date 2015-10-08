using UnityEngine;
using System.Collections;
using System;

public class BlockGrass : Block
{
	public BlockGrass(int id) : base(id)
	{

	}

	public override Tile GetTile (Direction direction)
	{
		Tile tile = new Tile();

		switch(direction)
		{
			case Direction.UP:
				tile.x = 2;
				tile.y = 0;
				return tile;
			case Direction.DOWN:
				tile.x = 1;
				tile.y = 0;
				return tile;
		}

		tile.x = 3;
		tile.y = 0;

		return tile;
	}
}
