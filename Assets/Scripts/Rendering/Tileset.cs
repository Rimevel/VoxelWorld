using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace VoxelWorld.Rendering
{

/**
 * A texture container with management functions to select specific
 * sections of the texture linked with a name.
 */
public class Tileset
{
	/** The width and height of each tile in the Tileset in pixels. */
	public int tileSize {get; private set;}

	/** The tileset texture. */
	public Texture2D texture {get; private set;}
	/**
	 * Dictionary matching texture names with their position
	 * on the tileset.
	 */
	private Dictionary<string, Tile> tiles;

	/** The maximum width or height the tileset texture is allowed to be. */
	private const int MAX_SPAN = 4096;

	public Tileset(int tileSize = 16, int startWidth = 64, int startHeight = 64)
	{
		this.texture = new Texture2D(startWidth, startHeight);
		this.tiles = new Dictionary<string, Tile>();
		this.tileSize = Mathf.Clamp(tileSize, 16, 128);
	}

	/**
	 * Get the Tile positon of the tile with the given name.
	 * Returns the tile in the top left corner (0, 0) if no
	 * tile with the given name was found,
	 */
	public Tile GetTilePos(string textureName)
	{
		Tile tile = new Tile(0, 0);
		tiles.TryGetValue(textureName, out tile);
		return tile;
	}

	/**
	 * Set the given textures and link to the given names.
	 */
	public void SetTextures(string[] names, Texture2D[] textures)
	{
		tiles.Clear();
		//List of power of 2 "levels" to test against.
		int[] powers = {2, 4, 8, 16, 32, 64, 128, 256, 512, 1024};
		//The power level found to be optimal.
		int powerLevel = 0;

		foreach(int power in powers)
		{
			//Check so the tiles are not bigger than the allowed max size.
			if(power * tileSize <= MAX_SPAN)
			{
				//Can the textures fit inside this power of two grid?
				if(textures.Length <= power * power)
				{
					//Extra safety check so the final size will be fine.
					if(texture.width < power * tileSize)
					{
						texture = new Texture2D(power * tileSize, power * tileSize);
					}

					//Pick the power level.
					powerLevel = power;
				}
				else
				{
					break;
				}
			}
		}

		int currentTile = 0;
		int tileX = 0;
		int tileY = 0;

		//Loop through all the textures and slot them into the tileset texture.
		foreach(Texture2D tex in textures)
		{
			//Add the tile data to the tiles dictionary.
			tiles.Add(names[currentTile], new Tile(tileX, tileY));

			//Loop through all the pixels in the tile textures and add them one and one
			//In the correct spots on the tileset texture.
			for(int y = 0; y < tileSize; y++)
			{
				for(int x = 0; x < tileSize; x++)
				{
					texture.SetPixel(tileX + x, tileY + y, textures[currentTile].GetPixel(x, y));
				}
			}

			//Adjust the tile positon being worked at.
			if(tileX < powerLevel)
			{
				tileX++;
			}
			else
			{
				tileX = 0;
				tileY++;
			}

			//Increment the time counter by one.
			currentTile++;
		}

		//Apply the texture changes.
		texture.Apply();
	}
}

}