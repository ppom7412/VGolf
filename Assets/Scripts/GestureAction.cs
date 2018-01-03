using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GestureAction : MonoBehaviour
{
    public float RotationSensitivity = 1.0f;
    public GameObject arrowOb;
    public bool isSelect;

    public Vector3 dir;

    private Vector3 position;
    private float rotationFactor;

    private void Start()
    {
        //arrowOb.SetActive(false);
        isSelect = false;
    }

    void Update()
    {
        PerformRotation();
    }

    private void PerformRotation()
    {
        if (isSelect && GazeGestureManager.Instance.isNavigating)
        {
            rotationFactor = GazeGestureManager.Instance.naviPos.y * RotationSensitivity;
            transform.Rotate(new Vector3(rotationFactor, 0, 0));

            //DebugText.instance.debug = "delta.y : " + rotationFactor;
        }
    }

    public void OnMouseDrag()
    {
        if (isSelect)
        {
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
            Vector3 delta = mousePos - position;
            transform.Rotate(delta.y, 0, 0);
            position = mousePos;

            //DebugText.instance.debug = "delta:" + delta.y +", Angles:"+ transform.eulerAngles.x;
        }
    }

    public void ObjectToRoTation(Vector3 pos)
    {
        float x = transform.eulerAngles.x;
        Vector3 targetDir = pos - transform.position;
        dir = Vector3.RotateTowards(transform.forward, targetDir, 100, 0.0F);
        transform.rotation = Quaternion.LookRotation(dir);
        transform.eulerAngles = new Vector3(x, transform.eulerAngles.y, transform.eulerAngles.z);
    }

    public void OnSelect()
    {
        isSelect = !isSelect;
        CheckSelect();
    }

    public void CheckSelect()
    {
        Ball ball = GetComponent<Ball>();

        if (isSelect)
        {
            ball.ViewForceText(false);
            transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            //arrowOb.SetActive(true);
            return;
        }

        ball.ViewForceText(true);
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        //arrowOb.SetActive(false);

        return;
    }

}
