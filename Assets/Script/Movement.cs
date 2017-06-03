using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

	public float x;
	public float z;
	public float y;

	public float speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		x = Input.GetAxis ("Horizontal") * speed;
		z = Input.GetAxis ("Vertical") * speed;
		if (Input.GetKey (KeyCode.Q)) {
			y = 1;
		} else if (Input.GetKey (KeyCode.E)) {
			y = -1;
		} else {
			y = 0;
		}
		y = y * speed;
		transform.position = new Vector3 (transform.position.x + x, transform.position.y+y, transform.position.z+z);
	}
}
