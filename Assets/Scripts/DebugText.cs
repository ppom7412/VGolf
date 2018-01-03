using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugText : MonoBehaviour {
    static public DebugText instance;
    private Text text;
    public string debug;

	void Awake () {
        instance = this;
        text = GetComponent<Text>();
    }
	
	void Update () {
        if (debug != "")
            StartCoroutine(ShowTextTimer());

    }

    IEnumerator ShowTextTimer()
    {
        text.text = debug;
        debug = "";
        yield return new WaitForSeconds(1.5f);
        text.text = debug;
    }
}
