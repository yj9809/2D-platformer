using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class State : MonoBehaviour
{
    public TMP_Text[] txt;
    [SerializeField] private Image[] item;

    private UiManager ui;
    private GameManager gm;

    public int cost;
    // Start is called before the first frame update
    void Start()
    {
        ui = UiManager.Instance;
        gm = GameManager.Instance;
        SetCost();
    }

    // Update is called once per frame
    void Update()
    {
        ui.OnStateBord(this.transform);
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
        cost = Mathf.RoundToInt(10 * gm.P.Level * 1.5f);
    }
}
