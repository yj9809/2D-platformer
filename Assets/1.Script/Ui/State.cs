using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class State : MonoBehaviour
{
    [SerializeField] private TMP_Text[] txt;
    private UiManager ui;
    
    // Start is called before the first frame update
    void Start()
    {
        ui = UiManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        ui.OnStateBord(this.transform);
        ui.OnStateSet(txt[0],txt[1]);
    }
    public void OnPush(int num)
    {
        ui.StatUp(num);
    }
}
