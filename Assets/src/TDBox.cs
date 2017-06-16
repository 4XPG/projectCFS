using UnityEditor;
using UnityEngine;
public class TDBox : MonoBehaviour {

	public RectTransform targetCanvas;
	public Texture targetbox;
	public GameObject TargetObject;
	public Vector3 screenPos;
	public Camera MainCam;

	void Start(){
        TargetObject = GameObject.FindGameObjectWithTag("SelectedTarget");
    }
	void OnGUI(){
		applyBox();
	}
		
	public void applyBox(){

		if (TargetObject != null) {
			GUI.DrawTexture(new Rect(screenPos.x, Screen.height - screenPos.y, 50, 50), targetbox, ScaleMode.ScaleToFit, true, 0);
			screenPos = MainCam.WorldToScreenPoint (TargetObject.transform.position);
		}
	}
}