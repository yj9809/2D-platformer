using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Gate : MonoBehaviour
{
    [SerializeField] private TMP_Text txt;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.GetComponent<Player>())
        {
            txt.transform.gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.GetComponent<Player>())
        {
            txt.transform.gameObject.SetActive(false);
        }
    }
}
