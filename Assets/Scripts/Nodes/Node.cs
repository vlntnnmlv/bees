using UnityEngine;
using System.Linq;
namespace VN
{
    public partial class Node : MonoBehaviour
    {
        const float LAYER_OFFSET = 1f;

        public float Layer
        {
            get => -transform.localPosition.z;
            set => transform.localPosition = new Vector3(
                    transform.localPosition.x,
                    transform.localPosition.y,
                    -value
                );
        }

        public Vector2 Offset
        {
            get => transform.position;
            set => transform.position = new Vector3(value.x, value.y, transform.position.z);
        }

        public Vector2 LocalScale
        {
            get => transform.localScale;
            set => transform.localScale = new Vector3(
                value.x,
                value.y,
                transform.localScale.z
            );
        }

        public Node Parent
        {
            get
            {
                return transform.parent == null
                    ? null
                    : transform.parent.GetComponent<Node>();
            }
        }

        public static Node Create(Node _Parent, Vector2 _Offset, string _ID)
        {
            Node node = Utility.CreateObject<Node>(_ID, _Parent);

            node.Create(_Offset);

            return node;
        }

        void Update()
        {
            OnUpdate();
        }

        protected virtual void OnUpdate()
        {
        }

        public void SetParent(Node _Parent, bool _WorldPositionStays = false)
        {
            if (_Parent == null || _Parent.gameObject == null || _Parent.transform == null)
                transform.parent = null;
            else
                transform.SetParent(_Parent.transform, _WorldPositionStays);

            SetLayers();
        }

        protected virtual void Create(Vector2 _Offset)
        {
            Offset = _Offset;
            SetLayers();
        }

        void SetLayers()
        {
            if (Parent != null && Parent.gameObject != null)
            {
                Node[] children = Parent.GetComponentsInChildren<Node>();
                float maxLayer = children.Select(_C => _C.Layer).Max();
                Layer = maxLayer + LAYER_OFFSET;
            }
            else
            {
                float maxLayer = UnityEngine.SceneManagement.SceneManager
                    .GetActiveScene()
                    .GetRootGameObjects()
                    .Where(_G => _G.GetComponent<Node>() != null)
                    .Select(_N => _N.layer)
                    .Max();
                Layer = maxLayer + LAYER_OFFSET;
            }
        }
    }
}