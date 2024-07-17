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
        if(nextScene == "Game (Stage 2)")
        {
            gm.OnGameSceneLode(nextScene);
            gm.pos = spwanPos.localPosition;
            UiManager.Instance.menu.OnSave();
        }
    }
}
