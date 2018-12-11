using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedScript : MonoBehaviour
{

    public Sprite[] feedImage;
    private SpriteRenderer spriteRenderer;

    private Rigidbody2D rigidBody2D;

	public float foodPoint;
    public float expPoint;

    private GameScript gameScript;

    // Use this for initialization
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        rigidBody2D = gameObject.GetComponent<Rigidbody2D>();

		//feed 레벨에 따라 foodPoint와 sprite에 차등을 줘야함.
        gameScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameScript>();
        int feedLevel = gameScript.feedLevel;
        foodPoint = 50f;
        expPoint = 10f;
        for(int i = 1; i < feedLevel; i++) {
            foodPoint += 10f;
            expPoint += 2f;
        }

        spriteRenderer.sprite = feedImage[feedLevel - 1];

        MoveFeed();
    }

    void FixedUpdate() {
        Vector3 pos = Camera.main.WorldToViewportPoint(gameObject.transform.position);

        if (pos.y < 0.05f)
        {
            Destroy(gameObject);
        }
    }

    void MoveFeed()
    {
        rigidBody2D.velocity = new Vector3(0, -2, 0);
    }
}
