using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerData
{
    public string name;
    public float hp = 10;
    public float mp = 10;
    public int attackDamage = 1;
    public float attackSpeed = 1;
    public Transform lastPos;
}

public class DataManager : Singleton<DataManager>
{
    public PlayerData nowPlayer = new PlayerData();
    public string path;
    public int nowSlot;

    private void Awake()
    {
        string data = JsonUtility.ToJson(nowPlayer);
        path = Application.persistentDataPath + "/Save";
        print(data);
    }
    // Start is called before the first frame update
    void Start()
    {

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
    }
}
