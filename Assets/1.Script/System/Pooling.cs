using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Pooling : Singleton<Pooling>
{
    [BoxGroup("Skill")] [SerializeField] private GameObject[] magic;
    [BoxGroup("Skill")] [SerializeField] private List<GameObject> poolMagic = new List<GameObject>();

    [FoldoutGroup("PoolObj")] [SerializeField] private GameObject pHit;
    [FoldoutGroup("PoolObj")] [SerializeField] private GameObject eHit;
    [FoldoutGroup("PoolObj")] [SerializeField] private GameObject spwanEffect;
    [FoldoutGroup("PoolObj")] [SerializeField] private GameObject item;
    [FoldoutGroup("PoolObj")] [SerializeField] private GameObject dashEffect;

    private Queue<GameObject> poolPHit = new Queue<GameObject>();
    private Queue<GameObject> poolEHit = new Queue<GameObject>();
    private Queue<GameObject> poolSpwanEffect = new Queue<GameObject>();
    private Queue<GameObject> poolItem = new Queue<GameObject>();
    private Queue<GameObject> poolDashs = new Queue<GameObject>();
    private void Start()
    {
        foreach (GameObject item in magic)
        {
            GameObject newMagic = Instantiate(item, transform);
            newMagic.SetActive(false);
            poolMagic.Add(newMagic);
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
    public GameObject GetMagic()
    {
        GameObject magic;

        if(poolMagic.Count > 0)
        {
            int randomNum = Random.Range(0, poolMagic.Count);
            magic = poolMagic[randomNum];
            magic.transform.SetParent(null);
            magic.SetActive(true);
            poolMagic.RemoveAt(randomNum);
        }
        else
        {
            int randomNum = Random.Range(0, this.magic.Length);
            magic = Instantiate(this.magic[randomNum]);
            magic.SetActive(true);
        }

        return magic;
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
        else if (obj.CompareTag("Magic"))
        {
            obj.GetComponent<Collider2D>().enabled = false;
            poolMagic.Add(obj);
        }
    }
}
