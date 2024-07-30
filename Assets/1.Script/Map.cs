using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] private string nextScene;
    [SerializeField] private Transform spwanPos;

    private GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
    }
    public void MoveMap()
    {
        switch(nextScene)
        {
            case "Game (Stage 1)":
                gm.OnGameSceneLoad(nextScene);
                gm.pos = spwanPos.localPosition;
                gm.P.Save();
                break;
            case "Game (Stage 2)":
                gm.OnGameSceneLoad(nextScene);
                gm.pos = spwanPos.localPosition;
                gm.P.Save();
                break;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>())
        {
            MoveMap();
        }
    }
}
