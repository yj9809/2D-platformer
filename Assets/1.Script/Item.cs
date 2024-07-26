using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum ItemType
{
    NomalItem,
    BossItem
}
public class Item : MonoBehaviour
{
    private float bounceForce = 300f;
    [Title("Itme Type")]
    [EnumToggleButtons] [SerializeField] private ItemType itemType;
    Rigidbody2D rd;
    private void OnEnable()
    {
        Drop();
    }
    private void Start()
    {
        rd = GetComponent<Rigidbody2D>();
        Drop();
    }
    private void Drop()
    {
        if (rd != null)
        {
            rd.AddForce(new Vector2(Random.Range(-50f, 50f), bounceForce));
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.GetComponent<Player>())
        {
            if(itemType == ItemType.NomalItem)
            { 
                GameManager.Instance.P.Coin += 5;
                Pooling.Instance.ReturnObj(this.gameObject);
            }
            else
            {

            }
        }

    }
}
