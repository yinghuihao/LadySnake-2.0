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
	private Animator animator;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		player = GameObject.Find ("head");
	}

	void OnTriggerStay2D(Collider2D other){
		if (other.name.StartsWith ("head")) {
			animator.SetTrigger ("monk1Attack");
			playerInRange = true;
			player.SendMessage ("TakeDamage", attackDamage*Time.deltaTime);
			Debug.Log ("In range");
		} else if(other.name.StartsWith("body")){
			animator.SetTrigger ("monk1Attack");
			player.SendMessage ("TakeDamage", attackDamage*Time.deltaTime);
			Debug.Log (other.name);
		}
	}
}