using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class FileManager
{
	/** The name of the folder where the save files are stored. */
	private const string saveFolderName = "Saves";
	/** The name of the folder where all mods are stored. */
	private const string modFolderName = "Mods";

	/**
	 * Get the save loctation relative to the games top folder.
	 **/
	public static string SaveLocation(string worldName)
	{
		string saveLocation = saveFolderName + "/" + worldName + "/";

		if(!Directory.Exists(saveLocation));
		{
			Directory.CreateDirectory(saveLocation);
		}

		return saveLocation;
	}

	/**
	 * Get the directory location of the mod with the given name.
	 */
	public static string ModLocation(string modName)
	{
		string modLocation = modFolderName + "/" + modName + "/";

		if(!Directory.Exists(modLocation));
		{
			Directory.CreateDirectory(modLocation);
		}

		return modLocation;
	}

	/**
	 * Get the file name for a chunk with the given WorldPos.
	 **/
	public static string FileName(WorldPos chunkLocation)
	{
		string fileName = chunkLocation.x + "," + chunkLocation.y + "," + chunkLocation.z + ".bin";
		return fileName;
	}

	/**
	 * Get a Dictionary with the filenames and textures from a given folder.
	 * Only loads .PNG files.
	 */
	public static Dictionary<string, Texture2D> LoadTextures(string folderLocation)
	{
		Dictionary<string, Texture2D> list = new Dictionary<string, Texture2D>();

		if(Directory.Exists(folderLocation))
		{
			foreach(string filePath in Directory.GetFiles(folderLocation))
			{
				if(Path.GetExtension(filePath) != ".PNG"){continue;}

				Texture2D tex = new Texture2D(2, 2, TextureFormat.DXT5, false);
				byte[] fileData = File.ReadAllBytes(filePath);
				tex.LoadImage(fileData);

				list.Add(Path.GetFileNameWithoutExtension(filePath), tex);
			}
		}

		return list;
	}

	/**
	 * Get the texture from the file path given and load it
	 * returning a Texture2D.
	 */
	public static Texture2D LoadTexture(string filePath)
	{
		Texture2D tex = null;
		byte[] fileData;

		if(File.Exists(filePath))
		{
			fileData = File.ReadAllBytes(filePath);
			tex = new Texture2D(2, 2, TextureFormat.DXT5, false);
			tex.LoadImage(fileData);
		}

		return tex;
	}
}

