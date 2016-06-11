using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VoxelWorld.Data.JSON;

public class GameManager : MonoBehaviour
{
	/** Enum of all the different states the game can be in. */
	public enum GameState
	{
		/** The game is initializing. This is the loading phase before the main menu. */
		INITIALIZING,
		/** In the main menu doing main menu things. */
		MAIN_MENU,
		/** Loading the selected level. */
		LOADING_LEVEL,
		/** The player / server is in world and operating normally. */
		IN_WORLD,
		/** If clientside the player has opened the pause menu. Only active serverside if its a "singleplayer" session. */
		PAUSE_MENU,
		/** The player, server is quiting back to the main menu. */
		EXIT_TO_MAIN_MENU,
		/** The game is about to shut down. */
		QUIT_GAME
	}

	public GameState state {get; private set;}

	private Dictionary<string, ModInfo> modList;

	private void Awake()
	{
		
	}

	private void Update()
	{
		switch(state)
		{
			case GameState.INITIALIZING:

			break;

			case GameState.MAIN_MENU:

			break;

			case GameState.LOADING_LEVEL:

			break;

			case GameState.IN_WORLD:

			break;

			case GameState.PAUSE_MENU:

			break;

			case GameState.EXIT_TO_MAIN_MENU:

			break;

			case GameState.QUIT_GAME:

			break;
		}
	}

	private void LoadModList()
	{
		
	}
}
