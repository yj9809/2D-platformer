using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class UiManager : Singleton<UiManager>
{
    private Image hp;

    public GameObject load;
    public GameObject state;


    // Start is called before the first frame update
    void Start()
    {
        SceneManager.activeSceneChanged += ActiveSceneChanged;
    }

    private void ActiveSceneChanged(Scene arg0, Scene arg1)
    {
        string sceneName = GameManager.Instance.scene.name;
        if (sceneName == "Main")
        {
            bool isLoad = false;

            for (int i = 0; i < 3; i++)
            {
                if (File.Exists(DataManager.Instance.path + $"{i}"))
                {
                    isLoad = true;
                }
            }
            load.SetActive(isLoad);
        }
        else if (sceneName != "Main" && sceneName != "Loding")
        {
            GameObject canvas = GameObject.Find("Ui Canvas");
            if (canvas != null)
            {
                Instantiate(state, canvas.transform);
                hp = canvas.transform.GetChild(0).
                    transform.GetChild(0).
                    transform.GetChild(0).
                    GetComponent<Image>();
            }
        }
    }
    public void SetHpImg()
    {
        Player p = GameManager.Instance.P;
        hp.fillAmount = p.SetHp / p.MaxHP;
    }
}
