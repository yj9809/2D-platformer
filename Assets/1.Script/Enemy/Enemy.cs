using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum Type
{
    Bat,
    Boss
}
public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private Player p;
    [SerializeField] private GameObject attackCollison;
    [SerializeField] private GameObject gate;

    private GameManager gm;
    private UiManager ui;
    private Rigidbody2D rigid;
    private SpriteRenderer sprite;
    private Animator anime;

    [SerializeField] protected Type type;
    protected int damage;
    protected float attackDis;
    protected float speed;
    protected float hp;

    [SerializeField] private int combo;
    private float attackCool = 2f;
    private int direction;

    private bool isMove;
    protected bool main = false;
    protected bool middle = false;
    public virtual void Init()
    {
        gm = GameManager.Instance;
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anime = GetComponent<Animator>();
        p = GameManager.Instance.P;
        ui = UiManager.Instance;
        gate = GameObject.Find("Gate");
        isMove = false;
        ChangeDirection();
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.GameType == GameType.Stop)
            return;

        if (hp <= 0)
            return;
        Move();
        Ground();
    }
    void Move()
    {
        if (type == Type.Bat)
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
            if (distance > attackDis && isMove)
            {
                Vector2 dis = p.transform.position - transform.position;
                Vector3 dir = dis.normalized * Time.deltaTime * speed;
                sprite.flipX = dis.normalized.x > 0 ? false : true;

                if(middle)
                    transform.GetChild(0).localPosition = sprite.flipX == false ? new Vector2(1.15f, -0.35f) : new Vector2(-1.15f, -0.35f);
                else if(main)
                    transform.GetChild(0).localPosition = sprite.flipX == false ? new Vector2(1.15f, -0.12f) : new Vector2(-1.15f, -0.12f);
                transform.Translate(dir);

                anime.SetFloat("Speed", 1);
                anime.SetBool("Attack", false);
            }
            else
            {
                anime.SetFloat("Speed", 0);
                attackCool -= Time.deltaTime;
                if (attackCool < 0)
                {
                    int ran = Random.Range(0, combo);
                    int ranCool = Random.Range(1, 4);
                    isMove = false;
                    anime.SetFloat("AttackNum", ran);
                    anime.SetTrigger("Attack");
                    attackCool = ranCool;
                }
            }
        }
    }
    protected void SetBossHp()
    {
        if (type == Type.Boss)
        {
            ui.BossMaxHp = hp;
            ui.BossHp = hp;
        }
    }
    public void OnMove()
    {
        isMove = true;
    }
    private void ChangeDirection()
    {
        direction = Random.Range(-1, 2);

        Invoke("ChangeDirection", 2f);
    }
    private void Ground()
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

        hp -= p.AttackDamage;
        if (type == Type.Boss)
            ui.BossHp -= p.AttackDamage;
        gameObject.layer = 14;
        sprite.color = new Color(1, 1, 1, 0.4f);

        if (hp <= 0)
        {
            GetComponent<Collider2D>().enabled = false;
            GetComponent<Rigidbody2D>().simulated = false;
            if (type == Type.Bat)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y + 0.27f);
                GameObject item = Pooling.Instance.getItems();
                Vector2 itemPos = new Vector2(transform.position.x, transform.position.y + 1f);
                item.transform.position = itemPos;
            }

            anime.SetBool("Death", true);
            if(type == Type.Boss)
                GateOpen();
            Destroy(gameObject, 3f);
        }

        Invoke("OffDamage", 0.5f);
    }
    private void OffDamage()
    {
        gameObject.layer = 13;
        sprite.color = new Color(1, 1, 1, 1);
    }
    private void GateOpen()
    {
        gate.transform.DOMoveY(-5.5f, 2f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<AttackCollison>())
        {
            Vector2 pos = new Vector2(transform.position.x, transform.position.y + 1);

            OnEnemyDamage(collision.transform.position);

            if (type == Type.Boss)
                pos = new Vector2(transform.position.x, transform.position.y);

            GameObject pHit = Pooling.Instance.GetObj(true);
            pHit.GetComponent<ParticleSystem>().Play();
            pHit.transform.position = pos;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.GetComponent<Player>())
        {
            Vector2 pos = new Vector2(p.transform.position.x, p.transform.position.y + 1);
            GameManager.Instance.P.OnPlayerDamage(transform.position);
            if (p.SetHp > 0)
            {
                p.SetHp -= damage;
            }

            GameObject eHit = Pooling.Instance.GetObj(false);
            eHit.GetComponent<ParticleSystem>().Play();
            eHit.transform.position = pos;
            //Instantiate(GameManager.Instance.hit[0], pos, Quaternion.identity);
        }
    }
    private void OnAttackCollision()
    {
        if (type == Type.Boss)
            attackCollison.SetActive(true);
    }
    private void SpwanEffect() 
    {
        GameObject effect = Pooling.Instance.GetSpwanEffect();
        if (effect != null)
        {
            effect.transform.position = transform.position;
            effect.SetActive(true);
        }
    }
}
