using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int direction;
    [SerializeField] private RaycastHit2D hit;

    private Vector2 rayPos;
    private Rigidbody2D rigid;
    private SpriteRenderer sprite;

    private float speed;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        speed = 3f;
        ChangeDirection();

    }

    // Update is called once per frame
    void Update()
    {
        Move();

        rayPos = new Vector2(transform.position.x, transform.position.y + 1);


        if (hit)
        {
            direction = -1;
            Debug.Log("Ãæµ¹");
        }
    }
    void Move()
    {
        if (direction < 0)
        {
            sprite.flipX = true;
            hit = Physics2D.Raycast(rayPos, Vector2.right, 0.5f, LayerMask.GetMask("Wall"));
        }
        else if (direction > 0)
        {
            sprite.flipX = false;
            hit = Physics2D.Raycast(rayPos, Vector2.left, 0.5f, LayerMask.GetMask("Wall"));
        }

        rigid.velocity = new Vector2(direction, rigid.velocity.y) * speed;
    }
    void ChangeDirection()
    {
        direction = Random.Range(-1, 2);

        Invoke("ChangeDirection", 2f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>())
        {
            GameManager.Instance.P.OnPlayerDamage(transform.position);
        }
    }
}
