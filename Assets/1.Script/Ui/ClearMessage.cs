using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ClearMessage : MonoBehaviour
{
    [SerializeField] private GameObject clearMessage;
    // Start is called before the first frame update
    void Start()
    {
        clearMessage.transform.DOScale(1, 2f).OnComplete(() => StartCoroutine(BackMainScene()));
    }
    IEnumerator BackMainScene()
    {
        yield return new WaitForSeconds(5f);
        LodingSceneController.LoadScene("Main");
    }
}
