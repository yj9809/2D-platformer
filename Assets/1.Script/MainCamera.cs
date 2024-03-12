using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField] private Player p;

    // Update is called once per frame
    void Update()
    {
        float clampX = Mathf.Clamp(p.transform.position.x, -21f, 21f);
        float clampY = Mathf.Clamp(p.transform.position.y, -9.3f, 7.3f);

        transform.position = new Vector3(clampX, clampY, -10f);
    }
}
