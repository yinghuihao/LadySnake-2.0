using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkChaseing : MonoBehaviour {
	public float timeBetweenAttacks = 0.5f;
	public float attackDamage = 20.0f;
	//public Snake player;
	public GameObject player;
	public Transform target;
	public float speed;
	bool playerInRange;
	float timer;
	Animator animator;
	public float chaseRange;

	void OnTriggerStay2D(Collider2D other){
		if (other.name.StartsWith ("head")) {
			playerInRange = true;
			player.SendMessage ("TakeDamage", attackDamage*Time.deltaTime);
			// add here 
			animator.SetTrigger("monk1Attack");
			//			Debug.Log ("In range");
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
	
	// Update is called once per frame
	void Update () {
		//chase the player AI
		float distanceToTarget = Vector3.Distance(transform.position,target.position);
		if (distanceToTarget < chaseRange) {
			//start chasing the player - turn and move towards target;
			Vector3 targetDir = target.position  - transform.position;
			float degree = Mathf.Atan2(targetDir.y,targetDir.x)*Mathf.Rad2Deg-90f;
			Quaternion newq = Quaternion.AngleAxis(degree,Vector3.forward);
			transform.rotation = Quaternion.RotateTowards(transform.rotation,newq,180);
			transform.Translate(Vector3.up*Time.deltaTime*speed);
		}
	}
}
