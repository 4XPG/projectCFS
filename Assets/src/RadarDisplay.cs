using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("MiniMap/Map canvas controller")]
[RequireComponent(typeof(RectTransform))]
public class RadarDisplay : MonoBehaviour {

    public static RadarDisplay Instance
    {
        get
        {
            if (!_instance)
            {
                RadarDisplay[] controllers = GameObject.FindObjectsOfType<RadarDisplay>();

                if (controllers.Length != 0)
                {
                    if (controllers.Length == 1)
                    {
                        _instance = controllers[0];
                    }
                    else
                    {
                        Debug.LogError("You have more than one RadarDisplay in the scene.");
                    }
                }
                else
                {
                    Debug.LogError("You should add Map prefab to your canvas");
                }


            }


            return _instance;
        }
    }

    private static RadarDisplay _instance;
    private float MapWidth;
    private float MapHeight;
    public Transform playerTransform;
    public float radarDistance = 10;
    public float maxRadarDistance = 10;

    public Text zoomText;
    public bool rotateMap = false;
    public float scale = 1.0f;

    public int RadarzoomLevelSelected = 1;
    public float[] RadarZoomLevels = new float[] { 10f, 20f, 40f, 80f }; //10, 20, 40, 80
    private int Radarzoomchange = 1;  //<<<<<<<<<<<<<
    public float minimalOpacity = 0.3f;
    public FCR radar;

    public BogeyList BogeyList
    {
        get
        {
            return markerGroup;
        }
    }

    private RectTransform mapRect;
    private BogeyList markerGroup;

    private Bogey markerAir;
    private Bogey markerGround;

    void Awake()
    {
        if (!playerTransform)
        {
            Debug.LogError("You must specify the player transform");
        }
        mapRect = GetComponent<RectTransform>();
        MapWidth = mapRect.rect.width;
        MapHeight = mapRect.rect.height;
        markerGroup = GetComponentInChildren<BogeyList>();
        if (!markerGroup)
        {
            Debug.LogError("MerkerGroup component is missing. It must be a child of InnerMap");
        }
    }

    void Update ()
    {
        if (!playerTransform)
        {
            return;
        }
        RadarZoomControl();
        zoomText.text = RadarZoomLevels[RadarzoomLevelSelected].ToString();
        mapRect.rotation = Quaternion.Euler(new Vector3(0, 0, playerTransform.eulerAngles.y));
    }

    public void RadarZoomControl(){
        if (Input.GetKeyDown(KeyCode.T) ){
            Radarzoomchange += 1;
        }
        else if (Input.GetKeyDown(KeyCode.Y) ){
            Radarzoomchange -= 1;
        }

        RadarzoomLevelSelected = Mathf.Clamp(Radarzoomchange, 0, RadarZoomLevels.Length-1);
        //Debug.Log("Zoom Level:" + RadarZoomLevels[RadarzoomLevelSelected]);
        //FCRCamera.fieldOfView = RadarZoomLevels[RadarzoomLevelSelected];
    }


    public void checkIn(Bogey marker)
    {
        if (!playerTransform)
        {
            return;
        }

        //float scaledRadarDistance = radarDistance * scale;
        //float scaledMaxRadarDistance = maxRadarDistance * scale;

        float scaledRadarDistance = radarDistance * RadarZoomLevels[RadarzoomLevelSelected];
        float scaledMaxRadarDistance = maxRadarDistance * RadarZoomLevels[RadarzoomLevelSelected];

        if (marker.isActive)
        {
            float distance = distanceToPlayer(marker.getPosition());
            float opacity = 1.0f;

            if (distance > scaledRadarDistance)
            {
                    if (marker.isVisible())
                    {
                        marker.hide();
                    }
                    return;
            }

            if (!marker.isVisible())
            {
                marker.show();
            }
            if((radar.RadarMode == FCR.RadarModes.Air) && (marker.tag == "Air")){
                marker.show();
            }
            else if((radar.RadarMode == FCR.RadarModes.Air) && (marker.tag == "Ground")){
                marker.hide();
            }
            else if((radar.RadarMode == FCR.RadarModes.Ground) && (marker.tag == "Ground")){
                marker.show();
            }
            else if((radar.RadarMode == FCR.RadarModes.Ground) && (marker.tag == "Air")){
                marker.hide();
            }

            else{

            }
            Vector3 posDif = marker.getPosition() - playerTransform.position;
            Vector3 newPos = new Vector3(posDif.x, posDif.z, 0);
            newPos.Normalize();

            //float markerRadius = (marker.markerSize / 2);
            float newLen = (distance / scaledRadarDistance) * (MapHeight - marker.blipHeight) * (MapWidth - marker.blipWidth);

            newPos *= newLen;
            marker.setLocalPos(newPos);
            //marker.setOpacity(opacity);
        }
        else
        {
            if (marker.isVisible())
            {
                marker.hide();
            }
        }
    }

    private float distanceToPlayer(Vector3 other)
    {

        return Vector2.Distance(new Vector2(playerTransform.position.x, playerTransform.position.z), new Vector2(other.x, other.z));
    }

}