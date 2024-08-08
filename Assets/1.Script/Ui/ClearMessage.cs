using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ClearMessage : MonoBehaviour
{
    [SerializeField] private GameObject clearMessage;

    [SerializeField] private GameObject managers;
    // Start is called before the first frame update
    void Start()
    {
        managers = GameObject.Find("Managers");
        clearMessage.transform.DOScale(1, 2f).OnComplete(() => StartCoroutine("BackMainScene"));
    }
    IEnumerable BackMainScene()
    {
        yield return new WaitForSeconds(5f);

        
        Destroy(managers);

        LodingSceneController.LoadScene("Main");
    }
}
