using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using VoxelWorld.Terrain;
using VoxelWorld.Util;

public static class UtilSerialization
{
	public static string saveFolderName = "SaveData";

	/**
	 * Get the static save loctation relative to the games top folder.
	 **/
	public static string SaveLocation(String worldName)
	{
		string saveLocation = saveFolderName + "/" + worldName + "/";

		if(!Directory.Exists(saveLocation));
		{
			Directory.CreateDirectory(saveLocation);
		}

		return saveLocation;
	}

	/**
	 * Get a file name for a chunk with the given WorldPos.
	 **/
	public static string FileName(WorldPos chunkLocation)
	{
		string fileName = chunkLocation.x + "," + chunkLocation.y + "," + chunkLocation.z + ".bin";
		return fileName;
	}

	/**
	 * Save and serialize a chunk.
	 **/
	public static void SaveChunk(Chunk chunk)
	{
		Save save = new Save(chunk);
		if(save.empty)
		{
			return;
		}

		string saveFile = SaveLocation(chunk.world.worldName);
		saveFile += FileName(chunk.pos);

		IFormatter formatter = new BinaryFormatter();
		Stream stream = new FileStream(saveFile, FileMode.Create, FileAccess.Write, FileShare.None);
		formatter.Serialize(stream, save);
		stream.Close();
	}

	/**
	 * Unserialize and load a chunk.
	 **/
	public static bool LoadChunk(Chunk chunk)
	{
		string saveFile = SaveLocation(chunk.world.worldName);
		saveFile += FileName(chunk.pos);

		if(!File.Exists(saveFile))
		{
			return false;
		}

		IFormatter formatter = new BinaryFormatter();
		FileStream stream = new FileStream(saveFile, FileMode.Open);

		Save save = (Save)formatter.Deserialize(stream);

		chunk.blocks = save.blocks;
		chunk.meta = save.meta;

		stream.Close();

		return true;
	}
}
