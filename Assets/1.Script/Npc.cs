using System.Collections;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class Npc : MonoBehaviour
{
    [SerializeField] private GameObject chatWindow;
    [SerializeField] private TMP_Text chatTxt;
    [SerializeField] private float distance = 3f;
    [SerializeField] private float windowDistance = 5f;
    [SerializeField] private float typingSpeed = 0.05f;  // 타이핑 속도
    [SerializeField] private float closeDelay = 3f;  // 대화 종료 후 대기 시간
    [SerializeField] private float delayBeforeRepeat = 5f; // 대화 반복 전 대기 시간

    [SerializeField] private EndMessage end;
    private Transform player;
    private bool isTyping = false;  // 타이핑 중인지 여부
    private bool canStartDialogue = true; // 대화 시작 가능 여부
    private Coroutine typingCoroutine;
    private Coroutine closeCoroutine;
    private Coroutine repeatDialogueCoroutine;

    private bool isEnd = false;
    private void Start()
    {
        player = GameManager.Instance.P.transform;
        end = FindObjectOfType<EndMessage>();
        end.gameObject.SetActive(false);
        chatWindow.SetActive(false);
        FloatOn();
    }

    private void Update()
    {
        float dis = Vector3.Distance(player.position, transform.position);

        if (dis <= distance && !isTyping && canStartDialogue)
        {
            if (!DataManager.Instance.NowPlayer.Scroll)
                OpenDialogue("집에 가는걸 도와줄테니, \n내가 잃어버린 마법의 두루마리를 찾는 것을 도와줄 수 있어 ?");
            else
            {
                OpenDialogue("마법의 두루마리를 찾아왔구나! \n이제 너를 집으로 보내줄게!");
                
                if(!isEnd)
                {
                    end.EndWindow();
                }
            }
        }
        else
        {
            if (dis > windowDistance && chatWindow.activeSelf)
            {
                CloseDialogue();
            }
        }
    }

    private void FloatOn()
    {
        if(transform != null)
        {
            transform.DOMoveY(5.5f, 2f)
            .OnComplete(() =>
            {
                transform.DOMoveY(5f, 2f)
                .OnComplete(() => FloatOn());
            });
        }
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

        // 대화 종료 후 일정 시간 대기
        if (repeatDialogueCoroutine != null)
        {
            StopCoroutine(repeatDialogueCoroutine);
        }

        repeatDialogueCoroutine = StartCoroutine(WaitBeforeRepeatDialogue());
    }

    private IEnumerator WaitBeforeRepeatDialogue()
    {
        canStartDialogue = false;
        yield return new WaitForSeconds(delayBeforeRepeat);
        canStartDialogue = true;
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