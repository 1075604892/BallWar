using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour
{
    // Start is called before the first frame update
    public string color;
    float speed;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Square") && collision.gameObject.GetComponent<Square>().color != color && collision.gameObject.GetComponent<Square>().color != "Black")
        {
            collision.gameObject.GetComponent<Square>().ChangeColor(color);

        }
    }

    void Start()
    {
        speed = GameManager.gameManager.speed;
        gameObject.layer += GameManager.gameManager.isBall;
    }

    // Update is called once per frame
    void Update()
    {
        if(Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) < speed * 0.4 && Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y) < speed * 0.4)
        {
            
            GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);

            float random = Random.Range(0, speed);
            float x1 = random * (Random.value < 0.5f ? -1f : 1f);
            float y1 = (speed - random) * (Random.value < 0.5f ? -1f : 1f);
            GetComponent<Rigidbody2D>().AddForce(new Vector2(x1, y1), ForceMode2D.Impulse);
        }
    }
}
