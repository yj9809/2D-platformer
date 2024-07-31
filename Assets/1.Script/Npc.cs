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
    [SerializeField] private float typingSpeed = 0.05f;  // Ÿ���� �ӵ�
    [SerializeField] private float closeDelay = 3f;  // ��ȭ ���� �� ��� �ð�

    private Transform player;
    private bool isTyping = false;  // Ÿ���� ������ ����
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
                    OpenDialogue("���� ���� �ʹٸ�, \n���� �Ҿ���� ������ �η縶���� ã�� ���� ������.");
                else
                    OpenDialogue("������ �η縶���� ã�ƿԱ���! \n���� �ʸ� ������ �����ٰ�!");
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
