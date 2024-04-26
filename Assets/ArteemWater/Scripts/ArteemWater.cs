using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArteemWater : MonoBehaviour
{
    protected struct WaterColumn
    {
        public float currentHeight;
        public float baseHeight;
        public float velocity;
        public float xPosition;
        public int vertexIndex;
    }

    public int pointPerUnits = 5;
    public Vector2 offset;
    public Vector2 size = new Vector2(6f, 2f);

    public int sortingLayerId;
    public int sortingLayerOrder = 3;
    public Material[] waterMaterials;
    public waterType _waterType;
    public float dampening = 0.93f;
    public float tension = 0.025f;
    public float neighbourTransfer = 0.03f;

    public BoxCollider2D boxCollider2D
    {
        get { return m_BoxCollider; }
    }

    protected MeshRenderer m_Renderer;
    protected MeshFilter m_Filter;
    protected Mesh m_Mesh;

    protected BoxCollider2D m_BoxCollider;
    protected ParticleSystem m_Bubbles;
    protected ParticleSystem m_Steam;
    protected BuoyancyEffector2D m_BuoyancyEffector;

    protected WaterColumn[] m_Columns;
    protected float m_Width;
    protected Vector2 m_LowerCorner;

    protected Vector3[] meshVertices;

    bool doChange = false;
    bool startCount = false;
    float timeToStop = 6f;
    float countedTime = 0;

    public GameObject acidSplashPrefab;
    private void OnEnable()
    {

        getSpriteData();

        GetReferences();

        m_BoxCollider.isTrigger = true;

        AdjustComponentSizes();
        RecomputeMesh();
        SetSortingLayer();
        meshVertices = m_Mesh.vertices;
        //SetSortingLayer();
    }

    private void Start()
    {

        m_Bubbles.Play();
        m_Steam.Play();
    }

    void FixedUpdate()
    {
        if (startCount)
        {
            if(countedTime > timeToStop)
            {
                doChange = false;
                startCount = false;
            }
            else
            {
                countedTime += Time.deltaTime;
            }
        }
        if (doChange)
        {
            for (int i = 0; i < m_Columns.Length; ++i)
            {
                //float ratio = ((float)i) / m_columns.Length;

                float leftDelta = 0;
                if (i > 0)
                    leftDelta = neighbourTransfer * (m_Columns[i - 1].currentHeight - m_Columns[i].currentHeight);

                float rightDelta = 0;
                if (i < m_Columns.Length - 1)
                    rightDelta = neighbourTransfer * (m_Columns[i + 1].currentHeight - m_Columns[i].currentHeight);

                float force = leftDelta;
                force += rightDelta;
                force += tension * (m_Columns[i].baseHeight - m_Columns[i].currentHeight);

                m_Columns[i].velocity = dampening * m_Columns[i].velocity + force;

                m_Columns[i].currentHeight += m_Columns[i].velocity;
            }

            for (int i = 0; i < m_Columns.Length; ++i)
            {
                meshVertices[m_Columns[i].vertexIndex].y = m_Columns[i].currentHeight;
            }

            m_Mesh.vertices = meshVertices;

            m_Mesh.UploadMeshData(false);
        }

    }

    void getSpriteData()
    {
        size.x = transform.localScale.x;
        size.y = transform.localScale.y;

        transform.localScale = new Vector3(1, 1, 1);
    }
    public void GetReferences()
    {
        m_Renderer = GetComponent<MeshRenderer>();
        m_Filter = GetComponent<MeshFilter>();

        m_BoxCollider = GetComponent<BoxCollider2D>();
        m_Bubbles = transform.Find("Bubbles").GetComponent<ParticleSystem>();
        m_Steam = transform.Find("Steam").GetComponent<ParticleSystem>();
        m_BuoyancyEffector = GetComponent<BuoyancyEffector2D>();

    }

    public void AdjustComponentSizes()
    {
        ParticleSystem.ShapeModule steamShape = m_Steam.shape;
        steamShape.radius = size.x * 0.5f;

        Vector3 steamLocalPosition = m_Steam.transform.localPosition;
        steamLocalPosition = offset + Vector2.up * size.y * 0.5f;
        m_Steam.transform.localPosition = steamLocalPosition;

        ParticleSystem.ShapeModule bubblesShape = m_Bubbles.shape;
        bubblesShape.radius = size.x * 0.5f;

        Vector3 bubblesLocalPosition = m_Bubbles.transform.localPosition;
        bubblesLocalPosition = offset + Vector2.down * size.y * 0.5f;
        m_Bubbles.transform.localPosition = bubblesLocalPosition;

        m_Steam.Simulate(0.1f);
        m_Bubbles.Simulate(0.1f);

        m_BoxCollider.size = size;
        m_BoxCollider.offset = offset;

        // m_BuoyancyEffector.surfaceLevel = (size.y * 0.5f) - 0.84f;
        m_BuoyancyEffector.surfaceLevel = 0;

    }

    public void RecomputeMesh()
    {
        //we recreate the mesh as we the previous one could come from prefab (and so every object would the same when they each need there...)
        //ref countign should take care of leaking, (and if it's a prefabed mesh, the prefab keep its mesh)
        m_Mesh = new Mesh();
        m_Mesh.name = "WaterMesh";
        m_Filter.sharedMesh = m_Mesh;

        m_LowerCorner = -(size * 0.5f - offset);

        m_Width = size.x;

        int count = Mathf.CeilToInt(size.x * (pointPerUnits - 1)) + 1;

        m_Columns = new WaterColumn[count + 1];

        float step = size.x / count;

        Vector3[] pts = new Vector3[(count + 1) * 2];
        Vector3[] normal = new Vector3[(count + 1) * 2];
        Vector2[] uvs = new Vector2[(count + 1) * 2];
        Vector2[] uvs2 = new Vector2[(count + 1) * 2];
        int[] indices = new int[6 * count];

        for (int i = 0; i <= count; ++i)
        {
            pts[i * 2 + 0].Set(m_LowerCorner.x + step * i, m_LowerCorner.y, 0);
            pts[i * 2 + 1].Set(m_LowerCorner.x + step * i, m_LowerCorner.y + size.y, 0);

            normal[i * 2 + 0].Set(0, 0, 1);
            normal[i * 2 + 1].Set(0, 0, 1);

            uvs[i * 2 + 0].Set(((float)i) / count, 0);
            uvs[i * 2 + 1].Set(((float)i) / count, 1);

            //Set the 2nd uv set to local position, allow for coherent tiling of normal map
            uvs2[i * 2 + 0].Set(pts[i * 2 + 0].x, pts[i * 2 + 0].y);
            uvs2[i * 2 + 1].Set(pts[i * 2 + 1].x, pts[i * 2 + 1].y);

            if (i > 0)
            {
                int arrayIdx = (i - 1) * 6;
                int startingIdx = (i - 1) * 2;

                indices[arrayIdx + 0] = startingIdx;
                indices[arrayIdx + 1] = startingIdx + 1;
                indices[arrayIdx + 2] = startingIdx + 3;

                indices[arrayIdx + 3] = startingIdx;
                indices[arrayIdx + 4] = startingIdx + 3;
                indices[arrayIdx + 5] = startingIdx + 2;
            }

            m_Columns[i] = new WaterColumn();
            m_Columns[i].xPosition = pts[i * 2].x;
            m_Columns[i].baseHeight = pts[i * 2 + 1].y;
            m_Columns[i].velocity = 0;
            m_Columns[i].vertexIndex = i * 2 + 1;
            m_Columns[i].currentHeight = m_Columns[i].baseHeight;
        }

        m_Mesh.Clear();

        m_Mesh.vertices = pts;
        m_Mesh.normals = normal;
        m_Mesh.uv = uvs;
        m_Mesh.uv2 = uvs2;
        m_Mesh.triangles = indices;

        meshVertices = m_Mesh.vertices;

        m_Mesh.UploadMeshData(false);
    }

    public void SetSortingLayer()
    {
        //m_Renderer.sortingLayerID = sortingLayerID;
        m_Renderer.sortingLayerID = sortingLayerId;
        m_Renderer.sortingOrder = sortingLayerOrder;

        switch (_waterType)
        {
            case waterType.Volcano:
                m_Renderer.material = waterMaterials[0];
                break;
            case waterType.Poison:
                m_Renderer.material = waterMaterials[1];
                break;
            case waterType.Normal:
                m_Renderer.material = waterMaterials[2];
                break;
        }

        Debug.Log(m_Renderer.sortingLayerName);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
        if (rb == null || rb.bodyType == RigidbodyType2D.Static)
            return; //we don't care about static rigidbody, they can't "fall" in water
            */

        if (collision.CompareTag("Player"))
        {

            switch (_waterType)
            {
                case waterType.Volcano:
                    collision.transform.parent.BroadcastMessage("reglarDamage", transform.position.x, SendMessageOptions.DontRequireReceiver);
                    break;
                case waterType.Poison:
                    m_Renderer.material = waterMaterials[1];
                    break;
                case waterType.Normal:
                    collision.transform.parent.BroadcastMessage("setWater", true, SendMessageOptions.DontRequireReceiver);
                    break;
            }
        }

        doChange = true;
        startCount = true;
        countedTime = 0;

        Bounds bounds = collision.bounds;

        List<int> touchedColumnIndices = new List<int>();
        float divisionWith = m_Width / m_Columns.Length;

        Vector3 localMin = transform.InverseTransformPoint(bounds.min);
        Vector3 localMax = transform.InverseTransformPoint(bounds.max);

        // find all our springs within the bounds
        var xMin = localMin.x;
        var xMax = localMax.x;


        PlaySplash(new Vector3(bounds.min.x + bounds.extents.x, bounds.min.y, bounds.min.z));

        for (var i = 0; i < m_Columns.Length; i++)
        {
            if (m_Columns[i].xPosition > xMin && m_Columns[i].xPosition < xMax)
                touchedColumnIndices.Add(i);
        }

        // if we have no hits we should loop back through and find the 2 closest verts and use them
        if (touchedColumnIndices.Count == 0)
        {
            for (var i = 0; i < m_Columns.Length; i++)
            {
                // widen our search to included divisitionWidth padding on each side so we definitely get a couple hits
                if (m_Columns[i].xPosition + divisionWith > xMin && m_Columns[i].xPosition - divisionWith < xMax)
                    touchedColumnIndices.Add(i);
            }
        }

        float testForce = 0.2f;
        for (int i = 0; i < touchedColumnIndices.Count; ++i)
        {
            int idx = touchedColumnIndices[i];
            m_Columns[idx].velocity -= testForce;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            switch (_waterType)
            {
                case waterType.Volcano:
                    //collision.transform.parent.BroadcastMessage("reglarDamage", transform.position.x, SendMessageOptions.DontRequireReceiver);
                    break;
                case waterType.Poison:
                    collision.transform.parent.BroadcastMessage("setWater", false, SendMessageOptions.DontRequireReceiver);
                    break;
                case waterType.Normal:
                    collision.transform.parent.BroadcastMessage("setWater", false, SendMessageOptions.DontRequireReceiver);
                    break;
            }
        }
    }

    void PlaySplash(Vector3 position)
    {
        GameObject g = Instantiate(acidSplashPrefab, position, Quaternion.identity);
    }

    public enum waterType
    {
        Volcano,
        Poison,
        Normal
    }


}
