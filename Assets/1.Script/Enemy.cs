using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int direction;

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
        Ground();
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

        rigid.velocity = new Vector2(direction * speed, rigid.velocity.y);
    }
    void ChangeDirection()
    {
        direction = Random.Range(-1, 2);

        Invoke("ChangeDirection", 2f);
    }

    void Ground()
    {
        //Ground Check
        Vector2 frontVec = new Vector2(rigid.position.x + (direction * 0.2f), rigid.position.y + 1);

        RaycastHit2D rayHitDown = Physics2D.Raycast(frontVec, Vector2.down, 1f, LayerMask.GetMask("Ground"));

        if (rayHitDown.collider == null)
            direction *= -1;

        //Wall Check
        Vector2 rayDircetion = Vector2.left;

        if (direction < 0)
            rayDircetion = Vector2.left;
        else if (direction > 0)
            rayDircetion = Vector2.right;

        RaycastHit2D rayHitWall = Physics2D.Raycast(frontVec, rayDircetion, 1f, LayerMask.GetMask("Wall"));
        if (rayHitWall.collider != null)
            direction *= -1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>())
        {
            GameManager.Instance.P.OnPlayerDamage(transform.position);
        }
        if (collision.GetComponent<AttackCollison>())
        {
            OnEnemyDamage(collision.transform.position);
        }
    }
    public void OnEnemyDamage(Vector2 pos)
    {
        gameObject.layer = 14;
        sprite.color = new Color(1, 1, 1, 0.4f);
        int dirc = transform.position.x - pos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1) * 1, ForceMode2D.Impulse);

        Invoke("OffDamage", 2f);
    }
    private void OffDamage()
    {
        gameObject.layer = 13;
        sprite.color = new Color(1, 1, 1, 1);
    }
}
