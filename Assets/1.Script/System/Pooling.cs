using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooling : Singleton<Pooling>
{
    [SerializeField] private GameObject pHit;
    [SerializeField] private GameObject eHit;
    [SerializeField] private GameObject spwanEffect;
    [SerializeField] private GameObject item;

    [SerializeField] private Queue<GameObject> poolPHit = new Queue<GameObject>();
    [SerializeField] private Queue<GameObject> poolEHit = new Queue<GameObject>();
    [SerializeField] private Queue<GameObject> spwanE = new Queue<GameObject>();
    [SerializeField] private Queue<GameObject> items = new Queue<GameObject>();
    private GameObject CreatObj(GameObject hit)
    {
        GameObject newObj = Instantiate(hit,transform).gameObject;
        newObj.SetActive(false);
        return newObj;
    }
    public GameObject GetSpwanEffect()
    {
        GameObject effect;

        if (spwanE.Count > 0)
            effect = spwanE.Dequeue();
        else
            effect = Instantiate(spwanEffect);

        effect.SetActive(true);
        return effect;
    }
    public GameObject getItems()
    {
        GameObject item;

        if (items.Count > 0)
            item = items.Dequeue();
        else
            item = Instantiate(this.item);

        item.SetActive(true);
        return item;
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
            spwanE.Enqueue(obj);
        else if (obj.CompareTag("Item"))
            items.Enqueue(obj);
    }
}
