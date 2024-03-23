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

    private void Update()
    {
        if (entrance.enabled)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (nextScene == "MiddleBossRoom")
                {
                    GameManager.Instance.P.transform.position = spawonPos.localPosition;
                    GameManager.Instance.OnMiddleBossSceneLode();
                }
                if (nextScene == "Game")
                {
                    GameManager.Instance.P.transform.position = spawonPos.localPosition;
                    GameManager.Instance.OnGameSceneLode();
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
