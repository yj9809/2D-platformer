using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Sirenix.OdinInspector;
[System.Serializable]
public class PlayerData
{
    [SerializeField] private string name;
    [SerializeField] private string currentScene = "Tutorial Scenes";
    [SerializeField] private int level = 1;
    [SerializeField] private int attackDamage = 2;
    [SerializeField] private int coin = 0;
    [SerializeField] private int potions = 6;
    [SerializeField] private float maxHp = 10f;
    [SerializeField] private float maxMp = 10f;
    [SerializeField] private float hp = 10f;
    [SerializeField] private float mp = 10f;
    [SerializeField] private float attackSpeed = 1.5f;
    [SerializeField] private float speed = 5f;
    [SerializeField] private bool newGame = true;
    [SerializeField] private bool door = false;
    [SerializeField] private bool blackSoul = false;
    [SerializeField] private bool wingsShoes = false;
    [SerializeField] private bool skillBook = false;
    [SerializeField] private bool scroll = false;
    [SerializeField] private bool mainBoss = true;
    [SerializeField] private bool middleBoss = true;
    [SerializeField] private bool mossyMainBoss = true;
    [SerializeField] private bool mossyMiddleBoss = true;
    [SerializeField] private Vector2 lastPos = new Vector2(-24f, -9f);
    
    public string Name { get => name; set => name = value; }
    public string CurrentScene { get => currentScene; set => currentScene = value; }
    public int Level { get => level; set => level = value; }
    public int AttackDamage { get => attackDamage; set => attackDamage = value; }
    public int Coin { get => coin; set => coin = value; }
    public int Potions { get => potions; set => potions = value; }
    public float MaxHp { get => maxHp; set => maxHp = value; }
    public float MaxMp { get => maxMp; set => maxMp = value; }
    public float Hp { get => hp; set => hp = value; }
    public float Mp { get => mp; set => mp = value; }
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    public float Speed { get => speed; set => speed = value; }
    public bool NewGame { get => newGame; set => newGame = value; }
    public bool Door { get => door; set => door = value; }
    public bool BlackSoul { get => blackSoul; set => blackSoul = value; }
    public bool WingsShoes { get => wingsShoes; set => wingsShoes = value; }
    public bool SkillBook { get => skillBook; set => skillBook = value; }
    public bool Scroll { get => scroll; set => scroll = value; }
    public bool MainBoss { get => mainBoss; set => mainBoss = value; }
    public bool MiddleBoss { get => middleBoss; set => middleBoss = value; }
    public bool MossyMainBoss { get => mossyMainBoss; set => mossyMainBoss = value; }
    public bool MossyMiddleBoss { get => mossyMiddleBoss; set => mossyMiddleBoss = value; }
    public Vector2 LastPos { get => lastPos; set => lastPos = value; }
}

public class DataManager : Singleton<DataManager>
{
    [SerializeField] private PlayerData nowPlayer = new PlayerData();
    
    [FoldoutGroup("Save")] public string savePath;
    [FoldoutGroup("Save")] public int nowSlot;

    public PlayerData NowPlayer
    {
        get { return nowPlayer; }
        set
        {
            nowPlayer = value;
        }
    }
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
        string filePath = GetSaveFilePath(nowSlot);
        string jsonData = JsonUtility.ToJson(nowPlayer);
        File.WriteAllText(filePath, jsonData);
    }
    public void LoadData()
    {
        string filePath = GetSaveFilePath(nowSlot);
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            nowPlayer = JsonUtility.FromJson<PlayerData>(jsonData);
            GameManager.Instance.pos = nowPlayer.LastPos;
        }
        else
        {
            Debug.LogWarning($"Save file not found: {filePath}");
        }
    }
    public PlayerData LoadData(int slotIndex)
    {
        string filePath = GetSaveFilePath(slotIndex);
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
    public void DeletSave(int slotIndex)
    {
        string filePath = GetSaveFilePath(slotIndex);
        if(File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        else
        {
            Debug.LogError($"{filePath} {savePath}�ش� ��ο� ������ �����ϴ�.");
        }
    }
}