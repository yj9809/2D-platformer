using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator anime;
    private Rigidbody2D rigid;

    private float speed;
    private float jumpPower;
    // Start is called before the first frame update
    void Start()
    {
        anime = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        speed = 5f;
        jumpPower = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
    }

    void Move()
    {

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);
            GetComponent<SpriteRenderer>().flipX = false;
            anime.SetBool("Run", true);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position = new Vector2(transform.position.x - speed * Time.deltaTime, transform.position.y);
            GetComponent<SpriteRenderer>().flipX = true;
            anime.SetBool("Run", true);
        }
        else
        {
            anime.SetBool("Run", false);
        }
    }
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.X))
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }    
}
