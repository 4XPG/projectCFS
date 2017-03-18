using UnityEngine;
using System.Collections;

public class FireWeapon : MonoBehaviour {
	public Projectile[] Payloads;
	public GameObject gun;
	public AeroplanePhysics parentplane;
    public int currentWeapon = 0; // 0 = IRM, 1 = SAHM, 2 = AHM, 3 = AGM, 4 = UGB
    private int nrweapon;
    public HUDHandling HUD;
    private float wpnselection;
	public Transform[] launchpos;
	public Transform gunpos;
	private Rigidbody projectileRB;


	// Use this for initialization
	void Start () {
        nrweapon = Payloads.Length;
        SwitchWeapon(currentWeapon);
        wpnselection = Input.GetAxis("Weaponswitch");
	}
	
	// Update is called once per frame
	void Update () {
         for (int i=0; i <= nrweapon; i++)    { 
             if (wpnselection == -1) {
                 currentWeapon = i-1;
                 SwitchWeapon(currentWeapon);
             }
             else if (wpnselection == 1){
                currentWeapon = i+1;
                SwitchWeapon(currentWeapon);
             }
         }     

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
