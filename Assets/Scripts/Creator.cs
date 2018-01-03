using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creator : MonoBehaviour {
    public GameObject holeCreator;
    public GameObject ballCreator;

    void Start () {
		
	}

	void Update () {
		
	}

    public void StartGame()
    {
        DebugText.instance.debug = "Call Game Start";

        holeCreator.SetActive(true);
        ballCreator.SetActive(true);

        holeCreator.GetComponent<CreateItem>().Init();
        ballCreator.GetComponent<CreateItem>().Init();
    }

    public void ResetGame()
    {
        DebugText.instance.debug = "Call Reset Game";

        foreach (GameObject ob in GameObject.FindGameObjectsWithTag("Hole"))
        {
            Destroy(ob);
        }
        foreach (GameObject ob in GameObject.FindGameObjectsWithTag("Ball"))
        {
            Destroy(ob);
        }
    }
}
