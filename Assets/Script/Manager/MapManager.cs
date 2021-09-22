using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    // Start is called before the first frame update

    string type;
    Transform squares;
    DeadZone deadZone;

    [Header("Prefab")]
    public Slider slider;//滑动条
    public Slider sliderForSpee;//滑动条
    public DeadZone prefabDeadZone;

    void Start()
    {
        type = "White";
        squares = GameObject.Find("Squares").transform;
    }

    public void ChangeType(string color)
    {
        type = color;
        if (isZoning)
        {
            Destroy(deadZone.gameObject);
        }
        isZoning = false;
    }

    bool isZoning;
    public void CreateZone(string color)
    {
        isZoning = true;
        float x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        float y = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
        deadZone = Instantiate(prefabDeadZone, new Vector3(x, y, 0f), Quaternion.identity);

        deadZone.ChangeColor(color);
        deadZone.transform.SetParent(GameObject.Find("Dead Circles").transform);
    }

    float xUp;
    float yUp;
    // Update is called once per frame
    void Update()
    {
        Transform deadCircles = GameObject.Find("Dead Circles").transform;
        //出生点大小
        GameManager.gameManager.speed = sliderForSpee.value;
        for (int i = 0; i < deadCircles.childCount; i++)
        {
            deadCircles.GetChild(i).localScale = new Vector3(slider.value,slider.value,slider.value);
        }

        if (Input.GetMouseButtonDown(0) && !isZoning)
        {
            int size = GameManager.gameManager.size;
            float x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
            float y = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
            Vector2 position = new Vector2( x > (2.0 * (int)x + 1)/2.0?(int)x + 1:(int)x, y > (2.0 * (int)y + 1) / 2.0 ? (int)y + 1 : (int)y);

            if(position.x >= 0 && position.x < size && position.y >= 0 && position.y < size)
            {
                //Debug.Log(position);
                xUp = position.x;
                yUp = position.y;
                squares.GetChild((int)(position.x * size + position.y)).GetComponent<Square>().ChangeColor(type);
            }
        } else if (Input.GetMouseButtonUp(0) && !isZoning)
        {
            int size = GameManager.gameManager.size;
            float x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
            float y = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
            Vector2 position = new Vector2(x > (2.0 * (int)x + 1) / 2.0 ? (int)x + 1 : (int)x, y > (2.0 * (int)y + 1) / 2.0 ? (int)y + 1 : (int)y);

            if (position.x >= 0 && position.x < size && position.y >= 0 && position.y < size)
            {
                Debug.Log("Up:" + position + ",Down:(" + xUp + ", " + yUp + ")");
                if(position.x > xUp)
                {
                    float temp = xUp;
                    xUp = position.x;
                    position.x = temp;
                }
                if (position.y > yUp)
                {
                    float temp = yUp;
                    yUp = position.y;
                    position.y = temp;
                }

                for (float i = position.x;i <= xUp; i++)
                {
                    for (float j = position.y; j <= yUp; j++)
                    {
                        squares.GetChild((int)(i * size + j)).GetComponent<Square>().ChangeColor(type);
                    }
                }
            }
        }//设置方块
        if (isZoning)
        {
            float x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
            float y = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
            Vector2 position = new Vector2(x > (2.0 * (int)x + 1) / 2.0 ? (int)x + 1 : (int)x, y > (2.0 * (int)y + 1) / 2.0 ? (int)y + 1 : (int)y);
            deadZone.transform.position = new Vector3(position.x,position.y,deadZone.transform.position.z);
        }//移动出生区
        if(isZoning && Input.GetMouseButtonDown(0))
        {
            int size = GameManager.gameManager.size;
            float x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
            float y = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
            Vector2 position = new Vector2(x > (2.0 * (int)x + 1) / 2.0 ? (int)x + 1 : (int)x, y > (2.0 * (int)y + 1) / 2.0 ? (int)y + 1 : (int)y);
            if (position.x >= 0 && position.x < size && position.y >= 0 && position.y < size)
            {
                c = StartCoroutine(LetZoningFalse());
            }
        }//放下出生区
    }

    Coroutine c;
    IEnumerator LetZoningFalse()
    {
        yield return new WaitForSeconds(0.1f);
        isZoning = false;
        StopCoroutine(c);
    }
}
