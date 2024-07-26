using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchMagic : MonoBehaviour
{
    [SerializeField] private Collider2D coll;

    private GameManager gm;
    private void OnEnable()
    {
        Invoke("ReturnMagic", 1.1f);
    }
    private void Start()
    {
        coll = transform.GetComponent<Collider2D>();
        gm = GameManager.Instance;
    }
    private void OnCollider()
    {
        coll.enabled = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>())
        {
            gm.P.OnPlayerDamage(transform.position, 10);
        }
    }
    private void ReturnMagic()
    {
        Pooling.Instance.ReturnObj(gameObject);
    }
}
