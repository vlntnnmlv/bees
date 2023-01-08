using UnityEngine;

namespace VN
{

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
[ExecuteAlways]
public class Image : Node
{
    #region creation

    public static Image Create(string _ID, Node _Parent, Rect _Rect, string _SpriteName)
    {
        Image image = Utility.CreateObject<Image>(_ID, _Parent);

        image.Create(_Rect, _SpriteName);
        return image;
    }

    #endregion

    #region properties

    public Color Color
    {
        get => MeshRenderer.sharedMaterial.color;
        set
        {
            m_Color = value;
            if (MeshRenderer.sharedMaterial != null)
                MeshRenderer.sharedMaterial.color = m_Color;
        }
    }

    public MeshRenderer MeshRenderer
    {
        get
        {
            if (m_MeshRenderer == null)
                m_MeshRenderer = GetComponent<MeshRenderer>();
            return m_MeshRenderer;
        }
    }

    public MeshFilter MeshFilter
    {
        get
        {
            if (m_MeshFilter == null)
                m_MeshFilter = GetComponent<MeshFilter>();
            return m_MeshFilter;
        }
    }

    #endregion

    #region attibutes

    [SerializeField] protected VN.Sprite     m_Sprite;
    [SerializeField] protected int           m_SpriteID;
    [SerializeField] Color         m_Color = Color.white;

    MeshRenderer m_MeshRenderer;
    MeshFilter   m_MeshFilter;

    #endregion

    #region engine methods

    protected override void Start()
    {
        base.Start();

        // set material
        MeshRenderer.sharedMaterial = new Material(Shader.Find("Sprites/Default"));
        MeshRenderer.sharedMaterial.mainTexture = m_Sprite.Texture;

        // set mesh
        MeshFilter.sharedMesh = CalculateMesh();
        MeshFilter.sharedMesh.uv = m_Sprite.CalculateUV(m_SpriteID);
    }

    protected override void Update()
    {
        base.Update();

        Color = m_Color;
        if (MeshRenderer.sharedMaterial != null && m_Sprite != null)
            MeshRenderer.sharedMaterial.mainTexture = m_Sprite.Texture;
        
        if (MeshFilter.sharedMesh != null && MeshFilter.sharedMesh.vertices != null && MeshFilter.sharedMesh.vertices.Length == 4)
            MeshFilter.sharedMesh.uv = m_Sprite.CalculateUV(m_SpriteID);
    }

    protected override void OnRectTransformDimensionsChange()
    {
        base.OnRectTransformDimensionsChange();

        if (MeshFilter.sharedMesh != null && MeshFilter.sharedMesh.vertices != null && MeshFilter.sharedMesh.vertices.Length == 4)
            MeshFilter.sharedMesh.vertices = CalculateVertices();
    }

    #endregion

    #region service methods

    protected virtual void Create(Rect _Rect, string _SpriteName)
    {
        base.Create(_Rect);

        m_Sprite = Resources.Load<VN.Sprite>("Sprites/" + _SpriteName);
    }

    Mesh CalculateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.name = "RectMesh";

        int[] triangles = new int[6]
        {
            0, 1, 2,
            0, 2, 3
        };

        mesh.vertices = CalculateVertices();
        mesh.triangles = triangles;

        return mesh;
    }

    Vector3[] CalculateVertices()
    {
        Vector2 pos = LocalRect.min;
        Vector2 size = LocalRect.size;

        Vector3[] vertices = new Vector3[4]
        {
            new Vector3(pos.x,          pos.y         ),
            new Vector3(pos.x + size.x, pos.y         ),
            new Vector3(pos.x + size.x, pos.y + size.y),
            new Vector3(pos.x,          pos.y + size.y)
        };

        return vertices;
    }


    #endregion

}

}