using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("ReturnHit", 0.75f);
    }
    private void ReturnHit()
    {
        Pooling.Instance.ReturnObj(this.gameObject);
    }
}
