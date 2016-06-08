using UnityEngine;
using System.Collections;
using VoxelWorld.Terrain;

namespace VoxelWorld.Player
{

[RequireComponent(typeof(CharacterController))]
public class CharacterScript : MonoBehaviour
{
	public float speed = 5.0f;
	public float rotateSpeed = 1.0f;
	public float jumpSpeed = 4.0f;

	public float gravity = 20.0f;

	public float camMaxUp = -85f;
	public float camMaxDown = 85f;

	static private int health = 3 ;
	static private float timeLeft = 0f;

	private float mY = 0f;

	private Vector3 moveDirection = Vector3.zero;

	// Use this for initialization
	void Start ()
	{

	}

	// Update is called once per frame
	void FixedUpdate ()
	{
		CharacterController controller = GetComponent<CharacterController>();
		Camera cam = GetComponentInChildren<Camera>();


		mY += Input.GetAxis("Mouse Y") * rotateSpeed;
		mY = Mathf.Clamp(mY, camMaxUp, camMaxDown);

		cam.transform.localEulerAngles = new Vector3(-mY, cam.transform.localEulerAngles.y, cam.transform.localEulerAngles.z);
		transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * rotateSpeed, 0));

		if(Input.GetMouseButtonUp(0))
		{
			RaycastHit hit;
			if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 100))
			{
				World.SetBlock(hit, 0);
			}
		}

		if(Input.GetMouseButtonUp(1))
		{
			RaycastHit hit;
			if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 100))
			{
				World.SetBlock(hit, 1, true);
			}
		}

		if(controller.isGrounded)
		{
			moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			moveDirection = transform.TransformDirection(moveDirection);
			moveDirection *= speed;

			if(Input.GetButton("Jump"))
			{
				moveDirection.y = jumpSpeed;
			}
		}
		moveDirection.y -= gravity * Time.deltaTime;
		controller.Move(moveDirection * Time.deltaTime);
	}
}

}