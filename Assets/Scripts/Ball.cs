using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour {
    public Text text; 

    [SerializeField]
    private Vector3 savePos;
    public bool isMove;

    private GameObject hole;
    private Rigidbody rigid;
    private bool isCrash;
    private float count;
    private bool isForce;
    private int force;
    private bool downup;

    void Start()
    {
        text = GameObject.Find("ForceTxt").GetComponent<Text>();
        rigid = GetComponent<Rigidbody>();

        hole = null;
        force = 0;
        savePos = transform.position;
        isCrash = false;
        isMove = false;

        //푸시했을때의 힘.
        //StartMove(Vector3.zero);
    }

    void Update()
    {
        UpdateForceText();

        if (hole == null)
            hole = GameObject.FindGameObjectWithTag("Hole");

        if (hole == null)
            return;

        if (HoleToDistance() < 0.2f)
        {
            DebugText.instance.debug = "Game Clear!";
            Destroy(hole);
            Destroy(gameObject);
            //게임 클리어 !
        }
        else if (HoleToDistance() < 0.5f)
        {
            transform.position = Vector3.Lerp(transform.position, hole.transform.position, Time.deltaTime);
        }
        else if (HoleToDistance() >= 0.45f && !isMove)
        {
            gameObject.transform.position = savePos;
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }
    }

    void UpdateForceText()
    {
        if (!isForce)
        {
            text.text = "";
            return;
        }

        if (force < 1)
            downup = true;
        else if (force > 29)
            downup = false;

        if (downup)
            force++;
        else
            force--;

        text.text = "<"+ force+">";

    }

    float HoleToDistance()
    {
        return Vector3.Distance(hole.transform.position, transform.position);
    }

    public void ViewForceText(bool sw)
    {
        isForce = sw;
    }

    public void StartMove()
    {
        count = 0;
        isMove = true;

        rigid.AddForce(transform.forward* TenAdd());
        StartCoroutine(FoundStopPoint());
    }

    //멈추는 시점 저장.
    public IEnumerator FoundStopPoint()
    {
        while (isMove)
        {
            yield return 0;
            count += Time.deltaTime;

            if (count > 4.5f)
            {
                isMove = false;
                break;
            }
        }

        if (isCrash)
        {
            savePos = transform.position;
        }
        else
        {
            gameObject.transform.position = savePos;
        }

        Debug.Log(" 마지막 지점 결정 ");
        yield return 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        isCrash = true;
        count = 0;
        Debug.Log(" 충돌 시작 ");
    }

    private void OnCollisionStay(Collision collision)
    {
        isCrash = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isCrash = false;
        count = 0;
        Debug.Log(" 충돌 해제 ");
    }

    private float TenAdd()
    {
        float num = 1;
        for (int i = 0; i < force; ++i)
            num *= 1.2f;

        return num;
    }
}
