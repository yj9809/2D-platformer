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

    public bool isMove;
    public bool isJump;
    public bool isDash;
    // Start is called before the first frame update
    void Start()
    {
        anime = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        speed = 5f;
        jumpPower = 8f;
        isMove = true;
        isJump = true;
        isDash = true;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        Attack();
        Dash();
        if (!isDash)
        {
            if (!spriteRenderer.flipX)
            {
                transform.position = 
                    Vector2.MoveTowards
                    (transform.position, new Vector2(transform.position.x + 0.1f, transform.position.y), Time.deltaTime * (speed * 2));
            }
            else if (spriteRenderer.flipX)
            {
                transform.position =
                    Vector2.MoveTowards
                    (transform.position, new Vector2(transform.position.x - 0.1f, transform.position.y), Time.deltaTime * (speed * 2));
            }
        }
    }

    private void Move()
    {

        if (Input.GetKey(KeyCode.RightArrow) && isMove)
        {
            transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);
            spriteRenderer.flipX = false;
            anime.SetBool("Run", true);
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && isMove)
        {
            transform.position = new Vector2(transform.position.x - speed * Time.deltaTime, transform.position.y);
            spriteRenderer.flipX = true;
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
        if (Input.GetKeyDown(KeyCode.Z) && isDash)
        {
            anime.SetTrigger("IsDash");
            isDash = false;
            Invoke("OnDash", 2f);
        }
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            anime.SetBool("IsJump", false);
            anime.SetBool("IsFalling", false);
        }
        if (collision.gameObject.layer == 11)
        {
            OnTrapDamage(collision.transform.position);
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
        isDash = true;
    }
    void OnTrapDamage(Vector2 pos)
    {
        gameObject.layer = 10;
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        int dirc = transform.position.x - pos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1) * 3, ForceMode2D.Impulse);

        Invoke("OffDamage", 2f);
    }
    void OffDamage()
    {
        gameObject.layer = 3;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }
}
