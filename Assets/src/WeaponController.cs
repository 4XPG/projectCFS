using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WeaponController : MonoBehaviour {
	public GameObject[] Payloads;
	public GameObject gun;
	public GameObject parentplane;
    public int currentWeapon = 0;
    public ProjGuidance currentGuidance;
    private int nrweapon;
    public HUDHandling HUD;
	private string weaponHUD = "SRM"; // default mode
	public Transform gunpos;

	private int maxgunammo = 512;
	public int IRMAmmo = 0;
	public int SAHMAmmo = 0;
	public int ARMAmmo = 0;
	public int AGMAmmo = 0;
	public int BombAmmo = 0;
	public int gunammo = 0;

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
            Payloads[0].transform.parent = hardpoint1pos.transform;
            Payloads[0].GetComponent<Rigidbody>().velocity = parentplane.transform.GetComponent<Rigidbody>().velocity;
            Payloads[0].GetComponent<Rigidbody>().angularVelocity = parentplane.transform.GetComponent<Rigidbody>().angularVelocity;
            Payloads[1] = Instantiate(Payloads[1], hardpoint2pos.transform.position,hardpoint1pos.transform.rotation);
            Payloads[1].transform.parent = hardpoint2pos.transform;
            Payloads[1].GetComponent<Rigidbody>().velocity = parentplane.transform.GetComponent<Rigidbody>().velocity;
            Payloads[1].GetComponent<Rigidbody>().angularVelocity = parentplane.transform.GetComponent<Rigidbody>().angularVelocity;
            Payloads[2] = Instantiate(Payloads[2], hardpoint3pos.transform.position,hardpoint1pos.transform.rotation);
            Payloads[2].transform.parent = hardpoint3pos.transform;
            Payloads[2].GetComponent<Rigidbody>().velocity = parentplane.transform.GetComponent<Rigidbody>().velocity;
            Payloads[2].GetComponent<Rigidbody>().angularVelocity = parentplane.transform.GetComponent<Rigidbody>().angularVelocity;
            Payloads[3] = Instantiate(Payloads[3], hardpoint4pos.transform.position,hardpoint1pos.transform.rotation);
            Payloads[3].transform.parent = hardpoint4pos.transform;
            Payloads[3].GetComponent<Rigidbody>().velocity = parentplane.transform.GetComponent<Rigidbody>().velocity;
            Payloads[3].GetComponent<Rigidbody>().angularVelocity = parentplane.transform.GetComponent<Rigidbody>().angularVelocity;
            Payloads[4] = Instantiate(Payloads[4], hardpoint5pos.transform.position,hardpoint1pos.transform.rotation);
            Payloads[4].transform.parent = hardpoint5pos.transform;
            Payloads[4].GetComponent<Rigidbody>().velocity = parentplane.transform.GetComponent<Rigidbody>().velocity;
            Payloads[4].GetComponent<Rigidbody>().angularVelocity = parentplane.transform.GetComponent<Rigidbody>().angularVelocity;
            Payloads[5] = Instantiate(Payloads[5], hardpoint6pos.transform.position,hardpoint1pos.transform.rotation);
            Payloads[5].transform.parent = hardpoint6pos.transform;
            Payloads[5].GetComponent<Rigidbody>().velocity = parentplane.transform.GetComponent<Rigidbody>().velocity;
            Payloads[5].GetComponent<Rigidbody>().angularVelocity = parentplane.transform.GetComponent<Rigidbody>().angularVelocity;
            //Debug.Log("Creating enemy number: " + i);

            if(Payloads[i].GetComponent<Projectile>().projType == Projectile.ProjTypes.IRM){
                IRMAmmo++;
            }
            else if(Payloads[i].GetComponent<Projectile>().projType == Projectile.ProjTypes.SAHM) {
                SAHMAmmo++;
			}
            else if(Payloads[i].GetComponent<Projectile>().projType == Projectile.ProjTypes.ARM) {
                ARMAmmo++;
            }
            else if(Payloads[i].GetComponent<Projectile>().projType == Projectile.ProjTypes.AGM) {
                AGMAmmo++;
            }
            else if(Payloads[i].GetComponent<Projectile>().projType == Projectile.ProjTypes.Bomb) {
				BombAmmo++;
			}
        }
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("1")) {
            /*for(int i=0;i<Payloads.Length;i++){
                currentGuidance = Payloads[i].GetComponent<ProjGuidance>();
                if(currentGuidance.guidanceType == ProjGuidance.GuidanceTypes.Infrared){
                    Payloads[i].gameObject.SetActive(true);
                }
            }*/
			//SwitchWeapon(0);
			currentWeapon = 0;
            HUD.HUDMode = 0;
		} else if (Input.GetKeyDown ("2")) {
			//SwitchWeapon(1);
			currentWeapon = 1;
            HUD.HUDMode = 1;
		} else if (Input.GetKeyDown ("3")) {
			currentWeapon = 2;
            HUD.HUDMode = 2;
		} else if (Input.GetKeyDown ("4")) {
			currentWeapon = 3;
            HUD.HUDMode = 3;
		} else if (Input.GetKeyDown ("5")) {
			currentWeapon = 4;
            HUD.HUDMode = 4;
		} else if (Input.GetKeyDown (KeyCode.C)) {
            //currentWeapon = 5;
            HUD.HUDMode = 5;
		} /*else {
			currentWeapon = 0;
			HUD.modetext.text = "SRM";			
		}*/


	    if(Input.GetButtonDown("Fire1"))
    {
			Fire(currentWeapon);
        }
	    if(Input.GetButton("Fire2"))
    {
    		FireGun();
        }
	}

/*    public void updateAmmoCounter(string ammo){
        AmmoCounter.text = ammo;
    }*/

   public void Fire(int wpn){
		//Instantiate(Payloads[wpn], Payloads[wpn].transform.position,Payloads[wpn].transform.rotation);
		Payloads[wpn].GetComponent<Projectile>().SetFire();
       Payloads[wpn].GetComponent<Rigidbody>().isKinematic = false;
       Payloads[wpn].transform.parent = null;

       if((Payloads[wpn].GetComponent<Projectile>().projType == Projectile.ProjTypes.IRM) &&(IRMAmmo > 0)){
           IRMAmmo--;
           Debug.Log ("Fox Two");
       }
       else if((Payloads[wpn].GetComponent<Projectile>().projType == Projectile.ProjTypes.SAHM) &&(SAHMAmmo > 0)) {
           SAHMAmmo--;
           Debug.Log ("Fox One");
       }
       else if((Payloads[wpn].GetComponent<Projectile>().projType == Projectile.ProjTypes.ARM) &&(ARMAmmo > 0)) {
           ARMAmmo--;
           Debug.Log ("Fox Three");
       }
       else if((Payloads[wpn].GetComponent<Projectile>().projType == Projectile.ProjTypes.AGM) &&(AGMAmmo > 0)) {
           AGMAmmo--;
           Debug.Log ("Rifle");
       }
       else if((Payloads[wpn].GetComponent<Projectile>().projType == Projectile.ProjTypes.Bomb) &&(BombAmmo > 0)) {
           BombAmmo--;
           Debug.Log ("Pickle");
       }
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
