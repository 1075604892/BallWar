using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameManager gameManager;

    [Header("Circle")]
    public Circle redCircle;
    public Circle blueCircle;
    public Circle yellowCircle;
    public Circle greenCircle;

    [Header("Square")]
    public Square blackSquare;
    public Square square;

    [Header("Data")]
    public int size;
    public int isBall;
    public float speed;
    public int createBallNum;
    public int deadBallNum;

    [Header("UI")]
    public List<Text> text;//百分数文本
    public List<int> percents;//被染色数量
    public GameObject menu;//主菜单
    public GameObject warningPanel;//提示框
    public GameObject mapPanel;//地图控制器
    public GameObject backButton;//返回主菜单
    public GameObject messageUI;//数据
    public Text ballButton;

    Camera cameraFollow;
    void Start()
    {
        gameManager = this;
        cameraFollow = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    public void ChanageBallLayer()
    {
        if (isBall == 0)
        {
            isBall = 5;
            ballButton.text = "球碰撞:开";
        }else
        {
            isBall = 0;
            ballButton.text = "球碰撞:关";
        }
    }

    public void CreateWall(int size) {
        Debug.Log("Create Wall");

        //生成墙壁
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                Square square1 = Instantiate(square, new Vector3(i, j, 0f), Quaternion.identity);
                square1.ChangeColor("White");
                square1.transform.SetParent(GameObject.Find("Squares").transform);
            }
        }

        //墙壁后生成边缘
        GameObject.Find("Dead Square").transform.localScale = new Vector3(3f,size + 6,1f);//左边缘
        GameObject.Find("Dead Square").transform.position = new Vector3(-2f, (size - 1f) / 2.0f,1f);

        GameObject.Find("Dead Square (1)").transform.localScale = new Vector3(3f, size + 6, 1f);//右边缘
        GameObject.Find("Dead Square (1)").transform.position = new Vector3(size + 1f, (size - 1f) / 2.0f, 1f);

        GameObject.Find("Dead Square (2)").transform.localScale = new Vector3(size + 6, 3f, 1f);//下边缘
        GameObject.Find("Dead Square (2)").transform.position = new Vector3((size - 1f) / 2.0f, -2f, 1f);

        GameObject.Find("Dead Square (3)").transform.localScale = new Vector3(size + 6, 3f, 1f);//上边缘
        GameObject.Find("Dead Square (3)").transform.position = new Vector3((size - 1f) / 2.0f, size + 1f, 1f);

        //摄像头
        GameObject.Find("Main Camera").transform.position = new Vector3((size - 1f)/2f, (size - 1f) / 2f, -10f);
        GameObject.Find("Main Camera").GetComponent<Camera>().orthographicSize = 15 + 15 * (size - 25f)/25f;

        //背景版
        GameObject.Find("Background").transform.localScale = new Vector3(size + 2f,size + 2f,size + 2f);
        GameObject.Find("Background").transform.position = new Vector3((size - 1f) / 2f, (size - 1f) / 2f, 0f);
    }

    public void CreateBall(string color,float x,float y)
    {
        Circle ball;
        switch (color) {
            case "Red":
                ball = Instantiate(redCircle, new Vector3(x, y, 0f), Quaternion.identity);
                break;
            case "Yellow":
                ball = Instantiate(yellowCircle, new Vector3(x, y, 0f), Quaternion.identity);
                break;
            case "Blue":
                ball = Instantiate(blueCircle, new Vector3(x, y, 0f), Quaternion.identity);
                break;
            case "Green":
                ball = Instantiate(greenCircle, new Vector3(x, y, 0f), Quaternion.identity);
                break;
            default:
                ball = Instantiate(redCircle, new Vector3(x, y, 0f), Quaternion.identity);
                break;
        }

        float random = Random.Range(0, speed);
        float x1 = random * (Random.value < 0.5f ? -1f : 1f);
        float y1 = (speed - random) * (Random.value < 0.5f ? -1f : 1f);
        ball.GetComponent<Rigidbody2D>().AddForce(new Vector2(x1, y1), ForceMode2D.Impulse);
        ball.transform.SetParent(GameObject.Find(color + " Circles").transform);
    }

    public void ClickNextButton()
    {
        //判断地图尺寸是否符合条件
        try {
            if (int.Parse(GameObject.Find("SizeInputField").transform.GetChild(2).GetComponent<Text>().text) <= 0)
            {
                SetWarningPanel("地图尺寸格式有问题，是不是设置成负数或者没有填写捏？");
            }
            size = int.Parse(GameObject.Find("SizeInputField").transform.GetChild(2).GetComponent<Text>().text);
            Debug.Log("size:" + size);
        } catch(System.Exception e) {
            Debug.Log("未知错误：" + e);
            SetWarningPanel("地图尺寸格式有问题，是不是设置成负数或者没有填写捏？");
        }

        menu.SetActive(false);
        CreateWall(size);
        mapPanel.SetActive(true);
    }

    public void SetWarningPanel(string content)
    {
        warningPanel.transform.GetChild(1).GetComponent<Text>().text = content;
        warningPanel.SetActive(true);
    }

    public void CloseWarningPanelP()
    {
        warningPanel.transform.GetChild(1).GetComponent<Text>().text = "只是一个提示框";
        warningPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {/*
        //控制鼠标视角
        float _mouseX = Input.GetAxis("Mouse X");
        float _mouseY = Input.GetAxis("Mouse Y");
        if (Input.GetMouseButton(2)) { 

            //相机位置的偏移量（Vector3类型，实现原理是：向量的加法）
            Vector3 moveDir = (_mouseX * -cameraFollow.transform.right + _mouseY * -cameraFollow.transform.forward);

            //限制y轴的偏移量
            //moveDir.y = 0;
            cameraFollow.transform.position += moveDir * 0.5f;
        }*/

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (cameraFollow.orthographicSize > (GameManager.gameManager.size + 10))
            {
                cameraFollow.orthographicSize += 0f;
            }
            else
            {
                cameraFollow.orthographicSize += 2f;
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (cameraFollow.orthographicSize < 5)
            {
                cameraFollow.orthographicSize += 0f;
            }
            else
            {
                cameraFollow.orthographicSize -= 2f;
            }
        }
        if (Input.anyKey)
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                cameraFollow.transform.position = new Vector3(cameraFollow.transform.position.x - 0.05f, cameraFollow.transform.position.y, -10f);
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                cameraFollow.transform.position = new Vector3(cameraFollow.transform.position.x + 0.05f, cameraFollow.transform.position.y, -10f);
            }
            else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                cameraFollow.transform.position = new Vector3(cameraFollow.transform.position.x, cameraFollow.transform.position.y + 0.05f, -10f);
            }
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                cameraFollow.transform.position = new Vector3(cameraFollow.transform.position.x, cameraFollow.transform.position.y - 0.05f, -10f);
            }else if (Input.GetKey(KeyCode.Q))
            {
                //摄像头
                GameObject.Find("Main Camera").transform.position = new Vector3((size - 1f) / 2f, (size - 1f) / 2f, -10f);
                GameObject.Find("Main Camera").GetComponent<Camera>().orthographicSize = 15 + 15 * (size - 25f) / 25f;
            }

            if (Input.GetKey(KeyCode.Space))
            {
                Time.timeScale = 10f;
            }
        }
        else {
            Time.timeScale = 1f;
        }

    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }
    public void GameStart()
    {
        try
        {
            if (int.Parse(GameObject.Find("CreateInputField").transform.GetChild(2).GetComponent<Text>().text) > 0)
            {
                createBallNum = int.Parse(GameObject.Find("CreateInputField").transform.GetChild(2).GetComponent<Text>().text);
            }
            if (int.Parse(GameObject.Find("DeadInputField").transform.GetChild(2).GetComponent<Text>().text) > 0)
            {
                deadBallNum = int.Parse(GameObject.Find("DeadInputField").transform.GetChild(2).GetComponent<Text>().text);
            }
        }
        catch (System.Exception e)
        {
            Debug.Log("大家当无事发生过咯：" + e);
        }

        messageUI.SetActive(true);
        //判断灰色
        text[0].color = Color.gray;
        text[0].transform.parent.GetComponent<Text>().color = Color.gray;
        text[1].color = Color.gray;
        text[1].transform.parent.GetComponent<Text>().color = Color.gray;
        text[2].color = Color.gray;
        text[2].transform.parent.GetComponent<Text>().color = Color.gray;
        text[3].color = Color.gray;
        text[3].transform.parent.GetComponent<Text>().color = Color.gray;
        for(int i = 0;i < GameObject.Find("Dead Circles").transform.childCount; i++)
        {
            switch(GameObject.Find("Dead Circles").transform.GetChild(i).GetComponent<DeadZone>().GetColor())
            {
                case "Red":
                    GameManager.gameManager.text[0].color = Color.gray;
                    GameManager.gameManager.text[0].transform.parent.GetComponent<Text>().color = Color.white;
                    break;
                case "Yellow":
                    GameManager.gameManager.text[2].color = Color.gray;
                    GameManager.gameManager.text[2].transform.parent.GetComponent<Text>().color = Color.white;
                    break;
                case "Blue":
                    GameManager.gameManager.text[1].color = Color.gray;
                    GameManager.gameManager.text[1].transform.parent.GetComponent<Text>().color = Color.white;
                    break;
                case "Green":
                    GameManager.gameManager.text[3].color = Color.gray;
                    GameManager.gameManager.text[3].transform.parent.GetComponent<Text>().color = Color.white;
                    break;
            }
        }

        backButton.SetActive(true);
        //mapPanel.SetActive(false);
        Destroy(mapPanel);
        c = StartCoroutine(CreateAll());
    }

    Coroutine c;
    IEnumerator CreateAll() {
        for (int i = 0; i < createBallNum; i++)
        {
            yield return new WaitForSeconds(Random.Range(0, 0.5f));
            Transform deadCircles = GameObject.Find("Dead Circles").transform;
            for (int j = 0; j < deadCircles.childCount; j++)
            {
                if (deadCircles.GetChild(j).GetComponent<DeadZone>().GetColor() != "White")
                {
                    CreateBall(deadCircles.GetChild(j).GetComponent<DeadZone>().GetColor(), deadCircles.GetChild(j).transform.position.x, deadCircles.GetChild(j).transform.position.y);
                }
            }
        }

        StopCoroutine(c);
    }
}
