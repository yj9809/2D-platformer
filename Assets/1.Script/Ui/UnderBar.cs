using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnderBar : MonoBehaviour
{
    [SerializeField] private Image image;

    private Image under;
    // Start is called before the first frame update
    void Start()
    {
        under = transform.GetComponent<Image>();
        under.fillAmount = image.fillAmount;
    }

    // Update is called once per frame
    void Update()
    {
        under.fillAmount = Mathf.Lerp(under.fillAmount, image.fillAmount, 3f * Time.deltaTime);   
    }
}
