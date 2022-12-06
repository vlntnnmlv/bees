// using UnityEngine;
// using VN;

// namespace VN
// {

// // TODO: Add Rect to VN.Object
// // TODO: Add stack-like data structure for layering system
// // TODO: Separate SelectionArea and UnitControlManager

// public class UnitControlManager : MonoBehaviour
// {
//     bool                        m_Held;
//     VN.Controller2D[]           m_Controllers;
//     VN.Object                   m_SelectionArea;

//     Vector2? m_FirstPoint = null;

//     void Start()
//     {
//         m_Controllers   = FindObjectsOfType<VN.Controller2D>();
//         m_SelectionArea = Utility.Load<VN.Object>("Prefabs/SelectionArea", "selection_area");
//         m_SelectionArea.gameObject.SetActive(false);
//     }

//     void Update()
//     {
//         if (Input.GetMouseButtonDown(0))
//         {
//             m_FirstPoint = m_SelectionArea.Offset;
//             m_SelectionArea.Offset = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//             m_SelectionArea.GetComponent<SpriteRenderer>().size = Vector2.zero;
//             m_SelectionArea.gameObject.SetActive(true);
//         }

//         if (Input.GetMouseButtonUp(0))
//         {
//             m_FirstPoint = null;
//             m_SelectionArea.gameObject.SetActive(false);
//         }

//         if (m_FirstPoint.HasValue)
//         {
//             Vector2 mouse = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
//             m_SelectionArea.GetComponent<SpriteRenderer>().size = 
//                 new Vector2(
//                     Mathf.Abs(mouse.x - m_FirstPoint.Value.x),
//                     Mathf.Abs(mouse.y - m_FirstPoint.Value.y)
//                 );
//         }
//     }
// }

// }