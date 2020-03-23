using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

	float speed = 5.0f;
	
	[SerializeField]
	private Transform target;

	void Start()
	{
		transform.LookAt(target);
	}

	void Update()
	{
		if (Input.GetMouseButton(0))
		{
			transform.LookAt(target);
			transform.RotateAround(target.position, Vector3.up, Input.GetAxis("Mouse X") * speed);
			transform.RotateAround(target.position, Vector3.right, Input.GetAxis("Mouse Y") * speed);
		}
	}
}
