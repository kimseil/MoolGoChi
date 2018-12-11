using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggScript : MonoBehaviour {

	public int value;
    private float speed;
    private Rigidbody2D rigidBody2D;

	// Use this for initialization
	void Start () {
		speed = -0.5f;
        rigidBody2D = gameObject.GetComponent<Rigidbody2D>();

        rigidBody2D.velocity = new Vector2(0, speed);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate () {
        Vector3 pos = Camera.main.WorldToViewportPoint(gameObject.transform.position);

        if (pos.y < 0) Destroy(gameObject);
    }
}
