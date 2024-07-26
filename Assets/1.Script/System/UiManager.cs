using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.U2D;
using System.IO;
using TMPro;
using DG.Tweening;
using Sirenix.OdinInspector;

public class UiManager : Singleton<UiManager>
{
    private GameManager gm;
    private PixelPerfectCamera pixelCamera;

    [TabGroup("Game Scene Ui")] [SerializeField] private GameObject hpState;
    [TabGroup("Game Scene Ui")] [SerializeField] private GameObject loadMenu;
    [TabGroup("Game Scene Ui")] public GameObject bossBar;

    [TabGroup("Potion")] public Sprite[] potionsImg;
    [TabGroup("Potion")] public Potions potions;
    [TabGroup("Potion")] public Image nowPotions;

    [TabGroup("Main Scene Ui")] public GameObject load;

    private Image hp;
    private Image mp;
    private State stateBord;
    private Menu menu;
    private GameObject hpBar;


    private float dis = 550f;
    private float time = 0.5f;
    [TabGroup("Ui Bool")] [ReadOnly] [SerializeField]private bool onBord = false;
    [TabGroup("Ui Bool")] [ReadOnly] [SerializeField]private bool onMenu = false;

    private float bossMaxHp;
    public float BossMaxHp
    {
        get { return bossMaxHp; }
        set { bossMaxHp = value; }
    }

    private float bossHp;
    public float BossHp
    {
        get { return bossHp; }
        set
        {
            bossHp = value;
            SetBossHpImg();
        }
    }

    void Start()
    {
        gm = GameManager.Instance;
        string sceneName = gm.scene.name;
        if (sceneName == "Main")
        {
            load.SetActive(CheckFile());
        }
        SceneManager.activeSceneChanged += ActiveSceneChanged;
    }
    private bool CheckFile()
    {
        for (int i = 0; i < 3; i++)
        {
            if (File.Exists($"{DataManager.Instance.savePath}/slot_{i}.json"))
            {
                return true;
            }
        }
        return false;
    }
    private void ActiveSceneChanged(Scene current, Scene next)
    {
        string sceneName = gm.scene.name;
        if (sceneName != "Main" && sceneName != "Loding")
        {
            SetHpBar();
        }

        if (sceneName == "BossRoom (Stage 1)" || sceneName == "BossRoom (Stage 2)")
        {
            bossBar = GameObject.Find("Boss Bar");
            bossBar.SetActive(false);
        }
    }
    private void SetHpBar()
    {
        GameObject canvas = GameObject.Find("Ui Canvas");
        if (canvas != null)
        {
            hpBar = Instantiate(this.hpState, canvas.transform);

            Transform healthTransform = canvas.transform.GetChild(0).GetChild(0);
            hp = healthTransform.GetChild(1).GetChild(1).GetComponent<Image>();
            mp = healthTransform.GetChild(2).GetChild(1).GetComponent<Image>();

            hpBar.SetActive(!DataManager.Instance.nowPlayer.newGame);
        }
    }
    public void SetMenu(Menu menu)
    {
        this.menu = menu;
    }
    public void SetState(State state)
    {
        this.stateBord = state;
    }
    public void SetHpImg()
    {
        Player p = gm.P;
        hp.fillAmount = p.SetHp / p.MaxHP;
    }
    public void SetMpImg()
    {
        Player p = gm.P;
        mp.fillAmount = p.SetMp / p.MaxMp;
    }
    public void SetBossHpImg()
    {
        Image bossHpImg = bossBar.transform.GetChild(0).GetComponent<Image>();
        bossHpImg.fillAmount = bossHp / bossMaxHp;
    }
    public void OnStateBord()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("½ÇÇà");
            float targetY = onBord ? transform.position.y - dis : transform.position.y + dis;
            gm.GameType = onBord ? GameType.Start : GameType.Stop;
            stateBord.GetComponent<RectTransform>().DOMoveY(targetY, time).SetEase(Ease.Linear);
            OnStateSet();
            onBord = !onBord;
        }
    }
    public void OnStateSet()
    {
        State state = FindObjectOfType<State>();
        Player p = gm.P;
        state.txt[2].text = $"Cost : {state.cost}";
        state.txt[0].text = $"{p.MaxHP}\n{p.AttackDamage}\n{p.AttackSpeed}\n{p.Speed}%";
        state.txt[1].text = $"{p.Coin}";
        state.txt[4].text = $"{p.Level}";
        state.UpdateItem();
    }
    public void StateUp(int num)
    {
        Player p = gm.P;
        switch (num)
        {
            case 0:
                p.MaxHP += 2;
                p.SetHp += 2;
                break;
            case 1:
                p.AttackDamage += 2;
                break;
            case 2:
                p.AttackSpeed += 0.2f;
                break;
            case 3:
                p.Speed += 0.5f;
                break;
        }
    }
    public void SetCoin()
    {
        State state = FindObjectOfType<State>();
        state.txt[3].text = gm.P.Coin.ToString();
    }
    public void OnMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            float targetY = onMenu ? transform.position.y - dis : transform.position.y + dis;
            menu.GetComponent<RectTransform>().DOMoveY(targetY, time).SetEase(Ease.Linear);
            onMenu = !onMenu;
        }
    }
    public GameObject OnLoadMenu(Transform pos)
    {
        GameObject newLoadMebnu = Instantiate(loadMenu, pos);
        return newLoadMebnu;
    }
    public void OnExit()
    {
        Application.Quit();
    }
    public void SetPotions(Potions potions)
    {
        this.potions = potions;
    }
    public void NewGameCamera(PixelPerfectCamera pixelCamera)
    {
        DOTween.To(() => (float)pixelCamera.assetsPPU, x => pixelCamera.assetsPPU = Mathf.RoundToInt(x), 16, 2f)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                gm.MainCamera.blind[0].rectTransform.DOAnchorPosY(200, 0.5f);
                gm.MainCamera.blind[1].rectTransform.DOAnchorPosY(-200, 0.5f)
                .OnComplete(() => 
                {
                    gm.GameType = GameType.Start;

                    if(bossBar != null)
                        bossBar.SetActive(true);

                    hpBar.SetActive(true);
                }
                );
                DataManager.Instance.nowPlayer.newGame = false;
            });
    }
    public void BossCamera()
    {
        hpBar.SetActive(false);
        PixelPerfectCamera ppc = gm.MainCamera.GetComponent<PixelPerfectCamera>();
        DOTween.To(() => (float)ppc.assetsPPU, x => ppc.assetsPPU = Mathf.RoundToInt(x), 36f, 2f);
        gm.MainCamera.transform.position = gm.P.transform.position;
        gm.MainCamera.blind[0].rectTransform.anchoredPosition = Vector2.zero;
        gm.MainCamera.blind[1].rectTransform.anchoredPosition = Vector2.zero;

        gm.P.OnBossRoomMove = true;
    }
}