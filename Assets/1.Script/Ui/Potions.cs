using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Potions : MonoBehaviour
{
    [SerializeField] private Image[] potions;

    private void Update()
    {
        UiManager.Instance.SetPotions(this);
    }
    public Image NowPotions()
    {
        for (int i = 0; i < potions.Length; i++)
        {
            if (potions[i].sprite == UiManager.Instance.potionsImg[2])
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
