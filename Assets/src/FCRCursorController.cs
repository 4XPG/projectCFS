using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class FCRCursorController : MonoBehaviour {
	//public RectTransform TWSCursor; // | |
	//public RectTransform GMCursor; // --+--
	public RectTransform Minimap;
	public RectTransform cursorObject;
	public Camera RadarCam;
	public Camera RaytracerCam;
	private RaycastHit vision;
	private bool isLocked;
	private GameObject lockedtarget;
	private float MapWidth;
	private float MapHeight;
	private Vector2 auxVec2;
	private PointerEventData pointer;
	public EventSystem eventSystem = null;
	// Use this for initialization
	void Start () {
		isLocked = false;
		MapWidth = Minimap.rect.width;
		MapHeight = Minimap.rect.height;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 CursorPos = cursorObject.anchoredPosition;
		if (Input.GetKey ("'")) {
			CursorPos.y++;
		}
		if (Input.GetKey (",")) {
			CursorPos.x--;
		}
		if (Input.GetKey (".")) {
			CursorPos.y--;
		}
		if (Input.GetKey ("/")) {
			CursorPos.x++;
		}
		cursorObject.anchoredPosition = CursorPos;
	}

	public void RadarCursorControl(){
	

	}
	public void MinimapClick()
	{
		var miniMapRect = Minimap.rect;
		var screenRect = new Rect(
			Minimap.transform.position.x, 
			Minimap.transform.position.y, 
			miniMapRect.width, miniMapRect.height);

		var mousePos = Input.mousePosition;
		mousePos.y -= screenRect.y;
		mousePos.x -= screenRect.x;

		var camPos = new Vector3(
			mousePos.x *  (MapWidth / screenRect.width),
			mousePos.y *  (MapHeight / screenRect.height),
			Camera.main.transform.position.z);
		Camera.main.transform.position = camPos;
	}

}
