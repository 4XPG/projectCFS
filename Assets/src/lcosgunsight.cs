using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class lcosgunsight : MonoBehaviour {
	public GameObject target;
    public GameObject self;
    public Camera MainCam;
    public RectTransform gunCrosshair;
    public float projSpeed;


    Vector3 shooterPosition;
    Vector3 targetPosition;
//velocities
    Vector3 shooterVelocity;
    Vector3 targetVelocity;

//calculate intercept
    Vector3 interceptPoint;
	// Use this for initialization
	void Start () {
        target = GameObject.FindGameObjectWithTag("SelectedTarget");
		shooterPosition = self.transform.position;
        targetPosition = target.transform.position;
        shooterVelocity = self.GetComponent<Rigidbody>() ? self.GetComponent<Rigidbody>().velocity : Vector3.zero;
        targetVelocity = target.GetComponent<Rigidbody>() ? target.GetComponent<Rigidbody>().velocity : Vector3.zero;
        interceptPoint = FirstOrderIntercept(shooterPosition, shooterVelocity, projSpeed, targetPosition, targetVelocity);
	}
	
	// Update is called once per frame
	void Update () {
        updateCrosshair();
	}

    public void updateCrosshair(){
        Vector3 screenPoint = MainCam.WorldToViewportPoint(target.transform.position);
        Vector3 screenPos = MainCam.WorldToScreenPoint (interceptPoint);
        Vector2 screenPos2D = screenPos;
        bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        if (target != null) {
            if (onScreen){
                //Debug.Log("targetpos:"+screenPos2D);
                gunCrosshair.position = screenPos2D;
            }
        }
    }


    public float time_of_impact(float px, float py, float vx, float vy, float s)
    {
        float a = s * s - (vx * vx + vy * vy);
        float b = px * vx + py * vy;
        float c = px * px + py * py;

        float d = b*b + a*c;

        float t = 0;
        if (d >= 0)
        {
            t = (b + Mathf.Sqrt(d)) / a;
            if (t < 0)
                t = 0;
        }

        return t;
    }

    public static Vector3 FirstOrderIntercept
    (
            Vector3 shooterPosition,
            Vector3 shooterVelocity,
            float shotSpeed,
            Vector3 targetPosition,
            Vector3 targetVelocity
    )  {
        Vector3 targetRelativePosition = targetPosition - shooterPosition;
        Vector3 targetRelativeVelocity = targetVelocity - shooterVelocity;
        float t = FirstOrderInterceptTime
        (
                shotSpeed,
                targetRelativePosition,
                targetRelativeVelocity
        );
        return targetPosition + t*(targetRelativeVelocity);
    }
//first-order intercept using relative target position
    public static float FirstOrderInterceptTime
    (
            float shotSpeed,
            Vector3 targetRelativePosition,
            Vector3 targetRelativeVelocity
    ) {
        float velocitySquared = targetRelativeVelocity.sqrMagnitude;
        if(velocitySquared < 0.001f)
            return 0f;

        float a = velocitySquared - shotSpeed*shotSpeed;

//handle similar velocities
        if (Mathf.Abs(a) < 0.001f)
        {
            float t = -targetRelativePosition.sqrMagnitude/
            (
            2f*Vector3.Dot
            (
                    targetRelativeVelocity,
                    targetRelativePosition
            )
            );
            return Mathf.Max(t, 0f); //don't shoot back in time
        }

        float b = 2f*Vector3.Dot(targetRelativeVelocity, targetRelativePosition);
        float c = targetRelativePosition.sqrMagnitude;
        float determinant = b*b - 4f*a*c;

        if (determinant > 0f) { //determinant > 0; two intercept paths (most common)
            float	t1 = (-b + Mathf.Sqrt(determinant))/(2f*a),
            t2 = (-b - Mathf.Sqrt(determinant))/(2f*a);
            if (t1 > 0f) {
                if (t2 > 0f)
                    return Mathf.Min(t1, t2); //both are positive
                else
                    return t1; //only t1 is positive
            } else
                return Mathf.Max(t2, 0f); //don't shoot back in time
        } else if (determinant < 0f) //determinant < 0; no intercept path
            return 0f;
        else //determinant = 0; one intercept path, pretty much never happens
            return Mathf.Max(-b/(2f*a), 0f); //don't shoot back in time
    }
}
