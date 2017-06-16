using UnityEngine;
using System.Collections;

public class Collisioncheck : MonoBehaviour {

	public GameObject explosion;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(){
		Instantiate(explosion, transform.position, transform.rotation);
		Destroy(gameObject); // destroy the grenade
		//Destroy(expl, 3); // delete the explosion after 3 seconds
	}
}
