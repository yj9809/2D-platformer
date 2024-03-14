using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject player;

    private Player p;
    public Player P
    {
        get
        {
            if (p == null)
                p = FindObjectOfType<Player>();

            return p;
        }
    }
    public void OnMiddleBossSceneLode()
    {
        SceneManager.LoadScene("MiddleBossRoom");
        Instantiate(player, new Vector2(-11, -5.8f), Quaternion.identity);
    }
    public void OnGameSceneLode()
    {
        SceneManager.LoadScene("Game");
    }
}
