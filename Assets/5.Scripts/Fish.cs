using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FishState
{
    Idle,
    Die
}

public class Fish : MonoBehaviour {

    public FishState fishState;
    
    //fish images
    public Sprite[] idleImage;
    public Sprite dieImage;

    //image renderer
    private SpriteRenderer spriteRenderer;

    //game object(fish)
    private GameObject fish;

    //fish 속도와 방향
    private float speedX, speedY;
    private int directionX, directionY;
    private float fishRandomSetTimer;

	// Use this for initialization
	void Start () {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        fish = gameObject;
        FishRandomSet();
	}
	
	// Update is called once per frame
	void Update () {
		if(fishState == FishState.Idle)
        {
            FishMove();
        }

        if (fishState == FishState.Die)
        {
            
        }

        
    }

    private void FishMove()
    {
        Vector3 moveSpeed = new Vector3(speedX * Time.deltaTime, speedY * Time.deltaTime, 0);

        fish.transform.Translate(moveSpeed);

        Vector3 pos = Camera.main.WorldToViewportPoint(fish.transform.position);

        if (pos.x < 0.05f) pos.x = 0.05f;
        if (pos.x > 0.95f) pos.x = 0.95f;
        if (pos.y < 0.05f) pos.y = 0.05f;
        if (pos.y > 0.95f) pos.y = 0.95f;

        fish.transform.position = Camera.main.ViewportToWorldPoint(pos);
    }

    //물고기의 속도와 방향 결정
    private void FishRandomSet()
    {
        directionX = Random.Range(0, 2) * 2 - 1;
        directionY = Random.Range(0, 2) * 2 - 1;
        speedX = Random.Range(0, 3.0f) / 2 * directionX;
        speedY = Random.Range(0, 3.0f) / 2 * directionY;

        if(directionX != GetImageDirection())
        {
            SetImageDirection();
        }

        fishRandomSetTimer = Random.Range(1, 3) * 0.5f + 1.0f;
        Invoke("FishRandomSet", fishRandomSetTimer);
    }

    //죽은 상태로 변경
    private void Die_On()
    {
        fishState = FishState.Die;
        spriteRenderer.sprite = dieImage;
    }

    //현재 이미지의 방향 리턴
    private int GetImageDirection()
    {
        int direction;

        if(fish.transform.localScale.x > 0)
        {
            direction = 1;
        } else
        {
            direction = -1;
        }

        return direction;
    }

    //현재 이미지의 방향 반전
    private void SetImageDirection()
    {
        Vector3 scale = fish.transform.localScale;
        scale.x *= -1;

        fish.transform.localScale = scale;
    }
}
