using UnityEngine;
using System.Collections;

public class damageControl : MonoBehaviour {

	public int myHP;
	public int myMaxHp = 10;

	// Use this for initialization
	void Start () {

		myHP = myMaxHp;
		Debug.Log("HP: " + myHP + " / " + myMaxHp);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
