using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollison : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(AutoEnable());
    }

    private IEnumerator AutoEnable()
    {
        yield return new WaitForSeconds(0.1f);

        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    void OnAttack()
    {

    }
}
