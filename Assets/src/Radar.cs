//can also be used for RWR
 
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
 
public class Radar : MonoBehaviour
{
 
	public enum RadarLocations : int {MFDLeft, MFDRight};
	public enum RadarModes : int {NAV, TWS, GM}; // NAV/passive, Track While Search, Ground Map
 
	// Display Location
	public RadarLocations radarLocation = RadarLocations.MFDLeft;
	public RadarModes radarMode = RadarModes.NAV;
	public Vector2 radarLocationCustom;
	public Color radarBackgroundA = new Color(255, 255, 0);
	public Color radarBackgroundB = new Color(0, 255, 255);
	public Texture2D radarTexture; // default/NAV
	public Texture2D radarTextureNAV; // default/NAV
	public Texture2D radarTextureAA; // Air-to-air/TWS
	public Texture2D radarTextureAG; // Air-to-ground/GM
	public float radarSize = 0.30f;  // The amount of the screen the radar will use
	public float radarZoom = 1.00f;
 
	// Center Object information
	public bool   radarCenterActive;
	public Color  radarCenterColor = new Color(255, 255, 255);
	public string radarCenterTag;
 
	// Blip information
	public bool   radarBlip1Active; // Air objects
	public Color  radarBlip1Color = new Color(0, 0, 255);
	public string radarBlip1Tag;
 
	public bool   radarBlip2Active; // Ground objects
	public Color  radarBlip2Color = new Color(0, 255, 0);
	public string radarBlip2Tag;
 
	// Internal vars
	private GameObject _centerObject;
	private int        _radarWidth;
	private int        _radarHeight;
	//private RectTransform    _radarCenter;
	private Image radarCenterTexture;
	private Image radarblipAir;
	private Image radarblipGround;
	private Texture2D  _radarCenterTexture;
	public Texture2D  _radarBlip1Texture;
	private Texture2D  _radarBlip2Texture;

	public RectTransform radarPos;
	public RectTransform MFDLeftPos;
	public RectTransform MFDRightPos;

	// Initialize the radar
	void Start ()
	{
		// Determine the size of the radar
    	_radarWidth = (int)(radarPos.rect.width * radarSize);
    	_radarHeight = _radarWidth;
 
    	// Get the location of the radar
    	setRadarLocation();
 
		// Create the blip textures
		_radarCenterTexture = new Texture2D(16, 16, TextureFormat.RGB24, false);
		//_radarBlip1Texture = new Texture2D(16, 16, TextureFormat.RGB24, false);
		_radarBlip2Texture = new Texture2D(16, 16, TextureFormat.RGB24, false);
 
		CreateBlipTexture(_radarCenterTexture, radarCenterColor);
		CreateBlipTexture(_radarBlip1Texture, radarBlip1Color);
		CreateBlipTexture(_radarBlip2Texture, radarBlip2Color);
 
 
		// Get our center object
		GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag(radarCenterTag);
		_centerObject = gos[0];
	}
 
	// Update is called once per frame
	void OnGUI ()
	{
		GameObject[] gos;
 
		// Draw th radar background
	
		GUI.DrawTexture(radarPos.rect, radarTexture);
 
		// Draw blips
		if (radarBlip1Active)
		{
			// Find all game objects
			gos = GameObject.FindGameObjectsWithTag(radarBlip1Tag); 
 
			// Iterate through them and call drawBlip function
			foreach (GameObject go in gos)
			{
				drawBlip(go, _radarBlip1Texture);
			}
		}
		if (radarBlip2Active)
		{
			gos = GameObject.FindGameObjectsWithTag(radarBlip2Tag); 
 
			foreach (GameObject go in gos)
			{
				drawBlip(go, _radarBlip2Texture);
			}
		}
 
		// Draw center oject
		if (radarCenterActive)
		{
			Rect centerRect = new Rect(radarPos.localPosition.x - 1.5f, radarPos.localPosition.y - 1.5f, 3, 3);
			GUI.DrawTexture(centerRect, _radarCenterTexture);
		}
	}
 
	// Draw a blip for an object
	void drawBlip(GameObject go, Texture2D blipTexture)
	{
		if (_centerObject)
		{
			Vector3 centerPos = _centerObject.transform.position;
			Vector3 extPos = go.transform.position;
 
			// Get the distance to the object from the centerObject
			float dist = Vector3.Distance(centerPos, extPos);
 
			// Get the object's offset from the centerObject
			float bX = centerPos.x - extPos.x;
			float bY = centerPos.z - extPos.z;
 
			// Scale the objects position to fit within the radar
			bX = bX * radarZoom;
			bY = bY * radarZoom;
 
			// For a round radar, make sure we are within the circle
			if(dist <= (_radarWidth - 2) * 0.5 / radarZoom)
			{
				Rect clipRect = new Rect(radarPos.localPosition.x - bX - 1.5f, radarPos.localPosition.y + bY - 1.5f, 3, 3);
				GUI.DrawTexture(clipRect, blipTexture);
			}
		}
	}
 
	// Create the blip textures
	void CreateBlipTexture(Texture2D tex, Color c)
	{
        Texture2D tmpTexture = new Texture2D (tex.width, tex.height);
		Color[] cols = {c, c, c, c, c, c, c, c, c};
		Color[] pixels = tex.GetPixels(0,0,16,16);
		TextureScale.Bilinear(tex, 256,256);
        for (int y =0; y<tmpTexture.height; y++) {
        	for (int x = 0; x<tmpTexture.width; x++) {
				tmpTexture.SetPixel(x,y,tex.GetPixel (x, y));
        	}
        }
		tmpTexture.Apply();

		//GetComponent<Renderer>().material.mainTexture = tmpTexture;		
	}
 
	// Figure out where to put the radar
	void setRadarLocation()
	{
		// Sets radarCenter based on enum selection

		if(radarLocation == RadarLocations.MFDLeft)
		{
			radarPos.anchoredPosition = MFDLeftPos.anchoredPosition;
			//_radarCenter = transform.InverseTransformPoint(transform.position);
		}	
		else if(radarLocation == RadarLocations.MFDRight)
		{
			radarPos.anchoredPosition = MFDRightPos.anchoredPosition;
		}					
	} 
 	
	public void switchRadarMode(int radarMode){
		if (radarMode == 0) { // TWS mode
			//TODO: change radar background texture
			radarTexture = radarTextureAA;
			radarBlip1Active = true;
			radarBlip2Active = false;
		} else if (radarMode == 1) { // GM mode
			//TODO: change radar background texture
			radarTexture = radarTextureAG;
			radarBlip1Active = false;
			radarBlip2Active = true;
		} else { // NAV mode
			radarTexture = radarTextureNAV;
			radarBlip1Active = false;
			radarBlip2Active = false;			
		}
	}
}