using UnityEngine;
using VN;
using UnityEngine.SceneManagement;

namespace VN
{

public class ButtonPlay : Button
{
    protected override void OnClicked()
    {
        SceneManager.LoadScene("MainScene");
    }

    float m_TimeAnimated = 0;

    // protected override void OnUpdate()
    // {
    //     if (Time.time - m_TimeAnimated < 4)
    //     {
    //         m_TimeAnimated = Time.time;
    //         StartCoroutine(Coroutines.Update(
    //                 null,
    //                 _Phase => LocalScale = Vector2.one * (
    //                     _Phase < 0.5f
    //                     ? (_Phase / 0.5f) * 1.2f
    //                     : ((1 - _Phase) * 2 * 1.2f)
    //                 ),
    //                 null,
    //                 1
    //             )
    //         );
    //     }
    // }
}

}