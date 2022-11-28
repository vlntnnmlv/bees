using UnityEngine;
using System.Linq;

namespace VN
{   
    public class Object : MonoBehaviour
    {  
        const float LAYER_OFFSET = 1f;
        public SpriteRenderer[] SpriteRenderers { get => GetComponentsInChildren<SpriteRenderer>(); }
        public Vector2 Offset
        {
            get => transform.position;
            set => transform.position = new Vector3(value.x, value.y, transform.position.z);
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

        public Vector2 LocalScale
        {
            get => transform.lossyScale;
            set => transform.localScale = value;
        }

        protected void Create(Vector2 _Offset, VN.Object _Parent = null)
        {
            SetParent(_Parent);
            Offset = _Offset;
        }

        protected void UpdateLayers(VN.Object _Parent)
        {
            if (_Parent == null)
            {
                Layer = UnityEngine
                                .SceneManagement
                                .SceneManager
                                .GetActiveScene()
                                .GetRootGameObjects()
                                .Select(_G => _G.GetComponent<VN.Object>())
                                .Where(_G => _G != null)
                                .Select(_O => _O.Layer)
                                .Max() + LAYER_OFFSET;
                return;
            }

            VN.Object[] objects = _Parent.GetComponentsInChildren<VN.Object>();
            Layer = objects.Select(_O => _O.Layer).Max() + LAYER_OFFSET;
        }

        public void SetParent(VN.Object _Parent)
        {
            transform.parent = _Parent?.transform;

            UpdateLayers(_Parent);
        }
    }
}