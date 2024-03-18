using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int direction;

    private Vector2 rayPos;
    private RaycastHit2D[] ray = new RaycastHit2D[2];
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
        Debug.Log(ray[0].collider.name);
        ray[0] = Physics2D.Raycast(rayPos, Vector2.right, 1f, LayerMask.GetMask("Wall"));
        ray[1] = Physics2D.Raycast(rayPos, Vector2.left, 1f, LayerMask.GetMask("Wall"));
    }
    void Move()
    {

        if (direction < 0)
        {
            sprite.flipX = true;
        }
        else if (direction > 0)
        {
            sprite.flipX = false;
        }

        if (ray[0])
            direction = -1;
        else if (ray[1])
            direction = 1;
        else
        

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
