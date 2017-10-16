using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

public class Snake_head : MonoBehaviour {


	// 关卡变量
	public static Snake_head instance = null;   
	public float levelStartDelay = 1.0f;
	private Text levelText;
	private GameObject levelImage;
	private int levelRange = 2;
	private static int level = 0;
	private bool doingSetup;
	private GameObject healthSetParent;
	private int wallDamage = 1;


	//By default the snake moves to right
	public float currentRotation;
	public float tailRotation;
	public List<Transform> tail = new List<Transform> ();
	public GameObject t;
	bool ate = false;
	bool rotated = false;
	public GameObject bodyPrefab;
	public GameObject tPrefab;
	public Vector2 curdir = Vector2.right;
	public Vector2 taildir = Vector2.left;
	public Image currentHealthBar;
	public Text ratioText;
	private float hitpoint = 150.0f;
	private float maxHitpoint = 150.0f;

	private Animator animator;

	private bool exit = false;
	private bool attack = false;


	// 道具相关变量
	private string[] foodEffects = new string[] { "Speed Up", "Slow Down" };
	private float[] speedVals = new float[] { 0.6f, 0.4f, 0.2f, 0.15f, 0.1f };
	private int speedIndex;
	private float speed;
	private Text statusText;
	private float statusDisplayDelay = 1.0f;



	private void UpdateHealthBar(){
		float ratio = hitpoint / maxHitpoint;
		currentHealthBar.rectTransform.localScale = new Vector3 (ratio, 1, 1);
		ratioText.text = (ratio * 100).ToString () + '%';
	}

	public void TakeDamage(float damage){
		hitpoint -= damage;
		UpdateHealthBar ();
	}

	public void Heal(float health){
		hitpoint += health;
		UpdateHealthBar ();
	}


	void InitGame(){
		// 关卡初始化
		LevelUpController();
		doingSetup = true;
		levelImage = GameObject.Find ("LevelImage");
		levelText = GameObject.Find ("LevelText").GetComponent<Text>();
		levelText.text = "Tower Floor " + level;
		levelImage.SetActive (true);

		healthSetParent = GameObject.Find ("HealthSet");
		currentHealthBar = GameObject.Find ("health_lvl").GetComponent<Image> ();
		ratioText = GameObject.Find ("Text").GetComponent<Text> ();
		statusText = GameObject.Find ("StatusText").GetComponent<Text> ();

		healthSetParent.SetActive (false);

		Invoke ("HideLevelImage", levelStartDelay);


		// 蛇属性初始化
		speedIndex = 2;
		speed = speedVals[speedIndex];

		hitpoint = 150.0f;
		maxHitpoint = 150.0f;


	
	}

	void LevelUpController() {
		level++;
		if (level > levelRange) {
			level = 1;
		}
	}

	protected void AttackWall(Vector2 pos, Vector2 dir){
		Debug.Log ("Attack");
		Vector2 end = pos;
		if (curdir == Vector2.up) {
			pos = pos + new Vector2 (0.0f, 0.6f);
			end = pos + new Vector2 (0.0f, 0.5f);
		} else if (curdir == Vector2.down) {
			pos = pos + new Vector2 (0.0f, -0.6f);
			end = pos + new Vector2 (0.0f, -0.5f);
		} else if (curdir == Vector2.left) {
			pos = pos + new Vector2 (-0.6f, 0.0f);
			end = pos + new Vector2 (-0.5f, 0.0f);
		} else if (curdir == Vector2.right) {
			pos = pos + new Vector2 (-0.6f, 0.0f);
			end = pos + new Vector2 (0.5f, 0.0f);
		}

		RaycastHit2D hit = Physics2D.Linecast (pos, end);

		if (hit.transform != null) {
			//Debug.Log ("Hit wall");
			Debug.Log(hit.transform);
			Wall hitWall = hit.transform.GetComponent <Wall> ();
			if (hitWall != null) {
				Debug.Log ("Hit wall");
				hitWall.DamageWall (wallDamage);
				//Set the attack trigger of the player's animation controller in order to play the player's attack animation.
				//animator.SetTrigger ("playerChop");
			}
		}
		attack = false;
	}

	private void HideLevelImage() {
		levelImage.SetActive (false);
		healthSetParent.SetActive (true);
		doingSetup = false;
	}

	// Use this for initialization
	void Start () {

		if (instance == null) {
			instance = this;
		}

		InitGame();

		animator = GetComponent<Animator> ();
		t = (GameObject)Instantiate (tPrefab, new Vector2(transform.position.x-0.5f, transform.position.y), Quaternion.identity);
		Invoke ("Move", speed);

		UpdateHealthBar ();
	}

	// Update is called once per frame, calculate head rotation degree per frame
	void Update () {

		if (exit || doingSetup) {
			return;
		}
		hitpoint -= 1.0f * Time.deltaTime;
		if (Input.GetKey (KeyCode.RightArrow)) {
			if (curdir == Vector2.up) {
				currentRotation += -90.0f;
				curdir = Vector2.right;
			} else if (curdir == -Vector2.up) {
				currentRotation += 90.0f;
				curdir = Vector2.right;
			} else {
				currentRotation += 0.0f;
			}
			rotated = true;
		} else if (Input.GetKey (KeyCode.DownArrow)) {
			if (curdir == -Vector2.right) {
				currentRotation += 90.0f;
				curdir = -Vector2.up;
			} else if (curdir == Vector2.right) {
				currentRotation += -90.0f;
				curdir = -Vector2.up;
			}else {
				currentRotation += 0.0f;
			}
			rotated = true;
		} else if (Input.GetKey (KeyCode.LeftArrow)) {
			if (curdir == Vector2.up) {
				currentRotation += 90.0f;
				curdir = -Vector2.right;
			} else if (curdir == -Vector2.up) {
				currentRotation += -90.0f;
				curdir = -Vector2.right;
			}else {
				currentRotation += 0.0f;
			}
			rotated = true;
		} else if (Input.GetKey (KeyCode.UpArrow)) {
			if (curdir == -Vector2.right) {
				currentRotation += -90.0f;
				curdir = Vector2.up;
			} else if (curdir == Vector2.right) {
				currentRotation += 90.0f;
				curdir = Vector2.up;
			}else {
				currentRotation += 0.0f;
			}
			rotated = true;
		}
		if (Input.GetKey (KeyCode.A)) {

			AttackWall (transform.position, curdir);
		} 
		if (hitpoint < 0) {
			hitpoint = 0.0f;
			exit = true;
			EditorUtility.DisplayDialog ("Health Exhausted", "Game Over", "OK");
			SceneManager.LoadScene( SceneManager.GetActiveScene().name );
		}else if (hitpoint > 150) {
			hitpoint = 150.0f;
		}
		UpdateHealthBar ();
	}

	//Tail rotation degree calculation
	void calculateDir(Vector2 lastpos, Vector2 curpos){
		if (curpos.x - lastpos.x == 0.5f) {//Heading Right
			if (taildir == Vector2.up) {
				tailRotation += 90.0f;
			} else if (taildir == Vector2.down) {
				tailRotation += -90.0f;
			} else {
				tailRotation += 0.0f;
			}
			taildir = Vector2.left;
		} else if (curpos.x - lastpos.x == -0.5f) {//Heading Left
			if (taildir == Vector2.up) {
				tailRotation += -90.0f;
			} else if (taildir == Vector2.down) {
				tailRotation += 90.0f;
			} else {
				tailRotation += 0.0f;
			}
			taildir = Vector2.right;
		} else if (curpos.y - lastpos.y == 0.5f) {//Heading Up
			if (taildir == Vector2.right) {
				tailRotation += -90.0f;
			} else if (taildir == Vector2.left) {
				tailRotation += 90.0f;
			} else {
				tailRotation += 0.0f;
			}
			taildir = Vector2.down;
		} else if (curpos.y - lastpos.y == -0.5f) {
			if (taildir == Vector2.right) {
				tailRotation += 90.0f;
			} else if (taildir == Vector2.left) {
				tailRotation += -90.0f;
			} else {
				tailRotation += 0.0f;
			}
			taildir = Vector2.up;
		}
	}

	void Move(){
		if (exit) {
			return;
		}
		Vector2 v = transform.position;
		if (ate) {
			GameObject g = (GameObject)Instantiate (bodyPrefab, v, Quaternion.identity);
			tail.Insert (0, g.transform);
			ate = false;
		} else if (tail.Count > 0) {
			Vector2 lastpos = tail.Last ().position;
			tail.Last ().position = v;
			tail.Insert (0, tail.Last ());
			tail.RemoveAt (tail.Count - 1);
			t.transform.position = lastpos;
			Vector2 curpos = tail.Last ().position;
			calculateDir (lastpos, curpos);
			t.transform.rotation = Quaternion.Euler(new Vector3(tPrefab.transform.rotation.x, tPrefab.transform.rotation.y, tailRotation));
		} else if (tail.Count == 0) {
			t.transform.position = v;
		}

		if (curdir == Vector2.right) {
			transform.position = new Vector3 (transform.position.x + 0.5f, transform.position.y, transform.position.z);
		} else if (curdir == Vector2.up) {
			transform.position = new Vector3 (transform.position.x, transform.position.y + 0.5f, transform.position.z);
		} else if (curdir == -Vector2.right) {
			transform.position = new Vector3 (transform.position.x - 0.5f, transform.position.y, transform.position.z);
		} else if (curdir == -Vector2.up) {
			transform.position = new Vector3 (transform.position.x, transform.position.y - 0.5f, transform.position.z);
		}

		//Rotation of snake's head and tail
		if (rotated == true) {
			transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, currentRotation));
			if (tail.Count == 0) {
				//tailRotation += currentRotation;
				taildir = curdir;
				t.transform.rotation = Quaternion.Euler(new Vector3(tPrefab.transform.rotation.x, tPrefab.transform.rotation.y, currentRotation));
				tailRotation = currentRotation;
			}
			rotated = false;
		}

		Invoke ("Move", speed);

	}

	//Collision with food
	void OnTriggerEnter2D(Collider2D coll){
		if (coll.name.StartsWith ("food")) {

			animator.SetTrigger("playerAttack");
			ate = true;
			Destroy (coll.gameObject);
			Heal (2.0f);

			// 随机产生食物效果
			int effectIndex = Random.Range (0, foodEffects.Length);
			Debug.Log ("Eat: " + effectIndex);
			switch (foodEffects[effectIndex]) {
			case "Speed Up": // 加速
				SpeedUp (foodEffects[effectIndex]);
				break;
			case "Slow Down": // 减速
				SlowDown(foodEffects[effectIndex]);
				break;
			default:
				break;
			}
		} else if (coll.name.StartsWith ("exit")) {
			exit = true;
			//			Debug.Log ("tail" + tail.Count);
			while (tail.Count > 0) {
				tail.RemoveAt (tail.Count - 1);
				//				Invoke ("removeBody", 0.3f);
				Debug.Log ("tail" + tail.Count);
			}
			EditorUtility.DisplayDialog ("Congrats", "Going to next level", "OK");

//			SceneManager.LoadScene( SceneManager.GetActiveScene().name );
			SceneManager.LoadScene (0);

		} else if(coll.name.StartsWith("monk")){
		} else {
			Debug.Log ("zhuang" + coll.name);
			EditorUtility.DisplayDialog ("Oops", "Game over", "OK");

//			SceneManager.LoadScene( SceneManager.GetActiveScene().name );
			SceneManager.LoadScene (0);
		} 
	}

	void SpeedUp(string info) {
		if (speedIndex < speedVals.Length - 1) {
			speed = speedVals [++speedIndex];
			ShowStatus (info);
		} else {
			ShowStatus ("Max Speed");
		}
	}

	void SlowDown(string info) {
		if (speedIndex > 0) {
			speed = speedVals [--speedIndex];
			ShowStatus (info);
		} else {
			ShowStatus ("Min Speed");
		}
	}

	void ShowStatus(string info) {
		statusText.text = info;
		Invoke ("HideStatus", statusDisplayDelay);
	}

	void HideStatus() {
		statusText.text = "";
		Debug.Log ("Hide Status: " + statusText.text);
	}
	public int getTailCount(){
		return tail.Count;
	}

	public int getLevel(){
		return level;
	}

}