using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject attackCollision;

    private Animator anime;
    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;

    private float speed;
    private float jumpPower;
    //대쉬
    private float dashSpeed;
    public float defaultTime;
    private float dashTime;
    
    public int damage;

    public bool isMove;
    public bool isJump;
    public bool isDash;
    public bool onDash;
    // Start is called before the first frame update
    void Start()
    {
        anime = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        speed = 5f;
        jumpPower = 8f;
        dashSpeed = 25f;

        damage = 2;

        isMove = true;
        isJump = true;
        onDash = true;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        Attack();
        Dash();
        //어택 스피드 조절 코드 나중에 활용
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            anime.SetFloat("AttackSpeed", anime.GetFloat("AttackSpeed") + 0.2f);
        }
    }

    private void Move()
    {

        if (Input.GetKey(KeyCode.RightArrow) && isMove)
        {
            transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);
            spriteRenderer.flipX = false;
            transform.GetChild(0).localPosition = new Vector2(1.28f, 1.26f);
            anime.SetBool("Run", true);
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && isMove)
        {
            transform.position = new Vector2(transform.position.x - speed * Time.deltaTime, transform.position.y);
            spriteRenderer.flipX = true;
            transform.GetChild(0).localPosition = new Vector2(-1.28f, 1.26f);
            anime.SetBool("Run", true);
        }
        else
        {
            anime.SetBool("Run", false);
        }
    }
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.X) && !anime.GetBool("IsJump") && isJump)
        {
            anime.SetBool("IsJump", true);
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }

        if (rigid.velocity.y < 0)
        {
            anime.SetBool("IsFalling", true);
        }
        else if (rigid.velocity.y > 0)
        {
            anime.SetBool("IsFalling", false);
        }
        else
        {
            anime.SetBool("IsFalling", false);
        }
    }    
    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.Z) && onDash && anime.GetBool("Run"))
        {
            gameObject.layer = 10;
            onDash = false;
            isDash = true;
            anime.SetTrigger("IsDash");

            Invoke("OnDash", 0.5f);
        }

        if (dashTime <=0)
        {
            speed = 5f;
            if (isDash)
                dashTime = 0.1f;
        }
        else
        {
            dashTime -= Time.deltaTime;
            speed = dashSpeed;
        }
        isDash = false;
    }
    public void OffDashDamage()
    {
        gameObject.layer = 3;
    }
    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            anime.SetTrigger("IsAttack");
            isMove = false;
            isJump = false;
        }
    }
    private void OnAttackCollision()
    {
        attackCollision.SetActive(true);
    }
    private void OnMove()
    {
        isMove = true;
        isJump = true;
    }
    private void OnDash()
    {
        onDash = true;
    }
    public void OnPlayerDamage(Vector2 pos)
    {
        gameObject.layer = 10;
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        Invoke("OffDamage", 0.5f);
    }
    private void OffDamage()
    {
        gameObject.layer = 3;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            anime.SetBool("IsJump", false);
            anime.SetBool("IsFalling", false);
        }
        if (collision.gameObject.layer == 11)
        {
            OnPlayerDamage(collision.transform.position);
        }
    }
}
