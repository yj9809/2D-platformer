using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class Selet : MonoBehaviour
{
    [SerializeField] private GameObject creat;
    [SerializeField] private TMP_Text[] slotText;
    [SerializeField] private TMP_Text newPlayerName;

    bool[] saveFile = new bool[3];

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            if (File.Exists(DataManager.Instance.path + $"{i}"))
            {
                saveFile[i] = true;
                PlayerData pd =  DataManager.Instance.LoadData(i);
                slotText[i].text = pd.name;
            }
            else
            {
                slotText[i].text = "New File";
            }
        }
    }
    public void NewGame(int number)
    {
        DataManager.Instance.nowSlot = number;

        if (!saveFile[number])
        {
            Creat();
        }
    }
    public void LoadGame(int number)
    {
        DataManager.Instance.nowSlot = number;

        if (saveFile[number])
        {
            DataManager.Instance.LoadData();
            GoGame();
        }
    }
    public void Creat()
    {
        creat.gameObject.SetActive(true);
    }
    public void GoGame()
    {
        if (!saveFile[DataManager.Instance.nowSlot])
        {
            DataManager.Instance.nowPlayer.name = newPlayerName.text;
            DataManager.Instance.SaveData();
        }
        GameManager.Instance.OnGameSceneLode();
    }
}
