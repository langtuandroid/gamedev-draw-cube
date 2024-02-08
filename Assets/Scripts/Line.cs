using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    private RectTransform _drawingBoard;
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private Transform mainCameraTransform;
    [SerializeField] private Transform lineCameraTransform;

    private List<Vector2> _fingerPositions = new();

    private Vector3 _linePos;
    private Vector3 _drawingBoardPosInWorldPoint;
    private GameObject _tempLine;
    private LineRenderer _lineRenderer;
    private EdgeCollider2D _edgeCollider;
    private Transform _playerTransform;

    private Vector2[] _leg1ColliderPositions, _leg2ColliderPositions;
    private Vector3[] _leg1RendererPositions, _leg2RendererPositions;

    private bool _isFirstDraw = true;

    public static Line Instance;

    private void Awake() => Instance = this;

    void Start()
    {
        _drawingBoard = DrawingBoardController.Instance.transform as RectTransform;
        _playerTransform = PlayerController.Instance.transform;
        _drawingBoardPosInWorldPoint = Camera.main.ScreenToWorldPoint(_drawingBoard.position);
        mainCameraTransform.GetChild(0).gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!DrawingBoardController.Instance.IsHovering) return;
            if (mainCameraTransform.childCount > 0)
                Destroy(mainCameraTransform.GetChild(0).gameObject);
            Time.timeScale = 0.2f;
            CreateLine();
        }

        if (Input.GetMouseButton(0))
        {
            if (_tempLine != null)
            {
                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition) -
                              new Vector3(mainCameraTransform.position.x, mainCameraTransform.position.y);
                Vector2 tempFingerPos = new Vector2(pos.x,
                    pos.y + Mathf.Abs(_drawingBoardPosInWorldPoint.y - CameraFollow.Instance.GetOffset().y));
                if (Vector2.Distance(tempFingerPos, _fingerPositions[_fingerPositions.Count - 1]) > 0.1f)
                    UpdateLine(tempFingerPos);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (_tempLine != null)
            {
                StopAllCoroutines();

                foreach (GameObject line in GameObject.FindGameObjectsWithTag("Line"))
                    Destroy(line);

                if (_isFirstDraw)
                {
                    _isFirstDraw = false;
                    _playerTransform.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
                    _playerTransform.gameObject.GetComponent<Animation>().Stop();
                }

                GameObject leg1 = Instantiate(_tempLine, _linePos + new Vector3(mainCameraTransform.position.x, mainCameraTransform.position.y), Quaternion.identity);
                Destroy(_tempLine);
                GameObject leg2 = Instantiate(leg1, _linePos + new Vector3(mainCameraTransform.position.x, mainCameraTransform.position.y), Quaternion.identity);
                leg1.transform.SetParent(_playerTransform.GetChild(0).transform.GetChild(0));
                leg1.transform.parent.transform.localScale /= 2f;
                leg2.transform.SetParent(_playerTransform.GetChild(0).transform.GetChild(1));
                leg2.transform.parent.transform.localScale /= 2f;
                _playerTransform.GetChild(0).transform.GetChild(1).transform.Rotate(Vector3.forward, 180f);
                leg1.GetComponent<EdgeCollider2D>().enabled = true;
                leg2.GetComponent<EdgeCollider2D>().enabled = true;

                leg1.transform.localPosition = new Vector3(leg1.transform.localPosition.x, leg1.transform.localPosition.y, 0f);
                leg2.transform.localPosition = new Vector3(leg2.transform.localPosition.x, leg2.transform.localPosition.y, 0f);


                LineRenderer lineRenderer1 = leg1.GetComponent<LineRenderer>();
                _leg1RendererPositions = new Vector3[lineRenderer1.positionCount];
                for (int i = 0; i < lineRenderer1.positionCount; i++)
                    _leg1RendererPositions[i] = lineRenderer1.GetPosition(i);
                lineRenderer1.positionCount = 0;

                LineRenderer lineRenderer2 = leg2.GetComponent<LineRenderer>();
                _leg2RendererPositions = new Vector3[lineRenderer2.positionCount];
                for (int i = 0; i < lineRenderer2.positionCount; i++)
                    _leg2RendererPositions[i] = lineRenderer2.GetPosition(i);
                lineRenderer2.positionCount = 0;

                EdgeCollider2D collider1 = leg1.GetComponent<EdgeCollider2D>();
                _leg1ColliderPositions = new Vector2[collider1.pointCount];
                for (int i = 0; i < collider1.pointCount; i++)
                    _leg1ColliderPositions[i] = collider1.points[i];
                collider1.points = new Vector2[0];

                EdgeCollider2D collider2 = leg2.GetComponent<EdgeCollider2D>();
                _leg2ColliderPositions = new Vector2[collider2.pointCount];
                for (int i = 0; i < collider2.pointCount; i++)
                    _leg2ColliderPositions[i] = collider2.points[i];
                collider2.points = new Vector2[0];

                StartCoroutine(GrowLegByLine(collider1, lineRenderer1, _leg1ColliderPositions, _leg1RendererPositions));
                StartCoroutine(GrowLegByLine(collider2, lineRenderer2, _leg2ColliderPositions, _leg2RendererPositions));


                float minPosX = 100f, minPosY = 100f, maxPosX = -100f, maxPosY = -100f;

                for (int i = 0; i < _leg1RendererPositions.Length; i++)
                {
                    if (_leg1RendererPositions[i].x < minPosX)
                        minPosX = _leg1RendererPositions[i].x;
                    if (_leg1RendererPositions[i].y < minPosY)
                        minPosY = _leg1RendererPositions[i].y;
                    if (_leg1RendererPositions[i].x > maxPosX)
                        maxPosX = _leg1RendererPositions[i].x;
                    if (_leg1RendererPositions[i].y > maxPosY)
                        maxPosY = _leg1RendererPositions[i].y;
                }

                PlayerController.Instance.SetRotation(new Vector2(minPosX, minPosY), new Vector2(maxPosX, maxPosY));
            }
            Time.timeScale = 1f;
        }
    }

    IEnumerator GrowLegByLine(EdgeCollider2D _collider, LineRenderer _lineRenderer, Vector2[] filledCollider, Vector3[] filledRenderer)
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

    private void CreateLine()
    {
        _linePos = _playerTransform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition) - new Vector3(0f, CameraFollow.Instance.GetOffset().y);

        _tempLine = Instantiate(linePrefab, lineCameraTransform);
        _tempLine.transform.localPosition = new Vector3(0f, 1f, 17.2f);

        _lineRenderer = _tempLine.GetComponent<LineRenderer>();
        _edgeCollider = _tempLine.GetComponent<EdgeCollider2D>();
        _fingerPositions.Clear();
        _fingerPositions.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition) - new Vector3(mainCameraTransform.position.x, mainCameraTransform.position.y - CameraFollow.Instance.GetOffset().y));
        _fingerPositions.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition) - new Vector3(mainCameraTransform.position.x, mainCameraTransform.position.y - CameraFollow.Instance.GetOffset().y));
        _lineRenderer.SetPosition(0, _fingerPositions[0]);
        _lineRenderer.SetPosition(1, _fingerPositions[1]);
        _edgeCollider.points = _fingerPositions.ToArray();
    }

    private void UpdateLine(Vector2 newFingerPos)
    {
        if (!DrawingBoardController.Instance.IsHovering) return;
        
        newFingerPos.x -= _drawingBoardPosInWorldPoint.x;
        newFingerPos.y -= Mathf.Abs(_drawingBoardPosInWorldPoint.y);

        _fingerPositions.Add(newFingerPos);

        if (Vector2.Distance(_fingerPositions[_fingerPositions.Count - 1], _fingerPositions[_fingerPositions.Count - 2]) > 0.1f)
        {
            _lineRenderer.positionCount++;
            _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, newFingerPos);
            _edgeCollider.points = _fingerPositions.ToArray();
        }
        else
        {
            _fingerPositions.RemoveAt(_fingerPositions.Count - 1);
        }
    }

    public Transform GetMainCameraTransform() => mainCameraTransform;
}
