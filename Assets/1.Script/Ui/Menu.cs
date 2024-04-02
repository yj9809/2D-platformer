using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private Image loding;
    private UiManager ui;
    
    // Start is called before the first frame update
    void Start()
    {
        ui = UiManager.Instance;
        ui.SetMenu(this);
    }

    // Update is called once per frame
    void Update()
    {
        ui.OnMenu(this.transform);
    }
    public void OnSave()
    {
        loding.gameObject.SetActive(true);
        GameManager.Instance.P.Save();
        Invoke("Loding", 1f);
    }
    public void OnLoad()
    {
        ui.OnLoadMenu(this.transform);
    }
    public void OnExit()
    {
        ui.OnExit();
    }
    private void Loding()
    {
        loding.gameObject.SetActive(false);
    }
}
