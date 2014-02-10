using UnityEngine;
using System.Collections;

public class baddie : MonoBehaviour {

	public enum enemyType { //possible basic enemy types 
		
		DEFAULT,
		SHOOTER,
		
	}

	public enemyType thisEnemyType;

	public int maxHP = 3; //enemies max total HP
	public int HP; //Enemies starting HP
	public float movementSpeed = 1;

	public GameObject enemyMat; //access the gameobject that contains the enemy material
	public Material hitMat; // this is the material used for when the enemy takes damage
	Material storeMat; //used to store the default enemy material

	public bool imHit = false; //Let's the enemy knows it's been hit so it can act accordingly

	public float flashTime; //sets the time for the material swap to last for when the enemy gets hit

	public Transform storePlayerPos; //Target players if there are no primary targets
	public GameObject saveMe; // Primary target..
	saveMe targetPrimary;

	Vector3 playerPosition;
	Vector3 targetPlayer;

	public bool walker = true; //If the enemy is a walker enemy

	public bool shooter;
	bool targetLocked;
	Vector3 myTarget;
	public float attackRate = 1.0f;
	bool reloadArrow;
	public GameObject myArrow;
	public float arrowSpeed = 10.0f;

	bool attackReady;

	// Use this for initialization
	void Start () {

		HP = maxHP; //initialize HP to start at Max HP
		storeMat = enemyMat.renderer.material; //Store the default enemy mate for later
		targetLocked = false;
		Vector3 myTarget = Vector3.zero;
		attackReady = true;
		reloadArrow = false;

		targetPrimary = saveMe.gameObject.GetComponent<saveMe>();


	
	}
	
	// Update is called once per frame
	void Update () {

		//Kill the enemy if their HP reaches 0
		if (HP <= 0) {

			Destroy(this.gameObject);

		}

		//If i've been hit, do this:
		if (imHit == true) {

			//Change the enemy material to indicate that it's been hit
			enemyMat.renderer.material = hitMat;
			StartCoroutine("flashTimer"); //resets imHIt bool after certain time

		} else {

			//Return to default material if i'm not hit;
			enemyMat.renderer.material = storeMat;

		}

	if (walker == true) {

			basicBaddieBehavior ();

		}

		if (shooter == true) {

			shooterBehavior ();

		}
	}

	//counts down by flashTime then returns false for imhit
	IEnumerator flashTimer () {
		
		yield return new WaitForSeconds (flashTime);
		imHit = false;
	}

	void basicBaddieBehavior () { 

		if (targetPrimary.safe == false) {

			playerPosition = targetPrimary.saveMePos;
			Debug.Log("Enemy moving towards Walker at: " +  playerPosition);

		} else {

			playerPosition = storePlayerPos.transform.position;

		}

		targetPlayer = new Vector3 (playerPosition.x, this.gameObject.transform.position.y, playerPosition.z);
		transform.position = Vector3.MoveTowards(transform.position, targetPlayer, movementSpeed * Time.deltaTime);

	}

	void shooterBehavior () {


		if (attackReady == true) {

			attackReady = false; 

			if (targetLocked == false) {

				myTarget = storePlayerPos.transform.position + Vector3.up;
				targetLocked = true;
			}

			if (targetLocked == true) {

				//Debug.Log("Target at: " + myTarget);
				targetLocked = false;
			}
		} 

		if (attackReady == false && reloadArrow == false) {

			//Debug.Log("Commence Firing!");
			reloadArrow = true;
			StartCoroutine("refreshAttack");
			//var newArrow = myArrow.GetComponent<arrowBehavior>();
		
			var arrow = (GameObject)Instantiate(myArrow, transform.position, Quaternion.identity);
			var arrowComponent = arrow.GetComponent<arrowBehavior>();
			arrowComponent.ShootSelf(myTarget, arrowSpeed );

		}
	}

	IEnumerator refreshAttack () {
		
		yield return new WaitForSeconds (Random.Range(attackRate * 0.9f, attackRate));
		attackReady = true;
		reloadArrow = false;
	}
}
