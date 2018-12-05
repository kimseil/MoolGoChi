using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpawn : MonoBehaviour {

    public GameObject bubble;

	// Use this for initialization
	void Start () {
        SpawnBubble();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void SpawnBubble()
    {
        GameObject instance = Instantiate(bubble, gameObject.transform.position, gameObject.transform.rotation) as GameObject;

        Invoke("SpawnBubble", Random.Range(1, 3));
    }
}
