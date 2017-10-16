using UnityEngine;
using System.Collections;


public class Loader : MonoBehaviour 
{
	public GameObject gameManager;          //GameManager prefab to instantiate.
	//public GameObject soundManager;         //SoundManager prefab to instantiate.

	public GameObject player;
	private Vector3 offset;

	void Awake (){
		if (GameManager.instance == null) {
			Instantiate (gameManager);
		}

		//Check if a SoundManager has already been assigned to static variable GameManager.instance or if it's still null
		/*if (SoundManager.instance == null)

			Instantiate(soundManager);*/
	}

	//move camera
	void Start () 
	{
		//Calculate and store the offset value by getting the distance between the player's position and camera's position.
		offset = transform.position - player.transform.position;
	}

	// LateUpdate is called after Update each frame
	void LateUpdate () 
	{
		// Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
		transform.position = player.transform.position + offset;
	}
}