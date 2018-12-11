using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FishState
{
    Idle,
    Chase,
    Die
}

public class Fish : MonoBehaviour
{

    public FishState fishState;
    public GameObject egg;
    public GameObject explosion;

    //fish images
    public Sprite[] idleImage;
    public Sprite dieImage;

    //image renderer
    private SpriteRenderer spriteRenderer;

    //game object(fish)
    private GameObject fish;
    private Rigidbody2D rigidBody2D;
    private GameObject[] feeds;
    private GameObject targetFeed;

    //fish 속도와 방향
    private float speedX, speedY;
    private float fishRandomSetTimer;

    //fish status
    public int fishType;
    private float food;
    public float maxFood;
    public float consumeFood;
    private int level;
    private float exp;
    public float maxExp;

    public int eggValue;
    public float eggTimer;

    // Use this for initialization
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        fish = gameObject;
        rigidBody2D = fish.GetComponent<Rigidbody2D>();
        SetFishStatus();
        FishRandomSet();
        InvokeRepeating("FishStatus", 1, 1);
        InvokeRepeating("SpawnEgg", eggTimer, eggTimer);
    }

    void FixedUpdate()
    {
        FishMoveInScreen();
        ScanFeed();
    }

    //물고기의 속도와 방향 결정
    private void FishRandomSet()
    {
        float directionX = Random.Range(0, 2) * 2 - 1;
        float directionY = Random.Range(0, 2) * 2 - 1;
        speedX = Random.Range(0, 3.0f) / 2 * directionX;
        speedY = Random.Range(0, 3.0f) / 2 * directionY;

        FishMove();

        fishRandomSetTimer = Random.Range(1, 4) * 0.5f + 1.0f;
        Invoke("FishRandomSet", fishRandomSetTimer);
    }

    //물고기의 속도와 방향을 실제로 적용시킴
    private void FishMove()
    {
        rigidBody2D.velocity = new Vector2(speedX, speedY);
        SetImageDirection();
    }

    //물고기가 화면 안에서만 움직이게 함
    private void FishMoveInScreen()
    {
        bool isOut = false;

        //게임 화면 밖으로 나가지 않게함
        Vector3 pos = Camera.main.WorldToViewportPoint(fish.transform.position);

        if (pos.x < 0.05f)
        {
            pos.x = 0.05f;
            isOut = true;
        }

        if (pos.x > 0.95f)
        {
            pos.x = 0.95f;
            isOut = true;
        }

        if (pos.y < 0.05f)
        {
            if (fishState != FishState.Die)
            {
                pos.y = 0.05f;
                isOut = true;
            }
            else
            {
                Destroy(fish);
                CancelInvoke();
                return;
            }
        }

        if (pos.y > 0.95f)
        {
            pos.y = 0.95f;
            isOut = true;
        }

        if (isOut)
        {
            fish.transform.position = Camera.main.ViewportToWorldPoint(pos);
        }
    }

    //가장 가까운 근처의 먹이를 스캔
    private void ScanFeed()
    {
        if (fishState != FishState.Die)
        {
            float targetDistance;
            feeds = GameObject.FindGameObjectsWithTag("Feed");

            if (targetFeed != null)
            {
                targetDistance = GetSqrMagnitude(fish, targetFeed);
            }
            else
            {
                targetDistance = 32.0f;
            }

            if (feeds.Length != 0)
            {
                foreach (GameObject feed in feeds)
                {
                    FeedScript feedScript = feed.gameObject.GetComponent<FeedScript>();
                    float foodPoint = feedScript.foodPoint;
                    if (GetSqrMagnitude(fish, feed) < targetDistance && maxFood >= food + foodPoint)
                    {
                        targetFeed = feed;
                        targetDistance = GetSqrMagnitude(fish, targetFeed);
                    }
                }
            }

            if (targetDistance > 32.0f)
            {
                targetFeed = null;
            }

            if (targetFeed != null)
            {
                if (fishState != FishState.Chase)
                {
                    Chase_On();
                }
                Vector2 v = targetFeed.transform.position - fish.transform.position;
                speedX = v.normalized.x * 3;
                speedY = v.normalized.y * 3;
                FishMove();
            } 
            else 
            {
                if(fishState != FishState.Idle) 
                {
                    Idle_On();
                }
            }
        }
    }

    //fish status 초기화
    private void SetFishStatus()
    {
        food = maxFood;
        level = 1;
        exp = 0;
    }

    //매 초마다 변화하는 fish status
    private void FishStatus()
    {
        food -= consumeFood;
        if (food < 0)
        {
            Die_On();
        }

        //test
        if (level < 3)
        {
            if (exp >= maxExp)
            {
                LevelUp();
            }
        }
    }

    //fish level up
    private void LevelUp()
    {
        level++;
        exp = 0;
        spriteRenderer.sprite = idleImage[level - 1];
        if(fishType == 1) {
            eggValue += 20;
            maxExp += 200;
            maxFood += 100;
            consumeFood += 5;
        } else if(fishType == 2) {
            eggValue += 30;
            maxExp += 200;
            maxFood += 100;
            consumeFood += 5;
        } else {
            eggValue += 9;
            maxExp += 600;
            maxFood += 300;
            consumeFood += 15;
        }

        food = maxFood;
    }

    private void ConsumeFood(Collider2D feed)
    {
        FeedScript feedScript = feed.gameObject.GetComponent<FeedScript>();
        float foodPoint = feedScript.foodPoint;
        float expPoint = feedScript.expPoint;
        if(maxFood >= food + foodPoint) {
            food += foodPoint;
            exp += expPoint;
            Destroy(feed.gameObject);
        }
    }

    //죽은 상태로 변경
    private void Die_On()
    {
        fishState = FishState.Die;
        spriteRenderer.sprite = dieImage;
        CancelInvoke();
        speedX = 0;
        speedY = -2.0f;
        rigidBody2D.velocity = new Vector2(speedX, speedY);
    }

    //먹이 추적상태로 변경
    private void Chase_On()
    {
        fishState = FishState.Chase;
        CancelInvoke("FishRandomSet");
    }

    //평소 상태로 변경
    private void Idle_On()
    {
        fishState = FishState.Idle;
        FishRandomSet();
    }

    //현재 이미지의 방향 리턴
    private int GetImageDirection()
    {
        int direction;

        if (fish.transform.localScale.x > 0)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }

        return direction;
    }

    //이미지 방향과 이동 방향이 다를 경우 이미지 방향 반전
    private void SetImageDirection()
    {
        if ((GetImageDirection() < 0 && speedX > 0) || (GetImageDirection() > 0 && speedX < 0))
        {
            Vector3 scale = fish.transform.localScale;
            scale.x *= -1;

            fish.transform.localScale = scale;
        }
    }

    //두 오브젝트 거리의 제곱을 리턴
    private float GetSqrMagnitude(GameObject a, GameObject b)
    {
        return (a.transform.position - b.transform.position).sqrMagnitude;
    }

    //fish 클릭 이벤트
    void OnMouseDown()
    {
        Instantiate(explosion, gameObject.transform.position, gameObject.transform.rotation);
        Die_On();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Feed"))
        {
            ConsumeFood(other);
        }
    }

    private void SpawnEgg() {
        GameObject instance = Instantiate(egg, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
        instance.gameObject.GetComponent<EggScript>().value = eggValue;
    }

}
