using UnityEngine;
using System.Collections;

public class Modify : MonoBehaviour
{
	Vector2 rot;

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			RaycastHit hit;
			if(Physics.Raycast(transform.position, transform.forward, out hit, 100))
			{
				World.SetBlock(hit, 0);
			}
		}

		rot = new Vector2(rot.x + Input.GetAxis("Mouse X") * 3, rot.y + Input.GetAxis("Mouse Y") * 3);

		transform.localRotation = Quaternion.AngleAxis(rot.x, Vector3.up);
		transform.localRotation *= Quaternion.AngleAxis(rot.y, Vector3.left);

		transform.position += transform.forward * 1 * Input.GetAxis("Vertical");
		transform.position += transform.right * 1 * Input.GetAxis("Horizontal");
	}
}
