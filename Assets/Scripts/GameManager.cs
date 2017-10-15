using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	private BoardManager boardScript;                      
	private int level = 1;

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
		Invoke ("InitExit", 20);
		InvokeRepeating ("SpawnFoods", 3, 4);
	}

	public int getLevel() {
		return this.level;
	}

}
