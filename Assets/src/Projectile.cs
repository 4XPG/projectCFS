using UnityEditor;
using UnityEngine;

public class Projectile : MonoBehaviour {
	new Transform transform;
	new Rigidbody rigidbody;
	new BoxCollider collider;


	public enum ProjTypes : int {
		Gun,
		IRM,
		SAHM,
		ARM,
		AGM,
		Bomb
	};

	public ProjTypes projType = ProjTypes.IRM;
	public float fuseDelay = 0.0f;
	public float ProjSpeed = 0.0f;

	public bool Fire = false;

//private ParticleSystem SmokePrefab;
//private AudioClip missileClip;


	private Vector3 distanceDelta;
	private Vector3 playerPos;
	public Vector3 projAccel;

	private Vector3 parentcraftvel;
	public Transform lockedtarget;

	public GameObject explosion;
	public Component missileTrail;
	public Transform parentcraft;
//private EmissionModule smoketrail;
// Use this for initialization

	public float seekerFOV = 45.0f;
	public float optimumRange = 50000.0f;
	public bool overrideInitialSpeed = false;
	public float initialSpeed = 0.0f;
	public float BoostFuel = 3.0f; // how long the rocket motor lives
	public float acceleration = 15.0f;
	public float turnRate = 45.0f;
	public float timeToLive = 15.0f;

	public float dropDelay = 0.0f;
	public Vector3 ejectVelocity = Vector3.zero;

	public bool gravity = true;
	private Vector3 launchVelocity = Vector3.zero;

	private float launchTime = 0.0f;
	private float activateTime = 0.0f;
	private float missileSpeed = 0.0f;

	private bool isLaunched = false;
	private bool missileActive = false;
	private bool motorActive = false;
	private bool targetTracking = true;

	private Vector3 targetPosLastFrame;
	private Quaternion guidedRotation;
    public FCR radar;

// Used to prevent lead markers from getting huge when missiles are very slow.
	private const float MINIMUM_GUIDE_SPEED = 1.0f;

	public bool MissileLaunched {
		get {
			return isLaunched;
		}
	}

	public bool MotorActive {
		get {
			return motorActive;
		}
	}

	private void Awake() {
        radar = FindObjectOfType<FCR>();
		transform = GetComponent<Transform>();
		rigidbody = GetComponent<Rigidbody>();
		collider = GetComponent<BoxCollider>();
        //transform.Find("missiletrail").gameObject.SetActive(false);
        if(lockedtarget == null)
			lockedtarget = GameObject.Find("NoAim").transform;
        else
            lockedtarget = radar.selectedTarget.transform;
	}

	private void Start() {
// Sets it so that missile cannot collide with the thing that launched it.
/*		if (parentcraft != null)
		{
			foreach (Collider col in parentcraft.GetComponentsInChildren<Collider>())
				Physics.IgnoreCollision(collider, col);
		}*/

// If this hasn't already been launched, make sure it's kinematic so that it can be mounted on
// stuff. When a missile is spawned and then launched immediately, Launch happens before start.
		if (!isLaunched)
			rigidbody.isKinematic = true;
	}

	private void FixedUpdate() {
		MissileGuidance();
		RunMissile();
	}

	private void OnCollisionEnter(Collision collision) {
// Prevent missile from exploding if it hasn't activated yet.
		if (isLaunched && TimeSince(launchTime) > dropDelay)
		{
// This is a good place to apply damage based on what was collided with.
			DestroyMissile(true);
		}
	}

	public void Launch(Transform newTarget) {
		Launch(newTarget, Vector3.zero);
	}

	public void Launch(Transform newTarget, Vector3 inheritedVelocity) {
		if (!isLaunched)
		{
			isLaunched = true;
			launchTime = Time.time;
			transform.parent = null;
			lockedtarget = newTarget;
			launchVelocity = inheritedVelocity;
			rigidbody.isKinematic = false;

			if (dropDelay > 0.0f)
			{
				rigidbody.useGravity = gravity;
				rigidbody.velocity = inheritedVelocity + transform.TransformDirection(ejectVelocity);
			}
			else
				ActivateMissile();
		}
	}

	private void RunMissile() {
		if (isLaunched)
		{
			if (!missileActive && dropDelay > 0.0f && TimeSince(launchTime) > dropDelay)
				ActivateMissile();

			if (missileActive)
			{
				if (BoostFuel > 0.0f && TimeSince(activateTime) > BoostFuel){
                    motorActive = false;
                    transform.Find("missiletrail").gameObject.SetActive(false);
                }
				else{
                    motorActive = true;
                    transform.Find("missiletrail").gameObject.SetActive(true);
                }


				if (motorActive)
					missileSpeed += acceleration * Time.deltaTime;

				if (targetTracking)
					transform.rotation = Quaternion.RotateTowards(transform.rotation, guidedRotation, turnRate * Time.deltaTime);

				rigidbody.velocity = transform.forward * missileSpeed;
			}

			if (TimeSince(launchTime) > timeToLive)
				DestroyMissile(false);
		}
	}

	private void MissileGuidance() {
		Vector3 relPos = lockedtarget.position - transform.position;
		float angleToTarget = Mathf.Abs(Vector3.Angle(transform.forward.normalized, relPos.normalized));
		float dist = Vector3.Distance(lockedtarget.position, transform.position);

		if (angleToTarget > seekerFOV || dist > optimumRange)
			targetTracking = false;

		if (targetTracking)
		{
			relPos = lockedtarget.position - transform.position;
			guidedRotation = Quaternion.LookRotation(relPos, transform.up);
		}
	}

	private void ActivateMissile() {
		if (overrideInitialSpeed)
		{
			if (dropDelay > 0.0f)
			{
				float localForwardSpeed = transform.InverseTransformDirection(rigidbody.velocity).z;
				initialSpeed = localForwardSpeed;
			}
			else
			{
				float localForwardSpeed = transform.InverseTransformDirection(launchVelocity).z;
				initialSpeed = localForwardSpeed;
			}
		}

		rigidbody.useGravity = false;
		rigidbody.velocity = Vector3.zero;
		missileActive = true;

		if (BoostFuel <= 0.0f)
			motorActive = true;

		activateTime = Time.time;
		missileSpeed = initialSpeed;

		if (lockedtarget != null)
			targetPosLastFrame = lockedtarget.transform.position;
	}

	private void DestroyMissile(bool impact) {
		Destroy(gameObject);
		if (impact)
			Instantiate(explosion, transform.position, transform.rotation);
	}

	private float TimeSince(float since) {
		return Time.time - since;
	}
}