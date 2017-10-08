using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random; 

public class BoardManager : MonoBehaviour {

	public GameObject stone;
	public GameObject big_tree;
	public GameObject small_tree;
	public GameObject m;
	public GameObject bg;
	public GameObject border_hor;
	public GameObject border_ver;
	public GameObject exit;

	private Transform boardHolder;
	private List <Vector3> usedPositions = new List<Vector3> ();
		
	void InitItem(GameObject target, float x, float y){
		Instantiate (exit, new Vector3(x, y, 0f), Quaternion.identity);
		usedPositions.Add (new Vector3 (x, y, 0f));
	}

	void parseData(string fileName){
		//Call InitItem to generate items
	}

	void BoardSetup(){
		boardHolder = new GameObject ("Board").transform;
		Instantiate (border_hor, new Vector3(0f, 4.9f, 0f), Quaternion.identity);
		Instantiate (border_hor, new Vector3(0f, -4.9f, 0f), Quaternion.identity);
		Instantiate (border_ver, new Vector3(-7.12f, 0f, 0f), Quaternion.identity);
		Instantiate (border_ver, new Vector3(7.12f, 0f, 0f), Quaternion.identity);
	}

	public void showExit(){
		Vector3 pos = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-4.0f, 4.0f), 0f);
		while (usedPositions.Contains (pos)) {
			pos = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-4.0f, 4.0f), 0f);
		}
		Instantiate (exit, pos, Quaternion.identity);
		usedPositions.Add (pos);
	}

	public void SetupScene(int level){
		//Depend on level, maybe read arguments from json files to locate items
		//To be discussed
		BoardSetup();
		string fileName = "level" + level.ToString ();
		parseData (fileName);
	}
}
