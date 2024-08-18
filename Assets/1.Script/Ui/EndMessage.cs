using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndMessage : MonoBehaviour
{
    private bool isEnd = false;

    private void Start()
    {
        UiManager.Instance.SetEnd(this);
        gameObject.SetActive(false);
    }
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
