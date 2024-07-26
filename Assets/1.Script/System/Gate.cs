using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Gate : MonoBehaviour
{
    [SerializeField] private Image entrance;
    [SerializeField] private string nextScene;
    [SerializeField] private Transform spawonPos;
    [SerializeField] private GameObject boss;

    private GameManager gm;
    private void Start()
    {
        gm = GameManager.Instance;
    }
    private void Update()
    {
        if (entrance.gameObject.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (nextScene == "BossRoom (Stage 1)")
                {
                    gm.OnBossSceneLode(nextScene);
                    gm.boss = this.boss;
                    gm.pos = spawonPos.localPosition;
                    gm.P.Save();
                }
                else if (nextScene == "Game (Stage 1)")
                {
                    gm.OnGameSceneLoad(nextScene);
                    gm.pos = spawonPos.localPosition;
                    gm.P.Save();
                }
                else if(nextScene == "BossRoom (Stage 2)")
                {
                    gm.OnBossSceneLode(nextScene);
                    gm.boss = this.boss;
                    gm.pos = spawonPos.localPosition;
                    gm.P.Save();
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.GetComponent<Player>())
        {
            entrance.transform.gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.GetComponent<Player>())
        {
            entrance.transform.gameObject.SetActive(false);
        }
    }
}
