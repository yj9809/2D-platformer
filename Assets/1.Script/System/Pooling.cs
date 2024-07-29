using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Pooling : Singleton<Pooling>
{
    [BoxGroup("Skill")] [SerializeField] private GameObject[] magic;
    [BoxGroup("Skill")] [SerializeField] private GameObject[] mainBossMagic;
    [BoxGroup("Skill")] [SerializeField] private GameObject[] playerMagic;
    [BoxGroup("Skill")] [SerializeField] private List<GameObject> poolMagic = new List<GameObject>();
    [BoxGroup("Skill")] [SerializeField] private List<GameObject> poolMainBossMagic = new List<GameObject>();
    [BoxGroup("Skill")] [SerializeField] private List<GameObject> poolPlayerMagic = new List<GameObject>();

    [FoldoutGroup("PoolObj")] [SerializeField] private GameObject pHit;
    [FoldoutGroup("PoolObj")] [SerializeField] private GameObject eHit;
    [FoldoutGroup("PoolObj")] [SerializeField] private GameObject spwanEffect;
    [FoldoutGroup("PoolObj")] [SerializeField] private GameObject item;
    [FoldoutGroup("PoolObj")] [SerializeField] private GameObject dashEffect;

    private Queue<GameObject> poolPHit = new Queue<GameObject>();
    private Queue<GameObject> poolEHit = new Queue<GameObject>();
    private Queue<GameObject> poolSpawnEffect = new Queue<GameObject>();
    private Queue<GameObject> poolItem = new Queue<GameObject>();
    private Queue<GameObject> poolDashs = new Queue<GameObject>();
    protected override void Awake()
    {
        base.Awake();
        InitializePool(magic, poolMagic);
        InitializePool(mainBossMagic, poolMainBossMagic);
        InitializePool(playerMagic, poolPlayerMagic);
    }
    private void InitializePool(GameObject[] prefabs, List<GameObject> pool)
    {
        foreach (GameObject prefab in prefabs)
        {
            GameObject newObj = Instantiate(prefab, transform);
            newObj.SetActive(false);
            pool.Add(newObj);
        }
    }
    private GameObject CreateObj(GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogError("Prefab is null.");
            return null;
        }
        GameObject newObj = Instantiate(prefab, transform);
        newObj.SetActive(false);
        return newObj;
    }
    public GameObject GetSpwanEffect()
    {
        GameObject effect;

        if (poolSpawnEffect.Count > 0)
            effect = poolSpawnEffect.Dequeue();
        else
            effect = CreateObj(spwanEffect);

        effect.SetActive(true);
        return effect;
    }
    public GameObject GetItems()
    {
        GameObject item;

        if (poolItem.Count > 0)
            item = poolItem.Dequeue();
        else
            item = CreateObj(this.item);

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
            dash = CreateObj(dashEffect);
            dash.transform.SetParent(null);
            dash.SetActive(true);
        }
        return dash;
    }
    public GameObject GetMagic(bool mainBoss)
    {
        List<GameObject> pool = mainBoss ? poolMainBossMagic : poolMagic;
        GameObject[] prefabs = mainBoss ? mainBossMagic : magic;

        if (pool.Count > 0)
        {
            int randomIndex = Random.Range(0, pool.Count);
            GameObject magic = pool[randomIndex];
            pool.RemoveAt(randomIndex);
            magic.transform.SetParent(null);
            magic.SetActive(true);
            return magic;
        }
        else
        {
            int randomIndex = Random.Range(0, prefabs.Length);
            GameObject magic = Instantiate(prefabs[randomIndex]);
            magic.SetActive(true);
            return magic;
        }
    }
    public GameObject GetMagic(Player p)
    {
        GameObject magic;

        if(poolPlayerMagic.Count > 0)
        {
            int randomNum = Random.Range(0, poolPlayerMagic.Count);
            magic = poolPlayerMagic[randomNum];
            magic.transform.SetParent(null);
            magic.SetActive(true);
            poolPlayerMagic.RemoveAt(randomNum);
        }
        else
        {
            int randomNum = Random.Range(0, playerMagic.Length);
            magic = CreateObj(playerMagic[randomNum]);
            magic.SetActive(true);
        }

        return magic;
    }
    public GameObject GetObj(bool isPHit)
    {
        return GetObjFromPool(isPHit ? poolPHit : poolEHit, isPHit ? pHit : eHit);
    }
    private GameObject GetObjFromPool(Queue<GameObject> pool, GameObject prefab)
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
        GameObject newObj = CreateObj(prefab);
        newObj.transform.SetParent(null);
        newObj.SetActive(true);
        return newObj;
    }
}
    public void ReturnObj(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(transform);

        switch (obj.tag)
        {
            case "PHit":
                poolPHit.Enqueue(obj);
                break;
            case "Ehit":
                poolEHit.Enqueue(obj);
                break;
            case "SpawnEffect":
                poolSpawnEffect.Enqueue(obj);
                break;
            case "Item":
                poolItem.Enqueue(obj);
                break;
            case "Dash":
                poolDashs.Enqueue(obj);
                break;
            case "Magic":
                obj.GetComponent<Collider2D>().enabled = false;
                poolMagic.Add(obj);
                break;
            case "MainBossMagic":
                obj.GetComponent<Collider2D>().enabled = false;
                poolMainBossMagic.Add(obj);
                break;
            case "Player Magic":
                obj.GetComponent<Collider2D>().enabled = false;
                poolPlayerMagic.Add(obj);
                break;
        }
    }
}
