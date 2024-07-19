using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using DG.Tweening;

public class Player : MonoBehaviour
{
    // 공격용 콜리더
    [SerializeField] private GameObject attackCollision;
    // 참조
    private GameManager gm;
    private Animator anime;
    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private UiManager ui;
    private DataManager dm;
    private PlayerData data;
    private PixelPerfectCamera pixelCamera;
    // 포션 관련 Num
    private int potionsNum = 2; 
    // 이동 관련 수치
    private float jumpPower;
    private float dashSpeed;
    public float defaultTime;
    private float dashTime;
    private float transTime;
    private float mpTime;
    //이동 관련 참/거짓
    public bool isMove;
    public bool isJump;
    public bool isDash;
    public bool onDash;
    public bool doubleJump;
    public bool trans;
    public bool OnBossRoomMove = false;
    //프로퍼티
    public float SetHp
    {
        get { return data.hp; }
        set
        {
            data.hp = value;
            ui.SetHpImg();
        }
    }
    public float SetMp
    {
        get { return data.mp; }
        set
        {
            data.mp = value;
            ui.SetMpImg();
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
    public float MaxMp
    {
        get { return data.maxMp; }
        set
        {
            data.maxMp = value;
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
    public int Level
    {
        get { return data.level; }
        set
        {
            data.level += value;
        }
    }
    public int Coin
    {
        get { return data.coin; }
        set
        {
            data.coin = value;
            ui.SetCoin();
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
        gm = GameManager.Instance;
        data = DataManager.Instance.nowPlayer;
        ui = UiManager.Instance;
        dm = DataManager.Instance;
    }
    // Start is called before the first frame update
    void Start()
    {
        pixelCamera = gm.MainCamera.GetComponent<PixelPerfectCamera>();
        if (data.newGame)
        {
            pixelCamera.assetsPPU = 36;
            gm.MainCamera.blind[0].rectTransform.anchoredPosition = Vector2.zero;
            gm.MainCamera.blind[1].rectTransform.anchoredPosition = Vector2.zero;
            anime.SetTrigger("Start");
        }

        ui.SetHpImg();
        ui.SetMpImg();
        ui.SetCoin();
        anime.SetFloat("AttackSpeed", AttackSpeed);

        jumpPower = 8f;
        dashSpeed = 25f;


        isMove = true;
        isJump = true;
        onDash = true;
        trans = false;
    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log(Level);
        //보스룸 입장시
        PlayerBossRoomMove();
        //게임 멈춤
        if (gm.GameType == GameType.Stop)
            return;
        //수치 제한
        HpClamp();
        //이동
        Move();
        Jump();
        Dash();
        //공격
        Attack();
        //포션&Mp 자연회복
        OnPotions();
        MpUp();
        //변신
        OnTransform();
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
            transform.position = new Vector2(transform.position.x + Speed * Time.deltaTime, transform.position.y);
            spriteRenderer.flipX = false;
            transform.GetChild(0).localPosition = new Vector2(1.28f, 1.26f);
            anime.SetBool("Run", true);
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && isMove)
        {
            transform.position = new Vector2(transform.position.x - Speed * Time.deltaTime, transform.position.y);
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
        if (Input.GetKeyDown(KeyCode.X) && isJump)
        {
            if (!anime.GetBool("IsJump"))
            {
                anime.SetBool("IsJump", true);
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                doubleJump = true;
            }
            else if(doubleJump)
            {
                anime.SetBool("IsJump", true);
                rigid.velocity = new Vector2(rigid.velocity.x, 0f);
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                doubleJump = false;
            }
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
            Speed = 5f;
            if (isDash)
                dashTime = 0.1f;
        }
        else
        {
            dashTime -= Time.deltaTime;
            Speed = dashSpeed;
        }
        isDash = false;
    }
    private void OnDash()
    {
        onDash = true;
    }
    private void OnTransform()
    {
        if(data.transOn && Input.GetKeyDown(KeyCode.F) && SetMp >= 3)
        {
            if(!trans)
            {
                anime.SetBool("Trans", true);
                isMove = false;
                isJump = false;
                AttackDamage *= 2;
                AttackSpeed *= 2;
            }
        }
        if (trans)
        {
            if (transTime <= 0)
            {
                SetMp -= 1;
                transTime = 1f;
            }
            else
            {
                transTime -= Time.deltaTime;
            }
            if(SetMp == 0)
            {
                anime.SetBool("Trans", false);
                isMove = false;
                isJump = false;
                trans = false;
                AttackDamage /= 2;
                AttackSpeed /= 2;
            }
        }
    }
    private void TransTrue()
    {
        trans = true;
    }
    private void MpUp()
    {
        if(SetMp <= 10 && !trans)
        {
            if (mpTime <= 0)
            {
                SetMp += 1f;
                mpTime = 3f;
            }
            else
            {
                mpTime -= Time.deltaTime;
            }
        }
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
            SetHp += 4;
            Potions--;
            potionsNum = potionsNum <= 0 ? 1 : potionsNum - 1;
            ui.potions.PotionsImage();
        }
    }
    //저장
    public void Save()
    {
        LastPos = new Vector2(transform.position.x, transform.position.y);
        data.currentScene = gm.scene.name;
        dm.SaveData();
    }
    private void GameStart()
    {
        ui.NewGameCamera(pixelCamera);
    }
    private void PlayerBossRoomMove()
    {
        if (OnBossRoomMove)
        {
            GameObject collision = GameObject.Find("BossSpwan");
            Vector2 currentPosition = transform.position;
            Vector2 targetPosition = collision.transform.position;
            

            float dis = Vector2.Distance(currentPosition, targetPosition);

            if (dis > 0.2f)
            {
                transform.position = Vector2.MoveTowards(currentPosition, targetPosition, 3 * Time.deltaTime);
                spriteRenderer.flipX = true;
                anime.SetBool("Run", true);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<BossSpwan>())
        {
            transform.position = collision.transform.position;
            anime.SetBool("Run", false);
            collision.transform.gameObject.SetActive(false);
            StartCoroutine(SpwanBoss());
            StartCoroutine(CameraReset());
            OnBossRoomMove = false;
        }
    }
    IEnumerator SpwanBoss()
    {
        yield return new WaitForSeconds(3f);
        gm.SpwanBoss();
    }
    IEnumerator CameraReset()
    {
        yield return new WaitForSeconds(3f);

        ui.NewGameCamera(pixelCamera);

        yield return new WaitForSeconds(2f);
        ui.bossBar.SetActive(true);
    }
}
