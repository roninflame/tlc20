using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PolloScripts.TargetSystem
{
    public class TargetManager : MonoBehaviour
    {
        public static TargetManager Instance { get; private set; }

        [Space]
        [Header("Target Area")]
        [Space]
        [SerializeField] private GameObject _targetArea;
        [SerializeField] private GameObject _targetCenter;
        [SerializeField] private float _targetAreaRadius = 0.08f;

        [Space]
        [Header("Target Selected")]
        [Space]
        [SerializeField] private GameObject _targetSelectedPrefab;
        [SerializeField] private int _targetSelectedInstances = 5;
        [SerializeField] private float _targetSelectedScale = 1.5f;

        [Space]
        [Header("RayCast")]
        [Space]
        [SerializeField] private LayerMask _raycastMask;
        [SerializeField] private float _raycastDistance = 400f;

        //Shares
        public float RaycastDistance => _raycastDistance;
        public Dictionary<int, Target> selectedTable = new Dictionary<int, Target>();

        //LOCAl
        private Queue<Transform> _targetQueue = new Queue<Transform>();
        private TargetUI[] _targetUIList = new TargetUI[2];

        private GameObject _targetSelectedGO = null;
        private RectTransform _targetAreaRect;
        private TargetUI _targetUI;

        private bool _activated;
       
        //private RectTransform _targetCanvasRect;
        //private RectTransform _targetCenterRect;
        //private Image _targetAreaImage;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                _targetAreaRect = _targetArea.GetComponent<RectTransform>();
                ActiveTargetArea(false);
                //_targetCanvasRect = GetComponent<RectTransform>();
                //_targetCenterRect = TargetCenter.GetComponent<RectTransform>();
                //_targetAreaImage = TargetArea.GetComponent<Image>();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        private void Start()
        {
            CrearTargets();
        }
        private void Update()
        {
            if (!_activated)
                return;
            UpdateTargetSelected();
        }
        public void AddSelected(Target target)
        {
            int id = target.TargetGO.GetInstanceID();
            if (!(selectedTable.ContainsKey(id)))
            {
                selectedTable.Add(id, target);
                //go.AddComponent<selection_component>();
                //Debug.Log("Added " + id + " to selected dict");
            }
        }
        public void Deselect(int id)
        {
            //Destroy(selectedTable[id].GetComponent<selection_component>());
            selectedTable.Remove(id);
        }
        public void Activate()
        {
            _targetCenter.SetActive(true);
            _activated = true;
            ActiveTargetArea(true);
        }
        public void Deactivate()
        {
            _activated = false;
            DeselectAll();
            if (_targetUI != null)
                _targetUI.Deactivate();
            _targetUI = null;
            _targetSelectedGO = null;
            ActiveTargetArea(false);
            _targetCenter.SetActive(true);
        }
        public bool IsInTargetArea(Vector3 targetPosition)
        {
           return (Vector2.Distance(Camera.main.WorldToScreenPoint(targetPosition)
                , RectTransformUtility.WorldToScreenPoint(null, _targetArea.transform.position))
                < _targetAreaRadius * Camera.main.pixelWidth) ? true : false;
        }
        public bool IsInRange(float distance)
        {
            if (distance < RaycastDistance && distance > 5f)
                return true;
            else
                return false;
        }

        public Vector3 GetCenterTarget()
        {
            Vector3 pos = Vector3.zero;
            Ray _ray = Camera.main.ScreenPointToRay(_targetAreaRect.position);
            RaycastHit _rayHit;

            if (Physics.Raycast(_ray, out _rayHit, _raycastDistance, _raycastMask))
            {
                pos = _rayHit.point;
            }
            else
            {
                pos = _ray.origin + (_ray.direction * _raycastDistance);
            }

            return pos;
        }
        public Vector3 GetClosetTarget()
        {
            Vector3 pos = Vector3.zero;
            if (_targetSelectedGO != null)
            {
                pos = _targetSelectedGO.transform.position;
            }
            else
            {
                Ray _ray = Camera.main.ScreenPointToRay(_targetAreaRect.position);
                RaycastHit _rayHit;

                if (Physics.Raycast(_ray, out _rayHit, _raycastDistance, _raycastMask))
                {
                    pos = _rayHit.point;
                    //print("TARGET: " + _rayHit.collider.transform.parent.name);
                }
                else
                {
                    pos = _ray.origin + (_ray.direction * _raycastDistance);
                }
            }
            return pos;
        }

        //public Vector3 GetClosetTarget2()
        //{
        //    Transform tra = null;
        //    Vector3 pos = Vector3.zero;
        //    float minDistance = float.MaxValue;
        //    foreach (KeyValuePair<int, Target> obj in selectedTable)
        //    {
        //        float distancia = obj.Value.DistanceFromPlayer;
        //        if (distancia < minDistance)
        //        {
        //            minDistance = distancia;
        //            tra = obj.Value.transform;
        //        }
        //    }

        //    if (tra == null)
        //    {
        //        Ray _ray = Camera.main.ScreenPointToRay(_targetAreaRect.position);
        //        RaycastHit _rayHit;

        //        if (Physics.Raycast(_ray, out _rayHit, _raycastDistance, _raycastMask))
        //        {
        //            pos = _rayHit.point;
        //        }
        //        else
        //        {
        //            pos = _ray.origin + (_ray.direction * _raycastDistance);
        //        }

        //    }
        //    else
        //    {
        //        pos = tra.position;
        //    }
        //    return pos;
        //}

        private TargetUI GetTarget(Transform t)
        {
            Transform tra = null;
            TargetUI target = null;
            if (!_targetQueue.Peek().gameObject.activeInHierarchy)
            {
                tra = _targetQueue.Dequeue();
                target = tra.GetComponent<TargetUI>();
                target.SetTarget(t, _targetSelectedScale);
                _targetQueue.Enqueue(tra);
            }
            else
            {
                GameObject go = Instantiate(_targetSelectedPrefab, Vector3.zero, Quaternion.identity) as GameObject;
                tra = go.transform;
                tra.parent = transform;
                go.name = go.name + "_" + _targetSelectedInstances;
                target = tra.GetComponent<TargetUI>();
                target.SetTarget(t, _targetSelectedScale);
                _targetQueue.Enqueue(tra);
            }
            return target;
        }
        private void CrearTargets()
        {
            for (int i = 0; i < _targetSelectedInstances; i++)
            {
                GameObject go = Instantiate(_targetSelectedPrefab, Vector3.zero, Quaternion.identity) as GameObject;
                go.transform.parent = transform;
                go.name = go.name + "_" + _targetSelectedInstances;
                go.SetActive(false);
                _targetQueue.Enqueue(go.transform);
            }
        }
        private void ActiveTargetArea(bool b)
        {
            _targetArea.SetActive(b);
        }
        private void DeselectAll()
        {
            selectedTable.Clear();

        }
        private void UpdateTargetSelected()
        {
            GameObject tobj = null;
            Vector3 pos = Vector3.zero;
            float minDistance = float.MaxValue;
            float distancia = 0f;

            foreach (KeyValuePair<int, Target> obj in selectedTable)
            {
                distancia = obj.Value.DistanceFromPlayer;
                if (distancia < minDistance)
                {
                    minDistance = distancia;
                    tobj = obj.Value.TargetGO;
                }
            }
            if (tobj != null)
            {
                if (_targetSelectedGO == null)
                {
                    _targetSelectedGO = tobj;
                    _targetUI = GetTarget(_targetSelectedGO.transform);

                }
                else if (tobj.GetInstanceID() != _targetSelectedGO.GetInstanceID())
                {
                    _targetSelectedGO = tobj;
                    if (_targetUI == null)
                        _targetUI = GetTarget(_targetSelectedGO.transform);
                    else
                        _targetUI.SetTarget(_targetSelectedGO.transform, _targetSelectedScale);
                }
                _targetCenter.SetActive(false);
            }
            else
            {
                if (_targetUI != null)
                    _targetUI.Deactivate();
                _targetUI = null;
                _targetSelectedGO = null;
                _targetCenter.SetActive(true);
            }
        }
    }

}
