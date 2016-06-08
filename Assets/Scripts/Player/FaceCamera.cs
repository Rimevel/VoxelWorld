using UnityEngine;
using System.Collections;

namespace VoxelWorld.Player
{

public class FaceCamera : MonoBehaviour
{
	private GameObject player;

	void Start()
	{
		player = GameObject.FindWithTag ("Player");
	}

	void LateUpdate()
	{
		transform.LookAt (player.transform.position, Vector3.up);
	}
}

}