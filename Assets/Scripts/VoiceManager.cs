using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Linq;

public class VoiceManager : MonoBehaviour {

    //1. 데이터 선언
    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    private void Start()
    {
        //2. 키워드 설정
        keywords.Add("Start", () =>
        {
            SendMessage("StartGame");
            DebugText.instance.debug = "<Start>";
        });

        keywords.Add("Reset", () =>
        {
            SendMessage("ResetGame");
            DebugText.instance.debug = "<Reset>";
        });

        keywords.Add("Test", () =>
        {
            DebugText.instance.debug = "<TEST>";
        });

        //3. 키워드가 들어있는 데이터를 인식하는 데이터에 삽입
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

        //4. 이벤트를 등록하고 인식을 시작해줌
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        //5. 추가한 함수(어구인식함수)를 작성
        System.Action keywordAction;
        // 인식 된 키워드가 사전에 있다면 해당 Action을 호출하십시오. 
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }
}
