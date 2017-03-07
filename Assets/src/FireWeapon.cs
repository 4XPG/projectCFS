using UnityEngine;
using System.Collections;

public class FireWeapon : MonoBehaviour {
	public GameObject mssl;
	public Projectile missile;
	public GameObject gun;
	public float launchdelay;
	public AeroplanePhysics parentplane;
	public Transform launchpos;
	public Transform gunpos;
	private Rigidbody projectileRB;


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);
	    if(Input.GetButtonDown("Fire1"))
    {
    		Fire();
        }
	    if(Input.GetButton("Fire2"))
    {
    		FireGun();
        }
	}

   public void Fire(){
 		
   		Instantiate(missile, transform.position,transform.rotation);

 
        	
 	}

	/*void Fire(){
    // Create the Bullet from the Bullet Prefab
    var bullet = (GameObject)Instantiate (
        mssl,
        launchpos.position,
        launchpos.rotation);

    // Add velocity to the bullet
    bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * parentplane.ForwardSpeed;

    // Destroy the bullet after 2 seconds
    Destroy(bullet, 2.0f);
	}*/

	void FireGun(){
    // Create the Bullet from the Bullet Prefab
    var bullet = (GameObject)Instantiate (
        gun,
        gunpos.position,
        gunpos.rotation);

    // Add velocity to the bullet
    bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 1500.0f;

    // Destroy the bullet after 2 seconds
    Destroy(bullet, 3.0f);
	}	

    /*void OnCollisionEnter (Collision col)
    {
        if(col.gameObject.tag = "Weapon")
        {
            Destroy(col.gameObject);
        }
    }	*/
}
