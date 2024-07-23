using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }
    protected override void Init()
    {
        damage = 4;
        speed = 4;
        hp = 8;
        type = Type.Frog;

        base.Init();
    }
}
