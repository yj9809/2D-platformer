using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MossyMiddleBoss : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        Init();
        SetBossHp();
    }
    protected override void Init()
    {
        damage = 10;
        hp = 70f;
        type = Type.Boss;
        attackDis = 5f;
        speed = 3f;

        base.Init();
    }
}
