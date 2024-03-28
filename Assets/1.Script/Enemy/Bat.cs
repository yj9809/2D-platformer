using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }
    public override void Init()
    {
        damage = 2;
        speed = 3;
        hp = 4;
        type = Type.Bat;

        base.Init();
    }
}
