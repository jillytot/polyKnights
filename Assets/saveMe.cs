using UnityEngine;
using System.Collections;

public class saveMe : MonoBehaviour {

	public Vector3 saveMePos; //Position of saveMe character
	public bool safe;
	public bool triggerSafe;
	Vector3 myPos;
	Quaternion myRot;

	public float mySpeed = 5f;

	// Use this for initialization
	void Start () {

		safe = true;
		triggerSafe = true;
	
	}
	
	// Update is called once per frame
	void Update () {


		saveMePos = this.gameObject.transform.position;

		if (triggerSafe == true) { 

			triggerSafe = false;
			StartCoroutine("triggerUnsafe");

		}

		print ("is derp happening?");
		//myPos += Vector3.forward * mySpeed * Time.deltaTime;
		transform.position += Vector3.forward * mySpeed * Time.deltaTime;
	
	}

	IEnumerator triggerUnsafe () {
	
		Debug.Log ("You won't be safe for long!!!");
		yield return new WaitForSeconds(5.0f);
		Debug.Log("Safety is off!");
		safe = false;

	}
}
