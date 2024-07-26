using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooling : Singleton<Pooling>
{
    [SerializeField] private GameObject pHit;
    [SerializeField] private GameObject eHit;
    [SerializeField] private GameObject spwanEffect;
    [SerializeField] private GameObject item;
    [SerializeField] private GameObject dashEffect;
    [SerializeField] private GameObject[] magic;

    [SerializeField] private Queue<GameObject> poolPHit = new Queue<GameObject>();
    [SerializeField] private Queue<GameObject> poolEHit = new Queue<GameObject>();
    [SerializeField] private Queue<GameObject> poolSpwanEffect = new Queue<GameObject>();
    [SerializeField] private Queue<GameObject> poolItem = new Queue<GameObject>();
    [SerializeField] private Queue<GameObject> poolDashs = new Queue<GameObject>();
    [SerializeField] private List<GameObject> poolMagic = new List<GameObject>();
    private void Start()
    {
        foreach (GameObject item in magic)
        {
            GameObject newMagic = Instantiate(item, transform);
        }
    }
    private GameObject CreatObj(GameObject hit)
    {
        GameObject newObj = Instantiate(hit,transform).gameObject;
        newObj.SetActive(false);
        return newObj;
    }
    public GameObject GetSpwanEffect()
    {
        GameObject effect;

        if (poolSpwanEffect.Count > 0)
            effect = poolSpwanEffect.Dequeue();
        else
            effect = Instantiate(spwanEffect);

        effect.SetActive(true);
        return effect;
    }
    public GameObject GetItems()
    {
        GameObject item;

        if (poolItem.Count > 0)
            item = poolItem.Dequeue();
        else
            item = Instantiate(this.item);

        item.SetActive(true);
        return item;
    }
    public GameObject GetDash()
    {
        GameObject dash;

        if (poolDashs.Count > 0)
        { 
            dash = poolDashs.Dequeue();
            dash.transform.SetParent(null);
            dash.SetActive(true);
        }
        else
        {
            dash = Instantiate(dashEffect);
            dash.transform.SetParent(null);
            dash.SetActive(true);
        }
        return dash;
    }
    public GameObject GetObj(bool isPHit)
    {
        if (isPHit)
            return GetObjFromPool(poolPHit);
        else
            return GetObjFromPool(poolEHit);
    }
    private GameObject GetObjFromPool(Queue<GameObject> pool)
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.transform.SetParent(null);
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject newObj = CreatObj(pool == poolPHit ? pHit : eHit);
            newObj.transform.SetParent(null);
            newObj.SetActive(true);
            return newObj;
        }
    }
    public void ReturnObj(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(transform);

        if (obj.CompareTag("PHit"))
            poolPHit.Enqueue(obj);
        else if (obj.CompareTag("Ehit"))
            poolEHit.Enqueue(obj);
        else if (obj.CompareTag("SpwanEffect"))
            poolSpwanEffect.Enqueue(obj);
        else if (obj.CompareTag("Item"))
            poolItem.Enqueue(obj);
        else if (obj.CompareTag("Dash"))
            poolDashs.Enqueue(obj);
    }
}
