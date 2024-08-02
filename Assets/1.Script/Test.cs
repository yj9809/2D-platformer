using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private GameObject[] joystick;
    private Player p;
    private UiManager ui;

    private bool isJoystick = false;
    private void Start()
    {
        p = GameManager.Instance.P;
        ui = UiManager.Instance;
    }
    public void Jump()
    {
        if (ui.OnMenu)
            ui.OnMenuBordButton();
        p.JumpButton();
    }
    public void Attack()
    {
        if (ui.OnMenu)
            ui.OnMenuBordButton();
        p.AttackButton();
    }
    public void Dash()
    {
        if (ui.OnMenu)
            ui.OnMenuBordButton();
        p.DashButton();
    }
    public void Potion()
    {
        if (ui.OnMenu)
            ui.OnMenuBordButton();
        p.PotionButton();
    }
    public void StateBord()
    {
        if (ui.OnMenu)
            ui.OnMenuBordButton();

        ui.OnStateBordButton();
        for (int i = 0; i < joystick.Length; i++)
        {
            joystick[i].SetActive(isJoystick ? true : false);
        }
        isJoystick = !isJoystick;
    }
}
