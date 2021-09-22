using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    // Start is called before the first frame update
    public string color;

    void Start()
    {
        //color = "White";
        //gameObject.layer = 13;
        //GetComponent<SpriteRenderer>().color = Color.white - new Color(0f,0f,0f,0.5f);
    }

    public int GetLayer()
    {
        switch (color)
        {
            case "Red":
                return 9;
            case "Yellow":
                return 12;
            case "Blue":
                return 10;
            case "Green":
                return 11;
            case "White":
                return 13;
            default:
                return 13;
        }
    }

    public void ChangeColor(string color) {
        switch (this.color)
        {
            case "Red":
                GameManager.gameManager.percents[0]--;

                break;
            case "Yellow":
                GameManager.gameManager.percents[2]--;
                break;
            case "Blue":
                GameManager.gameManager.percents[1]--;
                break;
            case "Green":
                GameManager.gameManager.percents[3]--;
                break;
        }

        this.color = color;

        switch (color) {
            case "Red":
                gameObject.layer = 9;
                GetComponent<SpriteRenderer>().color = Color.red - new Color(0f, 0f, 0f, 0.5f);
                GameManager.gameManager.percents[0]++;

                break;
            case "Yellow":
                gameObject.layer = 12;
                GetComponent<SpriteRenderer>().color = Color.yellow - new Color(0f, 0f, 0f, 0.5f);
                GameManager.gameManager.percents[2]++;
                break;
            case "Blue":
                gameObject.layer = 10;
                GetComponent<SpriteRenderer>().color = Color.blue - new Color(0f, 0f, 0f, 0.5f);
                GameManager.gameManager.percents[1]++;
                break;
            case "Green":
                gameObject.layer = 11;
                GetComponent<SpriteRenderer>().color = Color.green - new Color(0f, 0f, 0f, 0.5f);
                GameManager.gameManager.percents[3]++;
                break;
            case "White" :
                gameObject.layer = 13;
                GetComponent<SpriteRenderer>().color = Color.white - new Color(0f, 0f, 0f, 0.5f);
                break;
            case "Black":
                gameObject.layer = 8;
                GetComponent<SpriteRenderer>().color = Color.black;
                break;
        }

        for(int i = 0;i < 4; i++)
        {
            GameManager.gameManager.text[i].text = GameManager.gameManager.percents[i] * 100 / (GameManager.gameManager.size * GameManager.gameManager.size )+ "%";
        }
        //gameObject.name = color + " Square";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
