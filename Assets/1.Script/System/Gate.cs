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
            GateEnter();
        }
    }
    private void GateEnter()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            switch (nextScene)
            {
                case "BossRoom (Stage 1)":
                    gm.OnBossSceneLode(nextScene);
                    gm.BossPrefab = boss;
                    gm.pos = spawonPos != null ? spawonPos.localPosition : gm.P.LastPos;
                    gm.P.Save();
                    break;

                case "Game (Stage 1)":
                    gm.OnGameSceneLoad(nextScene);
                    gm.pos = spawonPos != null ? spawonPos.localPosition : gm.P.LastPos;
                    gm.P.Save();
                    break;

                case "Game (Stage 2)":
                    gm.OnGameSceneLoad(nextScene);
                    gm.pos = spawonPos != null ? spawonPos.localPosition : gm.P.LastPos;
                    gm.P.Save();
                    break;

                case "BossRoom (Stage 2)":
                    gm.OnBossSceneLode(nextScene);
                    gm.BossPrefab = boss;
                    gm.pos = spawonPos != null ? spawonPos.localPosition : gm.P.LastPos;
                    gm.P.Save();
                    break;
            }
        }
    }
    public void GateEnterButton()
    {
        switch (nextScene)
        {
            case "BossRoom (Stage 1)":
                gm.OnBossSceneLode(nextScene);
                gm.BossPrefab = boss;
                gm.pos = spawonPos != null ? spawonPos.localPosition : gm.P.LastPos;
                gm.P.Save();
                break;

            case "Game (Stage 1)":
                gm.OnGameSceneLoad(nextScene);
                gm.pos = spawonPos != null ? spawonPos.localPosition : gm.P.LastPos;
                gm.P.Save();
                break;

            case "Game (Stage 2)":
                gm.OnGameSceneLoad(nextScene);
                gm.pos = spawonPos != null ? spawonPos.localPosition : gm.P.LastPos;
                gm.P.Save();
                break;

            case "BossRoom (Stage 2)":
                gm.OnBossSceneLode(nextScene);
                gm.BossPrefab = boss;
                gm.pos = spawonPos != null ? spawonPos.localPosition : gm.P.LastPos;
                gm.P.Save();
                break;
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
