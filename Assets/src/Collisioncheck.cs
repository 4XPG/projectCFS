using UnityEngine;
using System.Collections;

public class Collisioncheck : MonoBehaviour {
	public void onCollision(Collision collision){
		Destroy(gameObject,0);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
