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
    public float attackSpeed = 1.5f;
    public Transform lastPos;
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

    public PlayerData LoadData(int index)
    {
        string data = File.ReadAllText(path + index.ToString());
        PlayerData pd = JsonUtility.FromJson<PlayerData>(data);

        return pd;
    }
}
