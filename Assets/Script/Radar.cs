using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Radar : MonoBehaviour {

	public GameObject player;
	public GameObject radar;

	public RectTransform map;
	public List<Enemy> mapEnemies;
	public List<GameObject> enemies;

	public bool gameEnd;

	void Start(){
		StartCoroutine (UpdateMapPos());
	}

	// Update is called once per frame
	void Update () {
		if(player.transform.position!=radar.transform.position){
			//Debug.Log("NOT SAME1");
			radar.transform.position = new Vector3 (player.transform.position.x,radar.transform.position.y,player.transform.position.z);
		}
	}

	void OnTriggerEnter(Collider collider){
		Debug.Log("Collide");
		if(collider.tag == "Enemy"){
			BoxCollider colliders = collider.GetComponent<BoxCollider>();
			if(enemies.Contains(collider.gameObject)){
			}else{
				collider.tag = "LockEnemy";
				Debug.Log(collider.tag);
				Enemy tempEnemy = Instantiate(mapEnemies[0].gameObject).GetComponent<Enemy>();
				mapEnemies.Add(tempEnemy);
				enemies.Add(collider.gameObject);
				tempEnemy.rTransform.SetParent(map);
				tempEnemy.rTransform.localRotation = Quaternion.Euler(Vector3.zero);
				tempEnemy.rTransform.localScale = new Vector3(1,1,1);
				tempEnemy.rTransform.anchoredPosition = Vector3.zero;
				tempEnemy.gameObject.SetActive(true);
				//colliders.enabled = false;
			}
		}
	}

	void OnTriggerExit(Collider collider){
		if(collider.tag == "LockEnemy"){
			//BoxCollider colliders = collider.GetComponent<BoxCollider>();
			if(enemies.Contains(collider.gameObject)){
				Debug.Log("ReleaseEnemy");
				Enemy tempRemoval = mapEnemies[enemies.IndexOf(collider.gameObject)+1];
				mapEnemies.Remove(tempRemoval);
				Destroy(tempRemoval.gameObject);
				collider.tag = "Enemy";
				enemies.Remove(collider.gameObject);
			}else{

			}
		}
	}

	IEnumerator UpdateMapPos(){
		while (!gameEnd) {
			if(enemies.Count>0){
				for (int i=0; i<enemies.Count; i++) {
					mapEnemies [i + 1].UpdatePos (enemies [i].transform.position.x - player.transform.position.x , enemies [i].transform.position.z- player.transform.position.z);
				}
			}
			yield return new WaitForSeconds(0.2f);
		}
		yield return 0;
	}
}
