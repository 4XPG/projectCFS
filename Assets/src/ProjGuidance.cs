using UnityEngine;
using System.Collections;

public class ProjGuidance : MonoBehaviour {

	public enum GuidanceTypes : int {None,Infrared,SemiActiveRadar,ActiveRadar,ElectroOptical};
	public enum TargetTypes : int {AirToAir,AirToGround,SurfaceToAir};


	public GuidanceTypes guidanceType = GuidanceTypes.ActiveRadar;
	public TargetTypes targetType = TargetTypes.AirToAir;

	public Projectile proj;
	public GameObject target;
	public Transform lockedTarget;
	private Quaternion targetRotation;

	public float seekerFOV = 15.0f;
	private float trackETA;
	public float trackDelay = 1.0f; // radar AAM only
	public float turnRate = 1.0f;
	public float targetAspectAngle; // IR AAM only
	// Use this for initialization

	private float curDistance;
	private float oldDistance;
	public float maxLockDistance = 1000.0f;

	private GameObject[] targets;
	private bool stopTracking;
	private bool TargetLock = false;
	private float turnDamping = 1.0f;
	private int TargetAspect; // 0 = air; 1 = ground 

	void Start () {
			//target = GameObject.FindGameObjectWithTag("Air");
			proj = gameObject.GetComponent<Projectile>();
		if (target != null) {
			lockedTarget = target.transform;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		trackETA += Time.deltaTime;	
		if (target != null) {
			if (proj.projType == Projectile.ProjTypes.Missile) {
					TrackTarget ();
			}
		}
	}

 	public void TrackTarget(){

         //GetComponent<Rigidbody>().AddForce(transform.up * 100);
 
		 float targetDelta = Vector3.Distance(gameObject.transform.position, lockedTarget.position);
         
         //get the angle between transform.forward and target delta
         //float angleDiff = Vector3.Angle(transform.forward, targetDelta);
           

		if (targetDelta < maxLockDistance) {
			stopTracking = false;
		} else {
			stopTracking = true;
		}
		//delay tracking for a certain amount of time...
		if (trackETA > trackDelay){
			if (stopTracking == false){
				//look at the target object at speed
				targetRotation = Quaternion.LookRotation(lockedTarget.position - transform.position);
				transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnRate);
			}
		}

         // get its cross product, which is the axis of rotation to
         // get from one vector to the other
         //Vector3 cross = Vector3.Cross(transform.forward, targetDelta);
  
     	 //proj.GetComponent<Rigidbody>().AddTorque(cross * angleDiff * proj.turnRate);
     	 //proj.GetComponent<Rigidbody>().transform.LookAt(lockedTarget.position);
		//transform.rotation = 
 	}
}
