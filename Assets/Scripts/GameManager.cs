﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	private BoardManager boardScript;                      
	private int level = 3;

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
	}

	void InitExit(){
		boardScript.showExit ();
	}
		
	void Start () {
		Invoke ("InitExit", 20);
	}

}