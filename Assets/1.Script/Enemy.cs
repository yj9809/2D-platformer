using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rigid;

    [SerializeField] private int direction;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        direction = Random.Range(-1, 2);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    void Move()
    {
        rigid.velocity = new Vector2(direction, rigid.velocity.y);
    }

}
