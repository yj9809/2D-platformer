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
    private GameManager gm;
    private PlayerData data;

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
        gm = GameManager.Instance;
        data = DataManager.Instance.NowPlayer;
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
                gm.P.Coin += 15;
                Pooling.Instance.ReturnObj(gameObject);
            }
            else
            {
                switch(gameObject.tag)
                {
                    case "Black Soul":
                        data.BlackSoul = true;
                        Destroy(gameObject);
                        break;
                    case "Wings Shoes":
                        data.WingsShoes = true;
                        Destroy(gameObject);
                        break;
                    case "Skill Book":
                        data.SkillBook = true;
                        Destroy(gameObject);
                        break;
                    case "Scroll":
                        data.Scroll = true;
                        Destroy(gameObject);
                        break;
                }
            }
        }

    }
}
