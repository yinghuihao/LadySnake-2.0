using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	private BoardManager boardScript;                      

	private static int level = 1;
	private int count;
	private bool condition = false;
	private int  levelFlag = level;

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

	void InitDefence(){
		boardScript.showDefence ();
	}

	void SpawnFoods(){
		boardScript.foodSpawn ();
	}

	//Display total 3 shielfs per i*2+4 seconds
	void DisplayShield(int sCount){
		for (int i = 0; i < sCount; i++) {
			Invoke ("InitDefence", i * 2 + 4);
		}
	}	

	void Start () {
		//Debug.Log ("defence1");
		//DisplayShield();
		InvokeRepeating ("SpawnFoods", 3, 4);
		//Debug.Log ("defence2");

	}

	void Update () {
		level = GameObject.Find ("head").GetComponent<Snake_head> ().getLevel ();
		count = GameObject.Find ("head").GetComponent<Snake_head> ().getTailCount ();
		int sCount = 4 - level;//total shield counts for every level
		if (levelFlag != level) {
			condition = false;
			InitGame ();
			DisplayShield (sCount);
		}
		if (count == level && condition == false) {
			Invoke ("InitExit", 1);
			condition = true;
		}
		levelFlag = level; // levalFlag used to check level change
	}
}
