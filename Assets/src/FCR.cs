using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FCR : MonoBehaviour {
    public enum RadarModes {Air,Ground};
    public RadarModes RadarMode = RadarModes.Air;
	public Camera FCRCamera;
	public GameObject playerObject;
    public GameObject trackedObject;
    public GameObject AGMTargetPos;
	public GameObject[] airObjects;
	public GameObject[] groundObjects;
	public Transform[] groundpositions;
	public Image FCRCursor;
    public Image RadarMask;
	public Sprite AirCursor;
    public Sprite GroundCursor;
    public Sprite AirOverlay;
    public Sprite GroundOverlay;
	private Vector3 CameraPos;


    public Text radarZoomText;
    private int RadarzoomLevelSelected = 1;
    private float[] RadarZoomLevels = new float[] { 10, 20, 40, 80, 160 };
    private int zoomchange = 0;  //<<<<<<<<<<<<<
	private Vector3 offset;
	private int cullmask;

    public GameObject radar;

    public RectTransform map;
    public List<Bogey> mapEnemies;
    public GameObject[] airTargets;
    public GameObject[] groundTargets;
    public List<GameObject> enemies;
    public GameObject bogeyIcon;
    public GameObject groundIcon;
    public GameObject airIcon;

	// Use this for initialization
	void Awake () {
        StartCoroutine (UpdateMapPos());
        airTargets = GameObject.FindGameObjectsWithTag("Air");
        groundTargets = GameObject.FindGameObjectsWithTag("Ground");
            if(RadarMode == RadarModes.Air){
                trackedObject = GetClosestEnemy(airTargets);
                trackedObject.gameObject.tag = "SelectedTarget";
            }
            else if(RadarMode == RadarModes.Ground){
                trackedObject = GetClosestEnemy(groundTargets);
                trackedObject.gameObject.tag = "SelectedTarget";
            }
		//CameraPos.x = transform.position.x - playerObject.transform.position.x;
		//CameraPos.z = transform.position.z - playerObject.transform.position.z;
	}
	
	// Update is called once per frame
	void Update () {
        radarZoomControl();
        changeRadarMode();
        if(RadarMode == RadarModes.Air){
            trackedObject = SelectTarget(airTargets);
            trackedObject.gameObject.tag = "SelectedTarget";
        }
        else if(RadarMode == RadarModes.Ground){
            trackedObject = SelectTarget(groundTargets);
            trackedObject.gameObject.tag = "SelectedTarget";
        }

        if(playerObject.transform.position!=radar.transform.position){
//Debug.Log("NOT SAME1");
            radar.transform.position = new Vector3 (playerObject.transform.position.x,radar.transform.position.y,playerObject.transform.position.z);
        }
		//CameraPos.x = playerObject.transform.position.x;
		//CameraPos.z = playerObject.transform.position.z;
		transform.position = playerObject.transform.position + CameraPos;

		//set so that radar icons only moves in z axis 
	}

    void onGUI(){
        radarZoomText.text = (RadarzoomLevelSelected).ToString();
    }

    private void radarZoomIn(){

    }
    void radarZoomControl(){
        if (Input.GetKeyDown(name:"t") ){
            zoomchange += 1;
        }
        else if (Input.GetKeyDown(name:"y") ){
            zoomchange -= 1;
        }
        RadarzoomLevelSelected = Mathf.Clamp(RadarzoomLevelSelected + zoomchange, 0, RadarZoomLevels.Length -1);
        FCRCamera.fieldOfView = RadarZoomLevels[RadarzoomLevelSelected];
    }

	public void changeRadarMode(){
        if(RadarMode == RadarModes.Air){
            if (Input.GetKeyDown(KeyCode.R) ){
                RadarMode = RadarModes.Ground;
                FCRCursor.gameObject.GetComponent<Image>().overrideSprite = GroundCursor;
                RadarMask.gameObject.GetComponent<Image>().overrideSprite = GroundOverlay;
                Debug.Log(RadarMode);
            }
        }
        else if(RadarMode == RadarModes.Ground){
            if (Input.GetKeyDown(KeyCode.R) ){
                RadarMode = RadarModes.Air;
                FCRCursor.gameObject.GetComponent<Image>().overrideSprite = AirCursor;
                RadarMask.gameObject.GetComponent<Image>().overrideSprite = AirOverlay;
                Debug.Log(RadarMode);
                trackedObject = AGMTargetPos;
            }
        }
	}

    GameObject SelectTarget(GameObject[] enemies){
        GameObject SelectedTarget = enemies[0];
        int targetindex = 0;
        if(Input.GetKeyDown(KeyCode.T)){
            if(targetindex ==enemies.Length - 1) targetindex = 0;
            else targetindex++;
            for(int i=0;i<enemies.Length;i++){
                if(i==targetindex)
                    SelectedTarget = enemies[i];
            }
            /*else{
                SelectedTarget = enemies[0];
                SelectedTarget.gameObject.tag = "SelectedTarget";
            }*/
        }
        return SelectedTarget;
    }

    void designateTarget(){
    //raycast this
        //enemyobject.tag = "SelectedTarget";
    }


    GameObject GetClosestEnemy (GameObject[] enemies)
    {
        GameObject bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach(GameObject potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if(dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
        Debug.Log(bestTarget.tag);
        return bestTarget;
    }

    void PopulateRadarScreen(){
            for(int i=0;i<airTargets.Length;i++) {
                Instantiate(bogeyIcon, airTargets[i].transform.position, transform.rotation);
            }
            for(int j=0;j<groundTargets.Length;j++) {
                Instantiate(groundIcon, groundTargets[j].transform.position, transform.rotation);
            }
    }

    void OnTriggerEnter(Collider collider){
        Debug.Log("Collide");
        if(collider.tag == RadarMode.ToString()){
            BoxCollider colliders = collider.GetComponent<BoxCollider>();
            if(enemies.Contains(collider.gameObject)){
            }else{
                collider.tag = "SelectedTarget";
                Debug.Log(collider.tag);
                Bogey tempEnemy = Instantiate(mapEnemies[0].gameObject).GetComponent<Bogey>();
                mapEnemies.Add(tempEnemy);
                enemies.Add(collider.gameObject);
                tempEnemy.rTransform.SetParent(map);
                tempEnemy.rTransform.localPosition = Vector3.zero;
                tempEnemy.rTransform.localRotation = Quaternion.Euler(Vector3.zero);
                tempEnemy.rTransform.localScale = new Vector3(1,1,1);
                tempEnemy.rTransform.anchoredPosition = Vector3.zero;
                tempEnemy.gameObject.SetActive(true);
//colliders.enabled = false;
            }
        }
    }

    void OnTriggerExit(Collider collider){
        if(collider.tag == "SelectedTarget"){
//BoxCollider colliders = collider.GetComponent<BoxCollider>();
            if(enemies.Contains(collider.gameObject)){
                Debug.Log("ReleaseEnemy");
                Bogey tempRemoval = mapEnemies[enemies.IndexOf(collider.gameObject)+1];
                mapEnemies.Remove(tempRemoval);
                Destroy(tempRemoval.gameObject);
                collider.tag = RadarMode.ToString();
                enemies.Remove(collider.gameObject);
            }else{

            }
        }
    }


    IEnumerator UpdateMapPos(){
        while (GameObject.Find("AircraftJet") != null) {
            if(enemies.Count>0){
                for (int i=0; i<enemies.Count; i++) {
                    mapEnemies [i + 1].UpdatePos (enemies [i].transform.position.x - playerObject.transform.position.x , enemies [i].transform.position.z- playerObject.transform.position.z);
                }
            }
            yield return new WaitForSeconds(0.2f);
        }
        yield return 0;
    }

}
