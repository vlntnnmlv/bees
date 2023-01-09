using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

namespace VN
{

public struct Anchors
{
    public Vector2 Min { get; set; }
    public Vector2 Max { get; set; }

    public Anchors(Vector2 _Min, Vector2 _Max)
    {
        Min = _Min;
        Max = _Max;
    }

    public Anchors(float _MinX, float _MaxX, float _MinY, float _MaxY)
    {
        Min = new Vector2(_MinX, _MinY);
        Max = new Vector2(_MaxX, _MaxY);
    }
}

[RequireComponent(typeof(RectTransform))]
public class Node : UIBehaviour
{
    #region creation

    public static Node Create(string _ID, Node _Parent, Rect _Rect)
    {
        Node control = Utility.CreateObject<Node>(_ID, _Parent);

        if (_Parent != null)
            control.SetParent(_Parent);

        control.Create(_Rect);
        return control;
    }

    #endregion

    #region constants

    const float LAYER_OFFSET = 1f;

    #endregion

    #region attributes

    RectTransform m_RectTransform;

    #endregion

    #region properties

    public Node Parent
    {
        get
        {
            return transform.parent == null
                ? null
                : transform.parent.GetComponent<Node>();
        }
    }

    public float Layer
    {
        get => -RectTransform.localPosition.z;
        set => RectTransform.localPosition = new Vector3(
                RectTransform.localPosition.x,
                RectTransform.localPosition.y,
                -value
            );
    }

    public float WorldLayer => (Parent == null ? 0 : Parent.WorldLayer) + Layer;

    public RectTransform RectTransform
    {
        get
        {
            if (m_RectTransform == null)
                m_RectTransform = GetComponent<RectTransform>();
            return m_RectTransform;
        }
    }

    public Quaternion Rotation
    {
        get => RectTransform.localRotation;
        set => RectTransform.localRotation = value;
    }

    public Vector2 Offset
    {
        get => RectTransform.anchoredPosition;
        set => RectTransform.anchoredPosition = value;
    }

    public Vector2 WorldOffset
    {
        get => RectTransform.position;
        set => RectTransform.position = new Vector3(value.x, value.y, RectTransform.position.z);
    }

    public Vector2 Size
    {
        get => LocalRect.size;
        set => LocalRect = new Rect(LocalRect.min, value);
    }

    public float Width
    {
        get => Size.x;
        set => Size = new Vector2(value, Size.y);
    }

    public float Height
    {
        get => Size.y;
        set => Size = new Vector2(Size.x, value);
    }

    public Vector2 Pivot
    {
        get => RectTransform.pivot;
        set => RectTransform.pivot = value;
    }

    public Anchors Anchors
    {
        get => new Anchors(RectTransform.anchorMin, RectTransform.anchorMax);
        set
        {
            RectTransform.anchorMin = value.Min;
            RectTransform.anchorMax = value.Max;
        }
    }

    public Vector2 LocalScale
    {
        get => RectTransform.localScale;
        set => RectTransform.localScale = new Vector3(value.x, value.y, RectTransform.localScale.z);
    }

    public Rect Rect
    {
        get => new Rect(LocalRect.min + Offset, LocalRect.size);
        set
        {
            Offset    = value.min - LocalRect.min;
            LocalRect = new Rect(LocalRect.min, value.size);
        }
    }

    public Rect LocalRect
    {
        get => RectTransform.rect;
        set
        {
            if (RectTransform.rect == value)
                return;

            RectTransform.sizeDelta = value.size;
            RectTransform.pivot = new Vector2(
                    Mathf.Approximately(value.size.x, 0) ? 0 : -value.min.x / value.size.x,
                    Mathf.Approximately(value.size.y, 0) ? 0 : -value.min.y / value.size.y
                );
        }
    }

    public Rect WorldRect
    {
        get => RectTransform.TransformRect(LocalRect);
        set
        {
            Rect = new Rect(
                    value.min + Offset - (Vector2) RectTransform.position,
                    value.size
                );
        }
    }

    #endregion

    #region engine methods

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(new Vector3(WorldRect.center.x, WorldRect.center.y, RectTransform.position.z), WorldRect.size);
    }

    protected virtual void Update()
    {
    }

    #endregion

    #region service methods

    protected virtual void Create(Rect _Rect)
    {
        RectTransform.anchoredPosition = _Rect.min;
        Pivot                          = Vector2.zero;
        RectTransform.sizeDelta        = _Rect.size;
    }

    public void SetParent(Node _Parent, bool _WorldPositionStays = false)
    {
        if (_Parent == null || _Parent.gameObject == null || _Parent.transform == null)
            _Parent = GameManager.Instance.Root;

        transform.SetParent(_Parent.transform, _WorldPositionStays);

        SetLayers();
    }

    void SetLayers()
    {
        if (Parent != null && Parent.gameObject != null)
        {
            Node[] children = Parent.GetComponentsInChildren<Node>();
            float maxLayer = children.Select(_C => _C.WorldLayer).Max();
            Layer = maxLayer - Parent.WorldLayer + LAYER_OFFSET;
        }
        else
        {
            float maxLayer = UnityEngine.SceneManagement.SceneManager
                .GetActiveScene()
                .GetRootGameObjects()
                .Select(_G => _G.GetComponent<Node>())
                .Where(_N => _N != null)
                .Select(_N => _N.WorldLayer)
                .Max();
            Layer = maxLayer + LAYER_OFFSET;
        }
    }

    #endregion

}

}