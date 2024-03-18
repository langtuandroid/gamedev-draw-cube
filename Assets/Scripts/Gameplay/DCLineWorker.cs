using System.Collections;
using System.Collections.Generic;
using UiControllers.Game;
using UnityEngine;

namespace Gameplay
{
    public class DCLineWorker : MonoBehaviour
    {
        public static DCLineWorker Instance;
        
        [SerializeField] private GameObject _linePrefab;
        
        private Transform _mainCameraTransform;
        private Transform _playerCameraTransform;
        private Transform _playerTransform;

        private RectTransform _drawingBoard;
        private List<Vector2> _fingerPositions = new();

        private Vector3 _linePos;
        private Vector3 _drawingBoardPosInWorld;
        private GameObject _tempLine;
        private LineRenderer _lineRenderer;
        private EdgeCollider2D _edgeCollider;

        private Vector2[] _firstLegColliderPositions, _secondLedColliderPositions;
        private Vector3[] _firstLegRendererPositions, _secondLegRendererPositions;

        private bool _isFirstDraw = true;

        private Camera _mainCamera;

        private void Awake() => Instance = this;

        void Start()
        {
            _mainCamera = Camera.main;
            _drawingBoard = DCDrawingBoardController.Instance.transform as RectTransform;
            _playerTransform = DCPlayerController.Instance.transform;
            _drawingBoardPosInWorld = _mainCamera.ScreenToWorldPoint(_drawingBoard.position);
            _mainCameraTransform = DCCameraFollow.Instance.transform;
            _playerCameraTransform = GameObject.FindWithTag("PlayerCamera").transform;
            _mainCameraTransform.GetChild(0).gameObject.SetActive(false);
        }

        void Update()
        {
            if (Input.touchCount > 1) return;
            if (Input.GetMouseButtonDown(0))
            {
                if (!DCDrawingBoardController.Instance.IsHovering) return;
                if (_mainCameraTransform.childCount > 0) Destroy(_mainCameraTransform.GetChild(0).gameObject);
                Time.timeScale = 0.2f;
                CreateTheLine();
            }

            if (Input.GetMouseButton(0))
            {
                if (_tempLine != null)
                {
                    Vector2 pos = _mainCamera.ScreenToWorldPoint(Input.mousePosition) - new Vector3(_mainCameraTransform.position.x, _mainCameraTransform.position.y);
                    Vector2 tempFingerPos = new Vector2(pos.x, pos.y + Mathf.Abs(_drawingBoardPosInWorld.y - DCCameraFollow.Instance.GetOffset().y));
                    if (Vector2.Distance(tempFingerPos, _fingerPositions[^1]) > 0.1f) UpdateTheLine(tempFingerPos);
                }
            }

            if (Input.GetMouseButtonUp(0)) ReleaseTheLine();
        }

        IEnumerator CreateLegByLine(EdgeCollider2D _collider, LineRenderer _lineRenderer, Vector2[] filledCollider, Vector3[] filledRenderer)
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

        private void CreateTheLine()
        {
            _linePos = _playerTransform.position - _mainCamera.ScreenToWorldPoint(Input.mousePosition) - new Vector3(0f, DCCameraFollow.Instance.GetOffset().y);

            _tempLine = Instantiate(_linePrefab, _playerCameraTransform);
            _tempLine.transform.localPosition = new Vector3(0f, 1f, 17.2f);

            _lineRenderer = _tempLine.GetComponent<LineRenderer>();
            _edgeCollider = _tempLine.GetComponent<EdgeCollider2D>();
            _fingerPositions.Clear();
            var pos = _mainCamera.ScreenToWorldPoint(Input.mousePosition) - new Vector3(_mainCameraTransform.position.x, _mainCameraTransform.position.y - DCCameraFollow.Instance.GetOffset().y);
            _fingerPositions.Add(pos);
            _fingerPositions.Add(pos);
            _lineRenderer.SetPosition(0, _fingerPositions[0]);
            _lineRenderer.SetPosition(1, _fingerPositions[1]);
            _edgeCollider.points = _fingerPositions.ToArray();
        }

        private void UpdateTheLine(Vector2 newFingerPos)
        {
            if (!DCDrawingBoardController.Instance.IsHovering) return;
        
            newFingerPos.x -= _drawingBoardPosInWorld.x;
            newFingerPos.y -= Mathf.Abs(_drawingBoardPosInWorld.y);

            Vector2 lastFingerPos = _fingerPositions[_fingerPositions.Count - 1];
            float distance = Vector2.Distance(newFingerPos, lastFingerPos);

            float constDistance = 0.4f;

            if (distance > constDistance)
            {
                int steps = Mathf.FloorToInt(distance / constDistance); // Определяем количество шагов
                Vector2 stepDirection = (newFingerPos - lastFingerPos).normalized * constDistance; // Направление шага

                for (int i = 0; i < steps; i++)
                {
                    Vector2 nextPoint = lastFingerPos + stepDirection * (i + 1); // Вычисляем следующую точку
                    _fingerPositions.Add(nextPoint);
                    _lineRenderer.positionCount++;
                    _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, nextPoint);
                    _edgeCollider.points = _fingerPositions.ToArray();
                }

                _fingerPositions.Add(newFingerPos);
                _lineRenderer.positionCount++;
                _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, newFingerPos);
                _edgeCollider.points = _fingerPositions.ToArray();
            }
            else
            {
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
        }

        private void ReleaseTheLine()
        {
            if (_tempLine != null)
            {
                StopAllCoroutines();

                foreach (GameObject line in GameObject.FindGameObjectsWithTag("Line"))
                    Destroy(line);
                _playerTransform.GetChild(0).transform.GetChild(0).localScale = Vector3.one;
                _playerTransform.GetChild(0).transform.GetChild(1).localScale = Vector3.one;

                if (_isFirstDraw)
                {
                    _isFirstDraw = false;
                    _playerTransform.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
                    _playerTransform.gameObject.GetComponent<Animation>().Stop();
                }

                GameObject leg1 = Instantiate(_tempLine,
                    _linePos + new Vector3(_mainCameraTransform.position.x, _mainCameraTransform.position.y),
                    Quaternion.identity);
                Destroy(_tempLine);
                GameObject leg2 = Instantiate(leg1,
                    _linePos + new Vector3(_mainCameraTransform.position.x, _mainCameraTransform.position.y),
                    Quaternion.identity);
                leg1.transform.SetParent(_playerTransform.GetChild(0).transform.GetChild(0));
                leg1.transform.parent.transform.localScale /= 2f;
                leg2.transform.SetParent(_playerTransform.GetChild(0).transform.GetChild(1));
                leg2.transform.parent.transform.localScale /= 2f;
                _playerTransform.GetChild(0).transform.GetChild(1).transform.Rotate(Vector3.forward, 180f);
                leg1.GetComponent<EdgeCollider2D>().enabled = true;
                leg2.GetComponent<EdgeCollider2D>().enabled = true;

                leg1.transform.localPosition =
                    new Vector3(leg1.transform.localPosition.x, leg1.transform.localPosition.y, 0f);
                leg2.transform.localPosition =
                    new Vector3(leg2.transform.localPosition.x, leg2.transform.localPosition.y, 0f);


                LineRenderer lineRenderer1 = leg1.GetComponent<LineRenderer>();
                _firstLegRendererPositions = new Vector3[lineRenderer1.positionCount];
                for (int i = 0; i < lineRenderer1.positionCount; i++)
                    _firstLegRendererPositions[i] = lineRenderer1.GetPosition(i);
                lineRenderer1.positionCount = 0;

                LineRenderer lineRenderer2 = leg2.GetComponent<LineRenderer>();
                _secondLegRendererPositions = new Vector3[lineRenderer2.positionCount];
                for (int i = 0; i < lineRenderer2.positionCount; i++)
                    _secondLegRendererPositions[i] = lineRenderer2.GetPosition(i);
                lineRenderer2.positionCount = 0;

                EdgeCollider2D collider1 = leg1.GetComponent<EdgeCollider2D>();
                _firstLegColliderPositions = new Vector2[collider1.pointCount];
                for (int i = 0; i < collider1.pointCount; i++)
                    _firstLegColliderPositions[i] = collider1.points[i];
                collider1.points = new Vector2[0];

                EdgeCollider2D collider2 = leg2.GetComponent<EdgeCollider2D>();
                _secondLedColliderPositions = new Vector2[collider2.pointCount];
                for (int i = 0; i < collider2.pointCount; i++)
                    _secondLedColliderPositions[i] = collider2.points[i];
                collider2.points = new Vector2[0];

                StartCoroutine(CreateLegByLine(collider1, lineRenderer1, _firstLegColliderPositions, _firstLegRendererPositions));
                StartCoroutine(CreateLegByLine(collider2, lineRenderer2, _secondLedColliderPositions, _secondLegRendererPositions));


                float minPosX = 100f, minPosY = 100f, maxPosX = -100f, maxPosY = -100f;

                for (int i = 0; i < _firstLegRendererPositions.Length; i++)
                {
                    if (_firstLegRendererPositions[i].x < minPosX)
                        minPosX = _firstLegRendererPositions[i].x;
                    if (_firstLegRendererPositions[i].y < minPosY)
                        minPosY = _firstLegRendererPositions[i].y;
                    if (_firstLegRendererPositions[i].x > maxPosX)
                        maxPosX = _firstLegRendererPositions[i].x;
                    if (_firstLegRendererPositions[i].y > maxPosY)
                        maxPosY = _firstLegRendererPositions[i].y;
                }

                DCPlayerController.Instance.SetRotation(new Vector2(minPosX, minPosY), new Vector2(maxPosX, maxPosY));
            }

            Time.timeScale = 1f;
        }

        public Transform GetMainCameraTransform() => _mainCameraTransform;
    }
}
