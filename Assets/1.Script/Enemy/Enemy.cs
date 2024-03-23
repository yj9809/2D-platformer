using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private int direction;

    private Rigidbody2D rigid;
    private SpriteRenderer sprite;
    private Animator anime;
    private Player p;

    protected float speed;
    protected float hp;

    public virtual void Init()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anime = GetComponent<Animator>();

        p = GameManager.Instance.P;

        ChangeDirection();
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0)
            return;

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
        if (collision.GetComponent<AttackCollison>())
        {
            OnEnemyDamage(collision.transform.position);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.GetComponent<Player>())
        {
            GameManager.Instance.P.OnPlayerDamage(transform.position);
        }
    }
    public void OnEnemyDamage(Vector2 pos)
    {
        if (hp <= 0)
            return;
        hp -= p.damage;
        if (hp <= 0)
        {
            GetComponent<Collider2D>().enabled = false;
            GetComponent<Rigidbody2D>().simulated = false;
            anime.SetBool("Death", true);
            Destroy(gameObject, 3f);
        }
        gameObject.layer = 14;
        sprite.color = new Color(1, 1, 1, 0.4f);


        Invoke("OffDamage", 0.5f);
    }
    private void OffDamage()
    {
        gameObject.layer = 13;
        sprite.color = new Color(1, 1, 1, 1);
    }
}
