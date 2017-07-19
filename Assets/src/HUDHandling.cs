using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDHandling : MonoBehaviour {
	public enum HUDModes : int {NAV, AA, AG}; // NAV/default, Air-to-air, Air-to-ground


	public HUDModes HudMode = HUDModes.NAV;
	private GameObject playerObject;
 	private GameObject targetObject;
	private float heading;
	private float pitch;
	private float roll;
	private float ias;
	private float lastLocation;
	private float altitude = 0.0f;
	private Vector3 iasbar;
	private RaycastHit hit	;

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
    private float rationPitchToPixel;
    private float numberOfPixelsNorthToNorth = 1216.0f;
	private float numberOfPixelstoMaxVel	 = 1184.0f;
	private float numberOfPixelstoMaxPitch	 = 7200.0f;
	private float altLimit	 = 1200.0f;
	private float velstartcoord;
	private float altstartcoord;
	private float pitchstartcoord;
	private float aspectstartangle;
	private float climbrate;
	private float machspeed;
	private float currentAoA;
	private float maxAoA; // also AoA limiter???
	private float p;
	private float targetrng;
	private float targetalt;
	public float rollangle;

	private float pitchf; private float rollf; private float yawf;

	public RectTransform hdgscale;
	public RectTransform velscale;
	public RectTransform altscale;
    public RectTransform ladder;
		public RectTransform pitchladder;
		public RectTransform TDBox;
	public RectTransform ASECircle;
	public RectTransform ASECaret;
	public RectTransform DLZCaret;
	public RectTransform radarHorizon;
	public RectTransform bankIndicatorCaret;
    public RectTransform LinePointer;
    public RectTransform CCIPSight;
    public RectTransform LCOSSight;

	//public RectTransform bankind;

	public RectTransform fpm;
	private AeroplanePhysics tgtac;
	public AeroplanePhysics ac;
	public Text modetext;
	public Text vel;
	public Text alt;
	public Text hdg;
	public Text machind;
	public Text curAoA;
	public Text peakAoA;
	public Text Tgt_Alt;
	public Text Tgt_Rng;
	public Text AmmoCounter;

	public FCR radar;
	public WeaponController wp;
	public int HUDMode;
	// Use this for initialization
	void Start () {
		playerObject = GameObject.FindGameObjectWithTag("Player");
		targetObject = GameObject.FindGameObjectWithTag ("SelectedTarget");
        if(targetObject != null)
			tgtac = targetObject.GetComponent<AeroplanePhysics>();
		iasbar = velscale.localPosition;
		hdginitialpos = hdgscale.localPosition;
		velinitialpos = velscale.localPosition;
		altinitialpos = altscale.localPosition;
		fpminitialpos = fpm.localPosition;
		rationAngleToPixel = numberOfPixelsNorthToNorth / 360f;
        rationPitchToPixel = numberOfPixelstoMaxPitch / 360f;
		velstartcoord = velscale.localPosition.y;
		altstartcoord = altscale.localPosition.y;
		pitchstartcoord = pitchladder.localPosition.y;
		ladderinitialpos = pitchladder.localPosition;
		aspectstartangle = ASECaret.rotation.z;
		//selectedWeapon = wp.currentWeapon;
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
	 	altscale.anchoredPosition = altinitialpos + (new Vector3(0,Mathf.Lerp(altitude,altLimit,climbrate) * ac.PitchAngle,0));
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
	 float rot = playerObject.transform.rotation.z;
     var localFlatForward = transform.InverseTransformDirection(pitchladder.transform.forward); //TODO: fix ladder movement/rotation
		Vector3 playerPos = playerObject.transform.position;
		//Quaternion plRotation = Quaternion.LookRotation(playerPos - pitchladder.localposition);
		Quaternion plRotation = Quaternion.AngleAxis(playerObject.transform.rotation.x,Vector3.up);
		//pitchladder.localRotation = Quaternion.Slerp(pitchladder.localRotation,plRotation, Time.deltaTime);
		ladder.localRotation = Quaternion.Euler(0,0,(playerObject.transform.rotation.eulerAngles.z * -1));
        Vector3 perp2 = Vector3.Cross(Vector3.up, playerObject.transform.up);
        float pdir = Vector3.Dot(perp2,Vector3.forward);
    //pitchladder.localPosition = ladderinitialpos + (new Vector3(0,Mathf.Lerp(pitchstartcoord,numberOfPixelstoMaxPitch,ac.PitchAngle),0));
        pitchladder.localPosition = ladderinitialpos + (new Vector3(0,Vector3.Angle(playerObject.transform.up,Vector3.up) * Mathf.Sign(pdir) * rationPitchToPixel,0));
	 
	radarHorizon.localRotation = Quaternion.Slerp(radarHorizon.localRotation,playerObject.transform.rotation, Time.deltaTime);
        bankIndicatorCaret.localRotation = Quaternion.Euler(0,0,playerObject.transform.rotation.eulerAngles.z);
		//bankIndicatorCaret.localRotation = Quaternion.Slerp(radarHorizon.localRotation,playerObject.transform.rotation.eulerAngles, Time.deltaTime);
     // FPM
     p = ac.PitchAngle * 1.5f + 128.0f; //TODO: synchronize with plane movement
     rollf = ac.RollAngle * 1.5f;
     //fpm.rotation = Quaternion.AngleAxis(ac.RollAngle, Vector3.back);
     fpm.localRotation = Quaternion.Euler(0,0,(playerObject.transform.rotation.eulerAngles.z * -1));
     fpm.localPosition = fpminitialpos + new Vector3((Vector3.Angle(playerObject.transform.forward,Vector3.forward) * Mathf.Sign(dir)),(Vector3.Angle(playerObject.transform.up,Vector3.up) * Mathf.Sign(pdir)),0) ;

     // mach indicator
     machspeed = ias / 767.269f;

	// target altitude & range
		if (targetObject != null) {
			targetrng = Vector3.Distance (targetObject.transform.position, playerObject.transform.position);
			targetalt = tgtac.Altitude;

			//A-SEC caret
			Vector3 tgtaspect = targetObject.transform.position - playerObject.transform.position;
			float angle = Mathf.Atan2 (tgtaspect.y, tgtaspect.x) * Mathf.Rad2Deg;
			ASECaret.localRotation = Quaternion.Euler (0, 0, aspectstartangle + angle);
		} else {
			targetrng = 0;
			targetalt = 0;
		}

	}

	void OnGUI () {
		hdg.text = (Mathf.Round(heading)).ToString();
		alt.text = (Mathf.Round(altitude)).ToString();
		vel.text = (Mathf.Round(ias)).ToString();
		machind.text = (System.Math.Round(machspeed,2)).ToString();
		curAoA.text = (Mathf.Round(ac.PitchAngle)).ToString();
		Tgt_Rng.text = (Mathf.Round(targetrng)).ToString();
		Tgt_Alt.text = (System.Math.Round(targetalt,2)).ToString();
	 	//GUI.Label(new Rect(20,0,100,40), heading.ToString());
     	//GUI.Label(new Rect(20,50,100,40), pitch.ToString());
     	//GUI.Label(new Rect(20,100,100,40), roll.ToString());
        ChangeHUDMode(HUDMode);
	}
	public void ChangeHUDMode(int HUDMode){
		if (HUDMode == 0) { // SRM HUDmode
			//SwitchWeapon(0);
			//currentWeapon = 0;
			modetext.text = "SRM";
            AmmoCounter.text = wp.IRMAmmo.ToString();
            ASECircle.gameObject.SetActive(true);
		} else if (HUDMode == 1) { // SRM HUDmode
			//SwitchWeapon(0);
			//currentWeapon = 0;
			modetext.text = "MRM";
            AmmoCounter.text = wp.SAHMAmmo.ToString();
            ASECircle.gameObject.SetActive(true);
		} else if (HUDMode == 2) { // SAR-AAM HUDmode
			//SwitchWeapon(0);
			//currentWeapon = 0;
			modetext.text = "MRM";
            AmmoCounter.text = wp.ARMAmmo.ToString();
            ASECircle.gameObject.SetActive(true);
		} else if (HUDMode == 3) { // AR-AAM HUDmode
			//SwitchWeapon(0);
			//currentWeapon = 0;
			modetext.text = "AGM";
            AmmoCounter.text = wp.AGMAmmo.ToString();
            ASECircle.gameObject.SetActive(false);
		} else if (HUDMode == 4) { // CCIP HUDmode
			//SwitchWeapon(0);
			//currentWeapon = 0;
			modetext.text = "CCIP";
            CCIPSight.gameObject.SetActive(true);
            LCOSSight.gameObject.SetActive(false);
            ASECircle.gameObject.SetActive(false);
            AmmoCounter.text = wp.BombAmmo.ToString();
		} else if (HUDMode == 5) { // LCOS HUDmode
            modetext.text = "LCOS";
            CCIPSight.gameObject.SetActive(false);
			LCOSSight.gameObject.SetActive(true);
            ASECircle.gameObject.SetActive(false);
            AmmoCounter.text = wp.gunammo.ToString();
		}
	}
/*
    public void LinePointToTarget(){

        Vector3 v3Pos = Camera.main.WorldToViewportPoint(targetObject.transform.position);

        if (v3Pos.z < Camera.main.nearClipPlane)
            return;  // Object is behind the camera

        if (v3Pos.x >= 0.0f && v3Pos.x <= 1.0f && v3Pos.y >= 0.0f && v3Pos.y <= 1.0f)
            return; // Object center is visible

        renderer.enabled = true;
        v3Pos.x -= 0.5f;  // Translate to use center of viewport
        v3Pos.y -= 0.5f;
        v3Pos.z = 0;      // I think I can do this rather than do a
//   a full projection onto the plane

        float fAngle = Mathf.Atan2 (v3Pos.x, v3Pos.y);
        transform.localEulerAngles = new Vector3(0.0f, 0.0f, -fAngle * Mathf.Rad2Deg);

        v3Pos.x = 0.5f * Mathf.Sin (fAngle) + 0.5f;  // Place on ellipse touching
        v3Pos.y = 0.5f * Mathf.Cos (fAngle) + 0.5f;  //   side of viewport
        v3Pos.z = Camera.main.nearClipPlane + 0.01f;  // Looking from neg to pos Z;
        transform.position = Camera.main.ViewportToWorldPoint(v3Pos);
    }*/
}
