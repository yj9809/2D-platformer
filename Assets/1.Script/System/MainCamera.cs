using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    private Player p;
    private GameManager gm;
    private void Start()
    {
        p = GameManager.Instance.P;
        gm = GameManager.Instance;
    }
    // Update is called once per frame
    void Update()
    {
        CameraMove();
    }
    private void CameraMove()
    {
        if (gm.scene.name == "Game (Stage 1)")
        {
            float clampX = Mathf.Clamp(p.transform.position.x, -21f, 21f);
            float clampY = Mathf.Clamp(p.transform.position.y + 2f, -9.3f, 9.4f);

            transform.position = new Vector3(clampX, clampY, -10f);
        }
        else if (gm.scene.name == "BossRoom (Stage 1)")
        {
            float clampX = Mathf.Clamp(p.transform.position.x, -8f, 10f);
            float clampY = Mathf.Clamp(p.transform.position.y + 2f, -1.8f, 9.4f);

            transform.position = new Vector3(clampX, clampY, -10f);
        }
    }
}
