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

    private DataManager dm;

    bool[] saveFile = new bool[3];

    // Start is called before the first frame update
    void Start()
    {
        dm = DataManager.Instance;
        for (int i = 0; i < 3; i++)
        {
            if (File.Exists(dm.path + $"{i}"))
            {
                saveFile[i] = true;
                PlayerData pd =  dm.LoadData(i);
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
        dm.nowSlot = number;

        if (!saveFile[number])
        {
            Creat();
        }
    }
    public void LoadGame(int number)
    {
        dm.nowSlot = number;

        if (saveFile[number])
        {
            dm.LoadData();
            GoGame(false);
        }
    }
    public void Creat()
    {
        creat.gameObject.SetActive(true);
    }
    public void GoGame(bool newGame)
    {
        if (!saveFile[dm.nowSlot])
        {
            dm.nowPlayer.name = newPlayerName.text;
            dm.SaveData();
        }
        if (!newGame)
        {
            dm.nowPlayer.newGame = false;
            GameManager.Instance.GameType = GameType.Start;
        }
        GameManager.Instance.OnGameSceneLode(DataManager.Instance.nowPlayer.currentScene);
    }
}
