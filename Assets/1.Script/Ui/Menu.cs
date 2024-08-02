using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private Image loding;
    private GameObject loadMenu;
    private UiManager ui;
    
    // Start is called before the first frame update
    void Start()
    {
        ui = UiManager.Instance;
        ui.SetMenu(this);
        loadMenu = ui.OnLoadMenu(this.transform);
    }
    public void OnMenuBordButton()
    {
        ui.OnMenuBordButton();
    }
    public void OnSave()
    {
        loding.gameObject.SetActive(true);
        GameManager.Instance.P.Save();
        Invoke("Loding", 1f);
    }
    public void OnLoad()
    {
        if(loadMenu == null)
            Debug.LogError("loadMenu에 아무것도 없어");

        loadMenu.SetActive(true);
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
