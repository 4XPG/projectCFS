//Proper fire control radar script, has cursor to lock on radar contact. Hopefully.
using UnityEngine;
using System.Collections;

public class FCR : MonoBehaviour {
	//public Camera FCRCamera;
	public GameObject playerObject;
	public GameObject[] airObjects;
	public GameObject[] groundObjects;
	private Vector3 CameraPos;
	private Vector3 offset;
	// Use this for initialization
	void Start () {
		CameraPos.x = transform.position.x - playerObject.transform.position.x;
		CameraPos.z = transform.position.z - playerObject.transform.position.z;
	}
	
	// Update is called once per frame
	void Update () {
		//CameraPos.x = playerObject.transform.position.x;
		//CameraPos.z = playerObject.transform.position.z;
		transform.position = playerObject.transform.position + CameraPos;
	}
}
