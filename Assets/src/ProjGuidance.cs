using UnityEngine;
using System.Collections;

public class ProjGuidance : MonoBehaviour {

	public Projectile proj;
	public GameObject target;
    public GameObject player;
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
			target = GameObject.FindGameObjectWithTag("SelectedTarget");

			proj = gameObject.GetComponent<Projectile>();
		if (target != null) {
			lockedTarget = target.transform;
			prevtargetpos = lockedTarget.position;
		}
		prevpos = transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		prevpos = currentpos;
		prevtargetpos = currentpos;
	}

	public void TrackTargetIR(){ //limited lock FOV
	}


    public void PNTrack(){
        float LOSRate;
        float closure;
        Vector3 DeltaLOS;
        trackETA += Time.deltaTime;
        Debug.Log(target.transform.position);
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
                    TrackTargetAR ();
            }
        }
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

    public Vector3 FindInterceptVector(Vector3 shotOrigin, float shotSpeed, Vector3 targetOrigin, Vector3 targetVel) {

        Vector3 dirToTarget = Vector3.Normalize(targetOrigin - shotOrigin);

// Decompose the target's velocity into the part parallel to the
// direction to the cannon and the part tangential to it.
// The part towards the cannon is found by projecting the target's
// velocity on dirToTarget using a dot product.
        Vector3 targetVelOrth = Vector3.Dot(targetVel, dirToTarget) * dirToTarget;

// The tangential part is then found by subtracting the
// result from the target velocity.
        Vector3 targetVelTang = targetVel - targetVelOrth;

        /*
        * targetVelOrth
        * |
        * |
        *
        * ^...7  <-targetVel
        * |  /.
        * | / .
        * |/ .
        * t--->  <-targetVelTang
        *
        *
        * s--->  <-shotVelTang
        *
        */

// The tangential component of the velocities should be the same
// (or there is no chance to hit)
// THIS IS THE MAIN INSIGHT!
        Vector3 shotVelTang = targetVelTang;

// Now all we have to find is the orthogonal velocity of the shot

        float shotVelSpeed = shotVelTang.magnitude;
        if (shotVelSpeed > shotSpeed) {
// Shot is too slow to intercept target, it will never catch up.
// Do our best by aiming in the direction of the targets velocity.
            return targetVel.normalized * shotSpeed;
        } else {
// We know the shot speed, and the tangential velocity.
// Using pythagoras we can find the orthogonal velocity.
            float shotSpeedOrth =
            Mathf.Sqrt(shotSpeed * shotSpeed - shotVelSpeed * shotVelSpeed);
            Vector3 shotVelOrth = dirToTarget * shotSpeedOrth;


// Find the time of collision (distance / relative velocity)
            //float timeToCollision = ((shotOrigin - targetOrigin).magnitude - shotRadius - targetRadius) / (shotVelOrth.magnitude-targetVelOrth.magnitude);
            float timeToCollision = ((shotOrigin - targetOrigin).magnitude) / (shotVelOrth.magnitude-targetVelOrth.magnitude);
// Calculate where the shot will be at the time of collision
            Vector3 shotVel = shotVelOrth + shotVelTang;
            Vector3 shotCollisionPoint = shotOrigin + shotVel * timeToCollision;

// Finally, add the tangential and orthogonal velocities.
            return shotVelOrth + shotVelTang;
        }
    }

    void CalculateAngleToHitTarget(out float? theta1, out float? theta2)
    {
//Initial speed
        float v = proj.ProjSpeed;

        Vector3 targetVec = target.transform.position - player.transform.position;

//Vertical distance
        float y = targetVec.y;

//Reset y so we can get the horizontal distance x
        targetVec.y = 0f;

//Horizontal distance
        float x = targetVec.magnitude;

//Gravity
        float g = 9.81f;


//Calculate the angles

        float vSqr = v * v;

        float underTheRoot = (vSqr * vSqr) - g * (g * x * x + 2 * y * vSqr);

//Check if we are within range
        if (underTheRoot >= 0f)
        {
            float rightSide = Mathf.Sqrt(underTheRoot);

            float top1 = vSqr + rightSide;
            float top2 = vSqr - rightSide;

            float bottom = g * x;

            theta1 = Mathf.Atan2(top1, bottom) * Mathf.Rad2Deg;
            theta2 = Mathf.Atan2(top2, bottom) * Mathf.Rad2Deg;
        }
        else
        {
            theta1 = null;
            theta2 = null;
        }
    }

	public void TrackDot(GameObject TargetPos){ //track raycasted point made by the EO targetting pod
        gameObject.transform.LookAt(TargetPos.transform);
	}
}
