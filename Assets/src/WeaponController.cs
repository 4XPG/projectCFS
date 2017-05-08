using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WeaponController : MonoBehaviour {
	public Projectile[] Payloads;
	public GameObject gun;
	public AeroplanePhysics parentplane;
    public int currentWeapon = 0; // 0 = IRM, 1 = SAHM, 2 = AHM, 3 = AGM, 4 = UGB
    private int nrweapon;
    public HUDHandling HUD;
	private string weaponHUD = "SRM"; // default mode
	public Transform gunpos;
	private Rigidbody projectileRB;

	private int maxgunammo = 512;
	private int gunammo;

	// Use this for initialization
	void Start () {
		gunammo = maxgunammo;
	}
	
	// Update is called once per frame
	void Update () {
		HUD.AmmoCounter.text = gunammo.ToString();
		if (Input.GetKeyDown ("1")) {
			//SwitchWeapon(0);
			currentWeapon = 0;
			HUD.modetext.text = "SRM";	
		} else if (Input.GetKeyDown ("2")) {
			//SwitchWeapon(1);
			currentWeapon = 1;
			HUD.modetext.text = "MRM";	
		} else if (Input.GetKeyDown ("3")) {
			//SwitchWeapon(2);
			currentWeapon = 2;
			HUD.modetext.text = "MRM";	
		} else if (Input.GetKeyDown ("4")) {
			currentWeapon = 3;
			//SwitchWeapon(3);
			HUD.modetext.text = "CCIP";	
		} else if (Input.GetKeyDown ("5")) {
			currentWeapon = 4;
			//SwitchWeapon(4);
			HUD.modetext.text = "AGM";	
		} else if (Input.GetKeyDown (KeyCode.C)) {
			//currentWeapon = 4;
			//SwitchWeapon(4);
			HUD.modetext.text = "LCOS";	

		} /*else {
			currentWeapon = 0;
			HUD.modetext.text = "SRM";			
		}*/


	    if(Input.GetButtonDown("Fire1"))
    {
			if (currentWeapon == 0) {
				Debug.Log ("Fox Two");
				Fire (currentWeapon);
			} else if (currentWeapon == 1) {
				Debug.Log ("Fox One");
				Fire (currentWeapon);				
			} else if (currentWeapon == 2) {
				Debug.Log ("Fox Three");
				Fire (currentWeapon);				
			} else if (currentWeapon == 3) {
				Debug.Log ("Pickle");
				Fire (currentWeapon);				
			} else if (currentWeapon == 4) {
				Debug.Log ("Rifle");
				Fire (currentWeapon);				
			} else if (currentWeapon > 4){
				FireGun ();
			}


        }
	    if(Input.GetButton("Fire2"))
    {
    		FireGun();
        }
	}

    public void SwitchWeapon(int index) {
		for (int i=0; i < Payloads.Length; i++)    {
             if (i == index) {
                Payloads[i].gameObject.SetActive(true);
				currentWeapon = index;
             } else { 
                 Payloads[i].gameObject.SetActive(false);
             }
         }
    }

   public void Fire(int wpn){
   		Instantiate(Payloads[wpn], Payloads[wpn].transform.position,transform.rotation);
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
		if (gunammo > 0) {
			var bullet = (GameObject)Instantiate (gun, gunpos.position, gunpos.rotation);
			// Add velocity to the bullet
			bullet.GetComponent<Rigidbody> ().velocity = bullet.transform.forward * 1500.0f;
			// Destroy the bullet after 2 seconds
			Destroy (bullet, 3.0f);
			gunammo--;
		} else {
			Debug.Log ("Gun Winchester!");
		}
	}
    /*void OnCollisionEnter (Collision col)
    {
        if(col.gameObject.tag = "Weapon")
        {
            Destroy(col.gameObject);
        }
    }	*/
}
