using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour {
	public Bogey[] bogeysonmap;
    public GameObject[] markersonmap;
    public GameObject AGMTarget;
    public FCR radar;
    public RectTransform TargetCursor;

	public Sprite AirmarkerSprite;
    public Sprite AirtrackedMarkerSprite;
    public Sprite GroundmarkerSprite;
    public Sprite GroundtrackedMarkerSprite;

    public WeaponCameraController wpn;
    private Camera wpncamera;

    private GameObject currentlySelectedTarget;
    private GameObject newTarget;


	void Awake () {
        wpncamera = wpn.tgpcam;
	}

    void Start(){
    }
	
	// Update is called once per frame
	void Update () {
        markersonmap = GameObject.FindGameObjectsWithTag("Enemy");
        bogeysonmap = new Bogey[markersonmap.Length];
        for(int i=0;i<bogeysonmap.Length;i++) {
            bogeysonmap[i] = markersonmap[i].gameObject.GetComponent<ObjectIdentifier>().objectPos;
        }
        Debug.Log("radar contacts:"+markersonmap.Length);
        TargetCursor = radar.FCRCursor;
/*        if(radar.RadarMode == FCR.RadarModes.Ground){
            //AGMTarget.transform.position = targetpos.transform.position;
            radar.selectedTarget = AGMTarget;
        }*/
        for(int i=0;i<markersonmap.Length;i++){
            checkMarkerSymbol(bogeysonmap[i]);

            if(CheckOverlap(TargetCursor,markersonmap[i].GetComponent<RectTransform>())){
                if (Input.GetKeyDown(KeyCode.Z)) {
                    GameObject targetpos = markersonmap[i].gameObject.GetComponent<ObjectIdentifier>().objectPos.gameObject;
                    SelectTarget(targetpos);
//targetpos.GetComponent<Bogey>().markerSprite = AirtrackedMarkerSprite;
                    TargetCursor.anchoredPosition = markersonmap[i].gameObject.GetComponent<RectTransform>().anchoredPosition;
                    if(radar.RadarMode == FCR.RadarModes.Ground){
                        wpncamera.transform.LookAt(targetpos.transform, Vector3.up);
                    }
                    if(radar.selectedTarget != targetpos){
                        targetpos.GetComponent<Bogey>().isLocked = false;
                    }
                }
            }
        }
	}


    bool rectOverlaps(RectTransform rectTrans1, RectTransform rectTrans2)
    {
        Rect rect1 = new Rect(rectTrans1.anchoredPosition.x, rectTrans1.anchoredPosition.y, rectTrans1.rect.width, rectTrans1.rect.height);
        Rect rect2 = new Rect(rectTrans2.anchoredPosition.x, rectTrans2.anchoredPosition.y, rectTrans2.rect.width, rectTrans2.rect.height);

        return rect1.Overlaps(rect2);
    }

    bool CheckOverlap(RectTransform RadarCursor, RectTransform marker){
//Debug.Log("Bandit position:"+markerImage.rectTransform.anchoredPosition);
//Debug.Log(RadarCursor.anchoredPosition);
        if(rectOverlaps(RadarCursor,marker)){
            Debug.Log("Bandit position:"+marker.anchoredPosition);
            Debug.Log(gameObject.transform.position);
            return true;
        }
        else
            return false;
    }

    void SelectTarget(GameObject target) {
//GameObject[] TargetArray;
        //Vector3 screenPoint = Camera.main.WorldToViewportPoint(target.transform.position);
        //target.tag = "SelectedTarget";
/*        if(radar.selectedTarget == currentlySelectedTarget){

        }
        else{*/
            radar.selectedTarget = target;
            Bogey marker = target.GetComponent<Bogey>();
            //marker.markerImage.sprite = marker.trackedMarkerSprite;
            currentlySelectedTarget = target;
            target.GetComponent<Bogey>().isLocked = true;
       // }
    }

    void SwitchTarget(GameObject prevObject, GameObject nextObject){
        GameObject tempObject;

    }

    void checkMarkerSymbol(Bogey target){
        if(target.isLocked){
            target.markerImage.sprite = target.trackedMarkerSprite;
        }
        else{
            target.markerImage.sprite = target.markerSprite;
        }
    }
}
