using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public GameObject player;

    private Player p;
    public Player P
    {
        get
        {
            if (p == null)
            {
                if (FindObjectOfType<Player>() == null)
                {
                    Instantiate(player, new Vector3(-24, -9, 0), Quaternion.identity);
                }
               
                p = FindObjectOfType<Player>();
                DontDestroyOnLoad(p);
            }
            return p;
        }
    }
    public void OnMiddleBossSceneLode()
    {
        SceneManager.LoadScene("MiddleBossRoom");
    }
    public void OnGameSceneLode()
    {
        SceneManager.LoadScene("Game");
    }
}
