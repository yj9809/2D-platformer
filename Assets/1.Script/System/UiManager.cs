using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.U2D;
using System.IO;
using TMPro;
using DG.Tweening;

public class UiManager : Singleton<UiManager>
{
    [SerializeField] private GameObject state;
    private GameManager gm;
    private PixelPerfectCamera pixelCamera;

    public GameObject bossBar;
    public Sprite[] potionsImg;
    private Image hp;
    public Menu menu;
    public Image nowPotions;
    public Potions potions; 

    public GameObject load;
    public GameObject loadMenu;
    private GameObject stateBar;

    private float dis = 550f;
    private float time = 0.5f;
    private bool onBord = false;
    private bool onMenu = false;
    private bool isMove = false;

    private float bossMaxHp;
    public float BossMaxHp
    {
        get { return bossMaxHp; }
        set
        {
            bossMaxHp = value;
        }
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
    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
        string sceneName = GameManager.Instance.scene.name;
        if (sceneName == "Main")
        {
            bool isLoad = CheckFile();
            load.SetActive(isLoad);
        }
        SceneManager.activeSceneChanged += ActiveSceneChanged;
    }
    private bool CheckFile()
    {
        for (int i = 0; i < 3; i++)
        {
            if (File.Exists(DataManager.Instance.path + $"{i}"))
            {
                return true;
            }
        }
        return false;
    }

    private void ActiveSceneChanged(Scene arg0, Scene arg1)
    {
        string sceneName = GameManager.Instance.scene.name;
        if (sceneName != "Main" && sceneName != "Loding")
        {
            GameObject canvas = GameObject.Find("Ui Canvas");
            if (canvas != null)
            {
                stateBar = Instantiate(this.state, canvas.transform);
                hp = canvas.transform.GetChild(0).
                    transform.GetChild(0).
                    transform.GetChild(1).
                    transform.GetChild(1).
                    GetComponent<Image>();
                if (DataManager.Instance.nowPlayer.newGame)
                    stateBar.SetActive(false);
                else
                    stateBar.SetActive(true);
            }
        }

        if (sceneName == "BossRoom (Stage 1)")
        {
            bossBar = GameObject.Find("Boss Bar");
            bossBar.SetActive(false);
        }
    }
    public void SetMenu(Menu menu)
    {
        this.menu = menu;
    }
    public void SetHpImg()
    {
        Player p = GameManager.Instance.P;
        hp.fillAmount = p.SetHp / p.MaxHP;
    }
    public void SetBossHpImg()
    {
        Image bossHpImg = bossBar.transform.GetChild(0).GetComponent<Image>();
        bossHpImg.fillAmount = bossHp / bossMaxHp;
    }
    public void OnStateBord(Transform state)
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !onBord)
        {
            state.transform.GetComponent<RectTransform>().DOMoveY(transform.position.y + dis, time)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    state.GetChild(0).DOScale(Vector3.one, 0.1f);
                });
            onBord = true;
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && onBord)
        {
            state.GetChild(0).DOScale(Vector3.zero, 0.1f)
                .OnComplete(() =>
                {
                    state.transform.GetComponent<RectTransform>().DOMoveY(transform.position.y - dis, time)
                    .SetEase(Ease.Linear);
                });
            onBord = false;
        }
    }
    public void OnStateSet(TMP_Text txt0, TMP_Text txt1)
    {
        Player p = GameManager.Instance.P;
        txt0.text = $"{p.MaxHP}\n{p.AttackDamage}\n{p.AttackSpeed}\n{p.Speed}%";
        txt1.text = $"{p.Coin}";
    }
    public void StatUp(int num)
    {
        Player p = GameManager.Instance.P;
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
    public void OnMenu(Transform menu)
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !onMenu)
        {
            menu.transform.GetComponent<RectTransform>().DOMoveY(transform.position.y + dis, time)
                .SetEase(Ease.Linear);
            onMenu = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && onMenu)
        {
            menu.transform.GetComponent<RectTransform>().DOMoveY(transform.position.y - dis, time)
                .SetEase(Ease.Linear);
            onMenu = false;
        }
    }
    public void OnLoadMenu(Transform pos)
    {
        Instantiate(loadMenu, pos);
        loadMenu.gameObject.SetActive(true);
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
        DOTween.To(() => pixelCamera.assetsPPU, x => pixelCamera.assetsPPU = x, 16, 2)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                gm.MainCamera.blind[0].rectTransform.DOAnchorPosY(200, 2f);
                gm.MainCamera.blind[1].rectTransform.DOAnchorPosY(-200, 2f).
                OnComplete(() => 
                { 
                    stateBar.SetActive(true);
                    gm.GameType = GameType.Start;
                });
                DataManager.Instance.nowPlayer.newGame = false;
                
            });
    }
    public void BossCamera()
    {
        state.SetActive(false);
        DOTween.To(() => gm.MainCamera.GetComponent<PixelPerfectCamera>().assetsPPU, x =>
        gm.MainCamera.GetComponent<PixelPerfectCamera>().assetsPPU = x, 36, 2);
        gm.MainCamera.transform.position = gm.P.transform.position;
        gm.MainCamera.blind[0].rectTransform.anchoredPosition = Vector2.zero;
        gm.MainCamera.blind[1].rectTransform.anchoredPosition = Vector2.zero;

        gm.P.OnBossRoomMove = true;
    }
    
}
