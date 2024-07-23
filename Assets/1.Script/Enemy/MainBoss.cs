using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBoss : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        Init();
        SetBossHp();
    }
    protected override void Init()
    {
        speed = 5f;
        hp = 50f;
        type = Type.Boss;
        attackDis = 1.8f;
        damage = 8;
        main = true;

        base.Init();
    }
}
