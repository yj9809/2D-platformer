using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleBoss : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        Init();
        SetBossHp();
    }
    protected override void Init()
    {
        speed = 3.5f;
        hp = 45;
        type = Type.Boss;
        attackDis = 2.5f;
        damage = 5;
        middle = true;

        base.Init();
    }
}
