using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Sirenix.OdinInspector;

public enum GameType
{
    Stop,
    Start,
    Clear
}
public class GameManager : Singleton<GameManager>
{
    [TabGroup("Obj")]
    [TabGroup("Obj")] [SerializeField] private GameObject player;
    [TabGroup("Obj")] [SerializeField] private GameObject[] gate;

    [TabGroup("Boss")] [SerializeField] private GameObject bossPrefab;
    public GameObject BossPrefab
    {
        get { return bossPrefab; }
        set
        {
            bossPrefab = value;
        }
    }

    public Scene scene;
    public Vector2 pos;

    private MainCamera mainCamera;
    public MainCamera MainCamera
    {
        get
        {
            if (mainCamera == null)
                mainCamera = FindObjectOfType<MainCamera>();
            return mainCamera;
        }
    }

    [Title("Game Type")]
    [EnumToggleButtons]
    [SerializeField] private GameType gameType = GameType.Stop;
    public GameType GameType
    {
        get { return gameType; }
        set
        {
            gameType = value;
        }
    }

    private Player p;
    public Player P
    {
        get
        {
            if (scene.name != "Main")
            {
                if (p == null)
                {
                    if (FindObjectOfType<Player>() == null)
                    {
                        Instantiate(player, pos, Quaternion.identity);
                    }

                    p = FindObjectOfType<Player>();
                }
            }
            return p;
        }
    }
    protected override void Awake()
    {
        base.Awake();
        scene = SceneManager.GetActiveScene();
        SceneManager.activeSceneChanged += OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene previousScene, Scene newScene)
    {
        scene = newScene;
        gate = GameObject.FindGameObjectsWithTag("Gate");
    }
    public void OnBossSceneLode(string nextScene)
    {
        LodingSceneController.LoadScene(nextScene);
        Invoke("BossSpwanTrigger", 0.6f);
    }
    public void OnGameSceneLoad(string nextScene)
    {
        if (nextScene == "Game (Stage 1)")
            LodingSceneController.LoadScene("Game (Stage 1)");
        else if (nextScene == "Game (Stage 2)")
            LodingSceneController.LoadScene("Game (Stage 2)");
        else if (nextScene == "Tutorial Scenes")
            LodingSceneController.LoadScene("Tutorial Scenes");
    }
    public void SpwanBoss()
    {
        Vector2 pos = new Vector2(0.5f, -4.3f);
        GameObject boss = Instantiate(bossPrefab, pos, Quaternion.identity);
        boss.name = bossPrefab.name;
    }
    private void BossSpwanTrigger()
    {
        gameType = GameType.Stop;
        UiManager.Instance.BossCamera();
    }
    public void GateCheck(PlayerData data)
    {
        for (int i = 0; i < gate.Length; i++)
        {
            switch(gate[i].name)
            {
                case "MiddleBossGate":
                    gate[i].SetActive(data.MiddleBoss ? true : false);
                    break;
                case "MainBossGate":
                    gate[i].SetActive(data.MainBoss ? true : false);
                    break;
                case "MossyMiddleBossGate":
                    gate[i].SetActive(data.MossyMiddleBoss ? true : false);
                    break;
                case "MossyMainBossGate":
                    gate[i].SetActive(data.MossyMainBoss ? true : false);
                    break;
            }
        }
    }
}
