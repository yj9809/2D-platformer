using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MossyMainBoss : Enemy
{
    private void Start()
    {
        Init();
        SetBossHp();
    }
    protected override void Init()
    {
        damage = 15;
        hp = 100f;
        type = Type.Boss;
        attackDis = 2f;
        speed = 5f;
        mossyMain = true;

        base.Init();
    }
}
