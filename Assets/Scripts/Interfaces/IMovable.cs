using UnityEngine;

namespace VN
{

public interface IMovable
{
    Vector2 Offset    { get; set; }
    Vector2 Direction { get; set; }
    bool    Paused    { get; set; }
}

}