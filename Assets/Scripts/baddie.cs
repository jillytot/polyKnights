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

	//public Transform storePlayerPos; //Target players if there are no primary targets

	GameObject playerObjects;
	Vector3 storePlayerPos;


	GameObject walkerObjects; //Eventually this might need to be an array of objects, but from here we can get any component we need from the Walker;
	Vector3 storeWalkerPos; //The vector3 position of the walker(s)
	bool walkerSafe; //If the walker is targetable or not. 

	Vector3 targetPosition; //current target of the enemy
	Vector3 groundTarget; //target position adjusted for ground unit

	public bool moveOnGround = true; //If the enemy is a walker enemy

	//Shooter specific behavior
	public bool shooter; //Enables this enemy to shoot arrows
	bool targetLocked; //If shooter has target
	Vector3 myTarget; //Location of target
	public float attackRate = 1.0f; // Frequency of attack rate
	bool reloadArrow; //triggers relaoding of the next round
	public GameObject myArrow; //the object which the shooter fires
	public float arrowSpeed = 10.0f; //Speed at the projectile goes at

	bool attackReady; //returns true when enemy is ready to attack

	// Use this for initialization
	void Start () {

		HP = maxHP; //initialize HP to start at Max HP
		storeMat = enemyMat.renderer.material; //Store the default enemy mate for later
		targetLocked = false;
		Vector3 myTarget = Vector3.zero;
		attackReady = true;
		reloadArrow = false;

		//targetPrimary = saveMe.gameObject.GetComponent<saveMe>();
		walkerObjects = GameObject.FindWithTag("Walker"); 
		playerObjects = GameObject.FindWithTag("Player");
		walkerSafe = walkerObjects.GetComponent<saveMe>().safe;
		Debug.Log (walkerSafe);



	
	}
	
	// Update is called once per frame
	void Update () {

		storeWalkerPos = walkerObjects.transform.position;
		storePlayerPos = playerObjects.transform.position;

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

	if (moveOnGround == true) {

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

	//Basic enemy behavior
	void basicBaddieBehavior () { 

		//check walker status to determine target, if walker is not safe, then target walker
		if (walkerSafe == false) {

			targetPosition = storeWalkerPos;
			Debug.Log("Enemy moving towards Walker at: " +  targetPosition);

		} else {

		//if (walkerSafe == true) {

			targetPosition = storePlayerPos;
			Debug.Log("Enemy moving towards Player at: " +  targetPosition);

		}

		groundTarget = new Vector3 (targetPosition.x, this.gameObject.transform.position.y, targetPosition.z);
		transform.position = Vector3.MoveTowards(transform.position, groundTarget, movementSpeed * Time.deltaTime);

	}

	void shooterBehavior () {


		if (attackReady == true) {

			attackReady = false; 

			if (targetLocked == false) {

				myTarget = storePlayerPos + Vector3.up;
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
