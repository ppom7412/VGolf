using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateItem : MonoBehaviour {
    static public bool isStart;
    public Vector3 startPos;
    public GameObject prefab;
    public string prefabName;
    public float force;

    private bool isCrash;
    private float incount;
    private float outcount;
    private Rigidbody body;

	void Start () {
        
    }

    public void Init()
    {
        gameObject.transform.position = Camera.main.transform.position;
        isCrash = false;
        isStart = false;
        incount = 0.0f;
        outcount = 0.0f;
        startPos = gameObject.transform.position;
        body = GetComponent<Rigidbody>();

        if (GameObject.FindGameObjectWithTag(prefabName) == null)
            StartCoroutine(FoundInstalPosition(prefab));

        AddNewVelocity();
    }

    public IEnumerator FoundInstalPosition(GameObject prefab)
    {
        while (!isStart)
        {
            //Debug.Log("out : "+outcount + " in : "+ incount);
            outcount += Time.deltaTime;

            if (isCrash)
            {
                incount += Time.deltaTime;

                if (incount > 2.5f)
                {
                    isStart = true;
                }
            }

            if (outcount > 3.0f)
            {
                gameObject.transform.position = startPos;
                outcount = 0;
                AddNewVelocity();
            }

            yield return 0;
        }

        // 골프 골인지점 생성 위치.
        GameObject ob = Instantiate(prefab);
        ob.transform.position = transform.position;

        if (prefabName == "Ball")
        {
            gameObject.SetActive(false);
            yield return 0;
        }

        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out hit))
        {
            ob.transform.position = hit.point;
        }

        gameObject.SetActive(false);
        yield return 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        isCrash = true;
        outcount = 0;
        Debug.Log(" 충돌 시작 ");
    }

    private void OnCollisionExit(Collision collision)
    {
        isCrash = false;
        incount = 0;
        Debug.Log(" 충돌 해제 ");
    }

    private void AddNewVelocity()
    {
        //운동량과 회전값을 초기화
        body.velocity = new Vector3(0, 0, 0);
        body.angularVelocity = new Vector3(0, 0, 0);

        //힘 부여
        body.AddForce(Vector3.forward * force);
    }
}
