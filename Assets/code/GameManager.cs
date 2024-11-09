using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public GameObject justTalk;
    public GameObject choiceTalk;
    public Text justTalkText;
    public Text choiceTalkText;
    public Button up;
    public Button down;
    public Button next;
    public Button exit;


    public GameObject apple; // �ٴ� ���� �̹���
    public float jumpHeight = 30f; // �ٴ� ����
    public float jumpSpeed = 0.5f; // �ٴ� �ӵ�

    int firstCard;
    int secondCard;
    int dialogueIndex = 0;
    bool isChoiceMode = false;
    bool isHigherSelected = false;

   string[] dialogues = {
    "���� ���� & �ο� ���� ����!",
    "���߸� �� ������ �̻縦 �ð� ť��~",
    "���� ������. ���� ���� ī�带 ������� �� ���� ���� �ž�!",
    "�� ��° ī���� ���ڰ� ù ��° ī���� ���ں���\nū��, ������ ���߸� ��!",
    "�׷� ���� ���� ù ��° ī�带 ������! ť��",
    "(ī�带 �̴� ��)",
    "(ī�带 �� �� �̾Ҵ�!)",
    };

    void Awake()
    {
        instance = this;
        justTalk.SetActive(true);
        choiceTalk.SetActive(false);
        exit.gameObject.SetActive(false);

        next.onClick.AddListener(OnNextDialogue);
        up.onClick.AddListener(OnUpSelected);
        down.onClick.AddListener(OnDownSelected);
        exit.onClick.AddListener(OnExitSelected);

        StartCoroutine(StartDialogue());
    }

    IEnumerator JumpAnimation()
    {
        Vector2 startPos = new Vector2(115.33f, 17f);

        for(int i = 0; i<2; i++)
        {
            // ���� �̵�
            apple.GetComponent<RectTransform>().anchoredPosition = startPos + new Vector2(0, jumpHeight);
            yield return new WaitForSeconds(jumpSpeed);

            // �Ʒ��� �̵�
            apple.GetComponent<RectTransform>().anchoredPosition = startPos;
            yield return new WaitForSeconds(jumpSpeed);
        }


        

    }


    IEnumerator StartDialogue()
    {
        next.gameObject.SetActive(false);  // ��� ���� ��ư Ȱ��ȭ

        for (dialogueIndex = 0; dialogueIndex<dialogues.Length; dialogueIndex++)
        {
            //justTalkText.text = dialogues[dialogueIndex];
            yield return StartCoroutine(DisplayTextOneByOne(dialogues[dialogueIndex])); // �� ���ھ� ���
            next.gameObject.SetActive(true);  // ��� ���� ��ư Ȱ��ȭ

            yield return new WaitUntil(() => !next.gameObject.activeSelf);
        }

        firstCard = Random.Range(1, 10);
        secondCard = Random.Range(1, 10);


        if(firstCard == 2 || firstCard == 4 || firstCard == 5 || firstCard == 9)
        {
            justTalkText.text = $"ù ��° ī���� ���ڴ� {firstCard}�� ���Ծ�!";
            yield return StartCoroutine(DisplayTextOneByOne(justTalkText.text));
            next.gameObject.SetActive(true);  // �ٽ� ��ư Ȱ��ȭ
            yield return new WaitUntil(() => !next.gameObject.activeSelf);
        }

        else
        {
            justTalkText.text = $"ù ��° ī���� ���ڴ� {firstCard}�� ���Ծ�!";
            yield return StartCoroutine(DisplayTextOneByOne(justTalkText.text));
            next.gameObject.SetActive(true);  // �ٽ� ��ư Ȱ��ȭ
            yield return new WaitUntil(() => !next.gameObject.activeSelf);
        }

        


        justTalk.SetActive(false);
        choiceTalk.SetActive(true);

       
        
        choiceTalkText.text = "�� ��° ī���� ���ڴ� Ŭ��? ������!ť��";
        yield return StartCoroutine(DisplayText(choiceTalkText.text));

        isChoiceMode = true;

    }

    IEnumerator DisplayTextOneByOne(string text)
    {
        justTalkText.text = "";
        foreach (char letter in text)
        {
            justTalkText.text += letter;
            yield return new WaitForSeconds(0.05f);  // �� ���� ������ ������
        }

    }

    IEnumerator DisplayText(string text) {
        choiceTalkText.text = "";
        foreach (char letter in text)
        {
            choiceTalkText.text += letter;
            yield return new WaitForSeconds(0.05f);  // �� ���� ������ ������
        }
    }
    public void OnNextDialogue()
    {
        next.gameObject.SetActive(false);
        StartCoroutine(JumpAnimation());
    }

    public void OnUpSelected()
    {
        if (!isChoiceMode) return;
        isHigherSelected = true;
        CheckPlayerChoice();
    }
    public void OnDownSelected()
    {
        if (!isChoiceMode) return;
        isHigherSelected = false;
        CheckPlayerChoice();
    }

    public void OnExitSelected()
    {
        Application.Quit();
    }



    void CheckPlayerChoice()
    {

        string result;

        if ((isHigherSelected && secondCard >= firstCard) || (!isHigherSelected && secondCard <= firstCard))
            result = $"�ι�° ī��� {secondCard} ����! �������� ���� �� ������ �̻�ð�! ť��";
        else
            result = $"�ι�° ī��� {secondCard} ����... ������ �ٽ� ��ð�! ť��";


        choiceTalk.SetActive(false);
        justTalk.SetActive(true);
        justTalkText.text = result;
        StartCoroutine(DisplayTextOneByOne(justTalkText.text)); //�Ƿ���??

        isChoiceMode = false;

        exit.gameObject.SetActive(true); // exit ��ư ǥ��

    }
}
