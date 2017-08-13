using UnityEditor;
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
    public GameObject playerTransform;
    public float radarDistance = 10;
    public float maxRadarDistance = 10;
    public bool rotateMap = false;
    public float scale = 1.0f;
    public float minimalOpacity = 0.3f;

    public RWRThreatList ThreatList
    {
        get
        {
            return markerGroup;
        }
    }

    private RectTransform mapRect;
    private RWRThreatList markerGroup;

    private GameObject[] threatAir;
    private GameObject[] threatGround;
    private RWRThreat highestThreat;
    private RWRThreat[] threatList;
    private GameObject[] TargetsonMap;
    void Awake()
    {
        if (!playerTransform)
        {
            Debug.LogError("You must specify the player transform");
        }
        mapRect = GetComponent<RectTransform>();
        MapWidth = mapRect.rect.width;
        MapHeight = mapRect.rect.height;
        markerGroup = GetComponentInChildren<RWRThreatList>();
        threatList = FindObjectsOfType<RWRThreat>();
        TargetsonMap = new GameObject[threatList.Length];
        for(int i=0;i<TargetsonMap.Length;i++){
            TargetsonMap[i] = threatList[i].gameObject;
        }
        if (!markerGroup)
        {
            Debug.LogError("MerkerGroup component is missing. It must be a child of InnerMap");
        }
    }

    void Update ()
    {
        threatAir = GameObject.FindGameObjectsWithTag("ThreatAir");
        threatGround = GameObject.FindGameObjectsWithTag("ThreatGround");
        if (!playerTransform)
        {
            return;
        }
        GameObject nearestThreat = GetClosestEnemy(playerTransform,TargetsonMap);
        if(nearestThreat.tag == "Air" && nearestThreat.GetComponent<RWRThreat>().hasRadar){
            GameObject high = nearestThreat.GetComponent<RWRThreat>().markerImageObject;
            high.tag = "ThreatAirHigh";
        }
        else if(nearestThreat.tag == "Ground" && nearestThreat.GetComponent<RWRThreat>().hasRadar){
            GameObject high = nearestThreat.GetComponent<RWRThreat>().markerImageObject;
            high.tag = "ThreatGroundHigh";
        }
        //mapRect.rotation = Quaternion.Euler(new Vector3(0, 0, playerTransform.eulerAngles.y));
    }


    public GameObject GetClosestEnemy (GameObject player, GameObject[] enemies)
    {
        GameObject bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = player.transform.position;
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


    public void checkIn(RWRThreat marker)
    {
        if (!playerTransform)
        {
            return;
        }
        float scaledRadarDistance = radarDistance * scale;
        float scaledMaxRadarDistance = maxRadarDistance * scale;

        if (marker.hasRadar)
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
            Vector3 posDif = marker.getPosition() - playerTransform.transform.position;
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

        return Vector2.Distance(new Vector2(playerTransform.transform.position.x, playerTransform.transform.position.z), new Vector2(other.x, other.z));
    }

}