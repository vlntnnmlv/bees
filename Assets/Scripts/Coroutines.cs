using UnityEngine;
using System;
using System.Collections;


public static class Coroutines
{
    public static IEnumerator Update(Action _OnStart, Action<float> _OnUpdate, Action _OnFinish, float _Duration)
    {
        float t = 0;        
        _OnStart?.Invoke();


        if (_OnUpdate != null)
        {
            _OnUpdate(0);

            while (t < _Duration)
            {
                _OnUpdate(t / _Duration);

                yield return null;

                t += Time.deltaTime;
            }

            _OnUpdate(1);
        }

        _OnFinish?.Invoke();
        yield break;
    }
}