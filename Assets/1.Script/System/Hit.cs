using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Invoke("ReturnHit", 0.1f);
    }
    private void ReturnHit()
    {
        Pooling.Instance.ReturnObj(this.gameObject);
    }
}
