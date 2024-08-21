using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Map : MonoBehaviour
{
    [SerializeField] private string nextScene;
    [SerializeField] private Transform spwanPos;
    [SerializeField] private GameObject door;

    private Vector2 doorPos = new Vector2(-8.3f, 20);

    private GameManager gm;
    private MainCamera main;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
        main = FindObjectOfType<MainCamera>();
        if(DataManager.Instance.NowPlayer.Door && gm.scene.name == "Game (Stage 1)")
        {
            door.transform.position = doorPos;
        }
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

    public void DoorOpen()
    {
        door.transform.DOMoveY(20, 3f).OnComplete(() =>
        {
            main.EndDoorOpenCamera();
            DataManager.Instance.NowPlayer.Door = true;
        }
        );
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>())
        {
            MoveMap();
        }
    }
}
