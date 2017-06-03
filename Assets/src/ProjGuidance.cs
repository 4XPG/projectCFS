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
	public float trackDelay = 1.0f;
	public float pitbullRange = 11.0f; // radar AAM only
	public float turnRate = 1.0f;
	public float targetAspectAngle; // IR AAM only
	// Use this for initialization

	private float curDistance;
	public float maxLockDistance = 1000.0f;

	private GameObject[] targets;
	private bool stopTracking;
	private bool TargetLock = false;
	private float turnDamping = 1.0f;
	private int TargetAspect; // 0 = air; 1 = ground 
	private Vector3 prevpos;
	private	Vector3 prevtargetpos;
	private Vector3 currentpos;
	private static float NavGain = 3.0f;
	private static float TargetAccel = 9.8f;

	void Start() {
			//target = GameObject.FindGameObjectWithTag("Air");
			proj = gameObject.GetComponent<Projectile>();
		if (target != null) {
			lockedTarget = target.transform;
			prevtargetpos = lockedTarget.position;
		}
		prevpos = transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float LOSRate;
		float closure;
		Vector3 DeltaLOS;
		trackETA += Time.deltaTime;	
		if ((prevpos != Vector3.zero) && (prevtargetpos != Vector3.zero)) {
			Vector3 oldLOS = prevtargetpos - prevpos;
			oldLOS.Normalize ();
			Vector3 currentLOS = lockedTarget.position - transform.position;
			currentLOS.Normalize ();
			if (oldLOS.magnitude == 0) {
				DeltaLOS = new Vector3 (0, 0, 0);
				LOSRate = 0.0f;
			} else {
				DeltaLOS = currentLOS - oldLOS;
				LOSRate = DeltaLOS.magnitude;
			}
			closure = -LOSRate;
			proj.projAccel = currentLOS * NavGain * closure + DeltaLOS * TargetAccel * (0.5f * NavGain);
			if (target != null) {
				if (proj.projType == Projectile.ProjTypes.Missile) {
					TrackTargetAR ();
				}
			}
		}
		prevpos = currentpos;
		prevtargetpos = currentpos;
	}

	public void TrackTargetIR(){ //limited lock FOV
	}

 	public void TrackTargetAR(){

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

	public void TrackDot(){ //track raycasted point made by the EO targetting pod
	}
}
