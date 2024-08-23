using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(EnemyFOV))]
public class EditorFOV : Editor
{
    private void OnSceneGUI()
    {
        EnemyFOV fow = (EnemyFOV)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.viewRadius);
        Vector3 viewAngleA = fow.DirFromAngle(-fow.viewAngle / 2, false);
        Vector3 viewAngleB = fow.DirFromAngle(fow.viewAngle / 2, false);

        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.viewRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadius);

    }
/*
    [SerializeField] LayerMask layerMask;
    private Mesh mesh;
    private Vector3 _origin;
    private float _fov;
    private float _viewDistance;
    private float _startingAngle;

    void Start()
    {
        mesh = new Mesh();
       // GetComponent<MeshFilter>().mesh = mesh;
        _origin = Vector3.zero;
        _fov = 50f;
        _viewDistance = 10f;
    }
    void LateUpdate()
    {


        int rayCount = 5;
        float angle = _startingAngle;
        float angleIncrease = _fov / rayCount;



        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = _origin;
        int vertexIndex = 1;
        int trianglesIndex = 0;
        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex;
            RaycastHit2D raycastHit = Physics2D.Raycast(_origin, GetVectorFromAngle(angle), _viewDistance, layerMask);
            if (raycastHit.collider == null)
            {
                vertex = _origin + GetVectorFromAngle(angle) * _viewDistance;
            }
            else
            {
                vertex = raycastHit.point;
            }

            vertices[vertexIndex] = vertex;
            if (i > 0)
            {
                triangles[trianglesIndex + 0] = 0;
                triangles[trianglesIndex + 1] = vertexIndex - 1;
                triangles[trianglesIndex + 2] = vertexIndex;

                trianglesIndex += 3;
            }

            vertexIndex++;
            angle -= angleIncrease;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }
    public void SetOrigin(Vector3 origin)
    {
        origin = _origin;
    }
    public void SetAimDirection(Vector3 aimDirection)
    {
        _startingAngle = GetAngleFromVector(aimDirection) - _fov / 2f;
    }
    public void SetFov(float fov)
    {
        _fov = fov;
    }
    public void SetViewDistance(float viewDistance)
    {
        _viewDistance = viewDistance;
    }

    public Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    public float GetAngleFromVector(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        if (n < 0)
            n += 360;

        return n;
    }
*/
}
