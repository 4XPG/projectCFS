using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bogey : MonoBehaviour {

    public Sprite markerSprite;
    public Sprite trackedMarkerSprite;
    public float markerSize;
    public float blipHeight;
    public float blipWidth;
    public bool isActive = true;
    public BoxCollider2D blipHitbox;
    private Rigidbody2D rb;
    public FCR radar;
    public RectTransform TargetCursor;
    public Image MarkerImage
    {
        get
        {
            return markerImage;
        }
    }

    private Image markerImage;

    void Start () {
        if (!markerSprite)
        {
            Debug.LogError(" Please, specify the marker sprite.");
        }

        GameObject markerImageObject = new GameObject("Marker");
        markerImageObject.AddComponent<Image>();

        blipHitbox = markerImageObject.AddComponent<BoxCollider2D>();
        rb = markerImageObject.AddComponent<Rigidbody2D>();
        blipHitbox.size = new Vector2(blipHeight,blipWidth);
        //blipHitbox.offset = new Vector2(blipHeight / 2,blipWidth / 2);
            blipHitbox.isTrigger = true;
        RadarDisplay controller = RadarDisplay.Instance;
        if (!controller)
        {
            Destroy(gameObject);
            return;
        }
        markerImageObject.transform.SetParent(controller.BogeyList.MarkerGroupRect);
        blipWidth = markerSprite.rect.width;
        blipHeight = markerSprite.rect.height;
        markerImageObject.tag = "Enemy";
        markerImage = markerImageObject.GetComponent<Image>();
        markerImage.sprite = markerSprite;
        markerImage.rectTransform.localPosition = Vector3.zero;
        markerImage.rectTransform.localScale = Vector3.one;
        markerImage.rectTransform.sizeDelta = new Vector2(markerSize, markerSize);
        markerImage.gameObject.SetActive(false);
        ChangeRadarMode();
    }


    void Update () {
        RadarDisplay controller = RadarDisplay.Instance;
        if (!controller)
        {
            return;
        }
        this.ChangeRadarMode();
        RadarDisplay.Instance.checkIn(this);
        markerImage.rectTransform.rotation = Quaternion.Euler(0,0,360-gameObject.transform.rotation.eulerAngles.y);


    }

    void OnDestroy()
    {
        if (markerImage)
        {
            Destroy(markerImage.gameObject);
        }
    }

    void ChangeRadarMode(){
        if((radar.RadarMode == FCR.RadarModes.Air) && (markerImage.tag == "Ground")){
            hide();
        }
        else if((radar.RadarMode == FCR.RadarModes.Ground) && (markerImage.tag == "Ground")){
            show();
        }
        else if((radar.RadarMode == FCR.RadarModes.Ground) && (markerImage.tag == "Air")){
            hide();
        }
        else if((radar.RadarMode == FCR.RadarModes.Air) && (markerImage.tag == "Air")){
            show();
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

    void OnTriggerEnter2D(Collider2D co){
        if(co.gameObject.tag == "RadarCursor"){
            markerImage.sprite = trackedMarkerSprite;
            Debug.Log("target tracked");
        }
    }

    void DeselectTarget(){

    }
}
