using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using TMPro;
using DG.Tweening;

public class UiManager : Singleton<UiManager>
{
    public Sprite[] potionsImg;
    private Image hp;
    public Image nowPotions;
    public Menu menu;
    public Potions potions;

    public GameObject load;
    public GameObject state;
    public GameObject loadMenu;

    private float dis = 550f;
    private float time = 0.5f;
    private bool onBord = false;
    private bool onMenu = false;
    // Start is called before the first frame update
    void Start()
    {
        string sceneName = GameManager.Instance.scene.name;
        if (sceneName == "Main")
        {
            bool isLoad = false;

            for (int i = 0; i < 3; i++)
            {
                if (File.Exists(DataManager.Instance.path + $"{i}"))
                {
                    isLoad = true;
                }
            }
            load.SetActive(isLoad);
        }
        SceneManager.activeSceneChanged += ActiveSceneChanged;
    }

    private void ActiveSceneChanged(Scene arg0, Scene arg1)
    {
        string sceneName = GameManager.Instance.scene.name;
        if (sceneName != "Main" && sceneName != "Loding")
        {
            GameObject canvas = GameObject.Find("Ui Canvas");
            if (canvas != null)
            {
                Instantiate(state, canvas.transform);
                hp = canvas.transform.GetChild(0).
                    transform.GetChild(0).
                    transform.GetChild(1).
                    transform.GetChild(1).
                    GetComponent<Image>();
            }
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
    public void TransPotionsImg(int num)
    {
        Image nowPotions = NowPotions();
        nowPotions.sprite = potionsImg[num];
    }
    public void SetPotions(Potions potions)
    {
        this.potions = potions;
    }
    public Image NowPotions()
    {
        return potions.NowPotions();
    }
}
