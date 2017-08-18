/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AGMTargetPos : MonoBehaviour {

    public Sprite markerSprite;
    public Sprite trackedMarkerSprite;
    public float markerSize;
    public float blipHeight;
    public float blipWidth;
    public bool isActive = true;
    public GameObject objectidentifier;
    */
/*    public FCR radar;
        public RectTransform TargetCursor;*//*

    public Image MarkerImage
    {
        get
        {
            return markerImage;
        }
    }

    private Image markerImage;
    */
/*    private GameObject prevTarget;
        private GameObject nextTarget;
        public GameObject AGMTarget;*//*


    void Start () {
//TargetCursor = GameObject.FindGameObjectWithTag("RadarCursor").GetComponent<RectTransform>();
        objectidentifier = this.gameObject;
        if (!markerSprite)
        {
            Debug.LogError(" Please, specify the marker sprite.");
        }

        GameObject markerImageObject = new GameObject("AGMPosition");
        markerImageObject.AddComponent<Image>();
        ObjectIdentifier o = markerImageObject.AddComponent<ObjectIdentifier>();
        o.objectPos = objectidentifier;
        RadarDisplay controller = RadarDisplay.Instance;
        if (!controller)
        {
            Destroy(gameObject);
            return;
        }
        markerImageObject.transform.SetParent(controller.BogeyList.MarkerGroupRect);
        blipWidth = markerSprite.rect.width;
        blipHeight = markerSprite.rect.height;
        markerImageObject.tag = "AGMTarget	";
        markerImage = markerImageObject.GetComponent<Image>();
        markerImage.rectTransform.localPosition = Vector3.zero;
        markerImage.rectTransform.localScale = Vector3.one;
        markerImage.rectTransform.sizeDelta = new Vector2(markerSize, markerSize);
        markerImage.gameObject.SetActive(false);

    }


    void Update () {
        */
/*        TargetCursor = radar.FCRCursor;
                CheckOverlap(TargetCursor,markerImage);*//*

        RadarDisplay controller = RadarDisplay.Instance;
        if (!controller)
        {
            return;
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

    public Vector3 getPosition()
    {
        return gameObject.transform.position;
    }

    public void setLocalPos(Vector3 pos)
    {
        markerImage.rectTransform.localPosition = pos;

    }
}
*/
