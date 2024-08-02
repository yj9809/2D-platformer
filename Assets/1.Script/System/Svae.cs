using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Svae : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>())
        {
            Debug.Log("save");
            collision.GetComponent<Player>().Save();
        }
    }
}
