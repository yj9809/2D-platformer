using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private Player p;
    private void Start()
    {
        p = GameManager.Instance.P;
    }
    public void Jump()
    {
        p.JumpButton();
    }
}
