using UnityEngine;
using System.Collections;

public class WeaponCameraController : MonoBehaviour {
	private float minazimuth = -10.0f;
	private float maxazimuth = 10.0f;

	private float minelev = -15.0f;
	private float maxelev = 15.0f;

	public float sensX = 0.2f;
	public float sensY = 0.2f;
	public float zoomsens = 100.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if ((Input.GetKeyDown ("i")) && (transform.rotation.y <= maxelev)) { //slew up
			transform.RotateAround(transform.position,transform.right,sensY * Time.deltaTime);
		}
		if (Input.GetKeyDown ("k") && (transform.rotation.y >= minelev)) { //slew down
			transform.RotateAround(transform.position,transform.right,-sensY * Time.deltaTime);
		}
		if (Input.GetKeyDown ("j") && (transform.rotation.x <= maxazimuth)) { //slew left
			transform.RotateAround(transform.position,Vector3.up,-sensX * Time.deltaTime);	
		}
		if (Input.GetKeyDown ("l") && (transform.rotation.x >= minazimuth)) { //slew right
			transform.RotateAround(transform.position,Vector3.up,sensX * Time.deltaTime);
		}
//		if (Input.GetKeyDown ("j")) { // TGP zoom out
//			SwitchWeapon(2);
//			HUD.modetext.text = "MRM";	
//		}
//		if (Input.GetKeyDown ("l")) { // TGP zoom in
//			SwitchWeapon(3);
//			HUD.modetext.text = "CCIP";	
//		}
		if (Input.GetKeyDown ("m")) { // designate target/create sensor point of interest
			//todo
		}
	}
}
