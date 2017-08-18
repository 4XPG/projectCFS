using UnityEngine;

public class WeaponController : MonoBehaviour {
	public GameObject[] Payloads;
	public GameObject gun;
	public GameObject parentplane;
    public GameObject lockedtarget;
    public float gunFireRate = 0.07f;
    private float nextFire = 0.0f;
    public int currentWeapon = 0;
    public int currentWeaponAmmo = 0;
    private int nrweapon;
    public HUDHandling HUD;
	private string weaponHUD = "SRM"; // default mode
	public Transform gunpos;
    public FCR radar;

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
        lockedtarget = radar.selectedTarget;
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

    // true: ground - AGM, air - IRM, SAHM, ARM
    public bool targetCheck(int wpn){
        Projectile weapon = Payloads[wpn].GetComponent<Projectile>();
        if(lockedtarget.tag == "Ground" && weapon.projType == Projectile.ProjTypes.AGM){
            return true;
        }
        else if (lockedtarget.tag == "Air" && weapon.projType == Projectile.ProjTypes.IRM || lockedtarget.tag == "Air" && weapon.projType == Projectile.ProjTypes.ARM || lockedtarget.tag == "Air" && weapon.projType == Projectile.ProjTypes.SAHM ){
            return true;
        }
        else{
            return false;
        }
    }

   public void Fire(int wpn){
       //if(currentWeaponAmmo > 0){
           //Payloads[wpn].GetComponent<Projectile>().SetFire();
           //Payloads[wpn].GetComponent<Rigidbody>().isKinematic = false;
           //Payloads[wpn].GetComponent<Rigidbody>().velocity = 300.0f *transform.forward;
           //Launch(Payloads[wpn].GetComponent<Projectile>());
       if(Payloads[wpn].GetComponent<Projectile>().projType == Projectile.ProjTypes.Bomb){
           Payloads[wpn].GetComponent<Rigidbody>().isKinematic = false;
           Payloads[wpn].GetComponent<BoxCollider>().enabled = true;
//gameObject.GetComponent<Rigidbody>().velocity = transform.forward * (ProjSpeed + parentcraftvel);
           float downwardSpeed;
           float bulletSpeed;
           float downWardSpeed = 0;
           Vector3 newPosition = new Vector3();
           Payloads[wpn].GetComponent<Rigidbody>().velocity = parentplane.gameObject.GetComponent<Rigidbody>().velocity;
           Payloads[wpn].GetComponent<Rigidbody>().AddForce(Vector3.forward);
       }
       else{
           if((lockedtarget == null) | targetCheck(wpn) == false){
               lockedtarget = new GameObject();
               Payloads[wpn].GetComponent<Projectile>().Launch(lockedtarget.transform,parentplane.gameObject.GetComponent<Rigidbody>().velocity);
           }
           else
                Payloads[wpn].GetComponent<Projectile>().Launch(lockedtarget.transform,parentplane.gameObject.GetComponent<Rigidbody>().velocity);
       }
       //}
		//Instantiate(Payloads[wpn], Payloads[wpn].transform.position,Payloads[wpn].transform.rotation);


       //Payloads[wpn].transform.parent = null;
       //Physics.IgnoreCollision(Payloads[wpn].GetComponent<Collider>(), transform.parent.GetComponent<Collider>());

       if((Payloads[wpn].GetComponent<Projectile>().projType == Projectile.ProjTypes.IRM) &&(IRMAmmo > 0)){
           IRMAmmo--;
           Debug.Log ("Fox Two");
           for(int i=0;i<Payloads.Length;i++){
               if(Payloads[i].GetComponent<Projectile>().projType == Projectile.ProjTypes.IRM)
                   Payloads[wpn] = Payloads[i];
           }
       }
       else if((Payloads[wpn].GetComponent<Projectile>().projType == Projectile.ProjTypes.SAHM) &&(SAHMAmmo > 0)) {
           SAHMAmmo--;
           Debug.Log ("Fox One");
           for(int i=0;i<Payloads.Length;i++){
               if(Payloads[i].GetComponent<Projectile>().projType == Projectile.ProjTypes.SAHM)
                   Payloads[wpn] = Payloads[i];
           }
       }
       else if((Payloads[wpn].GetComponent<Projectile>().projType == Projectile.ProjTypes.ARM) &&(ARMAmmo > 0)) {
           ARMAmmo--;
           Debug.Log ("Fox Three");
           for(int i=0;i<Payloads.Length;i++){
               if(Payloads[i].GetComponent<Projectile>().projType == Projectile.ProjTypes.ARM)
                   Payloads[wpn] = Payloads[i];
           }
       }
       else if((Payloads[wpn].GetComponent<Projectile>().projType == Projectile.ProjTypes.AGM) &&(AGMAmmo > 0)) {
           AGMAmmo--;
           Debug.Log ("Rifle");
           for(int i=0;i<Payloads.Length;i++){
               if(Payloads[i].GetComponent<Projectile>().projType == Projectile.ProjTypes.AGM)
                   Payloads[wpn] = Payloads[i];
           }
       }
       else if((Payloads[wpn].GetComponent<Projectile>().projType == Projectile.ProjTypes.Bomb) &&(BombAmmo > 0)) {
           BombAmmo--;
           Debug.Log ("Pickle");
           for(int i=0;i<Payloads.Length;i++){
               if(Payloads[i].GetComponent<Projectile>().projType == Projectile.ProjTypes.Bomb)
                   Payloads[wpn] = Payloads[i];
           }
       }
 	}

    public Projectile getEquippedWeapon(){
        return Payloads[currentWeapon].GetComponent<Projectile>();
    }

	void FireGun(){
		// Create the Bullet from the Bullet Prefab
        if(Time.time > nextFire){
            if (gunammo > 0) {
                nextFire = Time.time + gunFireRate;
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
	}
    /*void OnCollisionEnter (Collision col)
    {
        if(col.gameObject.tag = "Weapon")
        {
            Destroy(col.gameObject);
        }
    }	*/
}
