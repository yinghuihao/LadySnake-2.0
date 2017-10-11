using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkAttack : MonoBehaviour {
	public float timeBetweenAttacks = 0.5f;
	public float attackDamage = 20.0f;
	//public Snake player;
	public GameObject player;
	bool playerInRange;
	float timer;
	Animator animator;

	void OnTriggerStay2D(Collider2D other){
		if (other.name.StartsWith ("head")) {
			playerInRange = true;
			player.SendMessage ("TakeDamage", attackDamage*Time.deltaTime);
			// add here 
			animator.SetTrigger("monk1Attack");
			Debug.Log ("In range");
		} else if(other.name.StartsWith("body")){
			player.SendMessage ("TakeDamage", attackDamage*Time.deltaTime);
			// add here
			animator.SetTrigger("monk1Attack");
			Debug.Log (other.name);
		}
	}

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("head");
		animator = GetComponent<Animator> ();
	}
}