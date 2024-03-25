using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject player;

    Scene scene;

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
                    //DontDestroyOnLoad(p);
                }
            }
            return p;
        }
    }
    private void Start()
    {
        pos = new Vector2(-24, -9);
        scene = SceneManager.GetActiveScene();
    }
    public void OnMiddleBossSceneLode()
    {
        LodingSceneController.LoadScene("MiddleBossRoom");
    }
    public void OnGameSceneLode()
    {
        LodingSceneController.LoadScene("Game");
    }
}
