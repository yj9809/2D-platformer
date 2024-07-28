using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Sirenix.OdinInspector;
[System.Serializable]
public class PlayerData
{
    public string name;
    public string currentScene = "Game (Stage 1)";
    public int level = 1;
    public int attackDamage = 2;
    public int coin = 0;
    public int potions = 6;
    public float maxHp = 10f;
    public float maxMp = 10f;
    public float hp = 10f;
    public float mp = 10f;
    public float attackSpeed = 1.5f;
    public float speed = 5f;
    public bool newGame = true;
    public bool blackSoul = true;
    public Vector2 lastPos = new Vector2(-24f, -9f);
}

public class DataManager : Singleton<DataManager>
{
    [ReadOnly] public PlayerData nowPlayer = new PlayerData();
    public string savePath;
    public int nowSlot;

    protected override void Awake()
    {
        base.Awake();

        savePath = Path.Combine(Application.persistentDataPath, "Save");
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
    }

    private void Start()
    {
        Debug.Log(savePath);
    }

    public void SaveData()
    {
        string filePath = Path.Combine(savePath, $"slot_{nowSlot}.json");
        string jsonData = JsonUtility.ToJson(nowPlayer);
        File.WriteAllText(filePath, jsonData);
    }

    public void LoadData()
    {
        string filePath = Path.Combine(savePath, $"slot_{nowSlot}.json");
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            nowPlayer = JsonUtility.FromJson<PlayerData>(jsonData);
            GameManager.Instance.pos = nowPlayer.lastPos;
        }
        else
        {
            Debug.LogWarning($"Save file not found: {filePath}");
        }
    }

    public PlayerData LoadData(int slotIndex)
    {
        string filePath = Path.Combine(savePath, $"slot_{slotIndex}.json");
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            return JsonUtility.FromJson<PlayerData>(jsonData);
        }
        else
        {
            Debug.LogWarning($"Save file not found: {filePath}");
            return null;
        }
    }
    public string GetSaveFilePath(int slotIndex)
    {
        return Path.Combine(savePath, $"slot_{slotIndex}.json");
    }
}