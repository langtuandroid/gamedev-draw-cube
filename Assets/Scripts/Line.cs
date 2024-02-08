using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    public RectTransform drawingBoard;
    public GameObject line;
    public Transform mainCameraTransform, camera2Transform;

    [HideInInspector]
    public List<Vector2> fingerPositions;

    private Vector3 linePos, drawingBoardPosInWorldPoint;
    private GameObject tempLine;
    private LineRenderer lineRen;
    private EdgeCollider2D edgeCollider;
    private Transform playerTransform;

    private Vector2[] leg1ColliderPositions, leg2ColliderPositions;
    private Vector3[] leg1RendererPositions, leg2RendererPositions;

    private bool firstDraw = true;

    public static Line Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        drawingBoardPosInWorldPoint = Camera.main.ScreenToWorldPoint(drawingBoard.position);
        mainCameraTransform.GetChild(0).gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 fingerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - new Vector3(mainCameraTransform.position.x, mainCameraTransform.position.y - CameraFollow.Instance.offset.y);
            if ((fingerPos.x < Mathf.Abs(drawingBoard.rect.width / 2f)) && (fingerPos.x > -drawingBoard.rect.width / 2f) && ((fingerPos.y + Mathf.Abs(drawingBoardPosInWorldPoint.y)) < Mathf.Abs(drawingBoard.rect.height / 2f)) && ((fingerPos.y + Mathf.Abs(drawingBoardPosInWorldPoint.y)) > -drawingBoard.rect.height / 2f))
            {
                if (mainCameraTransform.childCount > 0)
                    Destroy(mainCameraTransform.GetChild(0).gameObject);
                Time.timeScale = 0.2f;
                CreateLine();
            }
        }

        
        
        if (Input.GetMouseButton(0))
        {
            if (tempLine != null)
            {
                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition) -
                              new Vector3(mainCameraTransform.position.x, mainCameraTransform.position.y);
                Vector2 tempFingerPos = new Vector2(pos.x,
                    pos.y + Mathf.Abs(drawingBoardPosInWorldPoint.y - CameraFollow.Instance.offset.y));
                if (Vector2.Distance(tempFingerPos, fingerPositions[fingerPositions.Count - 1]) > 0.1f)
                    UpdateLine(tempFingerPos);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (tempLine != null)
            {
                StopAllCoroutines();

                foreach (GameObject line in GameObject.FindGameObjectsWithTag("Line"))
                    Destroy(line);

                if (firstDraw)
                {
                    firstDraw = false;
                    playerTransform.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
                    playerTransform.gameObject.GetComponent<Animation>().Stop();
                }

                GameObject leg1 = Instantiate(tempLine, linePos + new Vector3(mainCameraTransform.position.x, mainCameraTransform.position.y), Quaternion.identity);
                Destroy(tempLine);
                GameObject leg2 = Instantiate(leg1, linePos + new Vector3(mainCameraTransform.position.x, mainCameraTransform.position.y), Quaternion.identity);
                leg1.transform.SetParent(playerTransform.GetChild(0).transform.GetChild(0));
                leg1.transform.parent.transform.localScale /= 2f;
                leg2.transform.SetParent(playerTransform.GetChild(0).transform.GetChild(1));
                leg2.transform.parent.transform.localScale /= 2f;
                playerTransform.GetChild(0).transform.GetChild(1).transform.Rotate(Vector3.forward, 180f);
                leg1.GetComponent<EdgeCollider2D>().enabled = true;
                leg2.GetComponent<EdgeCollider2D>().enabled = true;

                leg1.transform.localPosition = new Vector3(leg1.transform.localPosition.x, leg1.transform.localPosition.y, 0f);
                leg2.transform.localPosition = new Vector3(leg2.transform.localPosition.x, leg2.transform.localPosition.y, 0f);


                LineRenderer lineRenderer1 = leg1.GetComponent<LineRenderer>();
                leg1RendererPositions = new Vector3[lineRenderer1.positionCount];
                for (int i = 0; i < lineRenderer1.positionCount; i++)
                    leg1RendererPositions[i] = lineRenderer1.GetPosition(i);
                lineRenderer1.positionCount = 0;

                LineRenderer lineRenderer2 = leg2.GetComponent<LineRenderer>();
                leg2RendererPositions = new Vector3[lineRenderer2.positionCount];
                for (int i = 0; i < lineRenderer2.positionCount; i++)
                    leg2RendererPositions[i] = lineRenderer2.GetPosition(i);
                lineRenderer2.positionCount = 0;

                EdgeCollider2D collider1 = leg1.GetComponent<EdgeCollider2D>();
                leg1ColliderPositions = new Vector2[collider1.pointCount];
                for (int i = 0; i < collider1.pointCount; i++)
                    leg1ColliderPositions[i] = collider1.points[i];
                collider1.points = new Vector2[0];

                EdgeCollider2D collider2 = leg2.GetComponent<EdgeCollider2D>();
                leg2ColliderPositions = new Vector2[collider2.pointCount];
                for (int i = 0; i < collider2.pointCount; i++)
                    leg2ColliderPositions[i] = collider2.points[i];
                collider2.points = new Vector2[0];

                StartCoroutine(GrowLeg(collider1, lineRenderer1, leg1ColliderPositions, leg1RendererPositions));
                StartCoroutine(GrowLeg(collider2, lineRenderer2, leg2ColliderPositions, leg2RendererPositions));


                float minPosX = 100f, minPosY = 100f, maxPosX = -100f, maxPosY = -100f;

                for (int i = 0; i < leg1RendererPositions.Length; i++)
                {
                    if (leg1RendererPositions[i].x < minPosX)
                        minPosX = leg1RendererPositions[i].x;
                    if (leg1RendererPositions[i].y < minPosY)
                        minPosY = leg1RendererPositions[i].y;
                    if (leg1RendererPositions[i].x > maxPosX)
                        maxPosX = leg1RendererPositions[i].x;
                    if (leg1RendererPositions[i].y > maxPosY)
                        maxPosY = leg1RendererPositions[i].y;
                }

                Move.Instance.CheckIfCanRotate(new Vector2(minPosX, minPosY), new Vector2(maxPosX, maxPosY));
            }
            Time.timeScale = 1f;
        }
    }

    IEnumerator GrowLeg(EdgeCollider2D _collider, LineRenderer _lineRenderer, Vector2[] filledCollider, Vector3[] filledRenderer)
    {
        List<Vector2> colliderPositions = new List<Vector2>();
        for (int i = 0; i < filledCollider.Length; i++)
        {
            colliderPositions.Add(filledCollider[i]);
            _collider.points = colliderPositions.ToArray();
            _lineRenderer.positionCount = i + 1;
            _lineRenderer.SetPosition(i, filledRenderer[i]);

            yield return new WaitForEndOfFrame();
        }
    }

    public void CreateLine()
    {
        linePos = playerTransform.position - (Camera.main.ScreenToWorldPoint(Input.mousePosition)) - new Vector3(0f, CameraFollow.Instance.offset.y);

        tempLine = Instantiate(line, camera2Transform);
        tempLine.transform.localPosition = new Vector3(0f, 1f, 17.2f);

        lineRen = tempLine.GetComponent<LineRenderer>();
        edgeCollider = tempLine.GetComponent<EdgeCollider2D>();
        fingerPositions.Clear();
        fingerPositions.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition) - new Vector3(mainCameraTransform.position.x, mainCameraTransform.position.y - CameraFollow.Instance.offset.y));
        fingerPositions.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition) - new Vector3(mainCameraTransform.position.x, mainCameraTransform.position.y - CameraFollow.Instance.offset.y));
        lineRen.SetPosition(0, fingerPositions[0]);
        lineRen.SetPosition(1, fingerPositions[1]);
        edgeCollider.points = fingerPositions.ToArray();
    }

    private void UpdateLine(Vector2 newFingerPos)
    {
        if (!DrawingBoardController.Instance.IsHovering) return;
        
        newFingerPos.x -= drawingBoardPosInWorldPoint.x;
        newFingerPos.y -= Mathf.Abs(drawingBoardPosInWorldPoint.y);

        fingerPositions.Add(newFingerPos);

        if (Vector2.Distance(fingerPositions[fingerPositions.Count - 1], fingerPositions[fingerPositions.Count - 2]) > 0.1f)
        {
            lineRen.positionCount++;
            lineRen.SetPosition(lineRen.positionCount - 1, newFingerPos);
            edgeCollider.points = fingerPositions.ToArray();
        }
        else
        {
            fingerPositions.RemoveAt(fingerPositions.Count - 1);
        }
    }
}
