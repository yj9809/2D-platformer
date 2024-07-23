using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchMagic : MonoBehaviour
{
    [SerializeField] private Collider2D coll;
    private void Start()
    {
        coll = transform.GetComponent<Collider2D>();
    }
    private void OnCollider()
    {
        coll.enabled = true;
    }
}
