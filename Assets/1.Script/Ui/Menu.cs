using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    private UiManager ui;
    // Start is called before the first frame update
    void Start()
    {
        ui = UiManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        ui.OnMenu(this.transform);
    }
    public void OnSave()
    {
        GameManager.Instance.P.Save();
    }
}
