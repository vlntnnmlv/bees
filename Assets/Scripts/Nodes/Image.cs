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

        image.Create(_Rect);

        return image;
    }

    #endregion

    #region properties

    public Color Color
    {
        get => MeshRenderer.material.color;
        set => MeshRenderer.material.color = value;
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

    [SerializeField] VN.Sprite m_Sprite;
    [SerializeField] int       m_SpriteID;
    [SerializeField] Color     m_Color = Color.white;

    MeshRenderer m_MeshRenderer;
    MeshFilter   m_MeshFilter;

    #endregion

    #region engine methods

    protected override void Awake()
    {
        base.Awake();

        MeshRenderer.material = new Material(Shader.Find("Sprites/Default"));

        if (m_Sprite != null)
        {
            MeshRenderer.material.mainTexture = m_Sprite.Texture;
            m_Sprite.ID = m_SpriteID;
        }

        CreateMesh();
    }

    protected override void Update()
    {
        if (m_Sprite != null)
        {
            MeshRenderer.material.mainTexture = m_Sprite.Texture;
            MeshFilter.mesh.uv                = m_Sprite.GetUV();
        }
    }

    protected override void OnValidate()
    {
        base.OnValidate();

        if (m_Sprite == null)
            return;

        m_Sprite.ID = m_SpriteID;

        MeshRenderer.material.mainTexture = m_Sprite.Texture;
        MeshRenderer.material.color       = m_Color;
        CreateMesh();
    }

    protected override void OnRectTransformDimensionsChange()
    {
        base.OnRectTransformDimensionsChange();

        Vector2 pos  = LocalRect.min;
        Vector2 size = LocalRect.size;

        MeshFilter.mesh.vertices = new Vector3[]
        {
            new Vector3(pos.x, pos.y, 0),
            new Vector3(pos.x + size.x, pos.y, 0),
            new Vector3(pos.x + size.x, pos.y + size.y, 0),
            new Vector3(pos.x, pos.y + size.y, 0),
        };
    }

    #endregion

    #region service methods

    protected virtual void Create(Rect _Rect, string _SpriteName)
    {
        base.Create(_Rect);

        m_Sprite = Resources.Load<VN.Sprite>("Sprites/" + _SpriteName);
    }

    void CreateMesh()
    {
        if (MeshFilter.mesh == null)
        {
            MeshFilter.mesh = new Mesh();
            MeshFilter.mesh.name = "rect_mesh";
        }

        Vector2 pos  = LocalRect.min;
        Vector2 size = LocalRect.size;

        MeshFilter.mesh.vertices = new Vector3[]
        {
            new Vector3(pos.x, pos.y, 0),
            new Vector3(pos.x + size.x, pos.y, 0),
            new Vector3(pos.x + size.x, pos.y + size.y, 0),
            new Vector3(pos.x, pos.y + size.y, 0),
        };

        MeshFilter.mesh.triangles = new int[]
        {
            0, 1, 2,
            0, 2, 3,
        };

        if (m_Sprite != null)
            MeshFilter.mesh.uv = m_Sprite.GetUV();
    }

    #endregion

}

}