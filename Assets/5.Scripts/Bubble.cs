using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour {

    private float timer;
    private float speed;

	// Use this for initialization
	void Start () {
        speed = Random.Range(2f, 4);
    }

    private void Awake()
    {
        
    }

    // Update is called once per frame
    void Update () {
        gameObject.transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));

        if(timer < 0.3f)
        {
            gameObject.transform.Translate(new Vector3(0.5f * Time.deltaTime, 0, 0));
        } else
        {
            gameObject.transform.Translate(new Vector3(-0.5f * Time.deltaTime, 0, 0));
            if (timer > 0.6f)
            {
                timer = 0;
            }
        }

        timer += Time.deltaTime;

        Vector3 pos = Camera.main.WorldToViewportPoint(gameObject.transform.position);

        if (pos.y > 1f) Destroy(gameObject);
    }
}
