using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject gate;

    public GameObject boss;
    public GameObject[] hit;
    public Scene scene;

    public Vector2 pos;

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
    private void Start()
    {
        scene = SceneManager.GetActiveScene();
        SceneManager.activeSceneChanged += UpdateActiveScene;
    }
    private void UpdateActiveScene(Scene previousScene, Scene newScene)
    {
        scene = newScene;
    }
    public void OnMiddleBossSceneLode()
    {
        LodingSceneController.LoadScene("BossRoom (Stage 1)");
        Invoke("SpawnBoss", 3f);
    }
    public void OnGameSceneLode()
    {
        LodingSceneController.LoadScene("Game (Stage 1)");
    }
    public void SpawnBoss()
    {
        Vector2 pos = new Vector2(1.5f, -3.5f);
        Debug.Log(boss.name);
        Instantiate(boss, pos, Quaternion.identity);
    }
}
