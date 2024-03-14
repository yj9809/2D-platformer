using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Gate : MonoBehaviour
{
    [SerializeField] private Image entrance;

    private void Update()
    {
        if (entrance.enabled)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("입장합니다");
                GameManager.Instance.OnMiddleBossSceneLode();

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
