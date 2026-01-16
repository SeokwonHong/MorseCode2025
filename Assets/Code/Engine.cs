using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour
{

    public AudioSource beep;
    // Start is called before the first frame update

    float startTime=0f;
    float endTime=0f;
    float lastKeyUpTime = 0f;

    //textmesh

    public TextMesh refToText;

    public string Ol;
    char alphabet='\0';

    // 밑의 바 UI
    
    public float barSpeed=0.05f;
    public float barSize = 0.03f;
    float moveInterval;
    public GameObject refToBarPrefeb;
    private GameObject currentBar;


    public Transform SignalSpawnPoint;


    public GameObject refToSpace,refToR;

    private Color originalColor;
    private void Start()
    {
        Application.targetFrameRate = 60;

        SpriteRenderer sr = refToSpace.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            originalColor = sr.color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        


        if(Input.GetKeyDown(KeyCode.Space))
        {

            //ㅣ인지 O 인지 검사하기 위한 초세기
            startTime=Time.time;

            //띄어쓰기

            if (!beep.isPlaying)  // 소리가 이미 재생되고 있지 않으면
            {
                beep.Play();  // 소리 재생
            }

            //밑의 바 UI
            BarInstantiating();


            refToSpace.GetComponent<SpriteRenderer>().color = new Color32(0xE7, 0xE7, 0xE7, 0xFF);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            endTime=Time.time;
            float duration = endTime - startTime;
            LetterCalculation(duration);
            beep.Stop();

            //바 UI
            currentBar = null;


            //띄어쓰기
            lastKeyUpTime = Time.time;
            // 문자출력
            refToSpace.GetComponent<SpriteRenderer>().color = originalColor;
        }

        if (lastKeyUpTime > 0)
        {
            float timeElapsed = Time.time - lastKeyUpTime;

            bool characterSpaceTriggered = false;
            bool wordSpaceTriggered = false;

            // 띄어쓰기: 0.36초 ~ 0.7초
            if (timeElapsed > 0.36f && timeElapsed < 0.7f)
            {
                CharacterSpace();
                characterSpaceTriggered = true;
            }

            // 단어 간 띄어쓰기: 0.7초 이상
            if (timeElapsed >= 0.7f)
            {
                WordSpace();
                wordSpaceTriggered = true;
            }

            // 마지막으로 띄어쓰기를 처리한 후 초기화
            if (characterSpaceTriggered || wordSpaceTriggered)
            {
                lastKeyUpTime = -1f; // 띄어쓰기 후 초기화하여 중복 방지
            }
        }


        if(Input.GetKeyDown(KeyCode.R))
        {
            refToR.GetComponent<SpriteRenderer>().color = new Color32(0xE7, 0xE7, 0xE7, 0xFF);

            refToText.text = "";  // 현재까지 작성된 문자 초기화
            Ol = "";              // 모스코드 입력도 초기화
            alphabet = '\0';
        }
        else if (Input.GetKeyUp(KeyCode.R))
        {
            refToR.GetComponent<SpriteRenderer>().color = originalColor;
        }

    }

    // 바 제어용

    void BarInstantiating()
    {
        if (currentBar == null) // 현재 바가 없을 때만 생성
        {
            currentBar = Instantiate(refToBarPrefeb, SignalSpawnPoint.position, Quaternion.identity);
            StartCoroutine(BarMoving(currentBar));
        }

    }
 
    IEnumerator BarMoving(GameObject newBar)
    {
        while (newBar!=null)
        {
 

            if (newBar == currentBar && Input.GetKey(KeyCode.Space))
            {
                newBar.transform.localScale += new Vector3(barSize, 0, 0);
            }


            newBar.transform.position -= new Vector3(barSpeed, 0, 0);
            yield return new WaitForSeconds(moveInterval);

            if (newBar.transform.position.x < -10)
            {
                Destroy(newBar);
                if (newBar == currentBar) currentBar = null;
                yield break;
            }



        } 
    }

    void CharacterSpace()
    {
        refToText.text += alphabet.ToString();
        //alphabet = ' ';
        Ol = "";
    }

    void WordSpace() // 일단 이건 나중에 하고, 
    {
        Debug.Log("단어간 띄어쓰기.");
    }

    void LetterCalculation(float Duration)
    {
        startTime = 0; endTime=0;
        if(Duration>0.15f&&Duration<0.55f)
        {
            Ol +="l";
            LetterMaker(Ol);
        }
        else if(Duration<=0.15f)
        {
            Ol += "O";
            LetterMaker(Ol);
        }
        else if(Duration>=0.55f)
        {
            Debug.Log("오류?");
            alphabet = '\0';
        }
    }

    void LetterMaker(string OlRule)
    {
        alphabet = '\0';
        switch (OlRule)
        {
            // 기본 알파벳
            case "Ol": alphabet = 'A'; break;
            case "lOOO": alphabet = 'B'; break;
            case "lOlO": alphabet = 'C'; break;
            case "lOO": alphabet = 'D'; break;
            case "O": alphabet = 'E'; break;
            case "OOlO": alphabet = 'F'; break;
            case "llO": alphabet = 'G'; break;
            case "OOOO": alphabet = 'H'; break;
            case "OO": alphabet = 'I'; break;
            case "Olll": alphabet = 'J'; break;
            case "lOl": alphabet = 'K'; break;
            case "OlOO": alphabet = 'L'; break;
            case "ll": alphabet = 'M'; break;
            case "lO": alphabet = 'N'; break;
            case "lll": alphabet = 'O'; break;
            case "OllO": alphabet = 'P'; break;
            case "llOl": alphabet = 'Q'; break;
            case "OlO": alphabet = 'R'; break;
            case "OOO": alphabet = 'S'; break;
            case "l": alphabet = 'T'; break;
            case "OOl": alphabet = 'U'; break;
            case "OOOl": alphabet = 'V'; break;
            case "Oll": alphabet = 'W'; break;
            case "lOOl": alphabet = 'X'; break;
            case "lOll": alphabet = 'Y'; break;
            case "llOO": alphabet = 'Z'; break;

            // 숫자
            case "Ollll": alphabet = '1'; break;
            case "OOlll": alphabet = '2'; break;
            case "OOOll": alphabet = '3'; break;
            case "OOOOl": alphabet = '4'; break;
            case "OOOOO": alphabet = '5'; break;
            case "lOOOO": alphabet = '6'; break;
            case "llOOO": alphabet = '7'; break;
            case "lllOO": alphabet = '8'; break;
            case "llllO": alphabet = '9'; break;
            case "lllll": alphabet = '0'; break;

            // 기타 부호
            case "llOOll": alphabet = ','; break;
            case "OOllOO": alphabet = '?'; break;
            case "lllOOO": alphabet = ':'; break;
            case "lOOOOl": alphabet = '-'; break;
            case "OlOOlO": alphabet = '"'; break;
            case "lOllO": alphabet = '('; break;
            case "lOllOl": alphabet = ')'; break;
            case "lOOOl": alphabet = '='; break;
            case "OlOlOl": alphabet = '.'; break;
            case "lOlOlO": alphabet = ';'; break;
            case "lOOlO": alphabet = '/'; break;
            case "OllllO": alphabet = '\''; break;
            case "OllOlO": alphabet = '@'; break;
            case "OlOlO": alphabet = '+'; break;

            default: alphabet = '\0'; break; // 기본값 설정 (없는 경우 대비)
        }

    }


}

