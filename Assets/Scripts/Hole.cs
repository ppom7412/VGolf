using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        UIManager.instance.PushGameReset();
        DebugText.instance.debug = "Clear!!";
    }
}
