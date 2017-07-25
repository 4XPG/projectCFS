using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bogey : MonoBehaviour {

    public Sprite markerSprite;
    public Sprite trackedMarkerSprite;
    public float markerSize = 6.5f;
    public float blipHeight;
    public float blipWidth;
    public bool isActive = true;

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
        RadarDisplay controller = RadarDisplay.Instance;
        if (!controller)
        {
            Destroy(gameObject);
            return;
        }
        markerImageObject.transform.SetParent(controller.BogeyList.MarkerGroupRect);
        blipWidth = markerSprite.rect.width;
        blipHeight = markerSprite.rect.height;
        markerImage = markerImageObject.GetComponent<Image>();
        markerImage.sprite = markerSprite;
        markerImage.rectTransform.localPosition = Vector3.zero;
        markerImage.rectTransform.localScale = Vector3.one;
        markerImage.rectTransform.sizeDelta = new Vector2(markerSize, markerSize);
        markerImage.gameObject.SetActive(false);
    }


    void Update () {
        RadarDisplay controller = RadarDisplay.Instance;
        if (!controller)
        {
            return;
        }
        RadarDisplay.Instance.checkIn(this);
        markerImage.rectTransform.rotation = Quaternion.identity;
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
}
