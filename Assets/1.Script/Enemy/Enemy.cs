using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type
{
    nomal,
    Boss
}
public abstract class Enemy : MonoBehaviour
{
    private Rigidbody2D rigid;
    private SpriteRenderer sprite;
    private Animator anime;
   [SerializeField] private Player p;

    protected Type type;
    protected float attackDis;
    protected float speed;
    protected float hp;

    private float attackCool = 2f;
    private int direction;

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
        attackCool -= Time.deltaTime;

        Debug.Log(attackCool);
        if (type == Type.nomal)
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
        else if(type == Type.Boss)
        {
            float distance = Vector2.Distance(p.transform.position, transform.position);
            if (distance > attackDis)
            {
                Vector2 dis = p.transform.position - transform.position;
                Vector3 dir = dis.normalized * Time.deltaTime * speed;
                sprite.flipX = dis.normalized.x > 0 ? false : true;

                transform.Translate(dir);

                anime.SetBool("Run", true);
                anime.SetBool("Attack", false);
            }
            else
            {

            }
        }
        
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
    public void OnEnemyDamage(Vector2 pos)
    {
        if (hp <= 0)
            return;

        hp -= p.attackDamage;
        gameObject.layer = 14;
        sprite.color = new Color(1, 1, 1, 0.4f);

        if (hp <= 0)
        {
            GetComponent<Collider2D>().enabled = false;
            GetComponent<Rigidbody2D>().simulated = false;
            transform.position = new Vector2(transform.position.x, transform.position.y + 0.27f);
            anime.SetBool("Death", true);
            Destroy(gameObject, 3f);
        }

        Invoke("OffDamage", 0.5f);
    }
    private void OffDamage()
    {
        gameObject.layer = 13;
        sprite.color = new Color(1, 1, 1, 1);
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
}
