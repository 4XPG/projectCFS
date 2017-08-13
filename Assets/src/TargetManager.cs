using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour {
	public BogeyList bogeysonmap;
    public GameObject[] markersonmap;
    public GameObject[] airTargets;
    public GameObject[] groundTargets;
    private GameObject prevTarget;
    private GameObject nextTarget;
    public GameObject AGMTarget;
    public GameObject SelectedTarget;
    public FCR radar;
    public RectTransform TargetCursor;

	public Sprite AirmarkerSprite;
    public Sprite AirtrackedMarkerSprite;
    public Sprite GroundmarkerSprite;
    public Sprite GroundtrackedMarkerSprite;

// Use this for initialization
	void Awake () {


	}
	
	// Update is called once per frame
	void Update () {
        markersonmap = GameObject.FindGameObjectsWithTag("Enemy");
        airTargets = GameObject.FindGameObjectsWithTag("Air");
        groundTargets = GameObject.FindGameObjectsWithTag("Ground");
        Debug.Log("radar contacts:"+markersonmap.Length);
        SelectedTarget = GameObject.FindGameObjectWithTag("SelectedTarget");
        TargetCursor = radar.FCRCursor;
        if(radar.RadarMode == FCR.RadarModes.Ground){
            //AGMTarget.transform.position = targetpos.transform.position;
            AGMTarget.tag = "SelectedTarget";
        }
        for(int i=0;i<markersonmap.Length;i++){
            if(CheckOverlap(TargetCursor,markersonmap[i].GetComponent<RectTransform>())){
                if (Input.GetKeyDown(KeyCode.Z)) {
                    GameObject targetpos = markersonmap[i].gameObject.GetComponent<ObjectIdentifier>().objectPos;
                    SelectTarget(targetpos);
//TODO: air/ground object tag checking
//targetpos.GetComponent<Bogey>().markerSprite = AirtrackedMarkerSprite;
                    TargetCursor.anchoredPosition = markersonmap[i].gameObject.GetComponent<RectTransform>().anchoredPosition;
                    Debug.Log(gameObject.transform.position);

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
        target.tag = "SelectedTarget";

//GameObject prevTarget = target;
//TargetArray[0] = prevTarget;
//GameObject newTarget;
        /*if(prevTarget.tag == "SelectedTarget"){
            DeselectTarget(prevTarget,newTarget);
        }*/
    }

    void SwitchTarget(GameObject PrevTarget, GameObject NewTarget){
        PrevTarget = GameObject.FindGameObjectWithTag("SelectedTarget");
        if(NewTarget.tag == "Air"){
            PrevTarget.tag = "Air";
            NewTarget.tag = "SelectedTarget";
            PrevTarget.gameObject.GetComponent<Bogey>().markerSprite = AirmarkerSprite;
            NewTarget.gameObject.GetComponent<Bogey>().markerSprite = AirtrackedMarkerSprite;
        }
        else if(NewTarget.tag == "Ground"){
            PrevTarget.tag = "Ground";
            NewTarget.tag = "SelectedTarget";
            PrevTarget.gameObject.GetComponent<Bogey>().markerSprite = GroundmarkerSprite;
            NewTarget.gameObject.GetComponent<Bogey>().markerSprite = GroundtrackedMarkerSprite;
        }
    }

/*    void DeselectTarget(GameObject PrevTarget, GameObject NewTarget){
        PrevTarget = GameObject.FindGameObjectWithTag("SelectedTarget");
        if(NewTarget.tag == "Air"){
            PrevTarget.tag = "Air";
            NewTarget.tag = "SelectedTarget";
            PrevTarget.gameObject.GetComponent<Bogey>().markerSprite = markerSprite;
            NewTarget.gameObject.GetComponent<Bogey>().markerSprite = trackedMarkerSprite;
        }
        else if(NewTarget.tag == "Ground"){
            PrevTarget.tag = "Ground";
            NewTarget.tag = "SelectedTarget";
            PrevTarget.gameObject.GetComponent<Bogey>().markerSprite = markerSprite;
            NewTarget.gameObject.GetComponent<Bogey>().markerSprite = trackedMarkerSprite;
        }
    }*/

    void SetObjectTag(GameObject g, string previousTag, string newTag){
        if (previousTag == "SelectedTarget")
            g.tag = newTag;
    }
}
