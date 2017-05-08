using UnityEngine;
using System.Collections;

[RequireComponent(typeof (ProjectilePhysics))]
public class Projectile : MonoBehaviour {

 	public enum ProjTypes : int {Gun,Missile,Bomb};	
 	public ProjTypes projType = ProjTypes.Missile;
	public float fuseDelay = 0.1f;
	public float ProjSpeed = 0.0f;
	public float BoostFuel = 0.0f; // how long the rocket motor lives
	public float turnRate = 0.8f;
	public float proxymityrange = 0.1f;


	//private ParticleSystem SmokePrefab;
	//private AudioClip missileClip;


	private Vector3 distanceDelta;
	private Vector3 playerPos;

	private ProjGuidance guidance;
	private float parentcraftvel;
	private float timeSinceLaunch = 0.0f;

	public GameObject explosion;
	public GameObject parentcraft;

	// Use this for initialization



	void Start () {
			parentcraftvel = parentcraft.GetComponent<Rigidbody>().velocity.magnitude* 0.5f;
			guidance = parentcraft.GetComponent<ProjGuidance>();
    		//lockedTarget.position = target.position;
            //projectileRB = GetComponent<Rigidbody>();
            //Fire();
         // apply torque along that axis according to the magnitude of the angle.



	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		timeSinceLaunch += Time.deltaTime;	
		if(projType == ProjTypes.Missile){
			// wait until fuse
			if(timeSinceLaunch >= fuseDelay){
				//start tracking
				if (timeSinceLaunch <= BoostFuel) {
					//ProjSpeed = 100.0f;	
					//start emitting flame & smoke particle
					//if(guidance.lockedTarget != null){	  		
					//}

				} else if (timeSinceLaunch > BoostFuel) {
					ProjSpeed = 0.0f;
					//stop emitting flame & smoke particle
				}
				gameObject.GetComponent<Rigidbody> ().velocity = transform.forward * (ProjSpeed + parentcraftvel);
			}
		}
		else if(projType == ProjTypes.Bomb){
				gameObject.GetComponent<Rigidbody>().velocity = transform.forward * (ProjSpeed + parentcraftvel);
		}
		else if(projType == ProjTypes.Gun){

		}





/*		StartCoroutine(firingDelay());
		timeSinceLaunch += Time.deltaTime;		
   		if(lockedTarget != null){
    		if(timeSinceLaunch <= BoostFuel){
	    		BoostSpeed = 50000.0f;	
    		}
	    	Vector3 targetRotation = Quaternion.LookRotation(lockedTarget.position - transform.position);
 			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetRotation), turnRate * Time.deltaTime);
    	}
		transform.Translate(Vector3.forward * BoostSpeed * Time.deltaTime);
		if(timeSinceLaunch <= BoostFuel){
 			BoostSpeed = 0.0f;
 		}*/
	}

	void OnCollisionEnter(){
		GameObject expl = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
		Destroy(gameObject); // destroy the grenade
		//Destroy(expl, 3); // delete the explosion after 3 seconds
	}



	// checks target aspect
	//float checkAspect () {
//
	//}
}
