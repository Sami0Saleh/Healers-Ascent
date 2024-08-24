using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyFOV : MonoBehaviour
{
    [SerializeField] EnemyController enemyController;
    [SerializeField] Transform enemytransform;
    public float viewRadius;

    public bool PlayerDetected = false;

    [UnityEngine.Range(0,360)]
    public float viewAngle;

    public LayerMask playerMask;
    public LayerMask obstaclesMask;

    public List<Transform> visibleTargets = new List<Transform>();

    public float meshResolution;

    public MeshFilter viewMeshFilter;
    private Mesh _viewMesh;

    private PlayerController _playerController;
    private void Start()
    {
        _viewMesh = new Mesh();
        _viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = _viewMesh;
        StartCoroutine(FindPlayerWithDelay(0.2f));
    }

    private void Update()
    {
        DrawFieldOfView();
        UpdatePlayerDetection();
    }

    IEnumerator FindPlayerWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindPlayer();
        }
    }
    void FindPlayer()
    {
        visibleTargets.Clear();

        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, playerMask);
        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstaclesMask))
                {
                    visibleTargets.Add(target);
                    PlayerDetected = true;
                    
                }
            }
        }

        if (visibleTargets.Count == 0)
        {
            PlayerDetected = false;
        }
    }
    void UpdatePlayerDetection()
    {
        if (visibleTargets.Count > 0)
        {
            Transform target = visibleTargets[0];
            _playerController = target.GetComponent<PlayerController>();

            if (_playerController.CurrentHp > 0)
            {
                enemytransform.LookAt(target);
                enemyController.Shoot(_playerController);
            }
        }
    }
    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, viewRadius, obstaclesMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else return new ViewCastInfo(false, transform.position +dir * viewRadius, viewRadius, globalAngle);
    }
    public Vector3 DirFromAngle(float angleInDegree, bool angleIsGlobal)
    {
        if (!angleIsGlobal) { angleInDegree += transform.eulerAngles.y; }
        return new Vector3(Mathf.Sin(angleInDegree * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegree * Mathf.Deg2Rad));
    }
    
    void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewcast = ViewCast(angle);
            viewPoints.Add(newViewcast.point);
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);
            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }
        _viewMesh.Clear();
        _viewMesh.vertices = vertices;
        _viewMesh.triangles = triangles;
        _viewMesh.RecalculateNormals();
    }
    
    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dst;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
        {
            hit = _hit;
            point = _point;
            dst = _dst;
            angle = _angle;
        }
    }
}
