using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class State : MonoBehaviour
{
    [SerializeField] private GameObject coinBox;
    [SerializeField] private Tooltip tooltip;
    [SerializeField] private TMP_Text[] txt;
    [SerializeField] private Image[] item;

    private GameManager gm;
    private PlayerData data;
    private UiManager ui;

    private int cost;
    private float firstCost = 10;
    private float growthRate = 0.35f;
    // Start is called before the first frame update
    void Start()
    {
        ui = UiManager.Instance;
        gm = GameManager.Instance;
        data = DataManager.Instance.NowPlayer;

        if (data.NewGame)
            coinBox.SetActive(false);

        ui.SetState(this);
        SetCost();
    }
    public void OnPush(int num)
    {
        if (gm.P.Coin >= cost)
        {
            gm.P.Coin -= cost;
            gm.P.Level += 1;
            SetCost();
            ui.StateUp(num);
            ui.OnStateSet();
        }
    }
    private void SetCost()
    {
        cost = GetCost();
    }
    public void UpdateItem()
    {
        item[0].gameObject.SetActive(data.BlackSoul);
        item[1].gameObject.SetActive(data.WingsShoes);
        item[2].gameObject.SetActive(data.SkillBook);
        item[3].gameObject.SetActive(data.Scroll);
    }
    public void CoinBoxActive(bool isCoinBox)
    {
        coinBox.SetActive(isCoinBox);
    }
    public void StateTxtSet()
    {
       txt[0].text = $"{gm.P.MaxHP}\n{gm.P.MaxMp}\n{gm.P.AttackDamage}\n{gm.P.AttackSpeed}";
       txt[1].text = gm.P.Coin >= cost ? $"<color=green>{gm.P.Coin}</color>": $"<color=red>{gm.P.Coin}</color>";
       txt[2].text = $"Cost : {cost}";
       txt[4].text = $"{gm.P.Level}";
    }
    public void CoinSet()
    {
        txt[3].text = gm.P.Coin.ToString();
    }
    private int GetCost()
    {
        if(gm.P.Level < 1)
        {
            Debug.LogError("레벨이 이상합니다");
            return 0;
        }

        float cost = firstCost * Mathf.Exp(growthRate * (gm.P.Level - 1));

        return Mathf.RoundToInt(cost);
    }
}
