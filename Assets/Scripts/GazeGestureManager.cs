using UnityEngine;
using UnityEngine.VR.WSA.Input;

public class GazeGestureManager : MonoBehaviour
{
    public static GazeGestureManager Instance { get; private set; }
    public GameObject FocusedObject { get; private set; }
    public GameObject SelectedObject { get; private set; }
    public GameObject gazeObject;

    public bool isNavigating { get; private set; }
    public Vector3 naviPos { get; private set; }

    private RaycastHit hitInfo;

    //GestureRecognizer recognizer;
    GestureRecognizer naviRecognizer;
    Vector3 saveScale;

    void Awake()
    {
        // 싱글톤 등록
        Instance = this;
        naviRecognizer = new GestureRecognizer();

        // navigation 인식으로 셋팅 >> Y축,X축 주의!!
        naviRecognizer.SetRecognizableGestures(GestureSettings.Tap |GestureSettings.NavigationY);       //인식방향이 달라짐.

        // NavigationRecognizer_Event function를 등록
        naviRecognizer.TappedEvent += NavigationRecognizer_TappedEvent;
        naviRecognizer.NavigationStartedEvent += NavigationRecognizer_NavigationStartedEvent;
        naviRecognizer.NavigationUpdatedEvent += NavigationRecognizer_NavigationUpdatedEvent;
        naviRecognizer.NavigationCompletedEvent += NavigationRecognizer_NavigationCompletedEvent;
        naviRecognizer.NavigationCanceledEvent += NavigationRecognizer_NavigationCanceledEvent;

        naviRecognizer.StartCapturingGestures();
    }

    private void OnDestroy()
    {
        naviRecognizer.NavigationStartedEvent -= NavigationRecognizer_NavigationStartedEvent;
        naviRecognizer.NavigationUpdatedEvent -= NavigationRecognizer_NavigationUpdatedEvent;
        naviRecognizer.NavigationCompletedEvent -= NavigationRecognizer_NavigationCompletedEvent;
        naviRecognizer.NavigationCanceledEvent -= NavigationRecognizer_NavigationCanceledEvent;
    }

    void Update()
    {
        GameObject oldFocusObject = FocusedObject;

        var headPosition = Camera.main.transform.position;
        var gazeDirection = Camera.main.transform.forward;

        Debug.DrawRay(headPosition, gazeDirection);
        if (Physics.Raycast(headPosition, gazeDirection, out hitInfo))
        {
            FocusedObject = hitInfo.collider.gameObject;

            //Debug.Log(FocusedObject.name + "과 충돌");

            gazeObject.SetActive(true);
            gazeObject.transform.position = hitInfo.point;
            gazeObject.transform.eulerAngles = hitInfo.normal;

            if (FocusedObject.layer == 9 || FocusedObject.layer == 5 || FocusedObject.layer == 8)   //  아이템 과 UI 충돌
            {
                gazeObject.transform.GetChild(0).gameObject.SetActive(false);
                gazeObject.transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                gazeObject.transform.GetChild(0).gameObject.SetActive(true);
                gazeObject.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
        else
        {
            FocusedObject = null;
            gazeObject.SetActive(false);
        }

        //현재 바라보고 있는 오브젝트와 이전에 바라봤던 오브텍드가 동일 하지 않을때
        if (FocusedObject != oldFocusObject)
        {
            //제스쳐를 취소하고 재시작한다.
            naviRecognizer.CancelGestures();
            naviRecognizer.StartCapturingGestures();
        }

        //디버깅용 
        UpdateDebugMouseVer();
    }

    private void UpdateDebugMouseVer()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (FocusedObject == null)
                return;

            if (FocusedObject.layer == 9)
            {
                FocusedObject.SendMessageUpwards("OnSelect");
                return;
            }

            GameObject ball = GameObject.FindGameObjectWithTag("Ball");
            if (ball == null)     return;

            if (!ball.GetComponent<GestureAction>().isSelect)
            {
                ball.GetComponent<GestureAction>().ObjectToRoTation(hitInfo.point);
                ball.GetComponent<Ball>().StartMove();
            }
        }
    }

    private void NavigationRecognizer_NavigationStartedEvent(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
    {
        isNavigating = true;
        naviPos = relativePosition;

        //DebugText.instance.debug = "naviPos:" + naviPos;
    }
    private void NavigationRecognizer_NavigationUpdatedEvent(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
    {
        isNavigating = true;
        naviPos = relativePosition;

        //DebugText.instance.debug = "naviPos:" + naviPos;
    }

    private void NavigationRecognizer_NavigationCompletedEvent(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
    {
        isNavigating = false;

        //DebugText.instance.debug = "naviPos:" + naviPos;
    }
    private void NavigationRecognizer_NavigationCanceledEvent(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
    {
        //DebugText.instance.debug = "Motion Cancel";

        isNavigating = false;
    }

    private void NavigationRecognizer_TappedEvent(InteractionSourceKind source, int tapCount, Ray ray)
    {
        if (FocusedObject == null)
            return;

        if (FocusedObject.layer == 9)
        {
            FocusedObject.SendMessageUpwards("OnSelect");
            return;
        }

        GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        if (ball == null) return;

        if (!ball.GetComponent<GestureAction>().isSelect && !ball.GetComponent<Ball>().isMove)
        {
            ball.GetComponent<GestureAction>().ObjectToRoTation(hitInfo.point);
            ball.GetComponent<Ball>().StartMove();
        }
    }
    
}
