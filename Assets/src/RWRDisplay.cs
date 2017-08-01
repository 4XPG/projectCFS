using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("MiniMap/RWR controller")]
[RequireComponent(typeof(RectTransform))]
public class RWRDisplay : MonoBehaviour {

    public static RWRDisplay Instance
    {
        get
        {
            if (!_instance)
            {
                RWRDisplay[] controllers = GameObject.FindObjectsOfType<RWRDisplay>();

                if (controllers.Length != 0)
                {
                    if (controllers.Length == 1)
                    {
                        _instance = controllers[0];
                    }
                    else
                    {
                        Debug.LogError("You have more than one RWRDisplay in the scene.");
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

    private static RWRDisplay _instance;
    private float MapWidth;
    private float MapHeight;
    public Transform playerTransform;
    public float radarDistance = 10;
    public float maxRadarDistance = 10;

    public bool rotateMap = false;
    public float scale = 1.0f;
    public float minimalOpacity = 0.3f;

    public BogeyList ThreatList
    {
        get
        {
            return markerGroup;
        }
    }

    private RectTransform mapRect;
    private BogeyList markerGroup;

    private Bogey threatAir;
    private Bogey threatGround;

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
        //mapRect.rotation = Quaternion.Euler(new Vector3(0, 0, playerTransform.eulerAngles.y));
    }




    public void checkIn(Bogey marker)
    {
        if (!playerTransform)
        {
            return;
        }
        float scaledRadarDistance = radarDistance * scale;
        float scaledMaxRadarDistance = maxRadarDistance * scale;

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