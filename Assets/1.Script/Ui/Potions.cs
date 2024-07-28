using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Potions : MonoBehaviour
{
    [SerializeField] private Image[] potions;

    private UiManager ui;

    private void Start()
    {
        ui = UiManager.Instance;
        ui.SetPotions(this);
        PotionsImage();
    }
    public void PotionsImage()
    {
        switch(DataManager.Instance.NowPlayer.potions)
        {
            case 6:
                for (int i = 0; i < potions.Length; i++)
                    potions[i].sprite = ui.potionsImg[2];
                break;
            case 5:
                for (int i = 0; i < potions.Length - 1; i++)
                    potions[i].sprite = ui.potionsImg[2];
                potions[2].sprite = ui.potionsImg[1];
                break;
            case 4:
                for (int i = 0; i < potions.Length - 1; i++)
                    potions[i].sprite = ui.potionsImg[2];
                potions[2].sprite = ui.potionsImg[0];
                break;
            case 3:
                potions[0].sprite = ui.potionsImg[2];
                potions[1].sprite = ui.potionsImg[1];
                potions[2].sprite = ui.potionsImg[0];
                break;
            case 2:
                potions[0].sprite = ui.potionsImg[2];
                potions[1].sprite = ui.potionsImg[0];
                potions[2].sprite = ui.potionsImg[0];
                break;
            case 1:
                potions[0].sprite = ui.potionsImg[1];
                potions[1].sprite = ui.potionsImg[0];
                potions[2].sprite = ui.potionsImg[0];
                break;
            case 0:
                potions[0].sprite = ui.potionsImg[0];
                potions[1].sprite = ui.potionsImg[0];
                potions[2].sprite = ui.potionsImg[0];
                break;
        }
    }
}
