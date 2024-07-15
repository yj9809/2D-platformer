using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // 공격용 콜리더
    [SerializeField] private GameObject attackCollision;
    // 참조
    private Animator anime;
    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private UiManager ui;
    private DataManager dm;
    private PlayerData data;
    // 포션 관련 Num
    private int potionsNum = 2; 
    // 이동 관련 수치
    private float speed;
    private float jumpPower;
    private float dashSpeed;
    public float defaultTime;
    private float dashTime;
    //이동 관련 참/거짓
    public bool isMove;
    public bool isJump;
    public bool isDash;
    public bool onDash;
    //프로퍼티
    public float SetHp
    {
        get { return data.hp; }
        set
        {
            data.hp = value;
            UiManager.Instance.SetHpImg();
        }
    }
    public int AttackDamage
    {
        get { return data.attackDamage; }
        set
        {
            data.attackDamage = value;
        }
    }
    public float AttackSpeed
    {
        get { return data.attackSpeed; }
        set
        {
            data.attackSpeed = value;
            anime.SetFloat("AttackSpeed", AttackSpeed);
        }
    }
    public float Speed
    {
        get { return data.speed; }
        set
        {
            data.speed = value;
        }
    }
    public float MaxHP
    {
        get { return data.maxHp; }
        set
        {
            data.maxHp = value;
        }
    }
    public int Coin
    {
        get { return data.coin; }
        set
        {
            data.coin = value;
        }
    }
    public int Potions
    {
        get
        {
            return data.potions;
        }
        set
        {
            data.potions = value;
        }
    }
    public Vector2 LastPos
    {
        get
        {
            return data.lastPos;
        }
        set
        {
            data.lastPos = value;
        }
    }
    private void Awake()
    {
        anime = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        data = DataManager.Instance.nowPlayer;
        ui = UiManager.Instance;
        dm = DataManager.Instance;
    }
    // Start is called before the first frame update
    void Start()
    {
        ui.SetHpImg();
        anime.SetFloat("AttackSpeed", AttackSpeed);
        speed = data.speed;

        jumpPower = 8f;
        dashSpeed = 25f;


        isMove = true;
        isJump = true;
        onDash = true;
    }

    // Update is called once per frame
    void Update()
    {
        //수치 제한
        HpClamp();
        //이동
        Move();
        Jump();
        Dash();
        //공격
        Attack();
        //포션
        OnPotions();
    }
    //Ui 제한
    private void HpClamp()
    {
        if (SetHp > MaxHP)
        {
            SetHp = MaxHP;
        }
        else if (SetHp <= 0)
        {
            SetHp = 0;
        }
    }
    //이동 관련
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
    private void OnMove()
    {
        isMove = true;
        isJump = true;
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
    private void OnDash()
    {
        onDash = true;
    }
    public void OffDashDamage()
    {
        gameObject.layer = 3;
    }
    //공격 관련 & 데미지 함수
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
    public void OnPlayerDamage(Vector2 pos)
    {
        gameObject.layer = 10;
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        int dirc = transform.position.x - pos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1) * 2,ForceMode2D.Impulse);
        isMove = false;
        Invoke("OffDamage", 0.5f);
    }
    private void OffDamage()
    {
        gameObject.layer = 3;
        isMove = true;
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
    //포션
    public void OnPotions()
    {
        if (Input.GetKeyDown(KeyCode.R) && Potions > 0 && SetHp != MaxHP)
        {
            SetHp += 2;
            Potions--;
            potionsNum = potionsNum <= 0 ? 1 : potionsNum - 1;
            ui.TransPotionsImg(potionsNum);
        }
    }
    //저장
    public void Save()
    {
        LastPos = new Vector2(transform.position.x, transform.position.y);
        dm.SaveData();
    }
}
