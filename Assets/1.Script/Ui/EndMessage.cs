using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndMessage : MonoBehaviour
{
    private bool isEnd = false;

    public void EndWindow()
    {
        if (!isEnd)
        {
            gameObject.SetActive(true);
            isEnd = true;
        }
        else
        {
            gameObject.SetActive(false);
            isEnd = false;
        }
    }
    public void LoadClearScene()
    {
        LodingSceneController.LoadScene("Clear");
    }
}
