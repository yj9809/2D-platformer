using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum CameraType
{
    Nomal,
    Door,
    Non
}
public class MainCamera : MonoBehaviour
{
    private Player p;
    private GameManager gm;
    private Map map;
    private PlayerData data;
    public CameraType cT = CameraType.Nomal;

    private void Start()
    {
        p = GameManager.Instance.P;
        gm = GameManager.Instance;
        map = FindObjectOfType<Map>();
        data = DataManager.Instance.NowPlayer;
    }
    // Update is called once per frame
    void Update()
    {
        if (cT == CameraType.Nomal)
            CameraMove();
        else if(cT == CameraType.Door)
            DoorOpenCamera();
    }
    private void CameraMove()
    {
        if (gm.scene.name == "Game (Stage 1)")
        {
            float clampX = Mathf.Clamp(p.transform.position.x, -21f, 21f);
            float clampY = Mathf.Clamp(p.transform.position.y, -9.3f, 9.4f);

            transform.position = new Vector3(clampX, clampY, -10f);
        }
        else if(gm.scene.name == "Game (Stage 2)")
        {
            float clampX = Mathf.Clamp(p.transform.position.x, -36.5f, 67f);
            float clampY = Mathf.Clamp(p.transform.position.y, -9.3f, 9.4f);

            transform.position = new Vector3(clampX, clampY, -10f);
        }
        else if (gm.scene.name == "BossRoom (Stage 1)")
        {
            float clampX = Mathf.Clamp(p.transform.position.x, -8f, 10f);
            float clampY = Mathf.Clamp(p.transform.position.y, -4f, 9.4f);

            transform.position = new Vector3(clampX, clampY, -10f);
        }
        else if(gm.scene.name == "BossRoom (Stage 2)")
        {
            float clampX = Mathf.Clamp(p.transform.position.x, -8f, 10f);
            float clampY = Mathf.Clamp(p.transform.position.y, -7f, 9.4f);

            transform.position = new Vector3(clampX, clampY, -10f);
        }
        else if(gm.scene.name == "Tutorial Scenes")
        {
            float clampX = Mathf.Clamp(p.transform.position.x, -18f, 3.6f);
            float clampY = Mathf.Clamp(p.transform.position.y, -10f, -6f);

            transform.position = new Vector3(clampX, clampY, -10f);
        }
    }

    private void DoorOpenCamera()
    {
        gm.GameType = GameType.Stop;
        transform.DOMove(new Vector3(-9, 9.8f, -10), 2f).OnComplete(() =>
        {
            map.DoorOpen();
        }
        );
        cT = CameraType.Non;
    }

    public void EndDoorOpenCamera()
    {
        transform.DOMove(new Vector3(p.transform.position.x, p.transform.position.y, -10), 2f).OnComplete(() => 
        {
            cT = CameraType.Nomal;
            gm.GameType = GameType.Start;
        });
    }
}
