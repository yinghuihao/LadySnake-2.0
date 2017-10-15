using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	private BoardManager boardScript;                      
	private int level = 1;

	private int level1;
	private int count;
	private bool condition = false;
	private int  levelFlag;

	void Awake(){
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
		DontDestroyOnLoad(gameObject);
		boardScript = GetComponent<BoardManager>();
		InitGame();
	}

	void InitGame(){
		boardScript.SetupScene(level);
		//Debug.Log ("init");
	}

	void InitExit(){
		boardScript.showExit ();
	}

	void SpawnFoods(){
		boardScript.foodSpawn ();
	}
		
	void Start () {
		//Invoke ("InitExit", 20);
		InvokeRepeating ("SpawnFoods", 3, 4);

	}

	public int getLevel() {
		return this.level;
	}

	void Update () {
		level1 = GameObject.Find ("head").GetComponent<Snake_head> ().getLevel ();
		count = GameObject.Find ("head").GetComponent<Snake_head> ().getTailCount ();
		if (levelFlag != level1) {
			condition = false;
		}
		if (count == level1 && condition == false) {
			Invoke ("InitExit", 1);
			condition = true;
		}
		levelFlag = level1;//levalFlag used to check level change
	}
}
