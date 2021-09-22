using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeadZone : MonoBehaviour
{
    string color;
    bool trigger;

    private void Start()
    {
        trigger = true;
    }

    public void ChangeColor(string color)
    {
        this.color = color;
        switch (color)
        {
            case "Red":
                GetComponent<SpriteRenderer>().color = Color.red - new Color(0f, 0f, 0f, 0.5f);
                GameManager.gameManager.percents[0]++;

                break;
            case "Yellow":
                GetComponent<SpriteRenderer>().color = Color.yellow - new Color(0f, 0f, 0f, 0.5f);
                GameManager.gameManager.percents[2]++;
                break;
            case "Blue":
                GetComponent<SpriteRenderer>().color = Color.blue - new Color(0f, 0f, 0f, 0.5f);
                GameManager.gameManager.percents[1]++;
                break;
            case "Green":
                GetComponent<SpriteRenderer>().color = Color.green - new Color(0f, 0f, 0f, 0.5f);
                GameManager.gameManager.percents[3]++;
                break;
            case "White":
                GetComponent<SpriteRenderer>().color = Color.white - new Color(0f, 0f, 0f, 0.5f);
                break;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.CompareTag(color) && trigger)
        {
            switch (this.color)
            {
                case "Red":
                    GameManager.gameManager.text[0].color = Color.gray;
                    GameManager.gameManager.text[0].transform.parent.GetComponent<Text>().color = Color.gray;
                    break;
                case "Yellow":
                    GameManager.gameManager.text[2].color = Color.gray;
                    GameManager.gameManager.text[2].transform.parent.GetComponent<Text>().color = Color.gray;
                    break;
                case "Blue":
                    GameManager.gameManager.text[1].color = Color.gray;
                    GameManager.gameManager.text[1].transform.parent.GetComponent<Text>().color = Color.gray;
                    break;
                case "Green":
                    GameManager.gameManager.text[3].color = Color.gray;
                    GameManager.gameManager.text[3].transform.parent.GetComponent<Text>().color = Color.gray;
                    break;
            }

            if (color != "White")
            {
                Destroy(GameObject.Find(color + " Circles"));
            }
            Debug.Log(color + " dead");
            trigger = false;
            c = StartCoroutine(CreateDead(collision.tag));
        }
    }

    public string GetColor()
    {
        return color;
    }

    Coroutine c;
    IEnumerator CreateDead(string color) {
        for (int i = 0; i < GameManager.gameManager.deadBallNum; i++)
        {
            yield return new WaitForSeconds(Random.Range(0, 0.5f));
            GameManager.gameManager.CreateBall(color, transform.position.x, transform.position.y);
        }

        StopCoroutine(c);
        Destroy(this.gameObject);
    }
}
