using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

namespace VN
{

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
        get => -transform.localPosition.z;
        set => transform.localPosition = new Vector3(
                transform.localPosition.x,
                transform.localPosition.y,
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

    public Vector2 Offset
    {
        get => RectTransform.anchoredPosition;
        set => RectTransform.anchoredPosition = value;
    }

    public Vector2 LocalScale
    {
        get => RectTransform.localScale;
        set => RectTransform.localScale = value;
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
        Gizmos.DrawWireCube(WorldRect.center, WorldRect.size);
    }

    protected virtual void Update()
    {
    }

    #endregion

    #region service methods

    protected void Create(Rect _Rect)
    {
        RectTransform.anchoredPosition = _Rect.min;
        RectTransform.pivot            = Vector2.zero;
        RectTransform.sizeDelta        = _Rect.size;
    }

    public void SetParent(Node _Parent, bool _WorldPositionStays = false)
    {
        if (_Parent == null || _Parent.gameObject == null || _Parent.transform == null)
            transform.SetParent(null, true);
        else
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

public static class ExtensionTransform
{
    public static Rect TransformRect(this Transform _Transform, Rect _Rect)
    {
        return new Rect(
				_Rect.x * _Transform.lossyScale.x + _Transform.position.x,
				_Rect.y * _Transform.lossyScale.y + _Transform.position.y,
				_Rect.width * _Transform.lossyScale.x,
				_Rect.height * _Transform.lossyScale.y
			);
    }
}

}