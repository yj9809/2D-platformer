using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;

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
    private const float DashSpeed = 25f;
    private const float DefaultSpeed = 5f;
    private const float DashDuration = 0.1f;
    private const float MpRegenInterval = 3f;  // MP 회복 간격
    private const float TransformMpCostInterval = 1f;
    private const float TransformMpCost = 1f;
    // 점프 바닥 체크
    [FoldoutGroup("Jump Control")] 
    private const float JumpPower = 8f;
    [FoldoutGroup("Jump Control")] 
    [SerializeField] private Transform groundCheckObj;
    [FoldoutGroup("Jump Control")]
    [SerializeField] private LayerMask groundLayer;
    [FoldoutGroup("Jump Control")]
    [SerializeField] private bool isGround;


    private float dashTime;
    private float transTime;
    private float mpTime;
    //이동 관련 참/거짓
    [FoldoutGroup("Moving Control")] [ReadOnly] public bool isMove;
    [FoldoutGroup("Moving Control")] [ReadOnly] public bool isJump;
    [FoldoutGroup("Moving Control")] [ReadOnly] public bool isDash;
    [FoldoutGroup("Moving Control")] [ReadOnly] public bool onDash;
    [FoldoutGroup("Moving Control")] [ReadOnly] public bool doubleJump;
    [FoldoutGroup("Moving Control")] [ReadOnly] public bool darkTransform;
    [FoldoutGroup("Moving Control")] [ReadOnly] public bool OnBossRoomMove = false;
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
            data.level = value;
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

        isMove = true;
        isJump = true;
        onDash = true;
        darkTransform = false;
    }
    // Update is called once per frame
    void Update()
    {
        // 보스룸 입장시
        PlayerBossRoomMove();
        // ui 제어
        ui.OnMenu();
        ui.OnStateBord();
        // 게임 멈춤
        if (gm.GameType == GameType.Stop && !data.newGame)
        {
            anime.enabled = false;
            return;
        }
        else
            anime.enabled = true;

        // 수치 제한
        HpClamp();
        MpClamp();
        // 이동
        Move();
        Jump();
        GroundCheck();
        Dash();
        // 공격
        Attack();
        // 포션&Mp 자연회복
        OnPotions();
        MpUp();
        // 변신
        OnTransform();
    }
    //Ui 관련
    private void HpClamp()
    {
        SetHp = Mathf.Clamp(SetHp, 0, MaxHP);
    }
    private void MpClamp()
    {
        SetMp = Mathf.Clamp(SetMp, 0, MaxMp);
    }
    private void MpUp()
    {
        if (SetMp <= 10 && !darkTransform && mpTime <= 0)
        {
            SetMp += 1f;
            mpTime = MpRegenInterval;
        }
        mpTime -= Time.deltaTime;
    }
    //이동 관련
    private void Move()
    {
        if (!isMove)
        {
            anime.SetBool("Run", false);
            return;
        }

        float moveInput = Input.GetAxisRaw("Horizontal");
        if (moveInput != 0)
        {
            transform.position += new Vector3(moveInput * Speed * Time.deltaTime, 0);
            spriteRenderer.flipX = moveInput < 0;
            transform.GetChild(0).localPosition = new Vector2(spriteRenderer.flipX ? -1.28f : 1.28f, 0f);
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
        if (Input.GetKeyDown(KeyCode.X) && isJump && isGround)
        {
            if (!anime.GetBool("IsJump"))
            {
                rigid.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
                doubleJump = true;
            }
            else if (doubleJump)
            {
                anime.SetBool("IsJump", true);
                rigid.velocity = new Vector2(rigid.velocity.x, 0f);
                rigid.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
                doubleJump = false;
            }
        }
        anime.SetBool("IsJump", rigid.velocity.y > 0);
        anime.SetBool("IsFalling", rigid.velocity.y < 0);
    }    
    private void GroundCheck()
    {
        isGround = Physics2D.OverlapCircle(groundCheckObj.position, 0.1f, groundLayer);
    }
    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.Z) && onDash && anime.GetBool("Run"))
        {
            gameObject.layer = 10;
            GameObject dashEffect = Pooling.Instance.GetDash();
            dashEffect.transform.GetComponent<ParticleSystemRenderer>().flip = spriteRenderer.flipX ? new Vector3(1, 0, 0) : Vector3.zero;
            dashEffect.GetComponent<ParticleSystem>().Play();
            dashEffect.transform.position = transform.position;
            onDash = false;
            isDash = true;
            anime.SetTrigger("IsDash");

            Invoke("OffDashDamage", 0.2f);
            Invoke("OnDash", 0.5f);
        }

        if (dashTime <= 0)
        {
            Speed = DefaultSpeed;
            if (isDash)
                dashTime = DashDuration;
        }
        else
        {
            dashTime -= Time.deltaTime;
            Speed = DashSpeed;
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
    // 변신 관련
    private void OnTransform()
    {
        if (data.blackSoul && Input.GetKeyDown(KeyCode.G) && SetMp >= 3)
        {
            if (!darkTransform)
            {
                anime.SetBool("Trans", true);
                isMove = false;
                isJump = false;
                AttackDamage *= 2;
                AttackSpeed *= 2;
            }
        }

        if (darkTransform)
        {
            if (transTime <= 0)
            {
                SetMp -= TransformMpCost;
                transTime = TransformMpCostInterval;
            }
            else
            {
                transTime -= Time.deltaTime;
            }

            if (SetMp <= 0)
            {
                anime.SetBool("Trans", false);
                isMove = false;
                isJump = false;
                darkTransform = false;
                AttackDamage /= 2;
                AttackSpeed /= 2;
            }
        }
    }
    private void TransTrue()
    {
        darkTransform = true;
    }
    //공격 관련 & 데미지 함수
    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            anime.SetTrigger("IsAttack");
        }
    }
    private void OnAttackCollision()
    {
        attackCollision.SetActive(true);
    }
    public void OnPlayerDamage(Vector2 pos, int damage)
    {
        if (SetHp <= 0)
            return;

        gameObject.layer = 10;
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        int dirc = transform.position.x - pos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1) * 2, ForceMode2D.Impulse);
        SetHp -= damage;
        isMove = false;
        Invoke("OffDamage", 0.5f);

        if (SetHp <= 0)
        {
            GetComponent<Collider2D>().enabled = false;
            rigid.simulated = false;
            anime.SetTrigger("Death");
        }
    }
    private void OffDamage()
    {
        gameObject.layer = 3;
        isMove = true;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }
    // 포션
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
    // 부활
    private void Resurrection()
    {

    }
    // 저장
    public void Save()
    {
        if(gm.scene.name == "Game (Stage 1)" || gm.scene.name == "Game (Stage 2)")
        {
            data.currentScene = gm.scene.name;
            LastPos = new Vector2(transform.position.x, transform.position.y);
        }

        dm.SaveData();
    }
    // 새로운 게임 시작시
    private void GameStart()
    {
        ui.NewGameCamera(pixelCamera);
    }
    // 보스룸 입장시
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
    private void ChangeGameType()
    {
        gm.GameType = GameType.Stop;
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

        //yield return new WaitForSeconds(2f);
        //gm.GameType = GameType.Start;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 11)
        {
            OnPlayerDamage(collision.transform.position, 2);
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
}
