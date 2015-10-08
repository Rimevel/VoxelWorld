using UnityEngine;
using System.Collections;

public class FaceCamera : MonoBehaviour {

	private GameObject player;

	void Start() {
		player = GameObject.FindWithTag ("Player");
	}

	void LateUpdate() {
		transform.LookAt (player.transform.position, Vector3.up);
	}
}
