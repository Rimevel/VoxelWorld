using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using VoxelWorld.Terrain;
using VoxelWorld.Util;

namespace VoxelWorld.IO
{

public static class Serialization
{
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
	public static void SerializeChunk(Chunk chunk)
	{
		ChunkSerialized save = new ChunkSerialized(chunk);
		if(save.empty)
		{
			return;
		}

		string saveFile = FileManager.SaveLocation(chunk.world.worldName);
		saveFile += FileName(chunk.pos);

		IFormatter formatter = new BinaryFormatter();
		Stream stream = new FileStream(saveFile, FileMode.Create, FileAccess.Write, FileShare.None);
		formatter.Serialize(stream, save);
		stream.Close();
	}

	/**
	 * Unserialize and load a chunk.
	 **/
	public static bool DeserializeChunk(Chunk chunk)
	{
		string saveFile = FileManager.SaveLocation(chunk.world.worldName);
		saveFile += FileName(chunk.pos);

		if(!File.Exists(saveFile))
		{
			return false;
		}

		IFormatter formatter = new BinaryFormatter();
		FileStream stream = new FileStream(saveFile, FileMode.Open);

		ChunkSerialized save = (ChunkSerialized)formatter.Deserialize(stream);

		chunk.blocks = save.blocks;
		chunk.meta = save.meta;

		stream.Close();

		return true;
	}

	/**
	 * Deserialize the given string into a compatible given class.
	 */
	public static T DeserializeJson<T>(string JSONData)
	{
		 return JsonUtility.FromJson<T>(JSONData);
	}

	/**
	 * Serialize a compatible given class into a string of JSON data.
	 */
	public static string SerializeJson(object JSONData)
	{
		return JsonUtility.ToJson(JSONData);
	}
}

}