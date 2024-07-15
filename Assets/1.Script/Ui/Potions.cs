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
    }
    private void Update()
    {
        ui.SetPotions(this);
    }
    public Image NowPotions()
    {
        for (int i = 0; i < potions.Length; i++)
        {
            if (potions[i].sprite == ui.potionsImg[0])
            {
                if (i == 0)
                {
                    return potions[i];
                }
                return potions[i - 1];
            }
            else if (potions[i].sprite == ui.potionsImg[2])
            {
                if (i == 2)
                {
                    return potions[i];
                }
                continue;
            }
            else
            {
                return potions[i];
            }
        }
        return null;
    }
}
