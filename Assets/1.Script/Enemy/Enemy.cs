using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public enum Type
{
    Bat,
    Frog,
    Boss
}
public abstract class Enemy : MonoBehaviour
{
    [BoxGroup("Enemy Reference")] [SerializeField] private Player p;
    [BoxGroup("Enemy Reference")] [SerializeField] private GameObject attackCollison;
    [BoxGroup("Enemy Reference")] [SerializeField] private GameObject gate;
    [BoxGroup("Enemy Reference")] [SerializeField] private GameObject item;
    [BoxGroup("Enemy Reference")] [SerializeField] private int combo;

    private GameManager gm;
    private UiManager ui;
    private Pooling pool;
    private Rigidbody2D rigid;
    private SpriteRenderer sprite;
    private Animator anime;

    [Title("Enemy Type")]
    [EnumToggleButtons]
    [SerializeField] protected Type type;

    protected int damage;
    protected float attackDis;
    protected float speed;
    protected float hp;

    private float attackCool = 2f;
    private int direction;

    private bool isMove;
    protected bool main = false;
    protected bool middle = false;
    protected bool mossyMiddle = false;
    protected bool mossyMain = false;
    protected virtual void Init()
    {
        gm = GameManager.Instance;
        ui = UiManager.Instance;
        pool = Pooling.Instance;
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anime = GetComponent<Animator>();
        p = GameManager.Instance.P;
        gate = GameObject.Find("Gate");
        isMove = false;
        ChangeDirection();
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.GameType == GameType.Stop)
        {
            rigid.velocity = new Vector2(0, 0);
            return;
        }

        if (hp <= 0)
            return;

        if (CameraCheck())
            sprite.enabled = true;
        else
            sprite.enabled = false;

        Move();
        Ground();
    }
    private void Move()
    {
        if (type == Type.Bat)
        {
            sprite.flipX = direction < 0 ? true : false;
            rigid.velocity = new Vector2(direction * speed, rigid.velocity.y);
        }
        else if (type == Type.Frog)
        {
            sprite.flipX = direction < 0 ? true : false;
            rigid.velocity = new Vector2(direction * speed, rigid.velocity.y);

            if (rigid.velocity.x == 0)
                anime.SetBool("Run", false);
            else
                anime.SetBool("Run", true);
        }
        else if (type == Type.Boss)
        {
            float distance = Vector2.Distance(p.transform.position, transform.position);
            Vector2 dis = p.transform.position - transform.position;
            if (distance > attackDis && isMove)
            {
                Vector3 dir = dis.normalized * Time.deltaTime * speed;

                BossSpriteFilp(dis);

                transform.Translate(dir);

                anime.SetFloat("Speed", 1);
                anime.SetBool("Attack", false);
            }
            else
            {
                BossSpriteFilp(dis);
                anime.SetFloat("Speed", 0);
                attackCool -= Time.deltaTime;
                if (attackCool < 0)
                {
                    if(!mossyMiddle)
                    {
                        int ran = Random.Range(0, combo);
                        anime.SetFloat("AttackNum", ran);
                        anime.SetTrigger("Attack");
                    }
                    else
                    {
                        anime.SetBool("Attack", true);
                    }
                    isMove = false;
                    int ranCool = Random.Range(1, 4);
                    attackCool = ranCool;
                }
            }
        }
    }
    private void BossSpriteFilp(Vector2 dis)
    {
        sprite.flipX = dis.normalized.x > 0 ? false : true;

        if (middle)
            transform.GetChild(0).localPosition = sprite.flipX == false ? new Vector2(1.15f, -0.35f) : new Vector2(-1.15f, -0.35f);
        else if (main)
            transform.GetChild(0).localPosition = sprite.flipX == false ? new Vector2(1.15f, -0.12f) : new Vector2(-1.15f, -0.12f);
        else if (mossyMain)
            transform.GetChild(0).localPosition = sprite.flipX == false ? new Vector2(2.5f, -0.12f) : new Vector2(-2.5f, -0.12f);
    }
    private void GetMagic()
    {
        bool mainBoss = mossyMain ? true : false;
        GameObject magic = pool.GetMagic(mainBoss);
        magic.transform.position = p.transform.position;
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
        Vector2 frontVec = new Vector2(rigid.position.x + (direction * 0.2f), rigid.position.y);

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
    public void OnEnemyDamage()
    {
        if (hp <= 0)
            return;

        hp -= p.AttackDamage;

        if (type == Type.Boss && !anime.GetBool("Attack"))
        { 
            ui.BossHp -= p.AttackDamage;
            anime.SetTrigger("Hit");
        }
        gameObject.layer = 14;
        sprite.color = new Color(1, 1, 1, 0.4f);

        if (hp <= 0)
        {
            if (type == Type.Bat)
            {
                rigid.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
                rigid.gravityScale = 10;
            }
            gameObject.layer = 14;

            if (type != Type.Boss)
            {
                GameObject item = pool.GetItems();
                item.transform.position = transform.position;
            }
            else
            {
                GameObject item = Instantiate(this.item);
                item.transform.position = transform.position;
            }

            anime.SetBool("Death", true);
            if (type == Type.Boss)
                GateOpen();
            Destroy(gameObject, 3f);
            return;
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
    private void OnAttackCollision()
    {
        if (type == Type.Boss)
            attackCollison.SetActive(true);
    }
    private void SpwanEffect()
    {
        GameObject effect = pool.GetSpwanEffect();
        if (effect != null)
        {
            effect.transform.position = transform.position;
            effect.SetActive(true);
        }
    }
    private bool CameraCheck()
    {
        Vector3 viewport = Camera.main.WorldToViewportPoint(transform.position);

        return (viewport.x >= 0 && viewport.x <= 1 &&
            viewport.y >= 0 && viewport.y <= 1 &&
            viewport.z > 0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<AttackCollison>())
        {
            OnEnemyDamage();

            GameObject pHit = pool.GetObj(true);
            pHit.GetComponent<ParticleSystem>().Play();
            pHit.transform.position = transform.position;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.GetComponent<Player>())
        {
            if (p.SetHp > 0)
            {
                gm.P.OnPlayerDamage(transform.position, damage);
            }

            GameObject eHit = pool.GetObj(false);
            eHit.GetComponent<ParticleSystem>().Play();
            eHit.transform.position = p.transform.position;
        }
    }
}