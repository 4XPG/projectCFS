using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("MiniMap/Marker Group")]
[RequireComponent(typeof(RectTransform))]
public class BogeyList : MonoBehaviour {

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
