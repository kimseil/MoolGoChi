using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeTime : MonoBehaviour {

	public float time;
	// Use this for initialization
	void Start () {
		Invoke("Die", time);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void Die() {
		Destroy(gameObject);
	}
}
