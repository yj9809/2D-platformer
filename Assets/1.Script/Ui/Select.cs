using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class Select : MonoBehaviour
{
    [SerializeField] private GameObject createPanel;
    [SerializeField] private TMP_Text[] slotTexts;
    [SerializeField] private TMP_Text newPlayerNameText;

    private DataManager dataManager;
    private bool[] saveFileExists = new bool[3];

    // Start is called before the first frame update
    void Start()
    {
        dataManager = DataManager.Instance;
        UpdateSlotTexts();
    }

    private void UpdateSlotTexts()
    {
        for (int i = 0; i < saveFileExists.Length; i++)
        {
            string filePath = dataManager.GetSaveFilePath(i);
            if (File.Exists(filePath))
            {
                saveFileExists[i] = true;
                PlayerData playerData = dataManager.LoadData(i);
                slotTexts[i].text = playerData.name;
            }
            else
            {
                saveFileExists[i] = false;
                slotTexts[i].text = "New File";
            }
        }
    }

    public void StartNewGame(int slotIndex)
    {
        dataManager.nowSlot = slotIndex;

        if (!saveFileExists[slotIndex])
        {
            ShowCreatePanel();
        }
    }

    public void LoadGame(int slotIndex)
    {
        dataManager.nowSlot = slotIndex;

        if (saveFileExists[slotIndex])
        {
            Debug.Log("½ÇÇà");
            dataManager.LoadData();
            StartGame(false);
        }
    }

    private void ShowCreatePanel()
    {
        createPanel.SetActive(true);
    }

    public void StartGame(bool isNewGame)
    {
        if (!saveFileExists[dataManager.nowSlot])
        {
            dataManager.nowPlayer.name = newPlayerNameText.text;
            dataManager.SaveData();
        }

        if (!isNewGame)
        {
            dataManager.nowPlayer.newGame = false;
            GameManager.Instance.GameType = GameType.Start;
        }

        GameManager.Instance.OnGameSceneLoad(dataManager.nowPlayer.currentScene);
    }
}