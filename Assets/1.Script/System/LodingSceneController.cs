using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LodingSceneController : MonoBehaviour
{
    private static string nextScene;

    [SerializeField] private Image lodingBar;

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("Loding");
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LodingProcess());
    }
    IEnumerator LodingProcess()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        while(!op.isDone)
        {
            yield return null;

            if (op.progress >= 0.9f)
            {
                yield return new WaitForSeconds(0.5f);
                op.allowSceneActivation = true;
                SceneManager.LoadScene("Ui", LoadSceneMode.Additive);
            }
        }
    }
}
