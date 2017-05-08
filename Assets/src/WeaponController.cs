using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FireWeapon : MonoBehaviour {
	public Projectile[] Payloads;
	public GameObject gun;
	public AeroplanePhysics parentplane;
    public int currentWeapon = 0; // 0 = IRM, 1 = SAHM, 2 = AHM, 3 = AGM, 4 = UGB
    private int nrweapon;
    public HUDHandling HUD;
	private string weaponHUD = "SRM"; // default mode

    private float wpnselection;
	public Transform[] launchpos;
	public Transform gunpos;
	private Rigidbody projectileRB;


	// Use this for initialization
	void Start () {
		//HUD.modetext = weaponHUD;
        SwitchWeapon(currentWeapon);
        wpnselection = Input.GetAxis("Weaponswitch");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("1")) {
			currentWeapon = 0;
			SwitchWeapon(currentWeapon);
			HUD.modetext.text = "SRM";	
		}
		if (Input.GetKeyDown ("2")) {
			currentWeapon = 1;
			SwitchWeapon(currentWeapon);
			HUD.modetext.text = "MRM";	
		}
		if (Input.GetKeyDown ("3")) {
			currentWeapon = 2;
			SwitchWeapon(currentWeapon);
			HUD.modetext.text = "MRM";	
		}
		if (Input.GetKeyDown ("4")) {
			currentWeapon = 3;
			SwitchWeapon(currentWeapon);
			HUD.modetext.text = "CCIP";	
		}
		if (Input.GetKeyDown ("5")) {
			currentWeapon = 4;
			SwitchWeapon(currentWeapon);
			HUD.modetext.text = "AGM";	
		}
		/*
         for (int i=0; i <= nrweapon; i++)    { 
             if (wpnselection == -1) {

             }
             else if (wpnselection == 1){
                currentWeapon = i+1;
                SwitchWeapon(currentWeapon);
				HUD.HUDMode.text = "CCIP";
             }
         }   */  

	    if(Input.GetButtonDown("Fire1"))
    {
    		Fire(currentWeapon);
        }
	    if(Input.GetButton("Fire2"))
    {
    		FireGun();
        }
	}

    public void SwitchWeapon(int index) {
 
         for (int i=0; i < nrweapon; i++)    {
             if (i == index) {
                 Payloads[i].gameObject.SetActive(true);
             } else { 
                 Payloads[i].gameObject.SetActive(false);
             }
         }
    }

   public void Fire(int wpn){
   		Instantiate(Payloads[wpn], transform.position,transform.rotation);
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
