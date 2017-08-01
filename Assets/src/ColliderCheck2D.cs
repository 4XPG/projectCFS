using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderCheck2D : MonoBehaviour
{
    RectTransform Rect1;
    RectTransform Rect2;

    void Update(){
        Rect2 = GameObject.FindGameObjectWithTag("Enemy").gameObject.GetComponent<RectTransform>();
        if(rectOverlaps(gameObject.GetComponent<RectTransform>(),Rect2)){
            if (Input.GetKeyDown(KeyCode.Z) ) {
                Debug.Log("target tracked");
            }
        }
    }

    bool rectOverlaps(RectTransform rectTrans1, RectTransform rectTrans2)
    {
        Rect rect1 = new Rect(rectTrans1.localPosition.x, rectTrans1.localPosition.y, rectTrans1.rect.width, rectTrans1.rect.height);
        Rect rect2 = new Rect(rectTrans2.localPosition.x, rectTrans2.localPosition.y, rectTrans2.rect.width, rectTrans2.rect.height);

        return rect1.Overlaps(rect2);
    }

    void OnTriggerEnter2D(Collider2D coll){
        if(coll.gameObject.tag == "Enemy"){
            //markerImage.sprite = trackedMarkerSprite;
            Debug.Log("target tracked");
        }
    }
}