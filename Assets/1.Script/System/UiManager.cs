using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class UiManager : Singleton<UiManager>
{
    public GameObject load;
    public GameObject hp;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.scene.name == "Main")
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
    }
}
