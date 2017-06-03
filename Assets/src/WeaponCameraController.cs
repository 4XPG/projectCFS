using UnityEngine;
using System.Collections;

public class WeaponCameraController : MonoBehaviour {
    public Camera tgpcam;
	private float minazimuth = -30.0f;
	private float maxazimuth = 30.0f;

	private float minelev = -30.0f;
	private float maxelev = 30.0f;

	public float zoomvalue;
    public float defaultzoom = 5.0f;
    private float zoompos;
	private float xRot = 0;
	private float yRot = 0;
    public int zoomLevelSelected = 1;
    public float[] ZoomLevels = new float[] { 120, 60, 20 };
    private int zoomchange = 0;  //<<<<<<<<<<<<<
	public GameObject AGMTargetPos;

	// Use this for initialization
	void Start () {
		AGMTargetPos = GameObject.FindGameObjectWithTag("AGMTarget");
        //initialzoompos = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {

		if ((Input.GetKey ("i")) && (xRot <= maxelev)) { //slew up
			//transform.RotateAround(transform.position,transform.right,sensY * Time.deltaTime);
			xRot++;
		}
		else if (Input.GetKey ("k") && (xRot >= minelev)) { //slew down
			//transform.RotateAround(transform.position,transform.right,-sensY * Time.deltaTime);
			xRot--;
		}
		else if (Input.GetKey ("j") && (yRot >= minazimuth)) { //slew left
			//transform.RotateAround(transform.position,Vector3.up,-sensX * Time.deltaTime);
			yRot--;
		}
		else if (Input.GetKey ("l") && (yRot <= maxazimuth)) { //slew right
			//transform.RotateAround(transform.position,Vector3.up,sensX * Time.deltaTime);
			yRot++;
		}
        else if (Input.GetKeyDown(name:"o") ){
            zoomchange += 1;
        }
        else if (Input.GetKeyDown(name:"p") ){
            zoomchange -= 1;
        }
		if (Input.GetKeyDown ("m")) { // designate target/create sensor point of interest
				//Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			Ray ray = GetComponent<Camera>().ScreenPointToRay(new Vector3(0.5f,0.5f,0));
			RaycastHit Hit;
			if(Physics.Raycast(ray, out Hit)){
				//set the target position
				AGMTargetPos.transform.position = Hit.point;
				Debug.Log(AGMTargetPos.transform.position);
			}
		}
        zoomLevelSelected = Mathf.Clamp(zoomLevelSelected + zoomchange, 0, ZoomLevels.Length -1);
        gameObject.GetComponent<Camera>().fieldOfView = ZoomLevels[zoomLevelSelected];

		transform.eulerAngles = new Vector3 (xRot, yRot, 0);
		//Debug.Log (transform.eulerAngles);
	}
}
