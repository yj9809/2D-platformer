using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleBoss : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }
    public override void Init()
    {
        speed = 2;
        hp = 30;
        type = Type.Boss;
        attackDis = 4;

        base.Init();
    }
}
