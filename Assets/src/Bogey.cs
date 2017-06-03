using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bogey : MonoBehaviour {

    public int id;
    private Texture2D PrimaryThreatIcon;
    private Texture2D ThreatTrackingIcon;
    private Texture2D NewestThreatIcon;
    private Texture2D AirborneThreatIcon;

    public RectTransform rTransform;

    public void UpdatePos(float x, float y){
        rTransform.localPosition = new Vector3(x*5,y*5,0);
    }
}
