using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random; 

public class BoardManager : MonoBehaviour {

	public float exitx, exity;
	public GameObject food1, monk1, shield;
	public GameObject fences, flower1, flower2, flower3, keng, road1, road2, stone1, stone2, tai, tombstone, tree1, water1, water2, water3, water4, well, wood1, exit;

	private Transform boardHolder;
	private List <Vector3> usedPositions = new List<Vector3> ();
		
	void InitItem(GameObject target, float x, float y, float scalex, float scaley){
		if (target != null) {
			var temp = Instantiate (target, new Vector3 (x, y, 0f), Quaternion.identity);
			temp.transform.localScale = new Vector3 (scalex, scaley, 1.0f);
			usedPositions.Add (new Vector3 (x, y, 0f));
		}
	}

	public GameObject objMatch(string objName){
		switch (objName) {
		case "tree1":
			return tree1;
			break;
		case "stone2":
			return stone2;
			break;
		case "wood1":
			return wood1;
			break;
		case "well":
			return well;
			break;
		case "road1":
			return road1;
			break;
		case "road2":
			return road2;
			break;
		case "tombstone":
			return tombstone;
			break;
		case "flower1":
			return flower1;
			break;
		case "water1":
			return water1;
			break;
		case "water4":
			return water4;
			break;
		case "monk1":
			return monk1;
			break;
		default:
			return null;
			break;
		}
	}

	void parseData(string fileName){
		//Call InitItem to generate items
		using (StreamReader r = new StreamReader("./Assets/Level_Design/" + fileName + ".json"))
		{
			string json = r.ReadToEnd();
			JSONObject jobj = new JSONObject (json);
			for (int i = 0; i < jobj.list.Count; i++) {
				GameObject t = objMatch (jobj.keys [i]);
				//Debug.Log (t);
				if (jobj.list [i].type == JSONObject.Type.OBJECT) {
					float x = 0.0f, y = 0.0f, scalex = 0.0f, scaley = 0.0f;
					for(int j = 0; j < jobj.list [i].list.Count; j++){
						string key = (string)jobj.list [i].keys[j];
						JSONObject k = (JSONObject)jobj.list [i].list[j];
						if (key == "x") {
							x = k.n;
						} else if (key == "y") {
							y = k.n;
						} else if (key == "scaleX") {
							scalex = k.n;
						} else if (key == "scaleY") {
							scaley = k.n;
						}
					}
					InitItem (t, x, y, scalex, scaley);
				} else if (jobj.list [i].type == JSONObject.Type.ARRAY) {
					foreach(JSONObject ao in jobj.list [i].list){
						float x = 0.0f, y = 0.0f, scalex = 0.0f, scaley = 0.0f;
						for(int j = 0; j < ao.list.Count; j++){
							string key = (string)ao.keys[j];
							JSONObject k = (JSONObject)ao.list[j];
							if (key == "x") {
								x = k.n;
							} else if (key == "y") {
								y = k.n;
							} else if (key == "scaleX") {
								scalex = k.n;
							} else if (key == "scaleY") {
								scaley = k.n;
							}
						}
						InitItem (t, x, y, scalex, scaley);
					}
				}
			}
		}
	}

	void BoardSetup(){
		boardHolder = new GameObject ("Board").transform;
	}

	public void showExit(){
//		Vector3 pos = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-4.0f, 4.0f), 0f);
//		while (usedPositions.Contains (pos)) {
//			pos = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-4.0f, 4.0f), 0f);
//		}
//		Instantiate (exit, pos, Quaternion.identity);
//		usedPositions.Add (pos);
		InitItem (exit, -0.8f, -0.4f, 1, 1);
	}

	public void showDefence(){
		//InitItem (shield, -2.0f, -1.0f, 3, 3);
		Vector3 pos = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-4.0f, 4.0f), 0f);
		while (usedPositions.Contains (pos)) {
			pos = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-4.0f, 4.0f), 0f);
		}
		Instantiate (shield, pos, Quaternion.identity);
		usedPositions.Add (pos);
	}

	public void foodSpawn(){
		Vector3 pos = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-4.0f, 4.0f), 0f);
		while (usedPositions.Contains (pos)) {
			pos = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-4.0f, 4.0f), 0f);
		}
		Instantiate (food1, pos, Quaternion.identity);
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
