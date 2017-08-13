using UnityEngine;
using UnityEngine.UI;

public class RWRThreat : MonoBehaviour {

    public Sprite markerSprite; //HighThreat (diamond)
    public Sprite HighThreatMarkerSprite; //Danger (square)
    public float markerSize;
    public float blipHeight;
    public float blipWidth;
    public bool hasRadar = true;
    public GameObject objectidentifier;
    public GameObject markerImageObject;
    public Image MarkerImage
    {
        get
        {
            return markerImage;
        }
    }

    private Image markerImage;
/*    private GameObject prevTarget;
    private GameObject nextTarget;
    public GameObject AGMTarget;*/

    void Start () {
        //TargetCursor = GameObject.FindGameObjectWithTag("RadarCursor").GetComponent<RectTransform>();
        objectidentifier = this.gameObject;
        if (!markerSprite)
        {
            Debug.LogError(" Please, specify the marker sprite.");
        }

        markerImageObject = new GameObject("Marker");
        markerImageObject.AddComponent<Image>();
        ObjectIdentifier o = markerImageObject.AddComponent<ObjectIdentifier>();
        o.objectPos = objectidentifier;;
        RWRDisplay controller = RWRDisplay.Instance;
        if (!controller)
        {
            Destroy(gameObject);
            return;
        }
        markerImageObject.transform.SetParent(controller.ThreatList.MarkerGroupRect);
        blipWidth = markerSprite.rect.width;
        blipHeight = markerSprite.rect.height;
        if(getOriginTag(objectidentifier)=="Air"){
            markerImageObject.tag = "ThreatAir"; //available values: HighThreat, Danger, Launch
        }
        else if(getOriginTag(objectidentifier)=="Ground"){
            markerImageObject.tag = "ThreatGround"; //available values: HighThreat, Danger, Launch
        }
        markerImage = markerImageObject.GetComponent<Image>();
        markerImage.sprite = markerSprite;
        markerImage.rectTransform.localPosition = Vector3.zero;
        markerImage.rectTransform.localScale = Vector3.one;
        markerImage.rectTransform.sizeDelta = new Vector2(markerSize, markerSize);
        markerImage.gameObject.SetActive(false);

    }


    void Update () {
/*        TargetCursor = radar.FCRCursor;
        CheckOverlap(TargetCursor,markerImage);*/
        RadarDisplay controller = RadarDisplay.Instance;
        if (!controller)
        {
            return;
        }
        RWRDisplay.Instance.checkIn(this);
        markerImage.rectTransform.rotation = Quaternion.identity;
        if(markerImageObject.tag == "ThreatAir"){
            markerImage.sprite = markerSprite;
        }
        else if (markerImageObject.tag == "ThreatGround"){
            markerImage.sprite = markerSprite;
        }
        else if(markerImageObject.tag == "ThreatAirHigh"){
            markerImage.sprite = HighThreatMarkerSprite;
        }
        else if (markerImageObject.tag == "ThreatGroundHigh"){
            markerImage.sprite = HighThreatMarkerSprite;
        }
    }

    void OnDestroy()
    {
        if (markerImage)
        {
            Destroy(markerImage.gameObject);
        }
    }

    public void show()
    {
        markerImage.gameObject.SetActive(true);
    }

    public void hide()
    {
        markerImage.gameObject.SetActive(false);
    }

    public bool isVisible()
    {
        return markerImage.gameObject.activeSelf;
    }

    public Vector3 getPosition()
    {
        return gameObject.transform.position;
    }

    public void setLocalPos(Vector3 pos)
    {
        markerImage.rectTransform.localPosition = pos;

    }

    public void setOpacity(float opacity)
    {
        markerImage.color = new Color(1.0f, 1.0f, 1.0f, opacity);
    }

    public string getOriginTag(GameObject origin){
        return origin.gameObject.tag;
    }
}
