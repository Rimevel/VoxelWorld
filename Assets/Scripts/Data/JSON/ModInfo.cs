using UnityEngine;
using System.Collections;
using System;

namespace VoxelWorld.Data.JSON
{

[Serializable]
public class ModInfo
{
	/** The mod id of the mod. This is the non-fancy name used to identify the mod itself. Should be unique! */
	public string modId;
	/** The fancy display name of the mod. */
	public string modName;
	/** Version of the mod. */
	public string version;
	/** Text describing what the mod do. */
	public string description;
	/** Name of the author(s) of the mod. */
	public string author;
}

}