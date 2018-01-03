using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {
    static public UIManager instance;
    public float minDistance;
    public float sensitivity;
    public float getDist;

    public GameObject menuPanel;
    public GameObject pausePanel;

    private Transform mainTr;
    private RaycastHit hit;

	void Start () {
        instance = this;
        mainTr = Camera.main.transform;

        menuPanel.SetActive(false);
    }
	
	void Update () {
        if (CheckRemainDistance(mainTr.position + (mainTr.forward * minDistance)))
        {
            transform.position = Vector3.Lerp(transform.position, mainTr.position + (mainTr.forward * minDistance), Time.deltaTime);
        }

        //Vector3 currVec = new Vector3(transform.position.x, transform.position.y, 0);
        //currVec += mainTr.forward * minDistance;
        //transform.position = currVec;

        transform.LookAt(mainTr);
        transform.rotation = mainTr.rotation;
    }

    bool CheckRemainDistance(Vector3 point)
    {
        //Vector3 newdir = new Vector3(point.x, point.y, 0);
        //Vector3 newpos = new Vector3(transform.position.x, transform.position.y, 0);

        getDist = Vector3.Distance(point, transform.position);
        if (getDist < sensitivity)
        {
            return false;
        }

        return true;
    }

    bool CheckRemainDistanceZ()
    {
        Vector3 pos = transform.position;
        Vector3 vecZ = mainTr.position + (mainTr.forward * minDistance );
        if (vecZ.z > pos.z && pos.z > mainTr.position.z)
        {
            return false;
        }
        else if (vecZ.z < pos.z && pos.z < mainTr.position.z)
        {
            return false;
        }
        return true;
    }

    bool CheckdWindows()
    {
        Ray ray = new Ray(mainTr.position, mainTr.forward);
        if (Physics.Raycast(ray, sensitivity, 5))
        {
            return false;
        }
        return true;
    }

    public void OpenAndCloseMenuPanel()
    {
        if (menuPanel.active)
        {
            menuPanel.SetActive(false);
            return;
        }
        else
        {
            menuPanel.SetActive(true);
            return;
        }
    }

    public void PushGameStart()
    {
        GameObject.FindGameObjectWithTag("Manager").SendMessage("StartGame");
    }

    public void PushGameReset()
    {
        //모든 자식에게 호출
        //this.BroadcastMessage("ResetGame");

        GameObject.FindGameObjectWithTag("Manager").SendMessage("ResetGame");
    }

}
