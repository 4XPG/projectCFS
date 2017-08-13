using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("MiniMap/RWR Marker Group")]
[RequireComponent(typeof(RectTransform))]
public class RWRThreatList : MonoBehaviour {

    public RectTransform MarkerGroupRect
    {
        get
        {
            if (!_rectTransform)
            {
                _rectTransform = GetComponent<RectTransform>();
            }

            return _rectTransform;
        }
    }

    private RectTransform _rectTransform;
}
