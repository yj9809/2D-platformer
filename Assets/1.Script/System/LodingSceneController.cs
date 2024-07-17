using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LodingSceneController : MonoBehaviour
{
    private GameManager gm;
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
        gm = GameManager.Instance;
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
                SceneManager.LoadScene("Ui", LoadSceneMode.Additive);
                op.allowSceneActivation = true;
            }
        }
    }
    
}
