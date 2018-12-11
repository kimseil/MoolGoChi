using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameScript : MonoBehaviour {

	public GameObject feed;
    public GameObject fish1, fish2, fish3;
	public int feedLevel;
	public int feedUpgradeCost;
	public int feedCost;
	public int fishLevel;
	public int fishUpgradeCost;
	public int money;
	public int armLevel;
	public int armUpgradeCost;

	// Use this for initialization
	void Start () {
		
	}

	void Update() {
		if (Input.GetMouseButtonDown(0))
        {
            CastRay();
        }

        RefreshUI();

        GameObject[] fishs = GameObject.FindGameObjectsWithTag("Fish");
        if(fishs.Length == 0) SceneManager.LoadScene("GameOverScene");
	}

	void CastRay()
    {
        Vector2 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 vp = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        Ray2D ray = new Ray2D(wp, Vector2.zero);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null)
        {
            //배경 클릭시 먹이 생성
            if (hit.collider.gameObject.CompareTag("Background"))
            {
                if (vp.x < 1.0f && vp.x > 0.1f && vp.y < 0.9f && vp.y > 0.1f && money >= feedCost)
                {
                    DropFeed(wp);
                }
            } else if(hit.collider.gameObject.CompareTag("Bubble"))
            {
                Destroy(hit.collider.gameObject);
            } else if(hit.collider.gameObject.CompareTag("Egg"))
            {
                money += hit.collider.gameObject.GetComponent<EggScript>().value;
                Destroy(hit.collider.gameObject);
            } else if(hit.collider.gameObject.name.Equals("BuyFish1") && money >= 100)
            {
                money -= 100;
                GameObject instance = Instantiate(fish1, GameObject.Find("fish spawner").transform.position, gameObject.transform.rotation) as GameObject;
            } else if(hit.collider.gameObject.name.Equals("BuyFish2") && money >= 500)
            {
                money -= 500;
                GameObject instance = Instantiate(fish2, GameObject.Find("fish spawner").transform.position, gameObject.transform.rotation) as GameObject;
            } else if(hit.collider.gameObject.name.Equals("BuyFish3") && money >= 1010)
            {
                money -= 1000;
                GameObject instance = Instantiate(fish3, GameObject.Find("fish spawner").transform.position, gameObject.transform.rotation) as GameObject;
            } else if(hit.collider.gameObject.name.Equals("NextFeed") && money >= feedUpgradeCost && feedLevel < 11)
            {
                money -= feedUpgradeCost;
                feedLevel++;
                feedUpgradeCost += 200;
                feedCost++;
                GameObject.Find("NowFeed").transform.Find("Text").GetComponent<Text>().text = feedCost + "";
                GameObject.Find("NextFeed").transform.Find("Text").GetComponent<Text>().text = feedUpgradeCost + "";
            }
        }
    }

    void DropFeed(Vector2 wp)
    {
		money -=  feedCost;
        GameObject instance = Instantiate(feed, wp, gameObject.transform.rotation) as GameObject;
    }

    private void RefreshUI() {
        Text moneyText = GameObject.Find("normalUI").transform.Find("MoneyText").GetComponent<Text>();
        moneyText.text = "MONEY : " + money;
    }
}
