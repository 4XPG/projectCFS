using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDHandling : MonoBehaviour {

	private GameObject playerObject;
 	private GameObject ownObject;
	private float heading;
	private float pitch;
	private float roll;
	private float ias;
	private float lastLocation;
	private float altitude = 0.0f;
	private Vector3 iasbar;
	private Vector3 lastPos;
	private RaycastHit hit;

	private float topSpeed = 145.0f;
	private float velscrollspeed;

	private Vector3 hdginitialpos; // 0 value coord for heading bar
	private Vector3 velinitialpos; // 0 value coord for velocity bar
	private Vector3 altinitialpos; // 0 value coord for altitude bar
	private Vector3 fpminitialpos; // literally center of the hud
	private Vector3 ladderinitialpos;
	private Vector3 pitchladderinitialpos;

	private float rationVeltoPixel;
	private float rationAlttoPixel;
    private float rationAngleToPixel;
    private float numberOfPixelsNorthToNorth = 1216.0f;
	private float numberOfPixelstoMaxVel	 = 1200.0f;
	private float numberOfPixelstoMaxPitch	 = 1184.0f;
	private float altLimit	 = 1200.0f;
	private float velstartcoord;
	private float altstartcoord;
	private float pitchstartcoord;
	private float climbrate;
	private float machspeed;
	private float currentAoA;
	private float maxAoA; // also AoA limiter???
	private float p;
	public float rollangle;

	private float pitchf; private float rollf; private float yawf;

	public RectTransform hdgscale;
	public RectTransform velscale;
	public RectTransform altscale;
	public RectTransform pitchladder;
	//public RectTransform bankind;

	public RectTransform fpm;
	public AeroplanePhysics ac;
	public Text vel;
	public Text alt;
	public Text hdg;
	public Text machind;
	public Text curAoA;
	public Text peakAoA;
	
	// Use this for initialization
	void Start () {
		ownObject = GameObject.FindGameObjectWithTag("PlayerAbstract");
		//lastLocation = ownObject.transform.position;
		playerObject = GameObject.FindGameObjectWithTag("Player");
		lastPos = ownObject.transform.position;
		iasbar = velscale.localPosition;
		hdginitialpos = hdgscale.localPosition;
		velinitialpos = velscale.localPosition;
		altinitialpos = altscale.localPosition;
		fpminitialpos = fpm.localPosition;
		rationAngleToPixel = numberOfPixelsNorthToNorth / 360f;
		velstartcoord = velscale.localPosition.y;
		altstartcoord = altscale.localPosition.y;
		pitchstartcoord = pitchladder.localPosition.y;
		ladderinitialpos = pitchladder.localPosition;

		//altscale init

	}
	
	// Update is called once per frame
	void Update () {
	 // Altitude
   	 climbrate = ac.climbRate;
     altitude = ac.Altitude;
     rationAlttoPixel = altitude / ac.PitchAngle; // climb/dive rate?
     if(altitude > 0){
	 	//velscale.anchoredPosition = Mathf.Lerp(velstartcoord,numberOfPixelstoMaxVel,rationVeltoPixel);
	 	altscale.anchoredPosition = altinitialpos + (new Vector3(0,Mathf.Lerp(altitude,altLimit,climbrate) * ac.PitchAngle,0));  //TODO: make the texture loops
     }
     else if(altitude == 0){
 	 	altscale.anchoredPosition = altinitialpos;    	
     }

     // Heading
     //heading = Mathf.Atan2(transform.forward.z, transform.forward.x) * Mathf.Rad2Deg;
     heading = playerObject.transform.eulerAngles.y;
     Vector3 perp = Vector3.Cross(Vector3.forward, playerObject.transform.forward);
     float dir = Vector3.Dot(perp, Vector3.up);     
     hdgscale.anchoredPosition = hdginitialpos - (new Vector3(Vector3.Angle(new Vector3(playerObject.transform.forward.x, 0f, playerObject.transform.forward.z), Vector3.forward) * Mathf.Sign(dir) * rationAngleToPixel, 0, 0));


     //ias = ((180.0f - Vector3.Angle(displacementVector, transform.forward)) / 180.0f) * displacement;     
     ias = ac.ForwardSpeed * 3.281f;
     //speedscale = ias;
	 rationVeltoPixel = ias / (topSpeed* 3.281f);
     if(ias > 0){
	 	//velscale.anchoredPosition = Mathf.Lerp(velstartcoord,numberOfPixelstoMaxVel,rationVeltoPixel);
	 	velscale.anchoredPosition = velinitialpos - (new Vector3(0,Mathf.Lerp(velstartcoord,numberOfPixelstoMaxVel,(topSpeed* 3.281f)) * rationVeltoPixel,0));  
     }
     else if(ias == 0){
 	 	velscale.anchoredPosition = velinitialpos;    	
     }
     //iasbar.y -= speedscale;
     //fpm movement is dictated by raycast shoot from aircraft

     // pitch ladder
     
     var localFlatForward = transform.InverseTransformDirection(pitchladder.transform.forward); //TODO: fix ladder movement/rotation
     	pitchladder.localRotation = Quaternion.AngleAxis(rollf, localFlatForward) * pitchladder.localRotation;
     pitchladder.localPosition = ladderinitialpos + (new Vector3(0,Mathf.Lerp(pitchstartcoord,numberOfPixelstoMaxPitch,ac.PitchAngle),0));  
	 

     // FPM
     p = ac.PitchAngle * 1.5f + 128.0f; //TODO: synchronize with plane movement
     rollf = ac.RollAngle * 1.5f;
     //fpm.rotation = Quaternion.AngleAxis(ac.RollAngle, Vector3.back);
     fpm.rotation = Quaternion.Euler(0,0, rollf);
     fpm.anchoredPosition = fpminitialpos + (new Vector3(playerObject.transform.forward.x, playerObject.transform.forward.y, playerObject.transform.forward.z));

     // mach indicator
     machspeed = ias / 767.269f;
	}

	void OnGUI () {
		hdg.text = (Mathf.Round(heading)).ToString();
		alt.text = (Mathf.Round(altitude)).ToString();
		vel.text = (Mathf.Round(ias)).ToString();
		machind.text = (System.Math.Round(machspeed,2)).ToString();
		curAoA.text = (Mathf.Round(ac.PitchAngle)).ToString();
	 	//GUI.Label(new Rect(20,0,100,40), heading.ToString());
     	//GUI.Label(new Rect(20,50,100,40), pitch.ToString());
     	//GUI.Label(new Rect(20,100,100,40), roll.ToString());
	}

	Vector3 ProjectPointOnPlane(Vector3 planeNormal  ,   Vector3 planePoint  ,   Vector3 point){
		planeNormal.Normalize();
        float distance= -Vector3.Dot(planeNormal.normalized, (point - planePoint));
        return point + planeNormal * distance;
	}
	 	float SignedAngle ( Vector3 v1 ,  Vector3 v2 ,   Vector3 normal  ){
     	Vector3 perp= Vector3.Cross(normal, v1);
     	float angle= Vector3.Angle(v1, v2);
     	angle *= Mathf.Sign(Vector3.Dot(perp, v2));
     	return angle;
 	}	
}
