using UnityEngine;

namespace VN
{

[RequireComponent(typeof(TextMesh))]
public class Text : Node
{
    #region creation

    public static Text Create(string _ID, Node _Parent, Rect _Rect, string _Text = "")
    {
        Text control = Utility.CreateObject<Text>(_ID, _Parent);

        control.Create(_Rect, _Text);
        return control;
    }

    #endregion

    #region attributes

    [SerializeField] float m_CharacterSize = 1;
    [SerializeField] int   m_FontSize      = 1;

    TextMesh m_TextMesh;

    #endregion

    #region properties

    public TextMesh TextMesh
    {
        get
        {
            if (m_TextMesh == null)
                m_TextMesh = GetComponent<TextMesh>();
            return m_TextMesh;
        }
    }

    public string TextString
    {
        get => TextMesh.text;
        set => TextMesh.text = value;
    }

    #endregion

    #region engine methods


    protected override void OnValidate()
    {
        TextMesh.characterSize = m_CharacterSize * 0.01f;
        TextMesh.fontSize      = m_FontSize * 100;
    }

    #endregion

    #region public methods

    public void Create(Rect _Rect, string _Text)
    {
        base.Create(_Rect);

        TextMesh.hideFlags |= HideFlags.HideInInspector;

        TextMesh.font   = Resources.Load<Font>("Fonts/Electronic Highway Sign");
        TextMesh.anchor = TextAnchor.MiddleCenter;
        TextMesh.characterSize = m_CharacterSize * 0.01f;
        TextMesh.fontSize      = m_FontSize * 100;

        Pivot = Vector2.one / 2;

        TextString = _Text;
    }

    #endregion
}

}