using UnityEngine;
using System.Collections;

public class ProjGuidance : MonoBehaviour {

	public enum GuidanceTypes : int {None,Infrared,SemiActiveRadar,ActiveRadar,ElectroOptical};
	public enum TargetTypes : int {AirToAir,AirToGround,SurfaceToAir};
 	public enum AspectTypes : int {RearAspect,AllAspect};


	public GuidanceTypes guidanceType = GuidanceTypes.ActiveRadar;
	public TargetTypes targetType = TargetTypes.AirToAir;
	public AspectTypes aspectType = AspectTypes.AllAspect; 

	public Projectile proj;
	public GameObject target;
	public Transform lockedTarget;

	public float seekerFOV = 15.0f;
	private float trackETA;
	public float trackDelay = 1.0f; // radar AAM only
	// Use this for initialization

	private float curDistance;
	private float oldDistance;

	private GameObject[] targets;
	private bool TargetLock = false;
	private int TargetAspect; // 0 = air; 1 = ground 

	void Start () {
			//target = GameObject.FindGameObjectWithTag("Air");
			lockedTarget = target.transform;
			oldDistance = Mathf.Infinity;

            float diff = (lockedTarget.position - gameObject.transform.position).sqrMagnitude;
 
            if(diff < oldDistance)
            {
                oldDistance = diff;
                lockedTarget = target.transform;
            }        
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		trackETA += Time.deltaTime;	
		if(trackETA >= trackDelay){
			TrackTarget();
			}	
	}

 	public void TrackTarget(){

         //GetComponent<Rigidbody>().AddForce(transform.up * 100);
 
         Vector3 targetDelta = lockedTarget.position - transform.position;
         
         //get the angle between transform.forward and target delta
         float angleDiff = Vector3.Angle(transform.forward, targetDelta);
                            
         // get its cross product, which is the axis of rotation to
         // get from one vector to the other
         Vector3 cross = Vector3.Cross(transform.forward, targetDelta);
  
     	 proj.GetComponent<Rigidbody>().AddTorque(cross * angleDiff * proj.turnRate);
     	 proj.GetComponent<Rigidbody>().transform.LookAt(lockedTarget.position);
 		
 	}
}
