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


    public GameObject apple; // 뛰는 애플 이미지
    public float jumpHeight = 30f; // 뛰는 높이
    public float jumpSpeed = 0.5f; // 뛰는 속도

    int firstCard;
    int secondCard;
    int dialogueIndex = 0;
    bool isChoiceMode = false;
    bool isHigherSelected = false;

   string[] dialogues = {
    "나랑 하이 & 로우 게임 하자!",
    "맞추면 이 섬으로 이사를 올게 큐릉~",
    "룰은 간단해. 내가 먼저 카드를 순서대로 두 장을 뽑을 거야!",
    "두 번째 카드의 숫자가 첫 번째 카드의 숫자보다\n큰지, 작은지 맞추면 돼!",
    "그럼 내가 먼저 첫 번째 카드를 뽑을께! 큐릉",
    "(카드를 뽑는 중)",
    "(카드를 두 장 뽑았다!)",
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
            // 위로 이동
            apple.GetComponent<RectTransform>().anchoredPosition = startPos + new Vector2(0, jumpHeight);
            yield return new WaitForSeconds(jumpSpeed);

            // 아래로 이동
            apple.GetComponent<RectTransform>().anchoredPosition = startPos;
            yield return new WaitForSeconds(jumpSpeed);
        }


        

    }


    IEnumerator StartDialogue()
    {
        next.gameObject.SetActive(false);  // 대사 진행 버튼 활성화

        for (dialogueIndex = 0; dialogueIndex<dialogues.Length; dialogueIndex++)
        {
            //justTalkText.text = dialogues[dialogueIndex];
            yield return StartCoroutine(DisplayTextOneByOne(dialogues[dialogueIndex])); // 한 글자씩 출력
            next.gameObject.SetActive(true);  // 대사 진행 버튼 활성화

            yield return new WaitUntil(() => !next.gameObject.activeSelf);
        }

        firstCard = Random.Range(1, 10);
        secondCard = Random.Range(1, 10);


        if(firstCard == 2 || firstCard == 4 || firstCard == 5 || firstCard == 9)
        {
            justTalkText.text = $"첫 번째 카드의 숫자는 {firstCard}가 나왔어!";
            yield return StartCoroutine(DisplayTextOneByOne(justTalkText.text));
            next.gameObject.SetActive(true);  // 다시 버튼 활성화
            yield return new WaitUntil(() => !next.gameObject.activeSelf);
        }

        else
        {
            justTalkText.text = $"첫 번째 카드의 숫자는 {firstCard}이 나왔어!";
            yield return StartCoroutine(DisplayTextOneByOne(justTalkText.text));
            next.gameObject.SetActive(true);  // 다시 버튼 활성화
            yield return new WaitUntil(() => !next.gameObject.activeSelf);
        }

        


        justTalk.SetActive(false);
        choiceTalk.SetActive(true);

       
        
        choiceTalkText.text = "두 번째 카드의 숫자는 클까? 작을까!큐릉";
        yield return StartCoroutine(DisplayText(choiceTalkText.text));

        isChoiceMode = true;

    }

    IEnumerator DisplayTextOneByOne(string text)
    {
        justTalkText.text = "";
        foreach (char letter in text)
        {
            justTalkText.text += letter;
            yield return new WaitForSeconds(0.05f);  // 각 글자 사이의 딜레이
        }

    }

    IEnumerator DisplayText(string text) {
        choiceTalkText.text = "";
        foreach (char letter in text)
        {
            choiceTalkText.text += letter;
            yield return new WaitForSeconds(0.05f);  // 각 글자 사이의 딜레이
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
            result = $"두번째 카드는 {secondCard} 였어! 맞췄으니 내가 이 섬으로 이사올게! 큐릉";
        else
            result = $"두번째 카드는 {secondCard} 였어... 다음에 다시 놀러올게! 큐릉";


        choiceTalk.SetActive(false);
        justTalk.SetActive(true);
        justTalkText.text = result;
        StartCoroutine(DisplayTextOneByOne(justTalkText.text)); //되려나??

        isChoiceMode = false;

        exit.gameObject.SetActive(true); // exit 버튼 표시

    }
}
