using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WeaponController : MonoBehaviour {
	public GameObject[] Payloads;
	public GameObject gun;
	public GameObject parentplane;
    public int currentWeapon = 0;
    public int currentWeaponAmmo = 0;
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

        //instantiate loadout

            Payloads[0] = Instantiate(Payloads[0], hardpoint1pos.transform.position,Quaternion.identity);
            Payloads[0].transform.parent = hardpoint1pos.transform;
            Payloads[0].GetComponent<Rigidbody>().velocity = parentplane.transform.GetComponent<Rigidbody>().velocity;
            Payloads[0].GetComponent<Rigidbody>().angularVelocity = parentplane.transform.GetComponent<Rigidbody>().angularVelocity;
            Payloads[1] = Instantiate(Payloads[1], hardpoint2pos.transform.position,Quaternion.identity);
            Payloads[1].transform.parent = hardpoint2pos.transform;
            Payloads[1].GetComponent<Rigidbody>().velocity = parentplane.transform.GetComponent<Rigidbody>().velocity;
            Payloads[1].GetComponent<Rigidbody>().angularVelocity = parentplane.transform.GetComponent<Rigidbody>().angularVelocity;
            Payloads[2] = Instantiate(Payloads[2], hardpoint3pos.transform.position,Quaternion.identity);
            Payloads[2].transform.parent = hardpoint3pos.transform;
            Payloads[2].GetComponent<Rigidbody>().velocity = parentplane.transform.GetComponent<Rigidbody>().velocity;
            Payloads[2].GetComponent<Rigidbody>().angularVelocity = parentplane.transform.GetComponent<Rigidbody>().angularVelocity;
            Payloads[3] = Instantiate(Payloads[3], hardpoint4pos.transform.position,Quaternion.identity);
            Payloads[3].transform.parent = hardpoint4pos.transform;
            Payloads[3].GetComponent<Rigidbody>().velocity = parentplane.transform.GetComponent<Rigidbody>().velocity;
            Payloads[3].GetComponent<Rigidbody>().angularVelocity = parentplane.transform.GetComponent<Rigidbody>().angularVelocity;
            Payloads[4] = Instantiate(Payloads[4], hardpoint5pos.transform.position,Quaternion.identity);
            Payloads[4].transform.parent = hardpoint5pos.transform;
            Payloads[4].GetComponent<Rigidbody>().velocity = parentplane.transform.GetComponent<Rigidbody>().velocity;
            Payloads[4].GetComponent<Rigidbody>().angularVelocity = parentplane.transform.GetComponent<Rigidbody>().angularVelocity;
            Payloads[5] = Instantiate(Payloads[5], hardpoint6pos.transform.position,Quaternion.identity);
            Payloads[5].transform.parent = hardpoint6pos.transform;
            Payloads[5].GetComponent<Rigidbody>().velocity = parentplane.transform.GetComponent<Rigidbody>().velocity;
            Payloads[5].GetComponent<Rigidbody>().angularVelocity = parentplane.transform.GetComponent<Rigidbody>().angularVelocity;
            //Debug.Log("Creating enemy number: " + i);
        for(int i = 0; i < Payloads.Length; i++)
        {
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
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			currentWeapon = 0;
            //currentWeaponAmmo = IRMAmmo;
            HUD.HUDMode = 0;
		} else if (Input.GetKeyDown (KeyCode.Alpha2)) {
			//SwitchWeapon(1);
			currentWeapon = 1;
            //currentWeaponAmmo = SAHMAmmo;
            HUD.HUDMode = 1;
		} else if (Input.GetKeyDown (KeyCode.Alpha3)) {
			currentWeapon = 2;
            //currentWeaponAmmo = ARMAmmo;
            HUD.HUDMode = 2;
		} else if (Input.GetKeyDown (KeyCode.Alpha4)) {
			currentWeapon = 3;
            //currentWeaponAmmo = AGMAmmo;
            HUD.HUDMode = 3;
		} else if (Input.GetKeyDown (KeyCode.Alpha5)) {
			currentWeapon = 4;
            //currentWeaponAmmo = BombAmmo;
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


    public void Launch(Projectile wpn){
        wpn.GetComponent<Rigidbody>().isKinematic = false;
        wpn.transform.parent = null;
        wpn.BoostFuel--;
        if(wpn.projType != Projectile.ProjTypes.Bomb){
            wpn.transform.LookAt(wpn.lockedtarget.transform);
            if (wpn.BoostFuel > 0) {
//this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                Vector3 forwardforce = (wpn.transform.forward*wpn.ProjSpeed) + parentplane.gameObject.GetComponent<Rigidbody>().velocity;
                wpn.transform.Find("missiletrail").gameObject.SetActive(true);


                if(wpn.GetComponent<ProjGuidance>().target != null)
                    wpn.GetComponent<Rigidbody>().velocity = wpn.GetComponent<ProjGuidance>().FindInterceptVector(transform.position, wpn.ProjSpeed, wpn.lockedtarget.transform.position, wpn.lockedtarget.GetComponent<Rigidbody>().velocity);
                else
                    wpn.GetComponent<Rigidbody>().velocity = forwardforce;
            } else{
                wpn.ProjSpeed = 0.0f;
                wpn.transform.Find("missiletrail").gameObject.GetComponent<TrailRenderer>().enabled = false;
//this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
//stop emitting flame & smoke particle
            }

        }
        else if(wpn.projType == Projectile.ProjTypes.AGM){
            if (wpn.BoostFuel>0) {
                Vector3 forwardforce = (wpn.transform.forward*wpn.ProjSpeed) + parentplane.gameObject.GetComponent<Rigidbody>().velocity;
                //wpn.ProjSpeed = 400.0f;
                wpn.GetComponent<Rigidbody>().velocity = forwardforce;
                wpn.transform.Find("missiletrail").gameObject.SetActive(true);



//GetComponent<Rigidbody>().velocity = guidance.FindInterceptVector(transform.position, ProjSpeed, lockedtarget.transform.position, lockedtarget.GetComponent<Rigidbody>().velocity);
            } else{
                wpn.ProjSpeed = 0.0f;
                wpn.transform.Find("missiletrail").gameObject.SetActive(false);
//stop emitting flame & smoke particle
            }

        }
        else if(wpn.projType == Projectile.ProjTypes.Bomb){
//gameObject.GetComponent<Rigidbody>().velocity = transform.forward * (ProjSpeed + parentcraftvel);

            wpn.GetComponent<Rigidbody>().velocity = parentplane.gameObject.GetComponent<Rigidbody>().velocity;
        }
        else if(wpn.projType == Projectile.ProjTypes.Gun){
        }
    }
   public void Fire(int wpn){
       //if(currentWeaponAmmo > 0){
           //Payloads[wpn].GetComponent<Projectile>().SetFire();
           //Payloads[wpn].GetComponent<Rigidbody>().isKinematic = false;
           //Payloads[wpn].GetComponent<Rigidbody>().velocity = 300.0f *transform.forward;
           Launch(Payloads[wpn].GetComponent<Projectile>());
       //}
		//Instantiate(Payloads[wpn], Payloads[wpn].transform.position,Payloads[wpn].transform.rotation);


       //Payloads[wpn].transform.parent = null;
       //Physics.IgnoreCollision(Payloads[wpn].GetComponent<Collider>(), transform.parent.GetComponent<Collider>());

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

/*
    int CheckAmmo(){
    }*/

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
			Destroy (bullet, 2.0f);
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
