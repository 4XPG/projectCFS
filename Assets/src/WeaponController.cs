using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WeaponController : MonoBehaviour {
	public GameObject[] Payloads;
	public GameObject gun;
	public AeroplanePhysics parentplane;
    public int currentWeapon = 0;
    public ProjGuidance currentGuidance;
    private int nrweapon;
    public HUDHandling HUD;
	private string weaponHUD = "SRM"; // default mode
	public Transform gunpos;

	private int maxgunammo = 512;
	private int IRMAmmo;
	private int SAHMAmmo;
	private int ARMAmmo;
	private int AGMAmmo;
	private int BombAmmo;
	private int gunammo;

    public GameObject hardpoint1pos;
    public GameObject hardpoint2pos;
    public GameObject hardpoint3pos;
    public GameObject hardpoint4pos;
    public GameObject hardpoint5pos;
    public GameObject hardpoint6pos;
	// Use this for initialization
	void Start () {
		gunammo = maxgunammo;
        hardpoint1pos = gameObject.transform.Find("hardpointslot1").gameObject;
        hardpoint2pos = gameObject.transform.Find("hardpointslot2").gameObject;
        hardpoint3pos = gameObject.transform.Find("hardpointslot3").gameObject;
        hardpoint4pos = gameObject.transform.Find("hardpointslot4").gameObject;
        hardpoint5pos = gameObject.transform.Find("hardpointslot5").gameObject;
        hardpoint6pos = gameObject.transform.Find("hardpointslot6").gameObject;

        for(int i = 0; i < Payloads.Length; i++)
        {
            Payloads[0] = Instantiate(Payloads[0], hardpoint1pos.transform.position,hardpoint1pos.transform.rotation);
            Payloads[1] = Instantiate(Payloads[1], hardpoint2pos.transform.position,hardpoint1pos.transform.rotation);
            Payloads[2] = Instantiate(Payloads[2], hardpoint3pos.transform.position,hardpoint1pos.transform.rotation);
            Payloads[3] = Instantiate(Payloads[3], hardpoint4pos.transform.position,hardpoint1pos.transform.rotation);
            Payloads[4] = Instantiate(Payloads[4], hardpoint5pos.transform.position,hardpoint1pos.transform.rotation);
            Payloads[5] = Instantiate(Payloads[5], hardpoint6pos.transform.position,hardpoint1pos.transform.rotation);
            //Debug.Log("Creating enemy number: " + i);
        }
	}
	
	// Update is called once per frame
	void Update () {
		HUD.AmmoCounter.text = gunammo.ToString();
		if (Input.GetKeyDown ("1")) {
            for(int i=0;i<Payloads.Length;i++){
                currentGuidance = Payloads[i].GetComponent<ProjGuidance>();
                if(currentGuidance.guidanceType == ProjGuidance.GuidanceTypes.Infrared){
                    Payloads[i].gameObject.SetActive(true);
                }
            }
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

    //void CountAmmo(GameObject[] payloads){
//
//    }

//    public void SwitchWeapon(int index) {
//		for (int i=0; i < Payloads.Length; i++)    {
//             if (i == index) {
//                Payloads[i].gameObject.SetActive(true);
//				currentWeapon = index;
//             } else { 
//                 Payloads[i].gameObject.SetActive(false);
//             }
//         }
//    }

	public void countAmmo(){
		//for (int i = 0; i < Projectile [i]; i++) {

		//}
	}
   public void Fire(int wpn){
		//Instantiate(Payloads[wpn], Payloads[wpn].transform.position,Payloads[wpn].transform.rotation);

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
