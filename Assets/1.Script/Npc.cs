using System.Collections;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class Npc : MonoBehaviour
{
    [SerializeField] private GameObject keyObj;
    [SerializeField] private GameObject chatWindow;
    [SerializeField] private TMP_Text chatTxt;
    [SerializeField] private float distance = 3f;
    [SerializeField] private float windowDistance = 5f;
    [SerializeField] private float typingSpeed = 0.05f;  // 타이핑 속도
    [SerializeField] private float closeDelay = 3f;  // 대화 종료 후 대기 시간

    private Transform player;
    private bool isTyping = false;  // 타이핑 중인지 여부
    private Coroutine typingCoroutine;
    private Coroutine closeCoroutine;

    private void Start()
    {
        player = GameManager.Instance.P.transform;
        keyObj.SetActive(false);
        chatWindow.SetActive(false);
        FloatOn();
    }

    private void Update()
    {
        float dis = Vector3.Distance(player.position, transform.position);

        if (dis <= distance && !isTyping)
        {
            keyObj.SetActive(true);
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (!DataManager.Instance.NowPlayer.Scroll)
                    OpenDialogue("집에 가고 싶다면, \n내가 잃어버린 마법의 두루마리를 찾는 것을 도와줘.");
                else
                    OpenDialogue("마법의 두루마리를 찾아왔구나! \n이제 너를 집으로 보내줄게!");
            }
        }
        else
        {
            keyObj.SetActive(false);
            if (dis > windowDistance && chatWindow.activeSelf)
            {
                CloseDialogue();
            }
        }
    }

    private void FloatOn()
    {
        transform.DOMoveY(5.5f, 2f)
            .OnComplete(() =>
            {
                transform.DOMoveY(5f, 2f)
                .OnComplete(() => FloatOn());
            });
    }

    private void OpenDialogue(string dialogue)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        chatWindow.SetActive(true);
        typingCoroutine = StartCoroutine(TypeDialogue(dialogue));
    }

    private IEnumerator TypeDialogue(string dialogue)
    {
        isTyping = true;
        chatTxt.text = "";
        foreach (char letter in dialogue.ToCharArray())
        {
            chatTxt.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;

        if (closeCoroutine != null)
        {
            StopCoroutine(closeCoroutine);
        }

        closeCoroutine = StartCoroutine(CloseDialogueAfterDelay(closeDelay));
    }

    private IEnumerator CloseDialogueAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        CloseDialogue();
    }

    private void CloseDialogue()
    {
        if (chatWindow.activeSelf)
        {
            chatWindow.SetActive(false);
        }
    }
}
