using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerData
{
    public string name;
    public int level = 1;
    public bool newGame = true;
    public bool transOn = false;
    public float maxHp = 10;
    public float maxMp = 10;
    public float hp = 10;
    public float mp = 10;
    public int attackDamage = 2;
    public float attackSpeed = 1.5f;
    public float speed = 5;
    public int coin = 0;
    public int potions = 6;
    public string currentScene = "Game (Stage 1)";
    public Vector2 lastPos = new Vector2(-24, -9);
}

public class DataManager : Singleton<DataManager>
{
    public PlayerData nowPlayer = new PlayerData();
    public string path;
    public int nowSlot;

    private void Awake()
    {
        path = Application.persistentDataPath + "/Save";
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(path);
    }

    public void SaveData()
    {
        string data = JsonUtility.ToJson(nowPlayer);
        File.WriteAllText(path + nowSlot.ToString(), data);
    }
    public void LoadData()
    {
        string data = File.ReadAllText(path + nowSlot.ToString());
        nowPlayer = JsonUtility.FromJson<PlayerData>(data);
        GameManager.Instance.pos = nowPlayer.lastPos;
    }

    public PlayerData LoadData(int index)
    {
        string data = File.ReadAllText(path + index.ToString());
        PlayerData pd = JsonUtility.FromJson<PlayerData>(data);

        return pd;
    }
}
