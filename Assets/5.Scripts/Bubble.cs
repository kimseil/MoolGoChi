using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour {

    private float timer;
    private float speed;

    private Rigidbody2D rigidBody2D;

	// Use this for initialization
	void Start () {
        speed = Random.Range(2f, 4);
        rigidBody2D = gameObject.GetComponent<Rigidbody2D>();

        rigidBody2D.velocity = new Vector2(0.5f, speed);
        InvokeRepeating("BubbleMove", 0.3f, 0.3f);
    }

    private void Awake()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate () {
        Vector3 pos = Camera.main.WorldToViewportPoint(gameObject.transform.position);

        if (pos.y > 1f) Destroy(gameObject);
    }

    private void BubbleMove() {
        rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x * -1, speed);
    }
}
