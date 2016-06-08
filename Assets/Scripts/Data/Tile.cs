using UnityEngine;
using System.Collections;

/**
 * Struct representing a a tile on a tile sheet.
 **/
public struct Tile
{
	public int x;
	public int y;

	public Tile(int x, int y)
	{
		this.x = x;
		this.y = y;
	}
}
