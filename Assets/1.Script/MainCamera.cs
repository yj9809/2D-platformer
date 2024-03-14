using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    private Player p;
    private void Start()
    {
        p = GameManager.Instance.P;
    }
    // Update is called once per frame
    void Update()
    {
        float clampX = Mathf.Clamp(p.transform.position.x, -21f, 21f);
        float clampY = Mathf.Clamp(p.transform.position.y + 2f, -9.3f, 9.3f);

        transform.position = new Vector3(clampX, clampY, -10f);
    }
}
