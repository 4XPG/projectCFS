using UnityEditor;
using UnityEngine;

[RequireComponent(typeof (ProjectilePhysics))]
public class Projectile : MonoBehaviour {

 	public enum ProjTypes : int {Gun,IRM,SAHM,ARM,AGM,Bomb};
 	public ProjTypes projType = ProjTypes.IRM;
	public float fuseDelay = 0.0f;
	public float ProjSpeed = 0.0f;
	public float BoostFuel = 10.0f; // how long the rocket motor lives
	public float turnRate = 0.8f;
	public float proxymityrange = 0.1f;
	public bool Fire = false;

	//private ParticleSystem SmokePrefab;
	//private AudioClip missileClip;


	private Vector3 distanceDelta;
	private Vector3 playerPos;
	public Vector3 projAccel;

	private ProjGuidance guidance;
	private float parentcraftvel;
	private float timeSinceLaunch = 0.0f;
	private GameObject lockedtarget;

	public GameObject explosion;
    public Component missileTrail;
	public GameObject parentcraft;
	//private EmissionModule smoketrail;
	// Use this for initialization



	void Start () {
        //ProjSpeed = 0.0f;
        parentcraftvel = parentcraft.GetComponent<Rigidbody>().velocity.magnitude;
        guidance = gameObject.GetComponent<ProjGuidance>();
        lockedtarget = GameObject.FindGameObjectWithTag("SelectedTarget");
        //missileTrail = gameObject.GetComponentsInChildren< ParticleSystem >();
        //missileTrail.GetComponent<ParticleEmitter>().enableEmission = false;
        transform.Find("missiletrail").gameObject.SetActive(false);
    		//lockedTarget.position = target.position;
            //projectileRB = GetComponent<Rigidbody>();
            //Fire();
         // apply torque along that axis according to the magnitude of the angle.



	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{

		timeSinceLaunch += Time.deltaTime;	
		if(Fire == true){        if(projType != ProjTypes.Bomb){
// wait until fuse
            if(timeSinceLaunch >= fuseDelay){
//start tracking
                if (timeSinceLaunch <= BoostFuel) {
                    Vector3 forwardforce = Vector3.zero;
                    ProjSpeed = 100.0f;
                    forwardforce += ProjSpeed * transform.forward;
                    gameObject.GetComponent<Rigidbody>().AddForce(forwardforce);
                    transform.Find("missiletrail").gameObject.SetActive(true);



                    GetComponent<Rigidbody>().velocity = guidance.FindInterceptVector(transform.position, ProjSpeed, lockedtarget.transform.position, lockedtarget.GetComponent<Rigidbody>().velocity);
                } else if (timeSinceLaunch > BoostFuel) {
                    ProjSpeed = 0.0f;
                    transform.Find("missiletrail").gameObject.SetActive(false);
//stop emitting flame & smoke particle
                }

            }
        }
        else if(projType == ProjTypes.AGM){

// wait until fuse
            if(timeSinceLaunch >= fuseDelay){
//start tracking
                if (timeSinceLaunch <= BoostFuel) {
                    Vector3 forwardforce = Vector3.zero;
                    ProjSpeed = 100.0f;
                    forwardforce += ProjSpeed * transform.forward;
                    gameObject.GetComponent<Rigidbody>().AddForce(forwardforce);
                    transform.Find("missiletrail").gameObject.SetActive(true);



                    //GetComponent<Rigidbody>().velocity = guidance.FindInterceptVector(transform.position, ProjSpeed, lockedtarget.transform.position, lockedtarget.GetComponent<Rigidbody>().velocity);
                } else if (timeSinceLaunch > BoostFuel) {
                    ProjSpeed = 0.0f;
                    transform.Find("missiletrail").gameObject.SetActive(false);
//stop emitting flame & smoke particle
                }

            }
        }
        else if(projType == ProjTypes.Bomb){
            gameObject.GetComponent<Rigidbody>().velocity = transform.forward * (ProjSpeed + parentcraftvel);
        }
        else if(projType == ProjTypes.Gun){

        }
        }

	}

    public void CheckTargetAspect(){

    }

    public void SetFire(){
        Fire = true;
    }
	void OnCollisionEnter(){
		Instantiate(explosion, transform.position, transform.rotation);
		Destroy(gameObject); // destroy the grenade
		//Destroy(expl, 3); // delete the explosion after 3 seconds
	}


	// checks target aspect
	//float checkAspect () {
//
	//}
}
