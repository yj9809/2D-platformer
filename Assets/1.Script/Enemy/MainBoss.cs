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
        speed = 4f;
        hp = 75f;
        type = Type.Boss;
        attackDis = 2f;
        damage = 13;
        main = true;

        base.Init();
    }
}
