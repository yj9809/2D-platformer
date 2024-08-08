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
    [SerializeField] private float typingSpeed = 0.05f;  // Ÿ���� �ӵ�
    [SerializeField] private float closeDelay = 3f;  // ��ȭ ���� �� ��� �ð�
    [SerializeField] private float delayBeforeRepeat = 5f; // ��ȭ �ݺ� �� ��� �ð�

    [SerializeField] private EndMessage end;
    private Transform player;
    private bool isTyping = false;  // Ÿ���� ������ ����
    private bool canStartDialogue = true; // ��ȭ ���� ���� ����
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
                OpenDialogue("���� ���°� �������״�, \n���� �Ҿ���� ������ �η縶���� ã�� ���� ������ �� �־� ?");
            else
            {
                OpenDialogue("������ �η縶���� ã�ƿԱ���! \n���� �ʸ� ������ �����ٰ�!");
                
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

        // ��ȭ ���� �� ���� �ð� ���
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