using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using VoxelWorld.Terrain;
using VoxelWorld.Data.JSON;

namespace VoxelWorld.IO
{

/**
 * Class to make it more simple to quickly read and write files.
 */
public class FileManager
{
	/** The name of the folder where the save files are stored. */
	public const string saveFolderName = "Saves";
	/** The name of the folder where all mods are stored. */
	public const string modFolderName = "Mods";

	/**
	 * Get the save loctation relative to the games top folder.
	 **/
	public static string SaveLocation(string worldName)
	{
		string saveLocation = saveFolderName + "/" + worldName + "/";

		if(!Directory.Exists(saveLocation))
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

		if(!Directory.Exists(modLocation))
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
	 * Get all files with the given name in the given folder.
	 */
	public static List<string> GetFilesWithName(string folderPath, string fileName)
	{
		List<string> filePaths = new List<string>();

		if(Directory.Exists(folderPath))
		{
			foreach(string filePath in Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories))
			{
				Debug.Log(filePath);
				if(Path.GetFileName(filePath) == fileName)
				{
					filePaths.Add(filePath);
				}
			}
		}

		return filePaths;
	}

	/**
	 * Get all the file paths in the given folder path.
	 * Searching for a specific file extension is optional.
	 */
	public static string[] GetFilesInFolder(string folderPath, string extension = "", SearchOption searchOption = SearchOption.AllDirectories)
	{
		if(extension != "")
		{
			return Directory.GetFiles(folderPath);
		}

		return Directory.GetFiles(folderPath, "*" + extension, searchOption);
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
				if(Path.GetExtension(filePath) != ".png"){continue;}

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

	/**
	 * Load a JSON file at the given location and turn it into
	 * a compatible object of the given type.
	 */
	public static T LoadJSON<T>(string filePath)
	{
		string fileData;

		if(File.Exists(filePath) && Path.GetExtension(filePath) == ".json")
		{
			fileData = File.ReadAllText(filePath);
			return Serialization.DeserializeJson<T>(fileData);
		}

		//TODO: Might result in errors or bugs?
		return default(T);
	}

	/**
	 * Load a text file at the given location.
	 */
	public static string LoadText(string filePath)
	{
		string fileData = "";

		if(File.Exists(filePath))
		{
			fileData = File.ReadAllText(filePath);
			return fileData;
		}

		return fileData;
	}

	/**
	 * Save the given chunk to disk.
	 */
	public static void SaveChunk(Chunk chunk)
	{
		Serialization.SerializeChunk(chunk);
	}

	/**
	 * Load any available data for the given chunk.
	 */
	public static bool LoadChunk(Chunk chunk)
	{
		return Serialization.DeserializeChunk(chunk);
	}
}

}