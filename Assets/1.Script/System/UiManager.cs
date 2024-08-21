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
    private DataManager data;

    [TabGroup("Game Scene Ui")] [SerializeField] private GameObject hpState;
    [TabGroup("Game Scene Ui")] [SerializeField] private GameObject loadMenu;
    [TabGroup("Game Scene Ui")] [SerializeField] private GameObject coinBox;
    [TabGroup("Game Scene Ui")] [SerializeField] private Image portrait;
    [TabGroup("Game Scene Ui")] [SerializeField] private Sprite[] portraitSprite;
    [TabGroup("Game Scene Ui")] public GameObject bossBar;
    [TabGroup("Game Scene Ui"), SerializeField] private EndMessage end;

    [TabGroup("Potion")] public Sprite[] potionsImg;
    [TabGroup("Potion")] public Potions potions;
    [TabGroup("Potion")] public Image nowPotions;

    [TabGroup("Main Scene Ui")] public GameObject load;

    private Image hp;
    private Image mp;
    private State stateBord;
    private Menu menu;
    private GameObject hpBar;


    private float time = 0.1f;
    [TabGroup("Ui Bool")] [ReadOnly] [SerializeField]private bool onState = false;
    [TabGroup("Ui Bool")] [ReadOnly] [SerializeField]private bool onMenu = false;
    public bool OnMenu
    {
        get { return onMenu; }
    }
    public bool OnState
    {
        get { return onState; }
    }

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
    public EndMessage EndMessage
    {
        get { return end; }
    }

    void Start()
    {
        gm = GameManager.Instance;
        data = DataManager.Instance;
        string sceneName = gm.scene.name;
        if (sceneName == "Main")
        {
            load.SetActive(CheckFile());
        }
        SceneManager.activeSceneChanged += OnSceneLoaded;
    }
    private bool CheckFile()
    {
        for (int i = 0; i < 3; i++)
        {
            if (File.Exists($"{data.savePath}/slot_{i}.json"))
            {
                return true;
            }
        }
        return false;
    }
    private void OnSceneLoaded(Scene current, Scene next)
    {
        if (gm.GameType == GameType.Clear)
            return;

        string sceneName = gm.scene.name;
        if (sceneName != "Main" && sceneName != "Loding" && sceneName != "Clear")
        {
            SetHpBar();
            if (!data.NowPlayer.NewGame)
                SetPortrait();
        }

        if (sceneName == "BossRoom (Stage 1)" || sceneName == "BossRoom (Stage 2)")
        {
            InitBossBar();
        }
    }
    private void InitBossBar()
    {
        bossBar = GameObject.Find("Boss Bar");
        bossBar.SetActive(false);
    }
    private void SetPortrait()
    {
        portrait = GameObject.Find("Portrait").GetComponent<Image>();
        portrait.sprite = portraitSprite[0];
    }
    public void ChangePortrait()
    {
        portrait.sprite = (portrait.sprite == portraitSprite[0]) ? portraitSprite[1] : portraitSprite[0];
    }
    private void SetHpBar()
    {
        GameObject canvas = GameObject.Find("Ui Canvas");
        if (canvas != null)
        {
            hpBar = Instantiate(hpState, canvas.transform);

            InitStateBar(canvas);

            hpBar.SetActive(!data.NowPlayer.NewGame);
        }
    }
    private void InitStateBar(GameObject canvas)
    {
        Transform healthTransform = canvas.transform.GetChild(0).GetChild(0);
        hp = healthTransform.GetChild(1).GetChild(1).GetComponent<Image>();
        mp = healthTransform.GetChild(2).GetChild(1).GetComponent<Image>();
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
        UpdateFillAmount(hp, gm.P.SetHp, gm.P.MaxHP);
    }
    public void SetMpImg()
    {
        UpdateFillAmount(mp, gm.P.SetMp, gm.P.MaxMp);
    }
    public void SetBossHpImg()
    {
        UpdateFillAmount(bossBar.transform.GetChild(0).GetComponent<Image>(), bossHp, bossMaxHp);
    }
    private void UpdateFillAmount(Image image, float currentValue, float maxValue)
    {
        image.fillAmount = currentValue / maxValue;
    }
    private void ToggleUi(bool isOn, KeyCode key, RectTransform uiRect, System.Action onComplete)
    {
        if (Input.GetKeyDown(key))
        {
            int targetY = isOn ? 0 : 1;
            gm.GameType = isOn ? GameType.Start : GameType.Stop;
            uiRect.DOScale(targetY, time).SetEase(Ease.Linear)
                .OnComplete(() => onComplete());
        }
    }
    public void OnStateBord()
    {
        OnStateSet();
        ToggleUi(onState, KeyCode.Tab, stateBord.GetComponent<RectTransform>(), () => onState = !onState);
    }
    public void OnStateBordButton()
    {
        int targetY = onState ? 0 : 1;
        gm.GameType = onState ? GameType.Start : GameType.Stop;
        stateBord.GetComponent<RectTransform>().DOScale(targetY, time).SetEase(Ease.Linear)
            .OnComplete(() => onState = !onState);
    }
    public void OnStateSet()
    {
        stateBord.StateTxtSet();
        stateBord.UpdateItem();
    }
    public void StateUp(int num)
    {
        Player p = gm.P;
        switch (num)
        {
            case 0:
                p.MaxHP += 4;
                p.SetHp += 4;
                break;
            case 1:
                p.MaxMp += 2;
                p.SetMp += 2;
                break;
            case 2:
                p.AttackDamage += 2;
                break;
            case 3:
                p.AttackSpeed += 0.2f;
                break;
        }
        p.OriginalStats();
    }
    public void SetCoin()
    {
        stateBord.CoinSet();
    }
    public void OnMenuBord()
    {
        ToggleUi(onMenu, KeyCode.Escape, menu.transform.parent.GetComponent<RectTransform>(), () => onMenu = !onMenu);
    }
    public void OnMenuBordButton()
    {
        int targetY = onMenu ? 0 : 1;
        gm.GameType = onMenu ? GameType.Start : GameType.Stop;
        menu.transform.parent.GetComponent<RectTransform>().DOScale(targetY, time).SetEase(Ease.Linear)
            .OnComplete(() => onMenu = !onMenu);

    }
    public GameObject OnLoadMenu(Transform pos)
    {
        GameObject newLoadMebnu = Instantiate(loadMenu, pos);
        return newLoadMebnu;
    }
    public void SetEnd(EndMessage end)
    {
        this.end = end;
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
                gm.GameType = GameType.Start;

                if(bossBar != null)
                    bossBar.SetActive(true);
                hpBar.SetActive(true);
                SetPortrait();
                stateBord.CoinBoxActive(true);

                data.NowPlayer.NewGame = false;
            });
    }
    public void BossCamera()
    {
        hpBar.SetActive(false);
        stateBord.CoinBoxActive(false);
        PixelPerfectCamera ppc = gm.MainCamera.GetComponent<PixelPerfectCamera>();
        DOTween.To(() => (float)ppc.assetsPPU, x => ppc.assetsPPU = Mathf.RoundToInt(x), 36f, 2f);
        gm.MainCamera.transform.position = gm.P.transform.position;

        gm.P.OnBossRoomMove = true;
    }
}