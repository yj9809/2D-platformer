using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooling : Singleton<Pooling>
{
    [SerializeField] private GameObject pHit;
    [SerializeField] private GameObject eHit;

    [SerializeField] private Queue<GameObject> poolPHit = new Queue<GameObject>();
    [SerializeField] private Queue<GameObject> poolEHit = new Queue<GameObject>();
    void Awake()
    {
        Initialize(2, pHit, poolPHit);
        Initialize(2, eHit, poolEHit);
    }
    private GameObject CreatObj(GameObject hit)
    {
        GameObject newObj = Instantiate(hit,transform).gameObject;
        newObj.SetActive(false);
        return newObj;
    }
    private void Initialize(int count, GameObject hit, Queue<GameObject> pool)
    {
        for (int i = 0; i < count; i++)
        {
            pool.Equals(CreatObj(hit));
        }
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
            Debug.LogWarning("Pool is empty, creating new object");
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
        {
            poolPHit.Enqueue(obj);
        }
        else if (obj.CompareTag("Ehit"))
        {
            poolEHit.Enqueue(obj);
        }
    }
}
