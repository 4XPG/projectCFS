using UnityEditor;
using UnityEngine;

public class Projectile : MonoBehaviour {

 	public enum ProjTypes : int {Gun,IRM,SAHM,ARM,AGM,Bomb};
 	public ProjTypes projType = ProjTypes.IRM;
	public float fuseDelay = 0.0f;
	public float ProjSpeed = 0.0f;
	public float BoostFuel = 300.0f; // how long the rocket motor lives
	public float turnRate = 0.8f;
	public float optimumRange = 1000.0f;
	public bool Fire = false;

	//private ParticleSystem SmokePrefab;
	//private AudioClip missileClip;


	private Vector3 distanceDelta;
	private Vector3 playerPos;
	public Vector3 projAccel;

	private ProjGuidance guidance;
	private Vector3 parentcraftvel;
	public GameObject lockedtarget;

	public GameObject explosion;
    public Component missileTrail;
	public GameObject parentcraft;
	//private EmissionModule smoketrail;
	// Use this for initialization



	void Start () {
        //ProjSpeed = 0.0f;
        gameObject.GetComponent<BoxCollider>().enabled = false;
        parentcraftvel = parentcraft.GetComponent<Rigidbody>().velocity.normalized;
        guidance = gameObject.GetComponent<ProjGuidance>();
        lockedtarget = GameObject.FindGameObjectWithTag("SelectedTarget");
        //missileTrail = gameObject.GetComponentsInChildren< ParticleSystem >();
        //missileTrail.GetComponent<ParticleEmitter>().enableEmission = false;
        if(projType != ProjTypes.Bomb)
            transform.Find("missiletrail").gameObject.SetActive(false);
        //this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
    		//lockedTarget.position = target.position;
            //projectileRB = GetComponent<Rigidbody>();
            //Fire();
         // apply torque along that axis according to the magnitude of the angle.



	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{

	}


    void UpdateTrajectory ( Vector3 initialPosition, Vector3 initialVelocity, float mass, Vector3 gravity, Vector3 wind ) {
        int numSteps = 100; // for example
        float timeDelta = 1.0f / initialVelocity.magnitude; // for example

        Vector3 gravityWind = gravity + (wind / mass);

        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetVertexCount( numSteps );

        Vector3 position = initialPosition;
        Vector3 velocity = initialVelocity;
        for ( int i = 0; i < numSteps; ++i ) {
            lineRenderer.SetPosition( i, position );

            position += velocity * timeDelta + 0.5f * gravityWind * timeDelta * timeDelta;
            velocity += gravityWind * timeDelta;
        }
    }




	void OnTriggerEnter(Collider col){
		Instantiate(explosion, transform.position, transform.rotation);
		Destroy(gameObject); // destroy the grenade
        if(col.gameObject.tag == "Air" || col.gameObject.tag == "Ground")
        	Destroy(col.gameObject);
        //Destroy(col.gameObject);
		//Destroy(expl, 3); // delete the explosion after 3 seconds
	}


	// checks target aspect
	//float checkAspect () {
//
	//}
}
